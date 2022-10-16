using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class IntroScript : GameObjectComponent
    {
        public override void Init()
        {

        }
        public void Set()
        {

        }
        public override void Start()
        {

        }
        public override void Update(GameTime gameTime) {
            if (Input.GetKey.IsKeyDown(Keys.Space))
            {

                WorldManager.LoadMap(new Samouczek());
            }
        }
    }
}
