using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Evolutions
{
    public class KillAllSplit : Ability
    {
        public override Keys ActivationKey => Keys.D4;
        public override float CooldownTime => 60f;

        protected override void Activate(Player player)
        {
            var enemies = Game1.Instance.Enemies.ToList();
            foreach (var enemy in enemies)
            {
                if (!enemy.IsActive) continue;

                enemy.IsActive = false;

                for (int i = 0; i < 10; i++)
                {
                    Game1.Instance.SpawnEnemy();
                    //newEnemy.GoldDrop *= 10;
                }
            }
        }
    }
}
