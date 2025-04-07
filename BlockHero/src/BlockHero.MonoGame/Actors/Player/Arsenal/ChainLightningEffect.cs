using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ChainLightningEffect : ActiveWeaponEffect
    {
        private readonly List<Enemy> _targets = new();
        private readonly Texture2D _texture;
        private readonly ChainLightning _weapon;
        private readonly Vector2 _startPosition;

        private float _timer;
        private int _bounceCount;
        private int _maxBounces;

        public ChainLightningEffect(Vector2 start, Enemy firstTarget, ChainLightning weapon, Texture2D texture)
        {
            _startPosition = start;
            _weapon = weapon;
            _texture = texture;
            _targets.Add(firstTarget);
            _timer = weapon.LightningDuration;
            _maxBounces = weapon.MaxBounces;
            ApplyDamage(firstTarget, 0);
        }

        public override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer -= delta;

            if (_timer <= 0f)
            {
                if (_bounceCount < _maxBounces)
                {
                    var lastTarget = _targets.Last();
                    var next = _weapon.FindClosestEnemy(lastTarget.CenterPosition, _weapon.BounceRange, _targets, Game1.Instance.Enemies);

                    if (next != null && next.IsActive)
                    {
                        _targets.Add(next);
                        _bounceCount++;
                        _timer = _weapon.LightningDuration;
                        ApplyDamage(next, _bounceCount);
                        return;
                    }
                }

                IsFinished = true;
            }
        }

        private void ApplyDamage(Enemy enemy, int bounceIndex)
        {
            float baseDamage = _weapon.BaseDamage;
            float total = baseDamage * (float)Math.Pow(2, bounceIndex);
            enemy.TakeDamage((int)total);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_targets.Count == 0) return;

            Vector2 start = _startPosition;
            foreach (var target in _targets)
            {
                DrawLine(spriteBatch, _texture, start, target.CenterPosition, Color.Yellow, 2);
                start = target.CenterPosition;
            }
        }

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
