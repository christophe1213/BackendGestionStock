using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Models.Fournisseur;
using API.Connection;

namespace Services.Fournisseur
{
    public class FournisseurService
    {
        public static List<FournisseurStock> Fournisseurs { get; } = new List<FournisseurStock>();

        static FournisseurService()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "SELECT * FROM fournisseur;"; // Changer le nom de la table si nécessaire
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                FournisseurStock f = new FournisseurStock();
                                f.NumImm = read.GetString(0);
                                f.NomFourn = read.GetString(1);
                                f.Adresse = read.GetString(2);
                                f.Contact = read.GetString(3);
                                Fournisseurs.Add(f);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Gérer l'exception selon vos besoins
            }
        }

        public static List<FournisseurStock> GetAll() => Fournisseurs;

        //Get un seul id
        public static FournisseurStock  Get(string numim)
        {
            FournisseurStock f = new FournisseurStock();
            try{
                 using (MySqlConnection conn= new MySqlConnection(Connection.setting)){
                        conn.Open();
                        string query = "SELECT * FROM fournisseur WHERE  NumImm= @numim;";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@numim", numim);
                            using (MySqlDataReader read = cmd.ExecuteReader())
                            {
                            if (read.Read())
                                {
                                f.NumImm = read.GetString(0);
                                f.NomFourn = read.GetString(1);
                                f.Adresse = read.GetString(2);
                                f.Contact = read.GetString(3);
                                }
                            }
                        }
                }

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return f;
        }

        //Create
        public static void Create(FournisseurStock f)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "INSERT INTO fournisseur  VALUES (@numim,@nomfournisseur, @adresse, @contact);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@numim",f.NumImm);
                        cmd.Parameters.AddWithValue("@nomfournisseur",f.NomFourn);
                        cmd.Parameters.AddWithValue("@adresse", f.Adresse);
                        cmd.Parameters.AddWithValue("@contact", f.Contact);
                        cmd.ExecuteNonQuery();
                    }
                }
                Fournisseurs.Add(f); // Ajouter le client à la liste locale après l'insertion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public static void Update(FournisseurStock f)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "UPDATE fournisseur SET NomFourn = @nomfournisseur, Adresse = @adresse, Contact = @contact WHERE NumImm = @numim;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                       
                        cmd.Parameters.AddWithValue("@numim",f.NumImm);
                        cmd.Parameters.AddWithValue("@nomfournisseur",f.NomFourn);
                        cmd.Parameters.AddWithValue("@adresse", f.Adresse);
                        cmd.Parameters.AddWithValue("@contact", f.Contact);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = Fournisseurs.FindIndex(fournisseur => fournisseur.NumImm == f.NumImm);
                if (index != -1)
                {
                    Fournisseurs[index] = f; // Mettre à jour le client dans la liste locale
                }
            }catch(Exception ex)
            {
                Console.WriteLine("Erreur de mise à jour"+ex.Message);

            }
        }

        public static void Delete(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "DELETE FROM fournisseur where NumImm=@numim ;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@numim", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                var fournisseur = Fournisseurs.FirstOrDefault(f => f.NumImm == id);
                if (fournisseur != null)
                {
                    Fournisseurs.Remove(fournisseur); // Supprimer le client de la liste locale
                }
            }catch(Exception ex)
            {

                Console.WriteLine("Erreur de suppression du fournisseur "+ex.Message);
            }
        }
    }

}
