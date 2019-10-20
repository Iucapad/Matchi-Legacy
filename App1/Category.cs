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

        public Category(string n)
        {
            name = n;
            nbrplayer = "Illimité";
        }

        public Category(string n, int j)
        {
            name = n;
            nbrplayer = j.ToString();
        }
    }
    
}
