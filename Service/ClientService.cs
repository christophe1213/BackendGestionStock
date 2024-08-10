using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using API.Models.client;
using API.Connection;
using Services.Facture;
namespace Services.client
{
    public class ClientService
    {
        public static List<ClientStock> Clients { get; }=[];
        static ClientService()
        {
       

            try{
                using (MySqlConnection conn= new MySqlConnection(Connection.setting)){
                        conn.Open();
                        string query="SELECT * FROM client;";
                        using (MySqlCommand cmd = new MySqlCommand(query,conn)){
                            using(MySqlDataReader read=cmd.ExecuteReader()){
                                while(read.Read()){
                                    ClientStock c= new ClientStock();
                                    c.idclient=read.GetString(0);
                                    c.nomClient=read.GetString(1);
                                    c.adresse=read.GetString(2);
                                    c.numtel=read.GetString(3);
                                    Clients.Add(c);
            
                                }

                            }
                        }
                }
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
                   ClientStock c= new ClientStock();
                                    // c.idclient=1;
                                    // c.nomClient="df";
                                    // c.adresse="fd";
                                    // c.numtel="fd";
                                    // Clients.Add(c);
            }
           
        }
        public static List<ClientStock> GetAll() => Clients;
        //Get un seul element ,ilaina rehefa manao modifier 
        public static ClientStock Get(string id)
        {
            ClientStock c= new ClientStock();
            try{
                using (MySqlConnection conn= new MySqlConnection(Connection.setting)){
                        conn.Open();
                        string query = "SELECT * FROM client WHERE idclient = @idclient;";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@idclient", id);
                            using (MySqlDataReader read = cmd.ExecuteReader())
                            {
                            if (read.Read())
                                {
                                    c.idclient = read.GetString(0);
                                    c.nomClient = read.GetString(1);
                                    c.adresse = read.GetString(2);
                                    c.numtel = read.GetString(3);
                                }
                            }
                        }
                }
                
                        
                
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
                
            }
            return c;
        }
        //Insertion
          public static void Create(ClientStock c)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "INSERT INTO client  VALUES (@idclient,@nomClient, @adresse, @numtel);";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {   
                        cmd.Parameters.AddWithValue("@idclient",c.idclient);
                        cmd.Parameters.AddWithValue("@nomClient", c.nomClient);
                        cmd.Parameters.AddWithValue("@adresse", c.adresse);
                        cmd.Parameters.AddWithValue("@numtel", c.numtel);
                        cmd.ExecuteNonQuery();
                    }
                }
                Clients.Add(c); // Ajouter le client à la liste locale après l'insertion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //Mise à jour
         public static void Update(ClientStock c)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "UPDATE client SET nomClient = @nomClient, adresse = @adresse, numtel = @numtel WHERE idclient = @idclient;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idclient", c.idclient);
                        cmd.Parameters.AddWithValue("@nomClient", c.nomClient);
                        cmd.Parameters.AddWithValue("@adresse", c.adresse);
                        cmd.Parameters.AddWithValue("@numtel", c.numtel);
                        cmd.ExecuteNonQuery();
                    }
                }
                var index = Clients.FindIndex(client => client.idclient == c.idclient);
                if (index != -1)
                {

                    foreach(var f in FactureService.Factures.Where(f =>f.nom==Clients[index].nomClient))
                    {
                        f.nom=c.nomClient;
                    }
        
                    Clients[index] = c; // Mettre à jour le client dans la liste locale
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
//Delete
        public static void Delete(string id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Connection.setting))
                {
                    conn.Open();
                    string query = "DELETE FROM client WHERE idclient = @idclient;";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@idclient", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                var client = Clients.FirstOrDefault(c => c.idclient == id);
                if (client != null)
                {
                    Clients.Remove(client); // Supprimer le client de la liste locale
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
    }
    }

}