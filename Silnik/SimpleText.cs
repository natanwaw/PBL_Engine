using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class SimpleText : GameObjectComponent
    {
        public string popText = "*POP*";

        public Vector2 pos = new Vector2(800, 200);

        public Color popColor = Color.White;

        SpriteFont popFont;

        public void Load(string path)
        {
            popFont = InitContent.Content.Load<SpriteFont>(path);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(popFont, popText, pos, popColor);
        }
    }
}
