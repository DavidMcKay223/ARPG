﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems
{
    public abstract class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rarity { get; set; }
    }
}
