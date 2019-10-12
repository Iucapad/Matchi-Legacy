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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppArt
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar; 
            coreTitleBar.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            //Etend le contenu pour masquer la barre de titre
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.DimGray;
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonInactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Windows.UI.Colors.Gray;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.Gray;
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            //Paramétrage des boutons de fenêtre
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
                    navigationView.Header = "Match";
                    break;
                case "peopleNav":
                    contentFrame.Navigate(typeof(PeoplePage));
                    navigationView.Header = "Jouteurs";
                    break;
                case "editNav":
                    contentFrame.Navigate(typeof(EditPage));
                    navigationView.Header = "Éditer";
                    break;
                case "webNav":
                    contentFrame.Navigate(typeof(WebPage));
                    navigationView.Header = "";
                    break;
            }
        }
    }
}
