using BlockHero.MonoGame.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Systems
{
    public class Shop
    {
        public List<GearItem> Inventory { get; } = new();

        public Shop()
        {
            for (int i = 0; i < 5; i++)
                Inventory.Add(GearFactory.CreateRandomGear());
        }

        public GearItem Buy(int index)
        {
            if (index < 0 || index >= Inventory.Count) return null;
            var item = Inventory[index];
            Inventory.RemoveAt(index);
            return item;
        }
    }
}
