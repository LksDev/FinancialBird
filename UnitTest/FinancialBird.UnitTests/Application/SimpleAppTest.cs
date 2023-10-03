using FinancialBird.Application;
using FinancialBird.Application.Entities;
using FinancialBird.VereinigteVolksbank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialBird.UnitTests.Application {

    public class SimpleAppTest {

        [SetUp]
        public void SetUp() { 
        
        }

        [Test] 
        public void ReadBuchungssaetze() {

            IBankStatementService app = new BankStatementService();

            string csvFile = @"..\..\..\Application\TestData\Kontoauszüge_csv\Umsaetze_DummyDaten.csv";
            app.ReadBuchungssaetze(csvFile, new VereinigteVolksbank.CSVMapper());

            Assert.Pass();
        }

        [TestCase(@"..\..\..\Application\TestData\Kontoauszüge_csv\Umsaetze_DummyDaten.csv")]

        public void ReadBuchungssaetzeGruppieren(string csvFile) {

            var filename = Path.GetFileName(csvFile);


            Dictionary<Kategory, string[]> groupMap = new Dictionary<Kategory, string[]>();
            groupMap.Add(Kategory.Lebensmittel, new string[] { "edeka", "rossmann", "aldi", "rewe", "lidl", "marktkauf" });
            groupMap.Add(Kategory.Apotheke, new string[] { "apotheke" });
            groupMap.Add(Kategory.Gehalt, new string[] { "arbeitgeber" });
            groupMap.Add(Kategory.Tanken, new string[] { "Tanken", "Tankstelle", "tank" });
            groupMap.Add(Kategory.PayPal, new string[] { "PayPal" });

            IBankStatementService app = new BankStatementService();
            app.ReadBuchungssaetze(csvFile, new VereinigteVolksbank.CSVMapper());
            app.GruppierungNachEmpfaenger(groupMap);
            var lebensmittel = app.GetGruppierung(Kategory.Lebensmittel);
            var apotheke = app.GetGruppierung(Kategory.Apotheke);
            var gehalt = app.GetGruppierung(Kategory.Gehalt);
            var tanken = app.GetGruppierung(Kategory.Tanken);
            var paypal = app.GetGruppierung(Kategory.PayPal);



            string result =
                $"{filename}:\n" +
                $"{lebensmittel}\n" +
                $"{apotheke}\n"+
                $"{gehalt}\n" +
                $"{tanken}\n" +
                $"{paypal}";

            Assert.Pass(result);
        }


        [TestCase(@"..\..\..\Application\TestData\Kontoauszüge_csv\Umsaetze_DummyDaten.csv")]

        public void ReadBuchungssaetzeGruppierenNachStringKategorie(string csvFile) {

            var filename = Path.GetFileName(csvFile);


            Dictionary<string, string[]> groupMap = new Dictionary<string, string[]>();
            groupMap.Add(Kategory.Lebensmittel.ToString(), new string[] { "edeka", "rossmann", "aldi", "rewe", "lidl", "marktkauf" });
            groupMap.Add(Kategory.Apotheke.ToString(), new string[] { "apotheke" });
            groupMap.Add(Kategory.Gehalt.ToString(), new string[] { "arbeitgeber" });
            groupMap.Add(Kategory.Tanken.ToString(), new string[] { "Tanken", "Tankstelle", "tank" });
            groupMap.Add(Kategory.PayPal.ToString(), new string[] { "PayPal" });

            IBankStatementService app = new BankStatementService();
            app.ReadBuchungssaetze(csvFile, new VereinigteVolksbank.CSVMapper());
            app.GruppierungNachEmpfaenger(groupMap);
            var lebensmittel = app.GetGruppierung(Kategory.Lebensmittel.ToString());
            var apotheke = app.GetGruppierung(Kategory.Apotheke.ToString());
            var gehalt = app.GetGruppierung(Kategory.Gehalt.ToString());
            var tanken = app.GetGruppierung(Kategory.Tanken.ToString());
            var paypal = app.GetGruppierung(Kategory.PayPal.ToString());

            ;

            string result =
                $"{filename}:\n" +
                $"{app.GetBetrag(lebensmittel, Waehrung.EUR)}\n" +
                $"{app.GetBetrag(apotheke, Waehrung.EUR)}\n" +
                $"{app.GetBetrag(tanken, Waehrung.EUR)}\n" +
                $"{app.GetBetrag(paypal, Waehrung.EUR)}";

            Assert.Pass(result);
        }

    }
}
