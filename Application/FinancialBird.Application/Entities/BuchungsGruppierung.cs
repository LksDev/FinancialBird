using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application.Entities
{
    public class BuchungsGruppierung
    {
        public Kategory Kategory { get; private set; }
        
        public Geldbetrag Summe { get; private set; }

        private List<Buchung> buchungssaetze = new List<Buchung>();

        public BuchungsGruppierung(Kategory kategory, Waehrung waehrung) { 
            this.Kategory = kategory;
            Summe = new Geldbetrag(0.0, waehrung);
        }


        public void Add(Buchung buchung)
        {
            Summe.Addition(buchung.Geldbetrag.Betrag, buchung.Geldbetrag.Waehrung);
            // Umrechnung von Doller <-> EUR
            buchungssaetze.Add(buchung);
        }

        public Buchung[] Buchungssaetze => this.buchungssaetze.ToArray();

        public override bool Equals(object? obj) {
            return base.Equals(obj);
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string? ToString() {
            return $"{this.Kategory}: {this.Summe}";
        }
    }
}
