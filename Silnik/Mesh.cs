using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Mesh : GameObjectComponent
    {
        public Model model;
        public Mesh()
        {

        }
        public override void Init()
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
        public void Load(string path)
        {
            model = InitContent.Content.Load<Model>(path);
        }

    }
}
