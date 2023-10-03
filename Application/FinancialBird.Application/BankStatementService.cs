using FinancialBird.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application
{
    public class BankStatementService : IBankStatementService {

        private List<Buchung> buchungssaetze = new List<Buchung>();
        public Buchung[] Buchungsaetze => buchungssaetze.ToArray();

        private Dictionary<Kategory, BuchungsGruppierung> empfaengerGruppierung = new Dictionary<Kategory, BuchungsGruppierung>();

        public BankStatementService() {

        }

        public void ClearBuchungssaetze() {
            buchungssaetze.Clear();
        }

        public void ReadBuchungssaetze(string path, IBuchungsReader argBuchungsReader)
        {
            var lst = argBuchungsReader.Read(path);

            for(int i=0; i< lst.Length; i++)
            {
                buchungssaetze.Add(lst[i]);
            }
        }



        public void GruppierungNachEmpfaenger(Dictionary<Kategory, string[]> gruppierungsMap) {
            List<int> bereitsGespeichert = new List<int>();
            empfaengerGruppierung.Clear();

            foreach (var singleKategorie in gruppierungsMap) {

                BuchungsGruppierung gruppierung = new BuchungsGruppierung(singleKategorie.Key, Waehrung.EUR);

                for (int i=0; i< Buchungsaetze.Length; i++) {

                    if (bereitsGespeichert.Any(y => y==i)) { continue; }

                    if(singleKategorie.Value.ToList().Any(
                        empfaenger => Buchungsaetze[i].Empfaenger.ToLower().Contains(empfaenger.ToLower()))) {

                        gruppierung.Add(Buchungsaetze[i]);
                        bereitsGespeichert.Add(i);

                    }

                }

                this.empfaengerGruppierung.Add(gruppierung.Kategory, gruppierung);

            }

        }

        public BuchungsGruppierung GetGruppierung(Kategory category) {
            if(empfaengerGruppierung.TryGetValue(category, out var g)) {
                return g;
            }
            return new BuchungsGruppierung(category, Waehrung.EUR);
        }

        public void GruppierungNachEmpfaenger(Dictionary<string, string[]> gruppierungsMap) {
            List<int> bereitsGespeichert = new List<int>();
            empfaengerGruppierung.Clear();

            foreach (var singleKategorie in gruppierungsMap) {


                for (int i = 0; i < Buchungsaetze.Length; i++) {

                    if (bereitsGespeichert.Any(y => y == i)) { continue; }

                    if (singleKategorie.Value.ToList().Any(empfaenger 
                        => Buchungsaetze[i].Empfaenger.ToLower().Contains(empfaenger.ToLower())
                        )) {

                        Buchungsaetze[i].Kategorie = singleKategorie.Key;
                        bereitsGespeichert.Add(i);

                    }

                }
            }

            for (int i = 0; i < Buchungsaetze.Length; i++) {
                if (Buchungsaetze[i].Kategorie == string.Empty) {
                    Buchungsaetze[i].Kategorie = Kategory.Sonstiges.ToString();
                }
            }

        }

        public Buchung[] GetGruppierung(string kategory) {

            var query = from b in buchungssaetze
                        where b.Kategorie == kategory
                        select b;

            return query.ToArray();

        }

        public double GetBetrag(Buchung[] buchungs, Waehrung waehrung) {

            Geldbetrag betrag = new Geldbetrag(0.0, waehrung);
            foreach (var b in buchungs) {
                betrag.Addition(b.Geldbetrag.Betrag, b.Geldbetrag.Waehrung);
            }

            return betrag.Betrag;
        }
    }
}
