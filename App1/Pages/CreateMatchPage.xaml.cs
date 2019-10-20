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

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CreateMatchPage : Page
    {
        private List<int> number;//liste du nombre de jouteur
        private Matchimpro match;
        private MatchStorage store;
        public CreateMatchPage()
        {
            this.InitializeComponent();
            number = new List<int>();
            for(int i=1; i<=30; i++)
            {
                number.Add(i);
            }
            numberofround.ItemsSource = number;
            numberofround.SelectedIndex = 0;
            store = new MatchStorage();
            match = new Matchimpro();
        }

        private async void Create_match(object sender, RoutedEventArgs e)
        {
            if (nomeq1.Text.Length>30 || nomeq2.Text.Length>30)//les noms dépassent 30 caractères
            {
                await new MessageDialog("Les noms d'équipe ne peuvent pas dépasser 30 caractères.").ShowAsync();
                Clean();
            }
            else if(nomeq1.Text.Length == 0 || nomeq2.Text.Length == 0)//les noms n'ont pas été inscrits
            {
                await new MessageDialog("Vous devez nommer les équipes.").ShowAsync();
                Clean();
            }
            else if(nomeq1.Text == nomeq2.Text)//les noms sont identiques
            {
                await new MessageDialog("Les équipes doivent porter des noms différents.").ShowAsync();
                Clean();
            }
            else//tout est correct
            {
                Save();
            }
        }

        private void Clean()// vide les champs de sélection
        {
            nomeq1.Text = "";
            nomeq2.Text = "";        
        }

        public async void Save()//Sauvegarde les données entrées dans un fichier
        {
            if(store.Folder == null)//pas de dossier sélectionné
            {
                await new MessageDialog("Veuillez sélectionner un emplacement.").ShowAsync();
                Clean();
            }
            else//dossier sélectionné
            {
                match.Team1 = nomeq1.Text;
                match.Team2 = nomeq2.Text;
                match.Rounds = numberofround.SelectedIndex + 1;
                string filename = match.Team1 + "_vs_" + match.Team2 + ".matchi";

                //le ficher est créé dans le dossier sélectionné et si un autre fichier a 
                //un nom identique, le nouveau fichier aura un chiffre en plus dans son nom.
                StorageFile newFile = await store.Folder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);

                //on écrit le contenu des champs à l'intérieur du fichier contenu dans l'objet newFile
                await FileIO.WriteLinesAsync(newFile, new List<string>{match.Team1, match.Team2, match.Rounds.ToString()});

                MainPage.MainPageFrame?.Navigate(typeof(CurrentMatchPage)); //renvoie à la page de match

            }

        }

        /* CODE NON UTILISE
        private async void Choisir_dossier(object sender, RoutedEventArgs e)//Choisi le dossier 
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            store.Folder = await folderPicker.PickSingleFolderAsync();

            if (store.Folder != null)
            {        
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", store.Folder);
                this.filestate.Text = store.Folder.Path.ToString();             
            }
            else
            {
                this.filestate.Text = "Emplacment non-choisi.";
            }
        }*/

    }
}
