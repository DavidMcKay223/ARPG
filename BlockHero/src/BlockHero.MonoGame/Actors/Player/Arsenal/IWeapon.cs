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
    public interface IWeapon
    {
        float CooldownTime { get; }
        bool CanAttack { get; }

        void LoadContent(ContentManager content);
        void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking);
        void Draw(SpriteBatch spriteBatch); // For drawing the weapon itself, if needed
        // Returns a list of new projectiles created this frame
        List<Projectile> GetNewProjectiles();
    }
}
