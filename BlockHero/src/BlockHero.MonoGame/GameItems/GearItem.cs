using BlockHero.MonoGame.Actors.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems
{
    public class GearItem : Item
    {
        public GearSlot Slot { get; set; }
        public List<IGearModifier> Modifiers { get; set; } = new();

        public void Apply(Player player)
        {
            foreach (var mod in Modifiers)
                mod.Apply(player);
        }

        public void Remove(Player player)
        {
            foreach (var mod in Modifiers)
                mod.Remove(player);
        }
    }
}
