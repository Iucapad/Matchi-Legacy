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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CategoryPage : Page
    {
        private MatchStorage store;
        private List<Category> categorylist = new List<Category>();
        public CategoryPage()
        {
            this.InitializeComponent();
            page.Children.Remove(add_ui);
            store = new MatchStorage();
            Read_storage();
        }

        private async void Read_storage()//lit le contenu du dossier de stockage
        {
            StorageFolder storageFolder = store.Folder;
            IReadOnlyList<StorageFile> files = await storageFolder.GetFilesAsync();

            if (!File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))
                return; //TODO : Erreur ?

            StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei");
            IList<string> infos = await FileIO.ReadLinesAsync(cate_file);

            if (infos.Count == infos.Distinct().Count())
                return; //TODO : Erreur ?

            for (int n = 1; n <= infos.Count; n += 2)
            {
                if (int.TryParse(infos[n], out int playerCount)) 
                {
                    try 
                    {
                        categorylist.Add(new Category(infos[n - 1], playerCount));
                    } 
                    catch (ArgumentOutOfRangeException) 
                    {
                        categorylist.Clear();
                        break;
                    }
                } 
                else 
                {
                    categorylist.Clear();
                    break;
                }
            }
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
            }
        }

        private void Save_category(object sender, RoutedEventArgs e)
        {
            page.Children.Remove(add_ui);
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
    }
}
