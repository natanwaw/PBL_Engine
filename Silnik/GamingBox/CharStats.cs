using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Silnik.GamingBox
{
    class CharStats : GameObjectComponent
    {
        private Sprite sprite;
        private Rectangle spriteRect;
        //private WorldsTiles worldsTiles = new WorldsTiles();

        public override void Init()
        {
            sprite = gameObject.GetComponent<Sprite>();
        }
        public void Set()
        {
            //worldsTiles.UpdateGameObjectInTiles();
        }
        public override void Update(GameTime gameTime)
        {

        }
        public override void Start()
        {
            spriteRect = sprite.rect;
        }
        public void Damage(float healthPercent)
        {
            int temp = (int)((1 - healthPercent) * spriteRect.Height);
            sprite.rect.Y = spriteRect.Y + temp;
            sprite.rect.Height = spriteRect.Height - temp;
        }

    }
}
