namespace API.Models.Facture
{
    public class FactureClient
    {
        public int Idcommande { get; set;}=0;
        public string nom { get; set; } = "";
        public string designation { get; set; } = "";
        public int Quantite { get; set; } = 0; 
        public int Prix_unitaire { get; set; } = 0;
        public DateTime date { get; set; }
        
        public void Print()
        {
            Console.WriteLine(Quantite);
            Console.WriteLine(date);
        }
    }
}
