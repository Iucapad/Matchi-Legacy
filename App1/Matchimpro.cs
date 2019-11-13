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
using Windows.Storage.Search;

namespace MatchiApp
{
    class Matchimpro
    {
        public string Team1 { get; set; } = "Equipe 1";
        public string Team2 { get; set; } = "Equipe 2";
        public int Rounds { get; set; } = 5;
        public List<string> Categories { get; set; }
        public string Name { get => $"{Team1} vs {Team2}"; }
        private StorageFile file;

        public Matchimpro(string t1, string t2, int r)
        {
            Team1 = t1;
            Team2 = t2;
            Rounds = r;
        }

        private Matchimpro(string t1, string t2, int r, StorageFile f) 
        {
            Team1 = t1;
            Team2 = t2;
            Rounds = r;
            file = f;
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

        public static async Task<Matchimpro> ReadFile(StorageFile file)
        {
            if (file.FileType.ToLower() != ".matchi")
                throw new ArgumentOutOfRangeException();

            IList<string> infos = await FileIO.ReadLinesAsync(file);

            if (!int.TryParse(infos[2], out int testnumber) || testnumber <= 0)
                throw new FormatException();

            return new Matchimpro(infos[0], infos[1], testnumber, file);
        }

        public static async Task<List<Matchimpro>> ReadFolder(StorageFolder folder)
        {
            QueryOptions options = new QueryOptions(CommonFileQuery.DefaultQuery, new List<string>(){".matchi"});
            SortEntry se = new SortEntry();
            se.PropertyName = "System.DateModified";
            se.AscendingOrder = false;
            options.SortOrder.Clear();
            options.SortOrder.Add(se);
            IReadOnlyList<StorageFile> files = await folder.CreateFileQueryWithOptions(options).GetFilesAsync();
            List<Matchimpro> output = new List<Matchimpro>();
            foreach (StorageFile file in files)
            {
                try
                {
                    output.Add(await ReadFile(file));
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return output;
        }

        public async void Save(StorageFolder folder)
        {
            string filename = $"{Team1}_vs_{Team2}.matchi";
            StorageFile f = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            if (f != file) DeleteFile();
            await FileIO.WriteTextAsync(f, $"{Team1}\n{Team2}\n{Rounds}");
            file = f;
        }

        public async void DeleteFile()
        {
            if (file is null) return;
            await file.DeleteAsync();
            file = null;
        }
    }
}
