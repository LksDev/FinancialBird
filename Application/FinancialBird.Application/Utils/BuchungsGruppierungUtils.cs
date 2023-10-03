using FinancialBird.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application.Utils
{
    public class BuchungsGruppierungUtils
    {

        public BuchungsGruppierung QuerySameMonth(BuchungsGruppierung gruppierung, DateTime month)
        {

            var newGruppierung = new BuchungsGruppierung(gruppierung.Kategory, gruppierung.Summe.Waehrung); ;

            for (int i = 0; i < gruppierung.Buchungssaetze.Length; i++)
            {
                if (month.Month == gruppierung.Buchungssaetze[i].Buchungsdatum.Month)
                {
                    newGruppierung.Add(gruppierung.Buchungssaetze[i]);
                }
            }

            return newGruppierung;
        }

        public BuchungsGruppierung QuerySpanMonth(BuchungsGruppierung gruppierung, DateTime from, DateTime to) {

            var newGruppierung = new BuchungsGruppierung(gruppierung.Kategory, gruppierung.Summe.Waehrung); ;

            for (int i = 0; i < gruppierung.Buchungssaetze.Length; i++) {

                if (gruppierung.Buchungssaetze[i].Buchungsdatum >= from
                    && gruppierung.Buchungssaetze[i].Buchungsdatum <= to ) {
                    newGruppierung.Add(gruppierung.Buchungssaetze[i]);
                }

            }

            return newGruppierung;
        }

        public Buchung[] Filter(DateTime from, DateTime to, Buchung[] buchungen) {
            var filter = new List<Buchung>();

            for (int i = 0; i < buchungen.Length; i++) {

                if (buchungen[i].Buchungsdatum >= from && buchungen[i].Buchungsdatum <= to) {
                    filter.Add(buchungen[i]);
                }

            }

            return filter.ToArray();
        }

    }
}
