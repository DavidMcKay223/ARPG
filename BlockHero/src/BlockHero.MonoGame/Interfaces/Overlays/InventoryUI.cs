using BlockHero.MonoGame.GameItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public class InventoryUI : IOverlayUI
    {
        public bool Visible { get; private set; } = false;
        private const int Columns = 5;
        private const int Rows = 4;
        private const int SlotSize = 64;
        private const int Padding = 10;
        private readonly Rectangle _inventoryBounds;
        private readonly List<GearItem> _items;

        private GearItem _draggingItem;
        private Point _draggingOffset;
        private int _draggingIndex = -1;

        private Texture2D _placeholderIcon;
        private SpriteFont _font;

        public InventoryUI()
        {
            _inventoryBounds = new Rectangle(50, 50, Columns * (SlotSize + Padding) + Padding, Rows * (SlotSize + Padding) + 30 + Padding);
            _items = Game1.Instance.Player.Inventory.Items;
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _placeholderIcon = new Texture2D(graphicsDevice, 1, 1);
            _placeholderIcon.SetData(new[] { Color.OrangeRed });

            _font = content.Load<SpriteFont>("Aesthetics/Fonts/StatsFont");
        }

        public void Toggle()
        {
            Visible = !Visible;
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible) return;

            MouseState mouse = Mouse.GetState();
            Point mousePos = new Point(mouse.X, mouse.Y);

            if (mouse.LeftButton == ButtonState.Pressed && _draggingItem == null)
            {
                for (int i = 0; i < _items.Count; i++)
                {
                    var itemRect = GetItemSlotBounds(i);
                    if (itemRect.Contains(mousePos))
                    {
                        _draggingItem = _items[i];
                        _draggingOffset = mousePos - itemRect.Location;
                        _draggingIndex = i;
                        break;
                    }
                }
            }
            else if (mouse.LeftButton == ButtonState.Released && _draggingItem != null)
            {
                for (int i = 0; i < Rows * Columns; i++)
                {
                    var slotRect = GetItemSlotBounds(i);
                    if (slotRect.Contains(mousePos))
                    {
                        if (i != _draggingIndex)
                        {
                            var temp = (i < _items.Count) ? _items[i] : null;
                            if (_draggingIndex < _items.Count) _items[_draggingIndex] = temp;
                            if (i < _items.Count) _items[i] = _draggingItem;
                            else _items.Add(_draggingItem);
                        }
                        break;
                    }
                }
                _draggingItem = null;
                _draggingIndex = -1;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(Game1.Instance.WhitePixel, _inventoryBounds, Color.Gray * 0.8f);

            for (int i = 0; i < Rows * Columns; i++)
            {
                Rectangle slotRect = GetItemSlotBounds(i);
                spriteBatch.Draw(Game1.Instance.WhitePixel, slotRect, Color.Black * 0.2f);

                if (i < _items.Count && _items[i] != null && _items[i] != _draggingItem)
                {
                    // Draw placeholder icon
                    spriteBatch.Draw(_placeholderIcon, slotRect, Color.White);

                    // Draw item name below
                    if (_font != null && !string.IsNullOrWhiteSpace(_items[i].Name))
                    {
                        string name = _items[i].Name;
                        Vector2 textSize = _font.MeasureString(name);
                        Vector2 textPos = new Vector2(
                            slotRect.Center.X - textSize.X / 2,
                            slotRect.Bottom + 2
                        );
                        spriteBatch.DrawString(_font, name, textPos, Color.White);
                    }
                }
            }

            if (_draggingItem != null)
            {
                MouseState mouse = Mouse.GetState();
                Point drawPos = new Point(mouse.X - _draggingOffset.X, mouse.Y - _draggingOffset.Y);
                var drawRect = new Rectangle(drawPos, new Point(SlotSize, SlotSize));
                spriteBatch.Draw(_placeholderIcon, drawRect, Color.White);
            }
        }

        private Rectangle GetItemSlotBounds(int index)
        {
            int x = _inventoryBounds.X + Padding + (index % Columns) * (SlotSize + Padding);
            int y = _inventoryBounds.Y + Padding + (index / Columns) * (SlotSize + Padding);
            return new Rectangle(x, y, SlotSize, SlotSize);
        }
    }
}
