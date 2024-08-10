using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.EntrerStock
{
    public class EntreStock
    {
        public int IdEntrer { get; set; } = 0;
        public string Codepro { get; set; } = "";
        public int Quantite { get; set; } = 0;
        public DateTime date { get; set; }
        public string Designation { get; set; } = "";

    }
}