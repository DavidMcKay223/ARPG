using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Evolutions
{
    public abstract class Ability
    {
        public abstract Keys ActivationKey { get; }
        public abstract float CooldownTime { get; }

        private float _cooldownRemaining;

        public bool IsReady => _cooldownRemaining <= 0;

        public virtual void Update(GameTime gameTime)
        {
            if (_cooldownRemaining > 0)
                _cooldownRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void TryActivate(Player player)
        {
            if (IsReady && CanActivate(player))
            {
                Activate(player);
                _cooldownRemaining = CooldownTime;
            }
        }

        protected virtual bool CanActivate(Player player) => true;
        protected abstract void Activate(Player player);
    }
}
