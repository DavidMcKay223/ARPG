using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public abstract class ActiveWeaponEffect
    {
        public bool IsFinished { get; protected set; }

        protected readonly List<Enemy> Targets = new();
        protected readonly Texture2D Texture;
        protected readonly float Duration;
        protected float Timer;
        protected Vector2 Position;

        protected readonly AbstractWeapon Weapon;

        protected ActiveWeaponEffect(AbstractWeapon weapon, Texture2D texture, float duration)
        {
            Weapon = weapon;
            Texture = texture;
            Duration = duration;
            Timer = duration;
        }

        public virtual void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Timer -= delta;

            if (Timer <= 0f)
            {
                IsFinished = true;
            }
        }

        public virtual void Update(GameTime gameTime, Vector2? ownerPosition)
        {
            Update(gameTime);
        }

        protected virtual void ApplyDamage(Enemy enemy, float baseDamage, int bounceIndex = 0)
        {
            float total = baseDamage * (float)Math.Pow(2, bounceIndex);
            enemy.TakeDamage((int)total);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Targets.Count == 0) return;

            Vector2 start = GetStartPosition();
            foreach (var target in Targets)
            {
                DrawLine(spriteBatch, Texture, start, target.CenterPosition, Color.Yellow, 2);
                start = target.CenterPosition;
            }
        }

        protected abstract Vector2 GetStartPosition();

        private void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 startPoint, Vector2 endPoint, Color color, int thickness)
        {
            Vector2 edge = endPoint - startPoint;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(pixel,
                new Rectangle((int)startPoint.X, (int)startPoint.Y, (int)edge.Length(), thickness),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0);
        }
    }
}
