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

        public int BonusStrength;
        public int BonusDexterity;
        public int BonusVitality;
        public int BonusEnergy;

        public void Apply(Player player)
        {
            foreach (var mod in Modifiers)
                mod.Apply(player);

            var stats = player.Stats;
            stats.Strength += BonusStrength;
            stats.Dexterity += BonusDexterity;
            stats.Vitality += BonusVitality;
            stats.Energy += BonusEnergy;
        }

        public void Remove(Player player)
        {
            foreach (var mod in Modifiers)
                mod.Remove(player);

            var stats = player.Stats;
            stats.Strength -= BonusStrength;
            stats.Dexterity -= BonusDexterity;
            stats.Vitality -= BonusVitality;
            stats.Energy -= BonusEnergy;
        }
    }
}
