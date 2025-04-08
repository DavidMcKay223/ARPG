using BlockHero.MonoGame.Actors.Player;
using BlockHero.MonoGame.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Systems
{
    public class Inventory
    {
        public List<GearItem> Items { get; } = new();
        public Dictionary<GearSlot, GearItem> Equipped { get; } = new();

        public void Equip(GearItem gear, Player player)
        {
            if (Equipped.TryGetValue(gear.Slot, out var old))
                old.Remove(player);

            Equipped[gear.Slot] = gear;
            gear.Apply(player);
        }

        public void Unequip(GearSlot slot, Player player)
        {
            if (Equipped.TryGetValue(slot, out var gear))
            {
                gear.Remove(player);
                Equipped.Remove(slot);
            }
        }
    }
}
