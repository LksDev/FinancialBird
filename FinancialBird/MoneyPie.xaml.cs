using FinancialBird.Application.Entities;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinancialBird {
    /// <summary>
    /// Interaktionslogik für MoneyPie.xaml
    /// </summary>
    public partial class MoneyPie : UserControl {
        public MoneyPie(Dictionary<string, Geldbetrag> summaryBúchungen) {
            InitializeComponent();
            DisplayMonth(summaryBúchungen);
        }


        private void DisplayMonth(Dictionary<string, Geldbetrag> summaryBúchungen) {

            var piePieces = new SeriesCollection();

            foreach (var item in summaryBúchungen) {



                piePieces.Add(new PieSeries {
                    Title = $"{item.Key} ({item.Value.Waehrung})",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(item.Value.Betrag) },
                    DataLabels = true                     
                });               

            }

            MonthPieDiagram.Series = piePieces;
            MonthPieDiagram.Update(true, true);


        }

    }
}
