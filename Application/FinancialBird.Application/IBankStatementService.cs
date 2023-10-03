using FinancialBird.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.Application
{
    public interface IBankStatementService
    {

        /// <summary>
        /// Get alle Buchungen
        /// </summary>
        public Buchung[] Buchungsaetze { get; }

        /// <summary>
        /// Einlesen einer Datei die Buchungen enthält
        /// </summary>
        /// <param name="path"></param>
        public void ReadBuchungssaetze(string path, IBuchungsReader argBuchungsReader);

        public void ClearBuchungssaetze();

        /// <summary>
        /// Gruppierungsprozess nach Empfänger starten
        /// </summary>
        /// <param name="gruppierungsMap"></param>
        public void GruppierungNachEmpfaenger(Dictionary<Kategory, string[]> gruppierungsMap);

        /// <summary>
        /// Gruppierungsprozess nach Empfänger starten
        /// </summary>
        /// <param name="gruppierungsMap"></param>
        public void GruppierungNachEmpfaenger(Dictionary<string, string[]> gruppierungsMap);

        /// <summary>
        /// Ausgabe des Gruppierungsprozess
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public BuchungsGruppierung GetGruppierung(Kategory category);

        /// <summary>
        /// Ausgabe des Gruppierungsprozess
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public Buchung[] GetGruppierung(string kategory);

        public double GetBetrag(Buchung[] buchungs, Waehrung waehrung);

    }

    
}
