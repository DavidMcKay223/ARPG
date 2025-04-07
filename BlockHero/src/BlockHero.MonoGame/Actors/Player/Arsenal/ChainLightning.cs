using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using BlockHero.MonoGame.Workroom;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ChainLightning : AbstractWeapon
    {
        private WeaponEffectManager _effectManager = new();

        public override float CooldownTime => 1.5f;
        public int BaseDamage => 55;
        public float Range => 450f;
        public float BounceRange => 200f;
        public int MaxBounces => 6;
        public float LightningDuration => 0.25f;

        protected override void Attack(Vector2 ownerPosition)
        {
            if (Game1.Instance?.Enemies == null || !Game1.Instance.Enemies.Any())
                return;

            var firstTarget = EnemyQuery.FindClosestEnemy(ownerPosition, Range, null, Game1.Instance.Enemies);
            if (firstTarget != null && firstTarget.IsActive)
            {
                var effect = new ChainLightningEffect(ownerPosition, firstTarget, this, _projectileTexture);
                _effectManager.AddEffect(effect);
            }
        }

        public override void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            base.Update(gameTime, ownerPosition, isAttacking);
            _effectManager.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _effectManager.Draw(spriteBatch);
        }
    }
}
