using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankClientApplication
{
    public class Klant
    {
        
        public int KlantId { get; set; }
        public string Name { get; set; }
        public string Beschrijving { get; set; }
        public string liefheid { get; set; }
        public virtual ICollection<Rekening> Rekeningen { get; set; }
    }
}