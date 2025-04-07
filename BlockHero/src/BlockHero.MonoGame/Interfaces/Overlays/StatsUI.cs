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
        private Texture2D _barTexture;

        public StatsUI(Player player)
        {
            _player = player;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content, GraphicsDevice graphicsDevice)
        {
            _font = content.Load<SpriteFont>("Aesthetics/Fonts/StatsFont");
            _barTexture = new Texture2D(graphicsDevice, 1, 1);
            _barTexture.SetData(new[] { Color.White });
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

            Vector2 hpPos = new Vector2(20, 50);
            Vector2 manaPos = new Vector2(20, 65);

            int barWidth = 100;
            int barHeight = 10;

            spriteBatch.Draw(_barTexture, new Rectangle((int)hpPos.X, (int)hpPos.Y, (int)(barWidth * stats.HealthPercent), barHeight), Color.Red);
            spriteBatch.Draw(_barTexture, new Rectangle((int)manaPos.X, (int)manaPos.Y, (int)(barWidth * stats.ManaPercent), barHeight), Color.Blue);
        }
    }
}
