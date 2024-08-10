using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Models.NombreTotal;
using API.Connection;

namespace Services.NombreTotal
{
    public class NombreTotalService
    {
        public static List<NombreStock> GetAll()
        {
            var nombreTotalStocks = new List<NombreStock>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "SELECT sum(p.prix_unitaire * c.quantite) as recette from produit p join  commande c on p.codepro=c.codepro ;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader read = cmd.ExecuteReader())
                        {
                            if (read.Read())
                            {
                                NombreStock stock = new NombreStock
                                {
                                    qteEntre = read.IsDBNull(0) ? 0 : read.GetInt32(0)
                                };
                                nombreTotalStocks.Add(stock);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return nombreTotalStocks;
        }
    }
}