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
        private MatchStorage store = new MatchStorage();//objet relatif au stockage
        private ObservableCollection<string> source_category_list = new ObservableCollection<string>();
        public CategoryPage()
        {
            this.InitializeComponent();
            page.Children.Remove(add_ui);
            Read_storage();
        }

        private async void Read_storage()//lit le contenu du dossier de stockage
        {
            StorageFolder storageFolder = store.Folder;
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();

            if (!File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))
                return; //TODO : Erreur ?

            StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei");
            foreach (string category in await FileIO.ReadLinesAsync(cate_file))
                source_category_list.Add(category);


            if (source_category_list.Count != source_category_list.Distinct().Count())
                return; //TODO : Erreur ?

            list_of_categories.ItemsSource = source_category_list;
        }

        private void Selection(object sender, SelectionChangedEventArgs e)
        {
            deletebtn.Visibility = Visibility.Visible;
            addbtn.Margin = new Thickness(153, 0, 0, 60);
        }

        private void Show_addui(object sender, RoutedEventArgs e)
        {
            if (!page.Children.Contains(add_ui))
            {
                page.Children.Add(add_ui);
                list_of_categories.Visibility = Visibility.Collapsed;
            }
        }

        private async void Save_category(object sender, RoutedEventArgs e)
        {
            ContentDialog valuesErrorDialog = new ContentDialog
            {
                Title = "Attention",
                Content = "",
                CloseButtonText = "Ok"
            };
            if (category_name.Text.Length == 0 || category_name.Text.Length > 30)
            {
                valuesErrorDialog.Content = "Veuillez saisir un nom entre 1 et 30 caractères.";
                await valuesErrorDialog.ShowAsync();
                return;
            }
            if (source_category_list.Contains(category_name.Text))
            {
                valuesErrorDialog.Content = "Ce nom de catégorie existe déjà.";
                await valuesErrorDialog.ShowAsync();
                return;
            }
            source_category_list.Add(category_name.Text);
            category_name.Text = "";
            list_of_categories.Visibility = Visibility.Visible;
            page.Children.Remove(add_ui);
            Save_to_file();
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
                //TODO: faire en sorte que la listview disparaisse lorsqu'elle est vide car cela engeandre un crash
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
            list_of_categories.Visibility = Visibility.Visible;
            page.Children.Remove(add_ui);
        }
    }
}
