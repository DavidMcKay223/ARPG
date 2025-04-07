using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class WeaponEffectManager
    {
        private readonly List<ActiveWeaponEffect> _effects = new();

        public void AddEffect(ActiveWeaponEffect effect)
        {
            _effects.Add(effect);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = _effects.Count - 1; i >= 0; i--)
            {
                _effects[i].Update(gameTime);
                if (_effects[i].IsFinished)
                    _effects.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var effect in _effects)
            {
                effect.Draw(spriteBatch);
            }
        }
    }
}
