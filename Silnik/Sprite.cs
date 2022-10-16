using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Sprite : GameObjectComponent
    {
        public Texture2D texture;
        public Rectangle rect;
        /// <summary>
        /// rect of origin texture which will be displayed.
        /// </summary>
        public Rectangle sourceRectangle;
        /// <summary>
        /// color of texture, White = default
        /// </summary>
        public Color color = Color.White;
        public Vector2 origin = new Vector2(0, 0);
        public int layer = 0;
        public SpriteEffects spriteEffects = SpriteEffects.None;
        public float angle = 0;
        public override void Init()
        {

        }
        public override void Update(GameTime gameTime)
        {

        }

        public Sprite()
        {

        }
        public void Load(string path)
        {
            texture = InitContent.Content.Load<Texture2D>(path);
        }
        public void SetPercentage(float x, float y, float w, float h)
        {
            int width = InitContent.Graphics.GraphicsDevice.Viewport.Width;
            int height = InitContent.Graphics.GraphicsDevice.Viewport.Height;
            rect.X = (int)((x / 100.0f) * width);
            rect.Y = (int)((y / 100.0f) * height);
            rect.Width = (int)((w / 100.0f) * width);
            rect.Height = (int)((h / 100.0f) * height);
        }
    }
}
