using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace ProtonTestCase.Forms
{
    /// <summary>
    /// Interaction logic for EditGraphicForm.xaml
    /// </summary>
    public partial class EditGraphicForm : Window
    {
        public ChartValues<ObservableValue> newLine;
        public List<double> newLinePoints = new List<double>();

        public EditGraphicForm(LineSeries line)
        {
            InitializeComponent();

            List<string> pointsText = new List<string>();

            foreach (ObservableValue point in line.ActualValues)
                pointsText.Add(point.Value.ToString());


            tbEditGraphic.Text = string.Join(Environment.NewLine, pointsText);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            List<string> newStringLine = tbEditGraphic.Text.Split(Environment.NewLine).ToList();

            if (newStringLine.Count == 0) {
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

            if (newStringLine.Count == 1)
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

            try
            {
                newLine = new ChartValues<ObservableValue>();

                foreach (var point in newStringLine)
                {
                    double value = Convert.ToDouble(point);
                    newLinePoints.Add(value);
                    newLine.Add(new ObservableValue(value));
                }

                this.Close();
            }
            catch (FormatException) 
            {
                if (App.Language == CultureInfo.GetCultureInfoByIetfLanguageTag("ru-RU"))
                {
                    MessageBox.Show("Вы ввели недопустимые символы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("You entered invalid characters", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            
        }
    }
}
