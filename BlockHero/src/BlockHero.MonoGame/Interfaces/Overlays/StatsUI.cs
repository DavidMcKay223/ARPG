using BlockHero.MonoGame.Actors.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public class StatsUI
    {
        private SpriteFont _font;
        private Player _player;

        public StatsUI(Player player)
        {
            _player = player;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _font = content.Load<SpriteFont>("Aesthetics/Fonts/StatsFont"); // Make sure this font exists
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_font == null) return;

            var stats = _player.Stats;
            string statText = $"HP: {stats.CurrentHealth}/{stats.MaxHealth}  " +
                              $"Mana: {stats.CurrentMana}/{stats.MaxMana}  " +
                              $"STR: {stats.Strength}  DEX: {stats.Dexterity}  " +
                              $"VIT: {stats.Vitality}  ENG: {stats.Energy}";

            spriteBatch.DrawString(_font, statText, new Vector2(20, 20), Color.White);
        }
    }
}
