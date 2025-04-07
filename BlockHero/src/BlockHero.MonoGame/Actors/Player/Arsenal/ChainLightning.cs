using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ChainLightning : IWeapon
    {
        private Texture2D _lightningTexture;
        private float _cooldownTimer = 0f;
        private WeaponEffectManager _effectManager = new();

        public float CooldownTime => 1.5f;
        public int BaseDamage => 55;
        public float Range => 450f;
        public float BounceRange => 200f;
        public int MaxBounces => 6;
        public float LightningDuration => 0.25f;

        public bool CanAttack => _cooldownTimer <= 0f;

        public void LoadContent(ContentManager content)
        {
            _lightningTexture = content.Load<Texture2D>("Aesthetics/Sprites/white_pixel");
        }

        public void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_cooldownTimer > 0f)
                _cooldownTimer -= deltaTime;

            if (isAttacking && CanAttack)
            {
                _cooldownTimer = CooldownTime;
                PerformChainLightning(ownerPosition);
            }

            _effectManager.Update(gameTime);
        }

        private void PerformChainLightning(Vector2 startPosition)
        {
            if (Game1.Instance == null || Game1.Instance.Enemies == null || !Game1.Instance.Enemies.Any())
                return;

            var firstTarget = FindClosestEnemy(startPosition, Range, null, Game1.Instance.Enemies);
            if (firstTarget != null && firstTarget.IsActive)
            {
                var effect = new ChainLightningEffect(startPosition, firstTarget, this, _lightningTexture);
                _effectManager.AddEffect(effect);
            }
        }

        public Enemy FindClosestEnemy(Vector2 origin, float range, List<Enemy> ignoredTargets, List<Enemy> allEnemies)
        {
            Enemy closest = null;
            float minDistSq = float.MaxValue;
            foreach (var e in allEnemies)
            {
                if (!e.IsActive || (ignoredTargets != null && ignoredTargets.Contains(e)))
                    continue;
                float distSq = Vector2.DistanceSquared(origin, e.CenterPosition);
                if (distSq < minDistSq && distSq <= range * range)
                {
                    minDistSq = distSq;
                    closest = e;
                }
            }
            return closest;
        }

        public List<Projectile> GetNewProjectiles() => new();

        public void Draw(SpriteBatch spriteBatch)
        {
            _effectManager.Draw(spriteBatch);
        }
    }
}
