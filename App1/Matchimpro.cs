using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Matchimpro
    {
        private string equipe1;
        private string equipe2;
        private int jouteurs;
        private List<Categorie> categories;

        public Matchimpro(string team1, string team2, List<Categorie> c, int jou)
        {
            equipe1 = team1;
            equipe2 = team2;
            categories = c;
            jouteurs = jou;
        }

        public string Equipe1 { get => equipe1; set => equipe1 = value; }
        public string Equipe2 { get => equipe2; set => equipe2 = value; }
        public List<Categorie> Categories { get => categories; set => categories = value; }

        public void AjouterCate(Categorie cate)
        {
            if (!categories.Contains(cate))
            {
                categories.Add(cate);
            }
        }

        public void RetirerCate(Categorie cate)
        {
            if (categories.Contains(cate))
            {
                categories.Remove(cate);
            }
        }

    }
}
