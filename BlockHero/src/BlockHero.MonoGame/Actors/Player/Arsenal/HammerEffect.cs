using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class HammerEffect : ActiveWeaponEffect
    {
        private float _angle;
        private float _rotationSpeed = 6f; // radians per second
        private float _radius = 80f;
        private Vector2 _ownerPosition;

        public HammerEffect(Vector2 ownerPosition, Hammer weapon, Texture2D texture)
            : base(weapon, texture, duration: 3f) // hammer spins for 3 seconds
        {
            _ownerPosition = ownerPosition;
        }

        public override void Update(GameTime gameTime, Vector2? ownerPosition = null)
        {
            base.Update(gameTime, ownerPosition);

            if (IsFinished || ownerPosition == null)
                return;

            _ownerPosition = ownerPosition.Value;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _angle += _rotationSpeed * delta;

            // Hammer position relative to player
            Vector2 offset = new(
                (float)Math.Cos(_angle),
                (float)Math.Sin(_angle)
            );
            offset *= _radius;

            Position = _ownerPosition + offset;

            // Collision with enemies
            foreach (var enemy in Game1.Instance.Enemies)
            {
                if (!enemy.IsActive || Targets.Contains(enemy))
                    continue;

                if (Vector2.Distance(enemy.CenterPosition, Position) < 40f)
                {
                    enemy.TakeDamage(Weapon.Damage);
                    Targets.Add(enemy); // prevent repeated hits
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;

            spriteBatch.Draw(Texture, Position, null, Color.Orange, _angle,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                1.0f, SpriteEffects.None, 0f);
        }

        protected override Vector2 GetStartPosition()
        {
            return _ownerPosition;
        }
    }
}
