using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Connection;
using API.Models.Facture;
using API.Models.Commande;
using Services.client;
using Services.Produit;

namespace Services.Facture
{
    public class FactureService
    {
        public static List<FactureClient> Factures { get; } = new List<FactureClient>();

        static FactureService()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "SELECT commande.idCommande, client.nomClient, produit.designation, commande.quantite, produit.prix_unitaire, commande.date FROM client, produit, commande WHERE commande.Codepro = produit.Codepro AND client.idclient = commande.idclient;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                FactureClient facture = new FactureClient
                                {
                                    Idcommande=read.GetInt32(0),
                                    nom = read.GetString(1),
                                    designation = read.GetString(2),
                                    Quantite = read.GetInt32(3),
                                    Prix_unitaire = read.GetInt32(4),
                                    date = read.GetDateTime(5)
                                };

                                Factures.Add(facture);
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
        public static List<FactureClient> GetAll() => Factures;

        
        public static void setListFacture(CommandeStock cs)
        {
            Factures.Add(setFacture(cs));
        }
        public static void UpdateListFacture(CommandeStock cs)
        {
                FactureClient fc = new FactureClient();
                fc=setFacture(cs);
                var index = Factures.FindIndex(facture=>facture.Idcommande==cs.Idcommande);
                if (index!=-1)Factures[index]=fc;

        }
        public static void DeleteFacture(int id)
        {
            var index = Factures.FindIndex(f=>f.Idcommande==id);
            if(index!=-1)Factures.Remove(Factures[index]);
        }

        public static FactureClient setFacture(CommandeStock cs)
        {
            FactureClient facture = new FactureClient();
            facture.Idcommande=cs.Idcommande;
            facture.date=cs.date;
            facture.designation=cs.Designation;
            facture.Quantite=cs.Quantite;

            var index = ClientService.Clients.FindIndex(client =>client.idclient==cs.Idclient);
            facture.nom=ClientService.Clients[index].nomClient;
            var indexPro=ProduitService.Produits.FindIndex(produit=>produit.Codepro==cs.Codepro);
            facture.Prix_unitaire=ProduitService.Produits[indexPro].Prix_unitaire;
            return facture;

        }
    }
}
