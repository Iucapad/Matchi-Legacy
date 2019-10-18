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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MatchPage : Page
    {
        public MatchPage()
        {
            this.InitializeComponent();
            /*TODO
             if (!sauvegarde.match) {
                error_message.Visibility=Visibility.Visible;
                list_of_matches.Visibility=Visibility.Collapsed;
            }else{
                error_message.Visibility=Visibility.Collapsed;
                list_of_matches.Visibility=Visibility.Visible;

                foreach(xxx){
                list_of_matches.Items.Add(xxx);
            }
            }
             */
            list_of_matches.Items.Add("Match 1");
            list_of_matches.Items.Add("Match 2");
            list_of_matches.Items.Add("Match 3");

        }

        private void Create_Match(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageFrame?.Navigate(typeof(CreateMatchPage));     
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            choose_message.Visibility = Visibility.Collapsed;
            deletebtn.Visibility = Visibility.Visible;
            details_card.Visibility = Visibility.Visible;
            addbtn.Margin = new Thickness(153, 0, 0, 60);
            if (((Frame)).ActualWidth < 750)
            {
                list_of_matches.Margin = new Thickness(0, 200, 0, 160);
            }
        }

        private void Resize_page(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                list_of_matches.Margin = new Thickness(0, 130, 0, 160);
                choose_message.VerticalAlignment = VerticalAlignment.Top;
                choose_message.Margin = new Thickness(0, 90, 0, 0);
                details_card.Height = 125;
                details_card.VerticalAlignment = VerticalAlignment.Top;
                details_card.Margin = new Thickness(0, 60, 0, 0);
                if (details_card.Visibility == Visibility.Visible)
                {
                    list_of_matches.Margin = new Thickness(0, 200, 0, 160);
                }                          
            }
            else
            {
                list_of_matches.Margin = new Thickness(0, 60, 370, 160);
                choose_message.VerticalAlignment = VerticalAlignment.Center;
                choose_message.Margin = new Thickness(370, 0, 0, 0);
                details_card.Height = 260;
                details_card.VerticalAlignment = VerticalAlignment.Center;
                details_card.Margin = new Thickness(370, 60, 0, 160);                
            }
        }
    }
}
