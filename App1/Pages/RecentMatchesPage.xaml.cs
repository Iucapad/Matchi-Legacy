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
using Windows.Storage;
using Windows.UI.Popups;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentMatchesPage : Page
    {
        private List<Matchimpro> matchlist = new List<Matchimpro>();//liste des impros
        private MatchStorage store;//magasin de gestion des fichiers

        public RecentMatchesPage()
        {
            this.InitializeComponent();
            store = new MatchStorage();
            Read_storage();                      
        }

        private async void Read_storage()//lis le contenu du dossier de stockage
        {
            StorageFolder storageFolder = store.Folder;
            
            IReadOnlyList<StorageFile> match_files = await storageFolder.GetFilesAsync();

            info_messages.Visibility = Visibility.Collapsed;//messages par défaut si pas de fichiers ou mauvais format
            error_message.Visibility = Visibility.Visible;
            list_of_matches.Visibility = Visibility.Collapsed;
            header_title.Text = "Match";

            if (match_files.Count > 0)
            {
                header_title.Text = "Matchs récents";
                foreach (StorageFile match_file in match_files)
                {
                    if (match_file.FileType == ".matchi" || match_file.FileType == ".MATCHI")//existence de fichiers match
                    {
                        error_message.Visibility = Visibility.Collapsed;
                        info_messages.Visibility = Visibility.Visible;
                        list_of_matches.Visibility = Visibility.Visible;

                        IList<string> infos = await FileIO.ReadLinesAsync(match_file);

                        if (Int32.TryParse(infos[2], out int testnumber) && testnumber > 0)
                        {
                            list_of_matches.Items.Add(match_file.DisplayName);
                            matchlist.Add(new Matchimpro(infos[0], infos[1], testnumber));
                        }
                    }
                }
            }
        }

        private void Open_match(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageFrame?.Navigate(typeof(CurrentMatchPage));
        }

        private void Create_Match(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageFrame?.Navigate(typeof(CreateMatchPage));     
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            if (list_of_matches.Items.Count > 0)
            {
                choose_message.Visibility = Visibility.Collapsed;
                deletebtn.Visibility = Visibility.Visible;
                details_card.Visibility = Visibility.Visible;
                image_message.Visibility = Visibility.Collapsed;
                addbtn.Margin = new Thickness(153, 0, 0, 60);
                string str = list_of_matches.SelectedItem.ToString();
                match_name.Text = str.Replace('_', ' ').ToUpper();
                if (((Frame)).ActualWidth < 750)
                {
                    list_of_matches.Margin = new Thickness(30, 200, 30, 160);
                }
            }         
        }

        private void Resize_page(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                list_of_matches.Margin = new Thickness(30, 130, 30, 160);
                list_of_matches.Width = double.NaN;
                list_of_matches.HorizontalAlignment = HorizontalAlignment.Stretch;
                choose_message.VerticalAlignment = VerticalAlignment.Top;
                choose_message.Margin = new Thickness(0, 90, 0, 0);
                details_card.Height = 125;
                details_card.CornerRadius = new CornerRadius(0);
                details_card.Width = double.NaN;
                details_card.HorizontalAlignment = HorizontalAlignment.Stretch;
                details_card.VerticalAlignment = VerticalAlignment.Top;
                details_card.Margin = new Thickness(0, 60, 0, 0);
                match_name.TextWrapping = TextWrapping.NoWrap;
                if (details_card.Visibility == Visibility.Visible)
                {
                    list_of_matches.Margin = new Thickness(30, 200, 30, 160);                    
                }
                image_message.Visibility = Visibility.Collapsed;
            }
            else
            {
                list_of_matches.Margin = new Thickness(0, 60, 370, 160);
                list_of_matches.Width = 300;
                list_of_matches.HorizontalAlignment = HorizontalAlignment.Center;
                choose_message.VerticalAlignment = VerticalAlignment.Center;
                choose_message.Margin = new Thickness(370, 0, 0, 0);
                details_card.Height = 260;
                details_card.Width =300;
                details_card.CornerRadius = new CornerRadius(4);
                details_card.HorizontalAlignment = HorizontalAlignment.Center;
                details_card.VerticalAlignment = VerticalAlignment.Center;
                details_card.Margin = new Thickness(370, 60, 0, 160);
                match_name.TextWrapping = TextWrapping.Wrap;
                if (details_card.Visibility == Visibility.Collapsed)
                {
                    image_message.Visibility = Visibility.Visible;
                }
            }
        }

        private async void Delete_click(object sender, RoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Attention",
                Content = "Voulez-vous vraiment supprimer ce match ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler"
            };
            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                StorageFolder storageFolder = store.Folder;
                IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();
                foreach (var file in files)
                {
                    if (file.DisplayName == list_of_matches.SelectedItem.ToString())
                    {
                        await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
                    }
                }
                list_of_matches.Items.Clear();
                details_card.Visibility = Visibility.Collapsed;
                deletebtn.Visibility = Visibility.Collapsed;
                addbtn.Margin = new Thickness(0, 0, 0, 60);
                if (((Frame)).ActualWidth < 750)
                {
                    list_of_matches.Margin = new Thickness(30, 130, 30, 160);
                }
                Read_storage();
            }            
        }
    }
}
