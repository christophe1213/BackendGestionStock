using API.Models.FicheStock;
using API.Models.Commande;
using API.Models.Produit;
using Services.Produit;
using Services.Commande;
using Services.Entre;
using  API.Models.EntrerStock;

namespace Services.suviStock
{
    public class ServicesFicheStock
    {
        public static List<SuiviStock> ListSuivi { get; } = new List<SuiviStock>();
        public static List<ActionStock> l { get; } = new List<ActionStock>();

        static ServicesFicheStock()
        {
            ActionStock a = new ActionStock();
            ActionStock b = new ActionStock();
            ActionStock c = new ActionStock();
            SuiviStock F = new SuiviStock();
            SuiviStock G = new SuiviStock();


            F.design = "Tomates";
            a.date = new DateTime(2024, 11, 02);
            b.date = new DateTime(2023, 12, 02);
            a.etat_stock.pu = 12;
            a.etat_stock.qt = 1;
            b.entrer.qt = 1;
            b.entrer.pu = 2;
            F.mouvement.Add(a);
            F.mouvement.Add(b);
            F.mouvement.Sort(new ActionStockDateComparer());

            G.design = "farine";
            G.mouvement.Add(a);
            G.mouvement.Add(b);
            
            
            ListSuivi.Add(F);
            ListSuivi.Add(G);
            
            l.Add(a);
        }

        public static List<SuiviStock> GetAll() => ListSuivi;

        
    }
}
