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

        public void AddNewLine(double[] line) 
        {

            Line = new LineSeries();
            var newLine = new ChartValues<ObservableValue>();

            for (int point = 0; point < line.Length; point++)
                newLine.Add(new ObservableValue(line[point]));

            Line.Values = newLine;

            Line.Title = Guid.NewGuid().ToString();

            Graphics.Add(Line);
        }

        public void AddNewLines(List<List<double>> lines)
        {
            foreach (var line in lines)
                AddNewLine(line.ToArray());

        }

        public void ChangeFirstLine() 
        {
            var rand = new Random();

            SeriesCollection a = Graphics;

            LineSeries b = (LineSeries)(a.FirstOrDefault());

            ChartValues<ObservableValue> c = (ChartValues<ObservableValue>)b.Values;

            for (int i = c.Count; i > 0; i--)
                c[i - 1] = new ObservableValue(rand.Next(-15, 15));
        }

        public void ChangeCustomLine(string title, ChartValues<ObservableValue> points)
        {
            var rand = new Random();

            SeriesCollection seriesCollection = Graphics;

            LineSeries Line = (LineSeries)seriesCollection.Where(x=> x.Title == title).FirstOrDefault();

            ChartValues<ObservableValue> c = (ChartValues<ObservableValue>)Line.Values;

            for (int i = c.Count; i > 0; i--)
                c[i - 1] = points[i - 1];
        }

    }
}
