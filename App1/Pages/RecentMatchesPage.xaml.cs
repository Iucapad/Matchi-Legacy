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
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class RecentMatchesPage : Page
    {
        private ObservableCollection<Matchimpro> matchlist = new ObservableCollection<Matchimpro>();//liste des impros
        private MatchStorage store = new MatchStorage();//magasin de gestion des fichiers

        public RecentMatchesPage()
        {
            this.InitializeComponent();
            list_of_matches.ItemsSource = matchlist;
            list_of_matches.DisplayMemberPath = "Name";
            Read_storage();
            home_match.Children.Remove(details_card);
        }

        private async void Read_storage()//lit le contenu du dossier de stockage
        {
            matchlist = new ObservableCollection<Matchimpro>(await Matchimpro.ReadFolder(store.Folder));
            list_of_matches.ItemsSource = matchlist;
            if (matchlist.Count > 0)
            {
                error_message.Visibility = Visibility.Collapsed;
                info_messages.Visibility = Visibility.Visible;
                list_of_matches.Visibility = Visibility.Visible;
                header_title.Text = "Matchs récents";
            }
            else
            {
                info_messages.Visibility = Visibility.Collapsed;//messages par défaut si pas de fichiers ou mauvais format
                error_message.Visibility = Visibility.Visible;
                list_of_matches.Visibility = Visibility.Collapsed;
                header_title.Text = "Match";
            }
        }

        private async void Open_match(object sender, RoutedEventArgs e)
        {
            MainPage mainFrame = (MainPage)((Frame)Window.Current.Content).Content;
            if (mainFrame.navigationView.MenuItems.Count == 4)
            {
                ContentDialog deleteFileDialog = new ContentDialog
                {
                    Title = "Attention",
                    Content = "Vous avez un match en cours. Voulez vous en lancer un nouveau ?",
                    PrimaryButtonText = "Continuer",
                    CloseButtonText = "Annuler"
                };
                ContentDialogResult result = await deleteFileDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    mainFrame.navigationView.MenuItems.RemoveAt(0);
                    mainFrame.navigationView.MenuItems.Insert(0, new NavigationViewItem
                    {
                        Name = "CURRENT",
                        IsSelected = true,
                        Content = "Match en cours",
                        Icon = new SymbolIcon((Symbol)0xE945),
                        Tag = "currentNav"
                    });
                    MainPage.MainPageFrame?.Navigate(typeof(CurrentMatchPage), list_of_matches.SelectedItem);

                }
            }
            else
            {
                MainPage.MainPageFrame?.Navigate(typeof(CurrentMatchPage), list_of_matches.SelectedItem);
            }                
        }

        private void Create_Match(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageFrame?.Navigate(typeof(CreateMatchPage));     
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            if (list_of_matches.Items.Count == 0)
            {
                info_messages.Visibility = Visibility.Collapsed;
                error_message.Visibility = Visibility.Visible;
                list_of_matches.Visibility = Visibility.Collapsed;
                deletebtn.Visibility = Visibility.Collapsed;
                home_match.Children.Remove(details_card);
                addbtn.Margin = new Thickness(0, 0, 0, 60);
            }
            else if (list_of_matches.SelectedItem == null) 
            {
                info_messages.Visibility = Visibility.Visible;
                deletebtn.Visibility = Visibility.Collapsed;
                home_match.Children.Remove(details_card);
                addbtn.Margin = new Thickness(0, 0, 0, 60);
                if (((Frame)).ActualWidth < 750)
                {
                    list_of_matches.Margin = new Thickness(30, 130, 30, 160);
                }
                else
                {
                    image_message.Visibility = Visibility.Visible;
                }
            }
            else
            {
                info_messages.Visibility = Visibility.Collapsed;
                deletebtn.Visibility = Visibility.Visible;
                if (!home_match.Children.Contains(details_card)) {
                    home_match.Children.Add(details_card);
                }
                addbtn.Margin = new Thickness(153, 0, 0, 60);
                match_name.Text = ((Matchimpro)list_of_matches.SelectedItem).Name.ToUpper();
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
                choose_message.Margin = new Thickness(0, 100, 0, 0);
                details_card.Height = 125;
                details_card.CornerRadius = new CornerRadius(0);
                details_card.Width = double.NaN;
                details_card.HorizontalAlignment = HorizontalAlignment.Stretch;
                details_card.VerticalAlignment = VerticalAlignment.Top;
                details_card.Margin = new Thickness(0, 70, 0, 0);
                match_name.TextWrapping = TextWrapping.NoWrap;
                if (home_match.Children.Contains(details_card))
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
                if (!home_match.Children.Contains(details_card))
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
                Matchimpro match = (Matchimpro)list_of_matches.SelectedItem;
                match.DeleteFile();
                matchlist.Remove(match);
            }            
        }

        private async void SaveMatch(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            ((Matchimpro)list_of_matches.SelectedItem).Save(folder);

            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("Exportation du match"));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode("Le fichier de match a été créé avec succès"));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(1);
            ToastNotifier.Show(toast);
        }
    }
}
