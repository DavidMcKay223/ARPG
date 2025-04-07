using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using BlockHero.MonoGame.Actors.Player.Bio;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class Hammer : AbstractWeapon
    {
        public override int ManaCost => 5;
        public override int Damage => 20;

        private WeaponEffectManager _effectManager = new();

        public override float CooldownTime => 0.5f;

        public Hammer(Stats stats) : base(stats)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _projectileTexture = content.Load<Texture2D>("Aesthetics/Sprites/projectile_sprite");
        }

        protected override void Attack(Vector2 ownerPosition)
        {
            if (_stats.SpendMana(ManaCost))
            {
                var effect = new HammerEffect(ownerPosition, this, _projectileTexture);
                _effectManager.AddEffect(effect);
            }
        }

        public override void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            base.Update(gameTime, ownerPosition, isAttacking);
            _effectManager.Update(gameTime, ownerPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _effectManager.Draw(spriteBatch);
        }
    }
}
