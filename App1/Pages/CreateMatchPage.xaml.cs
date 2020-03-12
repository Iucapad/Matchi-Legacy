using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CreateMatchPage : Page
    {
        private List<int> number = new List<int>(Enumerable.Range(1, 30));//liste du nombre de jouteurs
        private MatchStorage store = new MatchStorage();
        private MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content; //prend la mainpage actuelle comme mainpage
        public CreateMatchPage()
        {
            this.InitializeComponent();
            numberofround.ItemsSource = number;
            numberofround.SelectedIndex = 0;
        }

        private async void Create_match(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(nomeq1.Text) || string.IsNullOrWhiteSpace(nomeq2.Text))//les noms n'ont pas été inscrits
                await new MessageDialog("Vous devez nommer les équipes.").ShowAsync();
            else if(nomeq1.Text == nomeq2.Text)//les noms sont identiques
                await new MessageDialog("Les équipes doivent porter des noms différents.").ShowAsync();
            else//tout est correct
                Save();
        }

        public async void Save()//Sauvegarde les données entrées dans un fichier
        {
            if(store.Folder == null)//pas de dossier sélectionné
            {
                await new MessageDialog("Veuillez sélectionner un emplacement.").ShowAsync();
                return;
            }
            Matchimpro match = new Matchimpro(nomeq1.Text.Trim(), nomeq2.Text.Trim(), numberofround.SelectedIndex + 1);
            match.Save(store.Folder);

            MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content;
            if (mainFrame.navigationView.MenuItems.Count == 5)
            {
                mainFrame.navigationView.MenuItems[0] = new NavigationViewItem
                {
                    Name = "CURRENT",
                    IsSelected = true,
                    Content = "Match en cours",
                    Icon = new SymbolIcon((Symbol)0xE945),
                    Tag = "currentNav"
                };
            }
            MainPage.MainPageFrame?.Navigate(typeof(CurrentMatchPage), match); //renvoie à la page de match
        }

        private void ResizeWindow(object sender, SizeChangedEventArgs e)
        {
            if (((mainFrame)).ActualWidth < 750)
            {
                header_title.Margin = new Thickness(50, 0, 0, 0);
            }
            else
            {
                header_title.Margin = new Thickness(20, 10, 0, 0);
            }
        }
    }
}
