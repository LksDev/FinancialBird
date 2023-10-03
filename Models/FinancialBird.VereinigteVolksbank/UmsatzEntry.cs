using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.VereinigteVolksbank
{
    public class UmsatzEntry
    {
        public string BezeichnungAuftragskonto { get; set; } = string.Empty;
        public string IBANAuftragskonto { get; set; } = string.Empty;
        public string BICAuftragskonto { get; set; } = string.Empty;
        public string BanknameAuftragskonto { get; set; } = string.Empty;
        public string Buchungstag { get; set; } = string.Empty;
        public string Valutadatum { get; set; } = string.Empty;
        public string NameZahlungsbeteiligter { get; set; } = string.Empty;
        public string IBANZahlungsbeteiligter { get; set; } = string.Empty;
        public string BICSWIFT_CodeZahlungsbeteiligter { get; set; } = string.Empty;
        public string Buchungstext { get; set; } = string.Empty;
        public string Verwendungszweck { get; set; } = string.Empty;
        public string Betrag { get; set; } = string.Empty;
        public string Waehrung { get; set; } = string.Empty;
        public string SaldonachBuchung { get; set; } = string.Empty;
        public string Bemerkung { get; set; } = string.Empty;
        public string Kategorie { get; set; } = string.Empty;
        public string Steuerrelevant { get; set; } = string.Empty;
        public string GlaeubigerID { get; set; } = string.Empty;
        public string Mandatsreferenz { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is UmsatzEntry entry &&
                   BezeichnungAuftragskonto == entry.BezeichnungAuftragskonto &&
                   IBANAuftragskonto == entry.IBANAuftragskonto &&
                   BICAuftragskonto == entry.BICAuftragskonto &&
                   BanknameAuftragskonto == entry.BanknameAuftragskonto &&
                   Buchungstag == entry.Buchungstag &&
                   Valutadatum == entry.Valutadatum &&
                   NameZahlungsbeteiligter == entry.NameZahlungsbeteiligter &&
                   IBANZahlungsbeteiligter == entry.IBANZahlungsbeteiligter &&
                   BICSWIFT_CodeZahlungsbeteiligter == entry.BICSWIFT_CodeZahlungsbeteiligter &&
                   Buchungstext == entry.Buchungstext &&
                   Verwendungszweck == entry.Verwendungszweck &&
                   Betrag == entry.Betrag &&
                   Waehrung == entry.Waehrung &&
                   SaldonachBuchung == entry.SaldonachBuchung &&
                   Bemerkung == entry.Bemerkung &&
                   Kategorie == entry.Kategorie &&
                   Steuerrelevant == entry.Steuerrelevant &&
                   GlaeubigerID == entry.GlaeubigerID &&
                   Mandatsreferenz == entry.Mandatsreferenz;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{this.Buchungstag} {this.NameZahlungsbeteiligter} {this.Betrag}";
        }
    }
}
