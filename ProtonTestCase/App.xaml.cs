﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProtonTestCase
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
		private static List<CultureInfo> m_Languages = new List<CultureInfo>();

		public static List<CultureInfo> Languages
		{
			get
			{
				return m_Languages;
			}
		}

		public static event EventHandler LanguageChanged;

		public App()
        {
            m_Languages.Clear();
            m_Languages.Add(new CultureInfo("en-US"));
            m_Languages.Add(new CultureInfo("ru-RU"));

            InitializeComponent();

            App.LanguageChanged += App_LanguageChanged;

            Language = ProtonTestCase.Properties.Settings.Default.DefaultLanguage;
        }

        private void Application_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Language = ProtonTestCase.Properties.Settings.Default.DefaultLanguage;
        }

        private void App_LanguageChanged(Object sender, EventArgs e)
        {
			ProtonTestCase.Properties.Settings.Default.DefaultLanguage = Language;
			ProtonTestCase.Properties.Settings.Default.Save();
        }

		public static CultureInfo Language
		{
			get
			{
				return System.Threading.Thread.CurrentThread.CurrentUICulture;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				if (value == System.Threading.Thread.CurrentThread.CurrentUICulture) return;

				//1. Меняем язык приложения:
				System.Threading.Thread.CurrentThread.CurrentUICulture = value;

				//2. Создаём ResourceDictionary для новой культуры
				ResourceDictionary dict = new ResourceDictionary();

				switch (value.Name)
				{
					case "ru-RU":
						dict.Source = new Uri(String.Format("Resources/lang.{0}.xaml", value.Name), UriKind.Relative);
						break;
					default:
						dict.Source = new Uri("Resources/lang.xaml", UriKind.Relative);
						break;
				}

				//3. Находим старую ResourceDictionary и удаляем его и добавляем новую ResourceDictionary
				ResourceDictionary oldDict = (from d in Current.Resources.MergedDictionaries
											  where d.Source != null && d.Source.OriginalString.StartsWith("Resources/lang.")
											  select d).First();
				if (oldDict != null)
				{
					int ind = Current.Resources.MergedDictionaries.IndexOf(oldDict);
					Current.Resources.MergedDictionaries.Remove(oldDict);
					Current.Resources.MergedDictionaries.Insert(ind, dict);
				}
				else
				{
					Current.Resources.MergedDictionaries.Add(dict);
				}

				//4. Вызываем евент для оповещения всех окон.
				LanguageChanged(Current, new EventArgs());
			}
		}
	}
}
