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
using Windows.UI;
using Windows.Storage;
using Windows.UI.Popups;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Frame MainPageFrame;
        private MatchStorage store;//magasin de gestion des fichiers
        private Matchimpro mostRecentMatch;
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
            { saison_logo.Source = new BitmapImage(new Uri("ms-appx:///Assets/app_saison_logo_light.png")); }
            MATCH.Icon = new SymbolIcon((Symbol)0xE81C);
            CATEGORIES.Icon= new SymbolIcon((Symbol)0xE74C);
            Date_display();
            store = new MatchStorage();
            Read_storage();
        }
        private async void Read_storage()//lit le contenu du dossier de stockage
        {
            List<Matchimpro> matchlist = await Matchimpro.ReadFolder(store.Folder);
            if (matchlist.Count > 0) {
                recent_matches.Visibility = Visibility.Visible;
                nb_matches.Text = $"{matchlist.Count} {(matchlist.Count > 1 ? "matchs récents" : "match récent")}";
                mostRecentMatch = matchlist[0];
                most_recent_match.Text = mostRecentMatch.Name;
            }
            else
            {
                recent_matches.Visibility = Visibility.Collapsed;
            }
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
                    contentFrame.Navigate(typeof(RecentMatchesPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
                case "categoryNav":
                    contentFrame.Navigate(typeof(CategoryPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
                case "editNav":
                    contentFrame.Navigate(typeof(MusicPage));
                    web_header.Fill = new SolidColorBrush(Color.FromArgb(255,30, 215, 96));
                    web_header.Visibility = Visibility.Visible;
                    break;
                case "currentNav":
                    contentFrame.Navigate(typeof(CurrentMatchPage));
                    web_header.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void Create_Match(object sender, RoutedEventArgs e)
        {
            navigationView.SelectedItem = MATCH;
            contentFrame.Navigate(typeof(CreateMatchPage));            
        }
        private void Category_button(object sender, RoutedEventArgs e)
        {
            navigationView.SelectedItem = CATEGORIES;        
        }
        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)Window.Current.Content).ActualHeight < 500)
            {
                date_of_day.Margin = new Thickness(0, 50, 0, 0);
                recent_matches.Margin=new Thickness(0, 0, 0, 50);
            }
            else
            {
                date_of_day.Margin = new Thickness(0, 110, 0, 0);
                recent_matches.Margin = new Thickness(0, 0, 0, 0);
            }
                if (((Frame)Window.Current.Content).ActualWidth < 500)
            {
                web_header.Margin = new Thickness(0, 0, 0, 0);
                navigationView.IsPaneToggleButtonVisible = false;
                navigationView.IsPaneVisible = false;
            }
            else
            {
                web_header.Margin = new Thickness(40, 0, 0, 0);
                navigationView.IsPaneToggleButtonVisible = true;
                navigationView.IsPaneVisible = true;
            }
        }

        private void MostRecent_click(object sender, RoutedEventArgs e)
        {
            try 
            {
                contentFrame.Navigate(typeof(CurrentMatchPage), mostRecentMatch);
            }
            catch (Exception)
            {
                return; //TODO : Message d'erreur
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Matchimpro)
            {
                contentFrame.Navigate(typeof(CurrentMatchPage), e.Parameter);
            }
        }
    }
}
