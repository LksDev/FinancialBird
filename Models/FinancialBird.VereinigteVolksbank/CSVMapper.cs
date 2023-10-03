using FinancialBird.Application;
using FinancialBird.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.VereinigteVolksbank
{

    public interface IUmsatzMapper
    {
        public UmsatzEntry[] FileReader(string filepath);
    }

    public class CSVMapper : IUmsatzMapper, IBuchungsReader
    {
        public UmsatzEntry[] FileReader(string filepath)
        {
            var umsaetze = new List<UmsatzEntry>();

            using (StreamReader sr = new StreamReader(filepath))
            {
                string line;
                uint i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if(i == 0) {
                        i++;
                        continue;
                    }
                    var column = line.Split(';');
                    if (column.Length <= 0) { continue; }

                    UmsatzEntry readUmsatz = new UmsatzEntry()
                    {
                        BezeichnungAuftragskonto = column[0],
                        IBANAuftragskonto = column[1],
                        BICAuftragskonto = column[2],
                        BanknameAuftragskonto = column[3],
                        Buchungstag = column[4],
                        Valutadatum = column[5],
                        NameZahlungsbeteiligter = column[6],
                        IBANZahlungsbeteiligter = column[7],
                        BICSWIFT_CodeZahlungsbeteiligter = column[8],
                        Buchungstext = column[9],
                        Verwendungszweck = column[10],
                        Betrag = column[11],
                        Waehrung = column[12],
                        SaldonachBuchung = column[13],
                        Bemerkung = column[14],
                        Kategorie = column[15],
                        Steuerrelevant = column[16],
                        GlaeubigerID = column[17],
                        Mandatsreferenz = column[18]
                    };

                    umsaetze.Add(readUmsatz);
                    i++;
                }
            }

            return umsaetze.ToArray();

        }

        public Buchung[] Read(string file) {
            var umsaetze = this.FileReader(file);


            List<Buchung> buchungen = new List<Buchung>();

            foreach(var umsatz in umsaetze) {
                // ToDo: Waehrung ermitteln

                buchungen.Add(new Buchung() {
                    Buchungsdatum = DateTime.Parse(umsatz.Buchungstag),
                    Absender = $"{umsatz.IBANAuftragskonto}",
                    Empfaenger = umsatz.NameZahlungsbeteiligter,
                    IBANEmpfaenger = umsatz.IBANZahlungsbeteiligter,
                    Verwendungszweck = umsatz.Verwendungszweck,
                    Geldbetrag = new Geldbetrag(double.Parse(umsatz.Betrag), Waehrung.EUR)
                });

            }


            return buchungen.ToArray();

        }


    }
}
