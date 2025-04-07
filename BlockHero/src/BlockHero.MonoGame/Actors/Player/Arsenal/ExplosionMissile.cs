using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ExplosionMissile : Projectile
    {
        private readonly float _explosionRadius = 60f;
        private bool _hasExploded = false;
        private float _timer = 0f;

        public ExplosionMissile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage, float lifespan)
            : base(texture, position, direction, speed, damage, lifespan)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (!_hasExploded)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += Velocity * delta;
                _timer += delta;

                if (_timer >= Lifespan)
                {
                    Explode();
                }
                else
                {
                    // Optional collision check — tweak distance as needed
                    foreach (var enemy in Game1.Instance.Enemies)
                    {
                        if (enemy.IsActive && Vector2.Distance(enemy.CenterPosition, Position) < 10f)
                        {
                            Explode();
                            break;
                        }
                    }
                }
            }
        }

        private void Explode()
        {
            _hasExploded = true;
            IsActive = false;

            foreach (var enemy in Game1.Instance.Enemies)
            {
                if (enemy.IsActive && Vector2.Distance(enemy.CenterPosition, Position) <= _explosionRadius)
                {
                    enemy.TakeDamage(Damage);
                }
            }

            // TODO: Add explosion VFX here if needed
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!_hasExploded && Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.Cyan,
                    0f,
                    new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                    1.0f,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
