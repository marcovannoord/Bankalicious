using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class Rekening
    {
        public int RekeningId { get; set; }
        public string RekeningNaam { get; set; }
        public string Beschrijving { get; set; }
        public double Saldo { get; set; }


        public int KlantId { get; set; }
        public virtual Klant Klant { get; set; }

    }
}