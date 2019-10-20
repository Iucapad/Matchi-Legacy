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
        private string team1;//nom de l'équipe 1
        private string team2;//nom de l'équipe 2
        private int rounds;//nombre de manche
        private List<Categorie> categories;//liste des catégories pour le match

        public Matchimpro()
        {
            team1 = "Equipe 1";
            team2 = "Equipe 2";
            rounds = 5;
        }
        public Matchimpro(string t1, string t2, int r)
        {
            team1 = t1;
            team2 = t2;
            rounds = r;
        }

        public string Team1 { get => team1; set => team1 = value; }
        public string Team2 { get => team2; set => team2 = value; }
        public int Rounds { get => rounds; set => rounds = value; }
        internal List<Categorie> Categories { get => categories; set => categories = value; }

        public void AddCate(Categorie cate)//Ajoute une catégorie à la liste de celles du match
        {
            if (!categories.Contains(cate))
            {
                categories.Add(cate);
            }
        }

        public void RemoveCate(Categorie cate)//Retire une catégorie
        {
            if (categories.Contains(cate))
            {
                categories.Remove(cate);
            }
        }
    }
}
