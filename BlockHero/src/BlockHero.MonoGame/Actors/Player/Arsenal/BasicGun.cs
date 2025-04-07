using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class BasicGun : AbstractWeapon
    {
        public override float CooldownTime => 0.5f;

        public override void LoadContent(ContentManager content)
        {
            _projectileTexture = content.Load<Texture2D>("Aesthetics/Sprites/projectile_sprite");
        }

        protected override void Attack(Vector2 ownerPosition)
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - ownerPosition;
            if (direction == Vector2.Zero)
                direction = Vector2.UnitX;

            Vector2 projectileStart = ownerPosition + new Vector2(20, 20);

            var projectile = new Projectile(
                _projectileTexture,
                projectileStart,
                direction,
                speed: 400f,
                damage: 10,
                lifespan: 2.0f
            );

            _newProjectiles.Add(projectile);
        }
    }
}
