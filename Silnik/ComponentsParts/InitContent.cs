using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class InitContent
    {
        static ContentManager content;
        static GraphicsDeviceManager graphics;
        static Random random = new Random(346435);
        public static void Init(ContentManager Content, GraphicsDeviceManager Graphics)
        {
            content = Content;
            graphics = Graphics;
        }
        public static ContentManager Content
        {
            get
            {
                return content;
            }
        }
        public static GraphicsDeviceManager Graphics
        {
            get
            {
                return graphics;
            }
        }
        public static Random Random
        {
            get
            {
                return random;
            }
        }
    }
}
