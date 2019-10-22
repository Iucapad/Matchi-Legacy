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

            if (File.Exists(storageFolder.Path + Path.DirectorySeparatorChar + "Categories.catei"))
            {
                StorageFile cate_file = await storageFolder.GetFileAsync("Categories.catei");
                bool e = false;
                IList<string> infos = await FileIO.ReadLinesAsync(cate_file);

                for (int n = 1; n <= infos.Count; n++)
                {
                    if (n % 2 == 0)
                    {
                        if (Int32.TryParse(infos[n - 1], out int testnumber) && testnumber > 0)
                        {
                            string catname = infos[n - 2];
                            categorylist.Add(new Category(catname, testnumber));
                            list_of_categories.Items.Add(catname);
                        }
                        else if (infos[n - 1] == "Illimité" || infos[n - 1] == "Tous")
                        {
                            string catname = infos[n - 2];
                            categorylist.Add(new Category(catname));
                            list_of_categories.Items.Add(catname);
                        }
                        else//fichier corrompu car données non-valides
                        {
                            Clear_lists();
                        }
                    }
                    else
                    {
                        foreach(string info in infos)
                        {
                            int v = 0;
                            foreach (string test in infos)
                            {
                                if (info == test)
                                {
                                    v++;
                                }
                                if (v > 1)
                                {
                                    e = true;
                                }
                            }
                        }   
                    }
                }
                if (e)
                {
                    Clear_lists();
                }
            }
        }

        private void Clear_lists()
        {
            list_of_categories.Items.Clear();
            categorylist.Clear();
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
