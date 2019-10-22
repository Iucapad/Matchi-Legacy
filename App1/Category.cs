using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Category
    {
        private string name;
        private string nbrplayer;

        public Category(string n, string j)
        {
            name = n;

            switch (j)
            {
                case "Illimité":
                    nbrplayer = "Illimité";
                    break;

                case "Tous":
                    nbrplayer = "Tous";
                    break;
                default:
                    break;
            }
        }

        public Category(string n, int j)
        {
            name = n;
            nbrplayer = j.ToString();
        }
    }
    
}
