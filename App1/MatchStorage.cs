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
        public static readonly StorageFolder Default_folder = ApplicationData.Current.LocalFolder;
        public StorageFolder Folder { get; set; }

        public MatchStorage()
        {
            Folder = Default_folder;
        }
        
        public MatchStorage(StorageFolder f)
        {
            Folder = f;
        }

    }
}
