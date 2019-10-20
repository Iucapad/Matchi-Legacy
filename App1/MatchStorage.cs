using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace App1
{
    class MatchStorage
    {
        private StorageFolder folder;//dossier de stockage du match choisi par l'utilisateur
        private StorageFolder default_folder;//dossier de stockage du match par défaut

        public MatchStorage()
        {
            default_folder = ApplicationData.Current.LocalFolder;
            folder = default_folder;
        }
        
        public MatchStorage(StorageFolder f)
        {
            folder = f;
        }

        public StorageFolder Default_folder { get => default_folder; set => default_folder = value; }
        public StorageFolder Folder { get => folder; set => folder = value; }
    }
}
