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
                Name = "Mystery " + GetRandomSlotName(),
                Slot = GetRandomSlot(),
                Rarity = rng.Next(1, 5),
                Description = "???"
            };

            // Add random modifiers
            if (rng.NextDouble() < 0.5) gear.Modifiers.Add(new DoubleAttackModifier());
            if (rng.NextDouble() < 0.5) gear.Modifiers.Add(new RangeModifier());

            // Add random stat bonuses (0 to 5 each)
            gear.BonusStrength = rng.Next(0, 6);
            gear.BonusDexterity = rng.Next(0, 6);
            gear.BonusVitality = rng.Next(0, 6);
            gear.BonusEnergy = rng.Next(0, 6);

            // Set description based on modifiers and stats
            var tooltipParts = gear.Modifiers.ConvertAll(m => m.GetTooltip());
            if (gear.BonusStrength > 0) tooltipParts.Add($"+{gear.BonusStrength} STR");
            if (gear.BonusDexterity > 0) tooltipParts.Add($"+{gear.BonusDexterity} DEX");
            if (gear.BonusVitality > 0) tooltipParts.Add($"+{gear.BonusVitality} VIT");
            if (gear.BonusEnergy > 0) tooltipParts.Add($"+{gear.BonusEnergy} ENG");

            gear.Description = string.Join(", ", tooltipParts);

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

        private static GearSlot GetRandomSlot()
        {
            Array values = Enum.GetValues(typeof(GearSlot));
            return (GearSlot)values.GetValue(rng.Next(values.Length));
        }

        private static string GetRandomSlotName()
        {
            return Enum.GetName(typeof(GearSlot), GetRandomSlot());
        }

        public static List<GearItem> CreateFullGearSet()
        {
            var set = new List<GearItem>();
            foreach (GearSlot slot in Enum.GetValues(typeof(GearSlot)))
            {
                var gear = CreateRandomGear();
                gear.Slot = slot;
                gear.Name = $"Mystery {slot}";
                set.Add(gear);
            }
            return set;
        }
    }
}
