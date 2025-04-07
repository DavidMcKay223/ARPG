using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class Projectile
    {
        private const float Scale = 1.0f;

        public Texture2D Texture { get; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; }
        public float Speed { get; }
        public int Damage { get; }
        public float Lifespan { get; }

        public bool IsActive { get; set; }

        private float _timer;
        private readonly Vector2 _origin;
        private readonly int _scaledWidth;
        private readonly int _scaledHeight;

        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, float speed, int damage, float lifespan)
        {
            Texture = texture;
            Position = position;
            Speed = speed;
            Velocity = Vector2.Normalize(direction) * speed;
            Damage = damage;
            Lifespan = lifespan;
            IsActive = true;
            _timer = 0f;

            _origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            _scaledWidth = (int)(texture.Width * Scale);
            _scaledHeight = (int)(texture.Height * Scale);
        }

        public Rectangle GetBoundingBox()
        {
            int x = (int)(Position.X - _scaledWidth / 2f);
            int y = (int)(Position.Y - _scaledHeight / 2f);
            return new Rectangle(x, y, _scaledWidth, _scaledHeight);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * deltaTime;

            _timer += deltaTime;
            if (_timer >= Lifespan)
            {
                IsActive = false;
            }

            // Optional off-screen check:
            if (Position.X < 0 || Position.X > 1200 || Position.Y < 0 || Position.Y > 900)
                IsActive = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) return;

            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.White,
                0f,
                _origin,
                Scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
