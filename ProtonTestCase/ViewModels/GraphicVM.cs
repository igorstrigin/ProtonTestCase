using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace ProtonTestCase.ViewModels
{
    public class GraphicVM : INotifyPropertyChanged
    {

        #region properties;
        private SeriesCollection graphics { get; set; }

        private LineSeries line { get; set; }

        private string[] labels { get; set; }

        #endregion

        #region PropertyChanged

        public SeriesCollection Graphics { 
            get
            {
                return graphics;
            } 
            set 
            {
                if (value != this.graphics)
                {
                    this.graphics = value;
                    NotifyPropertyChanged();
                }
            } }

        public LineSeries Line
        {
            get
            {
                return line;
            }
            set
            {
                if (value != this.line)
                {
                    this.line = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string[] Labels
        {
            get
            {
                return labels;
            }
            set
            {
                if (value != this.labels)
                {
                    this.labels = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public GraphicVM()
        {
            Graphics = new SeriesCollection();
        }

        public void AddNewLine() 
        {
            Line = new LineSeries() { Values = new ChartValues<ObservableValue> { new ObservableValue(1), new ObservableValue(2), new ObservableValue(34), new ObservableValue(5), new ObservableValue(6) }, Title = DateTime.Now.ToString()};
            Labels = new string[] { "a", "b", "c", "d", "e" };


            Graphics.Add(Line);

        }

        public void ChangeFirstLine() 
        {
            var rand = new Random();

            SeriesCollection a = Graphics;

            LineSeries b = (LineSeries)a.FirstOrDefault();

            ChartValues<ObservableValue> c = (ChartValues<ObservableValue>)b.Values;

            for (int i = c.Count; i > 0; i--)
                c[i - 1] = new ObservableValue(rand.Next(-15, 15));
        }

        public void ChangeCustomLine(string title)
        {
            var rand = new Random();

            SeriesCollection a = Graphics;

            LineSeries b = (LineSeries)a.Where(x=> x.Title == title).FirstOrDefault();

            ChartValues<ObservableValue> c = (ChartValues<ObservableValue>)b.Values;

            for (int i = c.Count; i > 0; i--)
                c[i - 1] = new ObservableValue(rand.Next(-15, 15));
        }

    }
}
