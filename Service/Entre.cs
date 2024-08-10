using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Models.Commande;
using Services.Produit;
using  API.Models.EntrerStock;
using API.Connection;

namespace Services.Entre
{
    public class EntreService
    {
        public static List<EntreStock> ListEntre { get; } = new List<EntreStock>();

        static EntreService()
        {
            try
            {
            
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "select entrer.* , Designation from entrer left join produit on produit.codepro=entrer.codepro;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                EntreStock es = new EntreStock();
                                es.IdEntrer = read.GetInt32(0);
                                es.Codepro = read.GetString(1);
                                es.Quantite = read.GetInt32(2);
                                es.date = read.GetDateTime(3);
                                es.Designation=read.GetString(4);
                                ListEntre.Add(es);
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

        public static List<EntreStock> GetAll() => ListEntre;

        public static void Create(EntreStock es)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "insert into entrer values(@idEntrer,@codepro,@qt,@date_entre);";
                                    
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@idEntrer", es.IdEntrer);
                        cmd.Parameters.AddWithValue("@codepro", es.Codepro);
                        cmd.Parameters.AddWithValue("@qt", es.Quantite);
                        cmd.Parameters.AddWithValue("@date_entre",es.date);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = ProduitService.Produits.FindIndex(produit => produit.Codepro == es.Codepro);
                if (index!=-1)es.Designation=ProduitService.Produits[index].Designation;
                ListEntre.Add(es); 

            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur d'entrer");
                Console.WriteLine(ex.Message);
            }
           
        }

        public static void Update(EntreStock es)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "UPDATE entrer SET codepro=@codepro, qt_entre=@qt, date_entrer=@date_commande WHERE idEntrer=@idEntrer;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idEntrer", es.IdEntrer);
                        cmd.Parameters.AddWithValue("@codepro", es.Codepro);
                        cmd.Parameters.AddWithValue("@qt", es.Quantite);
                        cmd.Parameters.AddWithValue("@date_commande", es.date);
                        cmd.ExecuteNonQuery();
                    }
                }

                var index = ListEntre.FindIndex(entrer => entrer.IdEntrer == entrer.IdEntrer);
                var i = ProduitService.Produits.FindIndex(produit => produit.Codepro == es.Codepro);
                if (i!=-1)es.Designation=ProduitService.Produits[i].Designation;
                if (index != -1)
                {
                    ListEntre[index] = es; // Mettre à jour la commande dans la liste locale
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la mise à jour de entre stock : " + ex.Message);
            }
        }

    // //Delete
        public static void Delete(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "delete from entrer where idEntrer=@idEntrer;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idEntrer", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                var entrer = ListEntre.FirstOrDefault(es => es.IdEntrer == id);
                if (entrer != null)
                {
                    ListEntre.Remove(entrer); // Supprimer list entrer de la liste locale
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
