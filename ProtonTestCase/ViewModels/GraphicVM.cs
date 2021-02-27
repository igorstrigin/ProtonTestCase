using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;

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

            ChartValues<ObservableValue> line = (ChartValues<ObservableValue>)Line.Values;

            try
            {
                //удаленные точки
                if (line.Count > points.Count)
                {
                    if (points.Count == 0)
                    {
                        if (App.Language == CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"))
                        {
                            MessageBox.Show("Нельзя создать график без точек", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("You cannot create a graph without points", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    if (points.Count == 1)
                    {
                        if (App.Language == CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"))
                        {
                            MessageBox.Show("График должен состоять хотя бы из 2 точек", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("The graph must contain at least 2 points", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    for (int i = line.Count; i > 0; i--)
                    {
                        if (points.Count <= i - 1)
                            line.Remove(line[i - 1]);
                        else
                            line[i - 1] = points[i - 1];
                    }

                }

                //перенос точек
                for (int i = line.Count; i > 0; i--)
                    line[i - 1] = points[i - 1];

                //добавление новых точек
                if (line.Count < points.Count)
                    for (int i = line.Count; i < points.Count; i++)
                        line.Add(points[i]);
            }
            //ошибка может возникнуть при закрытии окна
            catch (Exception ex) { }
        }

    }
}
