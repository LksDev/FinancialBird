using FinancialBird.Application.Entities;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Interaktionslogik für MoneyPoints.xaml
    /// </summary>
    public partial class MoneyPoints : UserControl {

        public BuchungsGruppierung gruppierteBuchungen { get; init; }
        
        public MoneyPoints(BuchungsGruppierung groupData) {
            InitializeComponent();
            gruppierteBuchungen = groupData;
            InitBuchung();
        }
        public MoneyPoints(Buchung[] buchungen) {
            InitializeComponent();
            MonthChart(buchungen);
        }

        private void InitBuchung() {

            var r = new Random();
            var Values = new ChartValues<ObservableValue>
            {
            };

            double minBetrag = 0.0;
            double maxBetrag = 0.0;

            foreach (var value in this.gruppierteBuchungen.Buchungssaetze) {

                Values.Add(new ObservableValue(value.Geldbetrag.Betrag));
                if(value.Geldbetrag.Betrag > maxBetrag) { maxBetrag = value.Geldbetrag.Betrag; }
                if (value.Geldbetrag.Betrag < minBetrag) { minBetrag = value.Geldbetrag.Betrag; }
            }

            var DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));

            //Lets define a custom mapper, to set fill and stroke
            //according to chart values...
            var Mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 200 ? DangerBrush : null)
                .Stroke(item => item.Value > 200 ? DangerBrush : null);
            

            this.lineSeries.Values = Values;
            this.lineSeries.Configuration = Mapper;

        }

        private void MonthChart(Buchung[] buchungen) {

            Geldbetrag[] allMonth = new Geldbetrag[] { 
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR),
            new Geldbetrag(0.0, Waehrung.EUR)
            };

            foreach (var item in buchungen) {
                int index = item.Buchungsdatum.Month - 1;
                allMonth[index].Addition(item.Geldbetrag);

            }


            var Values = new ChartValues<ObservableValue> {   };

            double minBetrag = 0.0;
            double maxBetrag = 0.0;

            foreach (var value in allMonth) {
                Values.Add(new ObservableValue(value.Betrag));
            }

            var DangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));

            //Lets define a custom mapper, to set fill and stroke
            //according to chart values...
            var Mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 20 ? DangerBrush : null)
                .Stroke(item => item.Value > 200 ? DangerBrush : null);


            this.lineSeries.Values = Values;
            this.lineSeries.Configuration = Mapper;

        }

    }
}
