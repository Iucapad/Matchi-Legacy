﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    class Category
    {
        public string Name { get; private set; }
        public int PlayerCount { get; private set; }

        public Category(string name, int playerCount) {
            if (playerCount < -1)
                throw new ArgumentOutOfRangeException("playerCount", "Player count must be positive or -1.");
            Name = name;
            PlayerCount = playerCount;
        }
    }
    
}
