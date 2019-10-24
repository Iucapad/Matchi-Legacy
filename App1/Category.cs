using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class Category
    {
        public string Name { get; private set; }//nom catégorie
        public int PlayerCount { get; private set; }//nombre de jouteur pour la catégorie

        public Category(string name, int playerCount) {
            if (playerCount < -1)
                throw new ArgumentOutOfRangeException("playerCount", "Player count must be positive or -1.");
            Name = name;
            PlayerCount = playerCount;
        }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            Category other = (Category)obj;
            return Name == other.Name && PlayerCount == other.PlayerCount;
        }

        public override string ToString() {
            return $"{Name}\n{PlayerCount}";
        }
    }
    
}
