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
using System.Collections.ObjectModel;

namespace App1
{
    class Matchimpro
    {
        public string Team1 { get; set; } = "Equipe 1";
        public string Team2 { get; set; } = "Equipe 2";
        public int Rounds { get; set; } = 5;
        internal List<string> Categories { get; set; }
        public string Name {
            get => $"{Team1} vs {Team2}";
        }
        public ObservableCollection<Matchimpro> Matches { get; set; } = new ObservableCollection<Matchimpro>();

        public Matchimpro() { }

        public Matchimpro(string t1, string t2, int r)
        {
            Team1 = t1;
            Team2 = t2;
            Rounds = r;
        }

        public Matchimpro(StorageFolder pathfolder, string choix)
        {
            switch (choix)
            {
                case "read":
                    Read_Match(pathfolder);
                    break;
                case "save":
                    Save_Match(pathfolder);
                    break;
                default:
                    break;
            }
        }

        public void AddCate(string cate)//Ajoute une catégorie à la liste de celles du match
        {
            if (!Categories.Contains(cate))
            {
                Categories.Add(cate);
            }
        }

        public void RemoveCate(string cate)//Retire une catégorie
        {
            Categories.Remove(cate);
        }

        public override string ToString()
        {
            return $"{Team1} vs {Team2} ({Rounds} round{(Rounds > 1 ? "s" : "")})";
        }

        public async void Read_Match(StorageFolder pathfolder)
        {
            IReadOnlyList<StorageFile> match_files = await pathfolder.GetFilesAsync();
            if (match_files.Count > 0)
            {
                foreach (StorageFile match_file in match_files)
                {
                    if (match_file.FileType == ".matchi" || match_file.FileType == ".MATCHI")//existence de fichiers match
                    {
                        IList<string> infos = await FileIO.ReadLinesAsync(match_file);
                        if (Int32.TryParse(infos[2], out int testnumber) && testnumber > 0)
                        {                     
                            Matches.Add(new Matchimpro(infos[0], infos[1], testnumber));
                        }
                    }
                }
            }
        }

        public void Save_Match(StorageFolder pathfolder)
        {

        }
    }
}
