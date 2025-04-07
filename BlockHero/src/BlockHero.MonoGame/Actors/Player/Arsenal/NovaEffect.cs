using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class NovaEffect : ActiveWeaponEffect
    {
        private class NovaProjectile
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Speed = 300f;
            public float Lifetime = 1f;
            public bool IsActive = true;

            public void Update(GameTime gameTime)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position += Direction * Speed * delta;
                Lifetime -= delta;
                if (Lifetime <= 0f)
                    IsActive = false;
            }
        }

        private List<NovaProjectile> _projectiles = new();
        private const int ProjectileCount = 12;
        private Vector2 _startPosition;

        public NovaEffect(Vector2 ownerPosition, Nova weapon, Texture2D texture)
            : base(weapon, texture, duration: 1.2f)
        {
            _startPosition = ownerPosition;

            for (int i = 0; i < ProjectileCount; i++)
            {
                float angle = MathHelper.TwoPi * i / ProjectileCount;
                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                _projectiles.Add(new NovaProjectile
                {
                    Position = ownerPosition,
                    Direction = dir
                });
            }
        }

        public override void Update(GameTime gameTime, Vector2? ownerPosition = null)
        {
            base.Update(gameTime, ownerPosition);

            foreach (var proj in _projectiles)
            {
                if (!proj.IsActive) continue;

                proj.Update(gameTime);

                foreach (var enemy in Game1.Instance.Enemies)
                {
                    if (!enemy.IsActive || Targets.Contains(enemy))
                        continue;

                    if (Vector2.Distance(enemy.CenterPosition, proj.Position) < 20f)
                    {
                        enemy.TakeDamage(Weapon.Damage);
                        Targets.Add(enemy); // Prevent double-hitting
                        break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var proj in _projectiles)
            {
                if (!proj.IsActive || Texture == null) continue;

                spriteBatch.Draw(Texture, proj.Position, null, Color.Cyan,
                    0f,
                    new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                    0.5f,
                    SpriteEffects.None,
                    0f);
            }
        }

        protected override Vector2 GetStartPosition() => _startPosition;
    }
}
