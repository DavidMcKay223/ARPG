using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Evolutions
{
    public class RighteousFire : Ability
    {
        public override Keys ActivationKey => Keys.D1;
        public override float CooldownTime => 0f; // Always on toggle, if you want

        private bool _active = false;

        protected override void Activate(Player player)
        {
            _active = !_active;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_active)
            {
                foreach (var enemy in Game1.Instance.Enemies)
                {
                    if (Vector2.Distance(enemy.CenterPosition, Game1.Instance.Player.CenterPosition) < 100f)
                        enemy.TakeDamage(10); // Fire aura
                }
            }
        }
    }
}
