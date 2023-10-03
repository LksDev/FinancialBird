using FinancialBird.Application;
using FinancialBird.Application.Entities;
using FinancialBird.Application.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Shapes;

namespace FinancialBird
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IBankStatementService bankStatementService;
        Dictionary<Kategory, string[]> mapKategory = new Dictionary<Kategory, string[]>();
        Dictionary<string, string[]> mapKategoryV2 = new Dictionary<string, string[]>();


        public MainWindow()
        {
            InitializeComponent();
            bankStatementService = new BankStatementService();

            // Init
            ReadKategorien($"{Environment.CurrentDirectory}\\configKategorie.txt");
            UpdateKategorie();
            
            toDate.SelectedDate = new DateTime(2023,9,30);
            fromDate.SelectedDate= new DateTime(2023, 9, 1);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            MessageBox.Show($"{((Exception)e.ExceptionObject).Message}", $"Oh je... Absturz!", MessageBoxButton.OK, MessageBoxImage.Error);
            
        }

        private void btnReadBankStatements_Click(object sender, RoutedEventArgs e) {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "CSV (*.csv)|*.csv|All files(*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            
            if (openFileDialog.ShowDialog() == true) {
                foreach (string filename in openFileDialog.FileNames) {
                    ReadBankStatementFile(this.bankStatementService, filename);

                }
            }

        }


        private void ReadBankStatementFile(IBankStatementService service, string file) {

            service.ReadBuchungssaetze(file, new VereinigteVolksbank.CSVMapper());

        }

        private void ReadKategorien(string categoryFile) {
            mapKategory.Clear();

            using (StreamReader reader = new StreamReader(categoryFile)) {

                string line;

                while((line = reader.ReadLine()) != null) {

                    var dataLine = line.Split(";");
                    List<string> names = new List<string>();

                   

                    for (int i = 1; i < dataLine.Length; i++) {
                        names.Add(dataLine[i]);

                    }

                    mapKategoryV2.Add(dataLine[0], names.ToArray());

                    if (Enum.TryParse<Kategory>(dataLine[0], out var category)) {
                    } else { continue; }

                    if (!mapKategory.ContainsKey(category)) {
                        mapKategory.Add(category, names.ToArray());
                    }

                }

            }

        }

        private void UpdateKategorie() {

            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Alle");

            foreach (var category in mapKategoryV2) {
                cmbCategory.Items.Add(category.Key);
            }

        }


        private bool CheckDateInput() {

            if (fromDate.SelectedDate is DateTime from && toDate.SelectedDate is DateTime to
                && !ValidDateInput(from, to)) {

                MessageBox.Show("Eingabe des Zeibereiches überprüfen.", "Eingabe fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnUpdateDiagram_Click(object sender, RoutedEventArgs e) {
            
            if(CheckDateInput()) {
                return;
            }

            /*
            Kategory selectCategory = Kategory.None;
            if(!Enum.TryParse<Kategory>(cmbCategory.SelectedItem?.ToString(), out selectCategory)) {
                MessageBox.Show("Gültige Kategorie auswählen.", "Eingabe fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.bankStatementService.GruppierungNachEmpfaenger(mapKategory);

            var guppierungNachKategorie = this.bankStatementService.GetGruppierung(selectCategory);

            var s = new BuchungsGruppierungUtils().QuerySpanMonth(guppierungNachKategorie, (DateTime)fromDate.SelectedDate, (DateTime)toDate.SelectedDate);

            Console.WriteLine(s);

            chartDisplay.Content = new MoneyPoints(s) {
            };
            */

            if(cmbCategory.SelectedItem == null) {
                MessageBox.Show("Gültige Kategorie auswählen.", "Eingabe fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bankStatementService.GruppierungNachEmpfaenger(mapKategoryV2);
            var b = bankStatementService.GetGruppierung(cmbCategory.SelectedItem.ToString());

            chartEinnahmen.Content = new MoneyPoints(b);

        }

        private bool ValidDateInput(DateTime from, DateTime to) {
            return from <= to;
        }

        private void btnShowMonthPie_Click(object sender, RoutedEventArgs e) {

            if (CheckDateInput() == false) {
                return;
            }

            bankStatementService.GruppierungNachEmpfaenger(mapKategoryV2);

            Dictionary<string, Geldbetrag> einnahmen = new Dictionary<string, Geldbetrag>();
            Dictionary<string, Geldbetrag> ausgaben = new Dictionary<string, Geldbetrag>();

            Geldbetrag einnahmeSumme = new Geldbetrag(0.0, Waehrung.EUR);
            Geldbetrag ausgabeSumme = new Geldbetrag(0.0, Waehrung.EUR);


            // Einnahmen und Ausgaben nach Kategorie zusammen suchen
            foreach ( var kvp in mapKategoryV2 ) {
               var filterBuchung = new BuchungsGruppierungUtils().Filter(
                    (DateTime)fromDate.SelectedDate, 
                    (DateTime)toDate.SelectedDate, 
                    bankStatementService.GetGruppierung(kvp.Key)
                    );


                Geldbetrag betrag = new Geldbetrag(0.0, Waehrung.EUR);

                foreach (var item in filterBuchung) {
                    betrag.Addition(item.Geldbetrag);
                }

                var roundBetrag = new Geldbetrag(Math.Round(betrag.Betrag, 2), betrag.Waehrung);
                if (roundBetrag.Betrag > 0.0) {
                    einnahmen.Add(kvp.Key, roundBetrag);
                    einnahmeSumme.Addition(roundBetrag);

                } else {
                    ausgaben.Add(kvp.Key, roundBetrag);
                    ausgabeSumme.Addition(roundBetrag);

                }

            }


            // nicht Kategorisierte Umsätze einzelnd auslisten
            var sonstigeBuchungen = new BuchungsGruppierungUtils().Filter(
                    (DateTime)fromDate.SelectedDate,
                    (DateTime)toDate.SelectedDate,
                    bankStatementService.GetGruppierung(Kategory.Sonstiges.ToString())
                    );

            for (int i = 0; i < sonstigeBuchungen.Length; i++) {                

                var roundBetrag = new Geldbetrag(Math.Round(sonstigeBuchungen[i].Geldbetrag.Betrag, 2), sonstigeBuchungen[i].Geldbetrag.Waehrung);

                if (roundBetrag.Betrag > 0.0) {

                    if (einnahmen.TryGetValue(sonstigeBuchungen[i].Empfaenger, out var vorhandenBetrag)) {
                        vorhandenBetrag.Addition(roundBetrag);
                    } else {
                        einnahmen.Add(sonstigeBuchungen[i].Empfaenger, roundBetrag);

                    }
                    einnahmeSumme.Addition(roundBetrag);

                } else {
                    if (ausgaben.TryGetValue(sonstigeBuchungen[i].Empfaenger, out var vorhandenBetrag)) {
                        vorhandenBetrag.Addition(roundBetrag);

                    } else {
                        ausgaben.Add(sonstigeBuchungen[i].Empfaenger, roundBetrag);

                    }
                    ausgabeSumme.Addition(roundBetrag);

                }
            }


            lblHeaderEinnahmen.Content = $"Einnahme (Summe): {einnahmeSumme}";
            lblHeaderAusgaben.Content = $"Ausgabe (Summe): {ausgabeSumme}";


            chartEinnahmen.Content = new MoneyPie(einnahmen);
            chartAusgaben.Content = new MoneyPie(ausgaben);
        }

    }
}
