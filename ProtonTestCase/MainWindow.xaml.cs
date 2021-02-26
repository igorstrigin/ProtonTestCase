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
using ProtonTestCase.Forms;

namespace ProtonTestCase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public GraphicVM VM;

        private Graphics.GraphicsClient client;

        public MainWindow()
        {
            InitializeComponent();
            InitializeLanguageChange();
            InitializeGrpcConnection();

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
            
            try
            {
                PointsArray reply = await client.GetRandomGraphicAsync(new PointsCount() { PointsCount_ = 5 });
                VM.AddNewLine(reply.GraphicPoints.ToArray());
            }
            catch (Grpc.Core.RpcException ex) 
            { 
                MessageBox.Show("Возникли проблемы с сервером. Возможные причины:" + 
                                Environment.NewLine + "1) Сервер не подключен" +
                                Environment.NewLine + "2) При запросе возникла ошибка" +
                                Environment.NewLine + Environment.NewLine + ex.Message
                                , "Ошибка"
                                , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void mChangeGraphic_Click(object sender, RoutedEventArgs e)
        {
            VM.ChangeFirstLine();
        }

        private async void chartMain_DataClick(object sender, ChartPoint chartPoint)
        {
            LineSeries line = (LineSeries)chartPoint.SeriesView;

            EditGraphicForm form = new EditGraphicForm(line);
            form.ShowDialog();

            //проверка на закрытие формы
            if (form.newLinePoints.Count == 0 || form.newLine.Count == 0)
                return;

            

            try
            {
                PointsArray pointsArray = new PointsArray();
                pointsArray.GraphicPoints.Add(form.newLinePoints);
                await client.GetCustomGraphicAsync(pointsArray);
            }
            catch (Grpc.Core.RpcException ex)
            {
                MessageBox.Show("Возникли проблемы с сервером. Возможные причины:" +
                                Environment.NewLine + "1) Сервер не подключен" +
                                Environment.NewLine + "2) При запросе возникла ошибка" +
                                Environment.NewLine + Environment.NewLine + ex.Message
                                , "Ошибка"
                                , MessageBoxButton.OK, MessageBoxImage.Error);
            }

            VM.ChangeCustomLine(((LineSeries)chartPoint.SeriesView).Title,form.newLine); 
            
        }

        private async void mLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GraphicsArray reply = await client.GetGraphicsFromFileAsync(new EmptyMessage() { Ok = true });

                List<List<double>> lines = new List<List<double>>();

                foreach (var pointsArray in reply.Lines)
                {
                    List<double> line = new List<double>();

                    foreach (var point in pointsArray.GraphicPoints)
                        line.Add(point);

                    lines.Add(line);
                }

                VM.AddNewLines(lines);
            }
            catch (Grpc.Core.RpcException ex) 
            { 
                MessageBox.Show("Возникли проблемы с сервером. Возможные причины:" + 
                                Environment.NewLine + "1) Сервер не подключен" +
                                Environment.NewLine + "2) При запросе возникла ошибка" +
                                Environment.NewLine + Environment.NewLine + ex.Message
                                , "Ошибка"
                                , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeGrpcConnection() 
        {
            try
            {
                var channel = GrpcChannel.ForAddress("https://localhost:5001");
                client = new Graphics.GraphicsClient(channel);
            }
            catch (Exception) { }
        }
    }
}
