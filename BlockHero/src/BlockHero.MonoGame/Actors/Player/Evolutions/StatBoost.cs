using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Evolutions
{
    public class StatBoost : Ability
    {
        public override Keys ActivationKey => Keys.D3;
        public override float CooldownTime => 120f;

        private float _duration = 10f;
        private float _activeTime;

        protected override void Activate(Player player)
        {
            _activeTime = _duration;
            int boostAmount = 1000 * player.Stats.Level;

            player.Stats.Strength += boostAmount;
            player.Stats.Dexterity += boostAmount;
            player.Stats.Vitality += boostAmount;
            player.Stats.Energy += boostAmount;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_activeTime > 0)
            {
                _activeTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_activeTime <= 0)
                {
                    int boostAmount = 1000 * Game1.Instance.Player.Stats.Level;
                    var stats = Game1.Instance.Player.Stats;
                    stats.Strength -= boostAmount;
                    stats.Dexterity -= boostAmount;
                    stats.Vitality -= boostAmount;
                    stats.Energy -= boostAmount;
                }
            }
        }
    }
}
