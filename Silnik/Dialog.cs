using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Silnik
{
    public class Dialog : GameObjectComponent
    {
        public SpriteFont DialogFont { get; private set; }
        public Vector2 CenterScreen
            => new Vector2(InitContent.Graphics.GraphicsDevice.Viewport.Width / 2f, InitContent.Graphics.GraphicsDevice.Viewport.Height / 2f);
        public DialogBox _dialogBox;
        private string text;
        /*public override void Start()
        {
            _dialogBox = new DialogBox { Text = "Debug dialog.\n" };
        }*/
        public override void Update(GameTime gameTime)
        {
            _dialogBox.Update();

        }
        public string Text
        {
            set
            {
                text = value;
            }
            get
            {
                return text;
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _dialogBox.Draw(spriteBatch);
        }

        public void Load(string path)
        {
            DialogFont = InitContent.Content.Load<SpriteFont>("fonts/"+path);
            _dialogBox = new DialogBox
            {
                Text = text
            };

            _dialogBox.Initialize();
        }

    }
}
