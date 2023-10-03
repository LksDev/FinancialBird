using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application.Entities
{
    public class Buchung
    {
        public string IBANEmpfaenger { get; set; }
        public string Absender { get; set; }
        public string Empfaenger { get; set; }
        public string Verwendungszweck { get; set; }
        public DateTime Buchungsdatum { get; set; }
        public Geldbetrag Geldbetrag { get; set; }

        public string Kategorie { get; set; } = string.Empty;

        public override bool Equals(object? obj) {

            if (obj == null) return false;
            if (obj is not Buchung) return false;
            if (ReferenceEquals(this, obj)) return true;

            return this.GetHashCode().Equals(obj.GetHashCode());
        }

        public override int GetHashCode() {
            return IBANEmpfaenger.GetHashCode() 
                + Absender.GetHashCode()
                + Empfaenger.GetHashCode()
                + Verwendungszweck.GetHashCode()
                + Buchungsdatum.GetHashCode()
                + Geldbetrag.GetHashCode(); 
        }

        public override string ToString() {
            return $"{this.Buchungsdatum:dd.MMMM yyyy} - {this.Absender} -> {this.Empfaenger}: {this.Geldbetrag}";
        }
    }

    public class Geldbetrag
    {
        public double Betrag { get; private set; }
        public Waehrung Waehrung { get; }

        public Geldbetrag(double betrag, Waehrung waehrung)
        {
            Betrag = betrag;
            Waehrung = waehrung;
        }

        public void Addition(Geldbetrag betrag) {
            Addition(betrag.Betrag, betrag.Waehrung);
        }

        public void Addition(double betrag, Waehrung waehrung)
        {
            double addBetrag = 0.0;
            if (this.Waehrung.Equals(waehrung))
            {
                addBetrag = betrag;
            } else
            {

            }

            Betrag += addBetrag;

        }

        public void Subtration(double betrag, Waehrung waehrung)
        {

            double subBetrag = 0.0;
            if (this.Waehrung.Equals(waehrung))
            {
                subBetrag = betrag;
            }
            else
            {

            }

            Betrag -= subBetrag;
        }

        public override int GetHashCode() {
            return this.Waehrung.GetHashCode() + Betrag.GetHashCode(); 
        }

        public override string ToString() {
            return $"{Math.Round(this.Betrag, 2)} {Waehrung}";
        }

    }

    public enum Waehrung
    {
        EUR = 0,
        DOLLER = 1
    }
}
