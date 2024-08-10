namespace API.Models.Commande
{
    public class CommandeStock
    {
        public int Idcommande { get; set; } = 0;
        public string Idclient { get; set; } = "";
        public string Codepro { get; set; } = "";
        public int Quantite { get; set; } = 0;
        public DateTime date { get; set; }
        public string Designation {get; set;}="";   
        public void print(){
            Console.WriteLine(Idcommande);
            Console.WriteLine(Idclient);
            Console.WriteLine(Codepro);
            Console.WriteLine(Quantite);
            Console.WriteLine(date);
        }
    }
}
