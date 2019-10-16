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
using Windows.UI.Popups;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class CreateMatchPage : Page
    {
        private List<int> nombre;
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
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (nomeq1.Text.Length>20 || nomeq2.Text.Length>20)
            {
                await new MessageDialog("Les noms d'équipe ne peuvent pas dépasser 20 caractères.").ShowAsync();
                vider();
            }
            else if(nomeq1.Text.Length == 0 || nomeq2.Text.Length == 0)
            {
                await new MessageDialog("Vous devez nommer les équipes.").ShowAsync();
                vider();
            }
            else
            {
                int temp = nombremanche.SelectedIndex + 1;
                //List<string> file = new List<string> { nomeq1.Text, nomeq2.Text, temp.ToString() };

                
                string DefaultLibraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\libraries\");
                string npath = DefaultLibraryPath + nomeq1.Text + "_vs_" + nomeq2.Text + ".txt";
                Stream stream = new FileStream(npath, FileMode.Create, FileAccess.Write);
                
            }
        }

        private void vider()
        {
            nomeq1.Text = "";
            nomeq2.Text = "";
        }
    }
}
