using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Services.Fournisseur;
using API.Models.Produit;
using API.Connection;
using Services.Entre;
using  API.Models.EntrerStock;
using API.Models.Commande;
using Services.Commande;
using Services.Facture;

namespace Services.Produit
{
    public class ProduitService
    {
        public static List<ProduitStock> Produits { get; } = new List<ProduitStock>();

        static ProduitService()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "select produit.*,coalesce(e.entrer_total,0)-coalesce(s.sortie_total,0) as qt_stock,f.NomFourn from produit "
                                   +"left join(select codepro,sum(qt_entre) as entrer_total from entrer group by codepro) e on produit.codepro=e.codepro "
                                   +"left join(select codepro,sum(Quantite) as sortie_total from commande group by codepro)s on produit.codepro=s.codepro "
                                   +"left join fournisseur f ON produit.NumImm = f.NumImm;"; 
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                ProduitStock p = new ProduitStock();
                                p.Codepro = read.GetString(0);
                                p.NumImm = read.GetString(1);
                                p.Designation = read.GetString(2);
                                p.Categorie = read.GetString(3);
                                p.Prix_unitaire = read.GetInt32(4);
                                p.Qte_produit=read.GetInt32(5);
                                p.NomFourn=read.GetString(6);
                                Produits.Add(p);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
        }
    
        public static List<ProduitStock> GetAll() => Produits;
        
        //Insertion du produit
        public static void Create(ProduitStock p)
        {
            Console.WriteLine("J'suis là");
            try
            {
                
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "INSERT INTO produit  VALUES (@codepro,@numim, @desingnation,@categorie,@pu);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@codepro", p.Codepro);
                        cmd.Parameters.AddWithValue("@numim",p.NumImm);
                        cmd.Parameters.AddWithValue("@desingnation",p.Designation);
                        cmd.Parameters.AddWithValue("@categorie",p.Categorie);
                        cmd.Parameters.AddWithValue("@pu",p.Prix_unitaire);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = FournisseurService.Fournisseurs.FindIndex(fournisseur => fournisseur.NumImm == p.NumImm);
                p.NomFourn=FournisseurService.Fournisseurs[index].NomFourn;
                Produits.Add(p); // Ajouter le produit à la liste locale après l'insertion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Il y a une erreur  d'ajout produit "+ex.Message);
                Console.WriteLine(ex.Message);
            } 

        }
        //Modifier la table produit
        public static void Update(ProduitStock p)
        { 
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "UPDATE produit SET designation = @desingnation,categorie=@categorie, NumImm= @numim, Prix_unitaire=@pu  WHERE codepro = @code_pro;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code_pro", p.Codepro);
                        cmd.Parameters.AddWithValue("@numim",p.NumImm);
                        cmd.Parameters.AddWithValue("@desingnation",p.Designation);
                        cmd.Parameters.AddWithValue("@categorie",p.Categorie);
                        cmd.Parameters.AddWithValue("@pu",p.Prix_unitaire);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = Produits.FindIndex(produit => produit.Codepro == p.Codepro);
                var index2 = FournisseurService.Fournisseurs.FindIndex(fournisseur => fournisseur.NumImm == p.NumImm);
                p.NomFourn=FournisseurService.Fournisseurs[index2].NomFourn;
                if (index != -1)
                {
                    foreach(var f in FactureService.Factures.Where(f =>f.designation==Produits[index].Designation))
                    {
                        f.designation=p.Designation;
                    }
                    Produits[index] = p; // Mettre à jour le produit dans la liste locale
                        foreach (var entrer in EntreService.ListEntre.Where(e => e.Codepro == p.Codepro))
                    {
                        
                        entrer.Designation= p.Designation;
                    }
                    foreach (var commande in CommandeService.Commandes.Where(c => c.Codepro == p.Codepro))
                    {
                        
                        commande.Designation= p.Designation;
                    }
                }

                

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }

        //Delete produit

        public static void Delete(string id)
        {
            try
            {
                 using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "DELETE FROM produit WHERE codepro = @code_pro;"
                                   +"DELETE FROM commande WHERE codepro=@code_pro;"
                                   +"DELETE FROM entrer WHERE codepro=@code_pro;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@code_pro", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                var produit = Produits.FirstOrDefault(p => p.Codepro == id);
                if (produit != null)
                {
                    Produits.Remove(produit); // Supprimer le produit de la liste locale
                }
                
                foreach (var entrer in EntreService.ListEntre.Where(e => e.Codepro == id))
                {
                    
                    EntreService.ListEntre.Remove(entrer);
                }
                foreach (var commande in CommandeService.Commandes.Where(c => c.Codepro == id))
                {
                    
                    CommandeService.Commandes.Remove(commande);
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

}
