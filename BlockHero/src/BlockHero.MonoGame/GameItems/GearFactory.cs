using BlockHero.MonoGame.GameItems.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems
{
    public static class GearFactory
    {
        private static Random rng = new();

        public static GearItem CreateRandomGear()
        {
            var gear = new GearItem
            {
                Name = "Mystery Weapon",
                Slot = GearSlot.Weapon,
                Rarity = rng.Next(1, 5),
                Description = "???"
            };

            // Add random modifiers
            if (rng.NextDouble() < 0.5) gear.Modifiers.Add(new DoubleAttackModifier());
            if (rng.NextDouble() < 0.5) gear.Modifiers.Add(new RangeModifier());

            // Set description based on modifiers
            gear.Description = string.Join(", ", gear.Modifiers.ConvertAll(m => m.GetTooltip()));

            return gear;
        }

        public static GearItem CreateUnique(string id)
        {
            // Expand this for named uniques later
            if (id == "echo_blade")
            {
                return new GearItem
                {
                    Name = "Echo Blade",
                    Slot = GearSlot.Weapon,
                    Rarity = 5,
                    Description = "Attacks twice and has longer range",
                    Modifiers = new List<IGearModifier>
                {
                    new DoubleAttackModifier(),
                    new RangeModifier()
                }
                };
            }

            return null;
        }
    }
}
