using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

namespace App1
{
    class Matchimpro
    {
        private string equipe1;//nom de l'équipe 1
        private string equipe2;//nom de l'équipe 2
        private int manches;//nombre de manche
        private List<Categorie> categories;//liste des catégories pour le match

        private StorageFolder folder;//dossier de stockage du match choisi par l'utilisateur
        private StorageFolder default_folder;//dossier de stockage du match par défaut

        public Matchimpro()
        {
            Defol();
            folder = default_folder;
        }
        public Matchimpro(string team1, string team2, int m)
        {
            equipe1 = team1;
            equipe2 = team2;
            manches = m;
            Defol();
            folder = default_folder;
        }

        public Matchimpro(string team1, string team2, int m, StorageFolder f)
        {
            equipe1 = team1;
            equipe2 = team2;
            manches = m;
            folder = f;
        }

        public string Equipe1 { get => equipe1; set => equipe1 = value; }
        public string Equipe2 { get => equipe2; set => equipe2 = value; }
        public List<Categorie> Categories { get => categories; set => categories = value; }
        public StorageFolder Folder { get => folder; set => folder = value; }
        public StorageFolder Default_folder { get => default_folder; }
        public int Manches { get => manches; set => manches = value; }

        public void AjouterCate(Categorie cate)//Ajoute une catégorie à la liste de celles du match
        {
            if (!categories.Contains(cate))
            {
                categories.Add(cate);
            }
        }

        public void RetirerCate(Categorie cate)//Retire une catégorie
        {
            if (categories.Contains(cate))
            {
                categories.Remove(cate);
            }
        }

        private async void Defol()//définit le dossier de stockage par défaut
        {
            default_folder = ApplicationData.Current.LocalFolder;
            StorageFolder newFolder = await Default_folder.CreateFolderAsync("ImproData", CreationCollisionOption.OpenIfExists);
            default_folder = newFolder;
        }
    }
}
