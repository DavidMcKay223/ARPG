using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class PunchEffect : ActiveWeaponEffect
    {
        private readonly Vector2 _ownerPosition;
        private float _range = 50f;

        public PunchEffect(Vector2 ownerPosition, Punch weapon, Texture2D texture)
            : base(weapon, texture, duration: 0.2f)
        {
            _ownerPosition = ownerPosition;
            Position = _ownerPosition;
        }

        public override void Update(GameTime gameTime, Vector2? ownerPosition = null)
        {
            base.Update(gameTime, ownerPosition);

            foreach (var enemy in Game1.Instance.Enemies)
            {
                if (!enemy.IsActive || Targets.Contains(enemy))
                    continue;

                if (Vector2.Distance(enemy.CenterPosition, _ownerPosition) < _range)
                {
                    enemy.TakeDamage(Weapon.Damage);
                    Targets.Add(enemy);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;

            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.Red,
                0f,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                0.5f,
                SpriteEffects.None,
                0f
            );
        }

        protected override Vector2 GetStartPosition()
        {
            return _ownerPosition;
        }
    }
}
