using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Text : GameObjectComponent
    {
        public SpriteFont TextFont { get; private set; }
        public Vector2 CenterScreen => new Vector2(InitContent.Graphics.GraphicsDevice.Viewport.Width / 2f, InitContent.Graphics.GraphicsDevice.Viewport.Height / 2f);
        public TextBox _textBox;
        private string text2;
        public override void Update(GameTime gameTime)
        {
            _textBox.Update();

            /*if (Input.GetKey.IsKeyDown(Keys.T))
            {
                if (!_textBox.Active)
                {
                    _textBox = new TextBox { Text = "Debug text.\n" };
                    _textBox.Initialize();
                }
            }*/
        }
        public string text
        {
            set
            {
                text2 = value;
            }
            get
            {
                return text2;
            }
        }
        public void Load(string path)
        {
            TextFont = InitContent.Content.Load<SpriteFont>("fonts/"+path);
            _textBox = new TextBox
            {
                Text = text2
            };

            _textBox.Initialize();

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _textBox.Draw(spriteBatch);
        }
    }
}
