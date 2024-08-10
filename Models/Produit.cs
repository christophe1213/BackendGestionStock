namespace API.Models.Produit
{
    public class ProduitStock
    {
        public string Codepro { get; set; } = "";
        public string NumImm { get; set; } = "";
        public string Designation { get; set; } = "";
        public string Categorie { get; set; } = "";
    
        public int Prix_unitaire { get; set; } = 0;
        public int Qte_produit{get;set;}=0;
        public string NomFourn { get; set; } = "";
    }
}
