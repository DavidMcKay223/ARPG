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
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Speed { get; set; }
        public int Damage { get; set; }
        public bool IsActive { get; set; }
        public float Lifespan { get; set; } // How long the projectile lasts in seconds
        private float _timer;

        // Bounding box for collision
        public Rectangle BoundingBox
        {
            get
            {
                if (Texture == null) return Rectangle.Empty;

                float scale = 0.05f;
                int scaledWidth = (int)(Texture.Width * scale);
                int scaledHeight = (int)(Texture.Height * scale);

                // Adjust position based on the origin used in Draw (center)
                int topLeftX = (int)(Position.X - scaledWidth / 2f);
                int topLeftY = (int)(Position.Y - scaledHeight / 2f);


                return new Rectangle(topLeftX, topLeftY, scaledWidth, scaledHeight);
            }
        }

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
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += Velocity * deltaTime;

            _timer += deltaTime;
            if (_timer >= Lifespan)
            {
                IsActive = false; // Deactivate projectile after lifespan expires
            }

            // Optional: Deactivate if it goes off-screen (adjust bounds as needed)
            // if (Position.X < 0 || Position.X > 1200 || Position.Y < 0 || Position.Y > 900)
            // {
            //     IsActive = false;
            // }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive && Texture != null)
            {
                Vector2 origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
                float scale = 0.05f;

                spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.White,
                    0f,
                    origin, // Using center origin
                    scale,  // Using scale
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
