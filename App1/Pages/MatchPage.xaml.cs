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
    }
}
