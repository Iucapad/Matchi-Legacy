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
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using System.Diagnostics;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace MatchiApp
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        private MatchStorage store = new MatchStorage(); //lieu de stockage des fichier
        private ObservableCollection<string> source_category_list = new ObservableCollection<string>(); //liste des catégories bind à la listview
        private ContentDialog ErrorDialog = new ContentDialog //Squelette de base d'un message d'erreur, à compléter en cas d'erreur
        {
            Title = "Attention",
            Content = "",
            CloseButtonText = "Ok"
        };
        public CategoryPage()
        {
            this.InitializeComponent();
            page.Children.Remove(add_ui);
            Read_storage();            
        }

        private async void Read_storage()
        {
            StorageFolder storageFolder = store.Folder; //le dossier courant est le dossier de stockage donné
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync(); //les fichiers dans le dossier courant sont pris

            if (!File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))//si le fichier de catégorie n'existe pas, on masque la listview
            {
                list_of_categories.Visibility = Visibility.Collapsed;
                return;
            }

            StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei"); //on prend le fichier de catégorie

            bool verif = false;
            foreach (string category in await FileIO.ReadLinesAsync(cate_file))
            {
                if (category.Length > 0 && category.Length <= 30)//si un ligne est bien écrite, on l'ajoute à la liste
                   source_category_list.Add(category);

                verif = (category.Length < 0 || category.Length > 30) ? true : false; //contrôle de la longueur des lignes
            }

            Refresh_Page();

            if (verif) //si une ligne possède plus de caractères que prévu
            {
                Save_to_file();
                ErrorDialog.Content = "Des noms de catégorie possédaient plus de 30 caractères dans le fichier, ces dernières ont été supprimées.";
                await ErrorDialog.ShowAsync();
            }   

            if (source_category_list.Count != source_category_list.Distinct().Count())// si une même catégorie est plusieurs fois dans le fichier
            {
                addbtn.Visibility = Visibility.Collapsed;
                ErrorDialog.Content = "Des doublons sont présents dans le fichier de catégorie, lecture impossible.";
                ErrorDialog.PrimaryButtonText = "Réparer";
                ErrorDialog.CloseButtonText = "Fermer";
                ContentDialogResult result = await ErrorDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)//réparer le fichier
                {
                    ObservableCollection<string> copy = new ObservableCollection<string>();

                    foreach (string cat in source_category_list.Distinct())
                        copy.Add(cat);

                    source_category_list = copy;

                    Save_to_file();
                    Refresh_Page();
                    addbtn.Visibility = Visibility.Visible;
                }
                return;
            }
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            if (list_of_categories.Items.Count == 0)
            {
                list_of_categories.Visibility = Visibility.Collapsed;
                empty_message.Visibility = Visibility.Visible;
                deletebtn.Visibility = Visibility.Collapsed;
                addbtn.Margin = new Thickness(0, 0, 0, 60);
            }
            else if (list_of_categories.SelectedItem == null)
            {
                deletebtn.Visibility = Visibility.Collapsed;
                addbtn.Margin = new Thickness(0, 0, 0, 60);
            }
            else
            {
                deletebtn.Visibility = Visibility.Visible;
                addbtn.Margin = new Thickness(153, 0, 0, 60);
            }
        }

        private void Show_addui(object sender, RoutedEventArgs e)
        {
            if (!page.Children.Contains(add_ui))
                page.Children.Add(add_ui);                 
        }

        private async void Save_category(object sender, RoutedEventArgs e)
        {
            if (category_name.Text.Length == 0 || category_name.Text.Length > 30)
            {
                ErrorDialog.Content = "Veuillez saisir un nom entre 1 et 30 caractères.";
                await ErrorDialog.ShowAsync();
                return;
            }
            if (source_category_list.Contains(category_name.Text))
            {
                ErrorDialog.Content = "Ce nom de catégorie existe déjà.";
                await ErrorDialog.ShowAsync();
                return;
            }
            source_category_list.Add(category_name.Text);            
            category_name.Text = "";
            page.Children.Remove(add_ui);
            Save_to_file();   
            Refresh_Page();           
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            if (((Frame)).ActualWidth < 750)
            {
                list_of_categories.Margin = new Thickness(30, 130, 30, 160);
                list_of_categories.Width = double.NaN;
                list_of_categories.HorizontalAlignment = HorizontalAlignment.Stretch;
            }
            else
            {
                list_of_categories.Margin = new Thickness(30, 130, 30, 160);
                list_of_categories.Width = 300;
                list_of_categories.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        private async void Delete_category(object sender, RoutedEventArgs e) 
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Attention",
                Content = "Voulez-vous vraiment supprimer cette catégorie ?",
                PrimaryButtonText = "Supprimer",
                CloseButtonText = "Annuler"
            };
            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                source_category_list.RemoveAt(list_of_categories.SelectedIndex);
                Save_to_file();
            }
        }

        private async void Save_to_file() 
        {
            StorageFile file = await store.Folder.CreateFileAsync("Categories.catei", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, string.Join("\n", source_category_list));
        }

        private void add_cancel_Click(object sender, RoutedEventArgs e)
        {
            category_name.Text = "";
            page.Children.Remove(add_ui);
        }

        private void Refresh_Page()
        {
            list_of_categories.ItemsSource = source_category_list;
            if (source_category_list.Count == 0 || (source_category_list.Count != source_category_list.Distinct().Count()))
            {
                list_of_categories.Visibility = Visibility.Collapsed;
                empty_message.Visibility = Visibility.Visible;
            }
            else
            {
                list_of_categories.Visibility = Visibility.Visible;
                empty_message.Visibility = Visibility.Collapsed;
            }
        }
    }
}
