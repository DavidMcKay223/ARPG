using BlockHero.MonoGame.Actors.Player.Bio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class Nova : AbstractWeapon
    {
        public override int ManaCost => 20;
        public override float CooldownTime => 1.5f;
        public override int Damage => 30;

        private WeaponEffectManager _effectManager = new();
        private Texture2D _novaTexture;

        public Nova(Stats stats) : base(stats)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _novaTexture = content.Load<Texture2D>("Aesthetics/Sprites/lightning_ball_sprite");
        }

        protected override void Attack(Vector2 ownerPosition)
        {
            if (_stats.SpendMana(ManaCost))
            {
                var effect = new NovaEffect(ownerPosition, this, _novaTexture);
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
