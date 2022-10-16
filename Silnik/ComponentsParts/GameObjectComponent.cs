using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class GameObjectComponent
    {
        public GameObject gameObject;
        public bool isActive = true;
        public int id;
        public GameObjectComponent() { }
        public virtual void Init() { }
        public virtual void Start() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(Matrix world, Matrix view, Matrix projection) { }
    }
}
