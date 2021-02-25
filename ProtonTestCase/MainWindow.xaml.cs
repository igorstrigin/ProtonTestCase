using Grpc.Net.Client;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using ProtonTestCase.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ProtonTestCase;
using GRPCServer.Protos;

namespace ProtonTestCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GraphicVM VM;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLanguageChange();

            VM = new GraphicVM();
            chartMain.DataContext = VM;
        }


        #region Language change methods

        private void LanguageChanged(Object sender, EventArgs e)
        {
            CultureInfo currLang = App.Language;

            //Отмечаем нужный пункт смены языка как выбранный язык
            foreach (MenuItem i in menuLanguage.Items)
            {
                CultureInfo ci = i.Tag as CultureInfo;
                i.IsChecked = ci != null && ci.Equals(currLang);
            }
        }

        private void ChangeLanguageClick(Object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi != null)
            {
                CultureInfo lang = mi.Tag as CultureInfo;
                if (lang != null)
                {
                    App.Language = lang;
                }
            }

        }

        private void InitializeLanguageChange() 
        {
            App.LanguageChanged += LanguageChanged;

            CultureInfo currLang = App.Language;

            //Заполняем меню смены языка:
            menuLanguage.Items.Clear();
            foreach (var lang in App.Languages)
            {
                MenuItem menuLang = new MenuItem();
                menuLang.Header = lang.DisplayName;
                menuLang.Tag = lang;
                menuLang.IsChecked = lang.Equals(currLang);
                menuLang.Click += ChangeLanguageClick;
                menuLanguage.Items.Add(menuLang);
            }
        }

        #endregion

        private void mGraphicsClear_Click(object sender, RoutedEventArgs e)
        {
            chartMain.Series.Clear();
        }

        private async void mGraphicsGenerateRandom_Click(object sender, RoutedEventArgs e)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Graphics.GraphicsClient(channel);

            

            PointsArray reply = await client.GetRandomGraphicAsync(new PointsCount() {PointsCount_ = 5 });

            VM.AddNewLine(reply.GraphicPoints.ToArray());
            
            //VM.AddNewLine();

        }

        private void mChangeGraphic_Click(object sender, RoutedEventArgs e)
        {
            VM.ChangeFirstLine();

            //var rand = new Random();

            //SeriesCollection a = VM.Graphics;

            //LineSeries b = (LineSeries)a.FirstOrDefault();

            //ChartValues<ObservableValue> c = (ChartValues<ObservableValue>)b.Values;

            //for (int i = c.Count; i > 0; i--)
            //    c[i-1] = new ObservableValue(rand.Next(-15,15));
        }

        private void chartMain_DataClick(object sender, ChartPoint chartPoint)
        {
            VM.ChangeCustomLine(((LineSeries)chartPoint.SeriesView).Title); 
            
        }

        
    }
}
