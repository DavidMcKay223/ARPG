using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Bio
{
    public class Stats
    {
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
            if (CurrentMana < amount)
                return false;

            CurrentMana -= amount;
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

    }
}
