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
        public string Team1 { get; set; } = "Equipe 1";
        public string Team2 { get; set; } = "Equipe 2";
        public int Rounds { get; set; } = 5;
        internal List<Category> Categories { get; set; }
        public string Name {
            get => $"{Team1} vs {Team2}";
        }

        public Matchimpro() { }

        public Matchimpro(string t1, string t2, int r)
        {
            Team1 = t1;
            Team2 = t2;
            Rounds = r;
        }

        public void AddCate(Category cate)//Ajoute une catégorie à la liste de celles du match
        {
            if (!Categories.Contains(cate))
            {
                Categories.Add(cate);
            }
        }

        public void RemoveCate(Category cate)//Retire une catégorie
        {
            Categories.Remove(cate);
        }

        public override string ToString()
        {
            return $"{Team1} vs {Team2} ({Rounds} round{(Rounds > 1 ? "s" : "")})";
        }
    }
}
