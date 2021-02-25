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
using System.Windows.Shapes;

namespace ProtonTestCase.Forms
{
    /// <summary>
    /// Interaction logic for EditGraphicForm.xaml
    /// </summary>
    public partial class EditGraphicForm : Window
    {
        public ChartValues<ObservableValue> newLine;

        public EditGraphicForm(LineSeries line)
        {
            InitializeComponent();

            List<string> pointsText = new List<string>();

            foreach (ObservableValue point in line.ActualValues)
                pointsText.Add(point.Value.ToString());


            tbEditGraphic.Text = string.Join(";" + Environment.NewLine, pointsText);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            List<string> newStringLine = tbEditGraphic.Text.Split(';').ToList();


            newLine = new ChartValues<ObservableValue>();

            foreach (var point in newStringLine)
                newLine.Add(new ObservableValue(Convert.ToDouble(point)));

            this.Close();
        }
    }
}
