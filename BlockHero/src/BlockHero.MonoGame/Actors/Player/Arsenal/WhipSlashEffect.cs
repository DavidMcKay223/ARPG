using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BlockHero.MonoGame.Workroom;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class WhipSlashEffect : ActiveWeaponEffect
    {
        private float _arc = MathHelper.PiOver2; // 90 degree arc
        private float _range = 60f;
        private Vector2 _ownerPosition;

        public WhipSlashEffect(Vector2 ownerPosition, WhipSlash weapon, Texture2D texture)
            : base(weapon, texture, duration: 0.15f) // very quick slash
        {
            _ownerPosition = ownerPosition;
        }

        public override void Update(GameTime gameTime, Vector2? ownerPosition = null)
        {
            base.Update(gameTime, ownerPosition);

            if (IsFinished || ownerPosition == null)
                return;

            _ownerPosition = ownerPosition.Value;

            // Facing direction
            Vector2 direction = Game1.Instance.Player.GetFacingDirection();

            foreach (var enemy in Game1.Instance.Enemies.ToList())
            {
                if (!enemy.IsActive || Targets.Contains(enemy))
                    continue;

                Vector2 toEnemy = enemy.CenterPosition - _ownerPosition;
                if (toEnemy.Length() > _range)
                    continue;

                float angle = Math.Abs(Vector2Extensions.AngleBetween(direction, toEnemy));
                if (angle < _arc / 2f)
                {
                    enemy.TakeDamage(Weapon.Damage);
                    Targets.Add(enemy);

                    if (!enemy.IsActive)
                    {
                        // Spawn 2 new enemies on kill
                        Game1.Instance.SpawnEnemy();
                        Game1.Instance.SpawnEnemy();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture == null) return;

            // Draw arc slashes (just like a stylized effect)
            Vector2 direction = Game1.Instance.Player.GetFacingDirection();
            float angle = (float)Math.Atan2(direction.Y, direction.X);

            for (int i = -3; i <= 3; i++)
            {
                float t = i / 6f;
                float theta = angle + t * _arc;
                Vector2 pos = _ownerPosition + new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * _range;

                spriteBatch.Draw(Texture, pos, null, Color.Purple * 0.6f, 0f,
                    new Vector2(Texture.Width / 2f, Texture.Height / 2f), 0.5f, SpriteEffects.None, 0f);
            }
        }

        protected override Vector2 GetStartPosition()
        {
            return _ownerPosition;
        }
    }
}
