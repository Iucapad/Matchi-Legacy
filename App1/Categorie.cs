using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Categorie
    {
        private string nom;
        private string nbrejouteur;

        public Categorie(string n)
        {
            nom = n;
            nbrejouteur = "Illimité";
        }

        public Categorie(string n, int j)
        {
            nom = n;
            nbrejouteur = j.ToString();
        }
    }
}
