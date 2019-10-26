using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CurrentMatchPage : Page
    {

        private Matchimpro matchimpro;
        private int round_value = 1;

        public CurrentMatchPage()
        {
            this.InitializeComponent();
            SharedShadow.Receivers.Add(Receiver);
            page.Children.Remove(new_round);
            MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content;
            if (mainFrame.navigationView.MenuItems.Count < 4)
            {
                mainFrame.navigationView.MenuItems.Insert(0, new NavigationViewItem
                {
                    Name = "CURRENT",
                    IsSelected = true,
                    Content = "Match en cours",
                    Icon = new SymbolIcon((Symbol)0xE945),
                    Tag = "currentNav"                    
                });
            }
        }

        private void show(object sender, RoutedEventArgs e)
        {
            if (!page.Children.Contains(new_round))
            {
                page.Children.Add(new_round);
                round_nb.Text = "Manche " + round_value.ToString() + "/" + matchimpro.Rounds.ToString();
            }
        }
        public static void HideNav(MainPage page)
        {
            page.navigationView.Visibility = Visibility.Collapsed;
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                ui_scroll.Margin = new Thickness(0, 60, 0, 100);
                ui_container.Orientation = Orientation.Vertical;
                ui_container.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_container.Padding = new Thickness(30,5,30,5);
                ui_vs.Height = 25;
                ui_vs.CornerRadius = new CornerRadius(15);
                ui_vs.Padding = new Thickness(2);
                ui_leftcard.Height = 125;
                ui_leftcard.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_leftcard.Width = double.NaN;
                ui_rightcard.Height = 125;
                ui_rightcard.HorizontalAlignment = HorizontalAlignment.Stretch;
                ui_rightcard.Width = double.NaN;
                ui_leftname.TextAlignment = TextAlignment.Left;
                ui_rightname.TextAlignment = TextAlignment.Left;
                ui_catname.HorizontalAlignment = HorizontalAlignment.Center;
                ui_catname.Margin = new Thickness(0, 10, 0, 60);
                ui_nbjouteurs.HorizontalAlignment = HorizontalAlignment.Center;
                ui_nbjouteurs.Margin = new Thickness(0, 60, 0, 20);
            }
            else
            {
                ui_scroll.Margin = new Thickness(0, 100, 0, 0);
                ui_container.Orientation = Orientation.Horizontal;
                ui_container.HorizontalAlignment = HorizontalAlignment.Center;
                ui_container.Padding = new Thickness(20);
                ui_vs.Height = 50;
                ui_vs.CornerRadius = new CornerRadius(25);
                ui_vs.Padding = new Thickness(15);
                ui_leftcard.Height = 250;
                ui_leftcard.HorizontalAlignment = HorizontalAlignment.Center;
                ui_leftcard.Width = 300;
                ui_rightcard.Height = 250;
                ui_rightcard.HorizontalAlignment = HorizontalAlignment.Center;
                ui_rightcard.Width = 300;
                ui_leftname.TextAlignment = TextAlignment.Center;
                ui_rightname.TextAlignment = TextAlignment.Center;
                ui_catname.HorizontalAlignment = HorizontalAlignment.Left;
                ui_catname.Margin = new Thickness(120, 30, 0, 60);
                ui_nbjouteurs.HorizontalAlignment = HorizontalAlignment.Right;
                ui_nbjouteurs.Margin = new Thickness(0, 30, 150, 120);
            }
        }

        private void Start_click(object sender, RoutedEventArgs e)
        {
            page.Children.Remove(new_round);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Matchimpro)
            {
                matchimpro = (Matchimpro)e.Parameter;
                Update_Match();
            }
        }

        private void Update_Match()
        {            
            ui_leftname.Text = matchimpro.Team1;
            ui_rightname.Text = matchimpro.Team2;
            round_nb.Text = "Manche "+round_value.ToString()+"/"+matchimpro.Rounds.ToString();
        }
    }
}
