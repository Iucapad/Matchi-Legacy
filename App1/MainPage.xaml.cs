using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using System.Net.NetworkInformation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Frame MainPageFrame;
        public MainPage()
        {
            this.InitializeComponent();
            MainPageFrame = contentFrame;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar; 
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.DimGray;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.DimGray;
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.Gray;
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            if (Application.Current.RequestedTheme == ApplicationTheme.Light)
            { saison_logo.Source = new BitmapImage(new Uri("ms-appx:///Assets/app_saison_logo_light.png"));}
            //Paramétrage de l'interface
            Date_display();
        }
        private void Date_display()
        {
            var culture = new System.Globalization.CultureInfo("fr-FR");
            string date = DateTime.Now.Day.ToString();
            string month = culture.DateTimeFormat.GetMonthName(DateTime.Today.Month);
            string day = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
            date_of_day.Text = day + " " + date + " " + month;
        }
        private void SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            home_interface.Visibility = Visibility.Collapsed;
            NavigationViewItem item = args.SelectedItem as NavigationViewItem;
            
            switch (item.Tag.ToString()) //Navigation vers la page souhaitée
            {
                case "matchNav":
                    contentFrame.Navigate(typeof(MatchPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
                case "categoryNav":
                    contentFrame.Navigate(typeof(CategoryPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
                case "editNav":
                    contentFrame.Navigate(typeof(EditPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
                case "webNav":
                    contentFrame.Navigate(typeof(WebPage));
                    web_header.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Create_Match(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(CreateMatchPage));
        }
    }
}
