namespace API.Models.FicheStock
{
    public class MouvStock
    {
        public int qt { get; set; } = 0;
        public int pu { get; set; } = 0;
        public int montant => qt * pu; // Propriété calculée

        public MouvStock() { }
    }

    public class ActionStock
    {
        public DateTime date { get; set; }
        public string libelle { get; set; } = "";
        public MouvStock entrer { get; set; } = new MouvStock();
        public MouvStock sorti { get; set; } = new MouvStock();
        public MouvStock etat_stock { get; set; } = new MouvStock();
    }

    public class SuiviStock
    {
        public string design { get; set; } = "";
        public List<ActionStock> mouvement { get; } = new List<ActionStock>();
    }
    public class ActionStockDateComparer : IComparer<ActionStock>
    {
        public int Compare(ActionStock x, ActionStock y)
        {
            if (x == null || y == null)
                return 0;

            return DateTime.Compare(x.date, y.date);
        }
    }
}