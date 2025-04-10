﻿using BlockHero.MonoGame.GameItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Bio
{
    public class Stats
    {
        // Player Stats
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Gold { get; set; } = 0;

        public int AttackCount { get; set; } = 1;
        public float Range { get; set; } = 50;

        // Core Attributes
        public int Strength { get; set; } = 10;
        public int Dexterity { get; set; } = 10;
        public int Vitality { get; set; } = 10;
        public int Energy { get; set; } = 10;

        // Derived Stats
        public int MaxHealth => Vitality * 10;
        public int MaxMana => Energy * 8;
        public int DamageModifier => Strength / 2;
        public int CritChance => Dexterity / 2;

        public int CurrentHealth { get; private set; }
        public int CurrentMana { get; private set; }

        public float HealthPercent => (float)CurrentHealth / MaxHealth;
        public float ManaPercent => (float)CurrentMana / MaxMana;

        public Stats()
        {
            CurrentHealth = MaxHealth;
            CurrentMana = MaxMana;
        }

        public void TakeDamage(int amount)
        {
            CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        }

        public bool SpendMana(int amount)
        {
            // If consuming mana (positive amount), make sure there's enough
            if (amount > 0 && CurrentMana < amount)
                return false;

            CurrentMana -= amount;

            // Clamp to max
            if (CurrentMana > MaxMana)
                CurrentMana = MaxMana;

            return true;
        }

        public void Regenerate(float healthRate = 1f, float manaRate = 1f)
        {
            CurrentHealth = Math.Min(CurrentHealth + (int)healthRate, MaxHealth);
            CurrentMana = Math.Min(CurrentMana + (int)manaRate, MaxMana);
        }

        public void Heal(int amount)
        {
            CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
        }

        public void RefillMana(int amount)
        {
            CurrentMana = Math.Min(CurrentMana + amount, MaxMana);
        }

        public void Recalculate()
        {
            CurrentHealth = Math.Min(CurrentHealth, MaxHealth);
            CurrentMana = Math.Min(CurrentMana, MaxMana);
        }

        public void AddBonusesFrom(GearItem item)
        {
            Strength += item.BonusStrength;
            Dexterity += item.BonusDexterity;
            Vitality += item.BonusVitality;
            Energy += item.BonusEnergy;
        }
    }
}
