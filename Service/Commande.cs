using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Models.Commande;
using Services.Produit;
using API.Connection;
using Services.Facture;
namespace Services.Commande
{
    public class CommandeService
    {
        public static List<CommandeStock> Commandes { get; } = new List<CommandeStock>();

        static CommandeService()
        {
            try
            {

                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "select commande.* , Designation from commande left join produit on commande.codepro=produit.codepro; ";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                CommandeStock cs = new CommandeStock();
                                cs.Idcommande = read.GetInt32(0);
                                cs.Idclient = read.GetString(1);
                                cs.Codepro = read.GetString(2);
                                cs.Quantite = read.GetInt32(3);
                                cs.date = read.GetDateTime(4);
                                cs.Designation=read.GetString(5);
                                Commandes.Add(cs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'initialisation des commandes : " + ex.Message);
            }
        }

        public static List<CommandeStock> GetAll() => Commandes;

        public static void Create(CommandeStock cs)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "insert into commande values(@idCommande,@idclient,@idproduit,@qt,@date_commande);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@idCommande",cs.Idcommande);
                        cmd.Parameters.AddWithValue("@idclient", cs.Idclient);
                        cmd.Parameters.AddWithValue("@idproduit", cs.Codepro);
                        cmd.Parameters.AddWithValue("@qt", cs.Quantite);
                        cmd.Parameters.AddWithValue("@date_commande",cs.date);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == cs.Codepro);
                if (index!=-1)cs.Designation=ProduitService.Produits[index].Designation;
                FactureService.setListFacture(cs);
                Commandes.Add(cs); // Ajouter commande à la liste locale après l'insertion

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur d'ajout");
                Console.WriteLine(ex.Message);
            }
           
        }

        public static void Update(CommandeStock cs)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "UPDATE commande SET idclient=@idclient, codepro=@codepro, quantite=@qt, date=@date_commande WHERE idcommande=@idcommande;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idcommande", cs.Idcommande);
                        cmd.Parameters.AddWithValue("@idclient", cs.Idclient);
                        cmd.Parameters.AddWithValue("@codepro", cs.Codepro); // Assurez-vous que le nom de colonne est correct
                        cmd.Parameters.AddWithValue("@qt", cs.Quantite);
                        cmd.Parameters.AddWithValue("@date_commande", cs.date);
                        cmd.ExecuteNonQuery();
                    }
                }

                var index = Commandes.FindIndex(commande => commande.Idcommande == cs.Idcommande);
                var i = ProduitService.Produits.FindIndex(produit => produit.Codepro == cs.Codepro);
                if (index!=-1)cs.Designation=ProduitService.Produits[i].Designation;
                if (index != -1)
                {
                    FactureService.UpdateListFacture(cs); 
                    Commandes[index] = cs; // Mettre à jour la commande dans la liste locale
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour de la commande : " + ex.Message);
            }
        }

    //Delete
        public static void Delete(int id)
        {
            try
            {
                Console.WriteLine("id de suppression = "+id);
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "delete from commande where Idcommande=@idcommande;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idcommande", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                var commande = Commandes.FirstOrDefault(cs => cs.Idcommande == id);
                if (commande != null)
                {
                    
                    Commandes.Remove(commande); // Supprimer le commande de la liste locale
                    FactureService.DeleteFacture(id);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de suppression de commande");
                Console.WriteLine(ex.Message);
            }
    }

    }
}
