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
        private List<int> nombre;//liste du nombre de jouteur
        private Matchimpro match;
        public CreateMatchPage()
        {
            this.InitializeComponent();
            nombre = new List<int>();
            for(int i=1; i<=10; i++)
            {
                nombre.Add(i);
            }
            nombremanche.ItemsSource = nombre;
            nombremanche.SelectedIndex = 0;
            match = new Matchimpro(nomeq1.Text, nomeq2.Text, nombremanche.SelectedIndex+1);
            this.filestate.Text = match.Folder.Path.ToString();
        }

        private async void Creer_match(object sender, RoutedEventArgs e)
        {
            if (nomeq1.Text.Length>30 || nomeq2.Text.Length>30)//les noms dépassent 30 caractères
            {
                await new MessageDialog("Les noms d'équipe ne peuvent pas dépasser 30 caractères.").ShowAsync();
                Vider();
            }
            else if(nomeq1.Text.Length == 0 || nomeq2.Text.Length == 0)//les noms n'ont pas été inscrits
            {
                await new MessageDialog("Vous devez nommer les équipes.").ShowAsync();
                Vider();
            }
            else if(nomeq1.Text == nomeq2.Text)//les noms sont identiques
            {
                await new MessageDialog("Les équipes doivent porter des noms différents.").ShowAsync();
                Vider();
            }
            else//tout est correct
            {
                Sauvegarder();
            }
        }

        private void Vider()// vide les champs de sélection
        {
            nomeq1.Text = "";
            nomeq2.Text = "";        
        }

        public async void Sauvegarder()//Sauvegarde les données entrées dans un fichier
        {
            if(match.Folder == null)//pas de dossier sélectionné
            {
                await new MessageDialog("Veuillez sélectionner un emplacement.").ShowAsync();
                Vider();
            }
            else//dossier sélectionné
            {
                string filename = match.Equipe1 + "_vs_" + match.Equipe2 + ".matchi";

                //le ficher est créé dans le dossier sélectionné et si un autre fichier a 
                //un nom identique, le nouveau fichier aura un chiffre en plus dans son nom.
                StorageFile newFile = await match.Folder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);

                //on écrit le contenu des champs à l'intérieur du fichier contenu dans l'objet newFile
                await FileIO.WriteLinesAsync(newFile, new List<string>{match.Equipe1, match.Equipe2, match.Manches.ToString()});
                
            }

        }

        private async void Choisir_dossier(object sender, RoutedEventArgs e)//Choisi le dossier 
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            match.Folder = await folderPicker.PickSingleFolderAsync();

            if (match.Folder != null)
            {
                
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                FutureAccessList.AddOrReplace("PickedFolderToken", match.Folder);
                this.filestate.Text = match.Folder.Path.ToString();             
            }
            else
            {
                this.filestate.Text = "Emplacment non-choisi.";
            }
        }

    }
}
