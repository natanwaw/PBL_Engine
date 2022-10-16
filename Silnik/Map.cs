using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class Map : GameObjectComponent
    {
        public float X, Y;
        
        private GameObject hero;
        private float angle = (float)(5 * Math.PI / 4);

        public override void Start()
        {
            foreach (GameObject GO in World.AllGameObjects)
            {
                if (GO.tag == "hero")
                {
                    hero = GO;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            float tmpX, tmpY;
            X = hero.GetComponent<Transform>().position.X;
            Y = -hero.GetComponent<Transform>().position.Z;
            tmpX = (X / 1081 * 500) + 550 - 10;
            tmpY = (Y / 1081 * 500) + 150 - 10;
            //almost almost good           //tmpX = (Vector2.Distance(new Vector2(tmpX, tmpY), new Vector2(800, 400))*(float)Math.Cos(5*Math.PI/4)+tmpX);
            //almost almost good           //tmpY = (Vector2.Distance(new Vector2(tmpX, tmpY), new Vector2(800, 400)) * (float)Math.Sin(5 * Math.PI / 4) + tmpY);
            if((int)(((tmpX - 800) * Math.Cos(angle) - (tmpY - 400) * Math.Sin(angle) + 800) - 10)<800) gameObject.GetComponent<SpriteRenderer>().ChangeSize(new Rectangle((int)((tmpX-800)*Math.Cos(angle)-(tmpY-400)*Math.Sin(angle)+800-10+(2*(800 - ((tmpX - 800) * Math.Cos(angle) - (tmpY - 400) * Math.Sin(angle) + 800) ))), (int)((tmpX - 800) * Math.Sin(angle) + (tmpY - 400) * Math.Cos(angle) + 400)-25, 20, 20));
            else gameObject.GetComponent<SpriteRenderer>().ChangeSize(new Rectangle((int)(((tmpX - 800) * Math.Cos(angle) - (tmpY - 400) * Math.Sin(angle) + 800) - 10 - 2*((((tmpX - 800) * Math.Cos(angle) - (tmpY - 400) * Math.Sin(angle) + 800) ) - 800)), (int)((tmpX - 800) * Math.Sin(angle) + (tmpY - 400) * Math.Cos(angle) + 400) - 25, 20, 20));
            //almost almost good           //gameObject.GetComponent<SpriteRenderer>().ChangeSize(new Rectangle((int)tmpX,(int)tmpY, 20, 20));
            //Na sztywno środek            //gameObject.GetComponent<SpriteRenderer>().ChangeSize(new Rectangle(790, 390, 20, 20));
            //X = +0.1f;
            //Console.WriteLine((int)((tmpX - 800) * Math.Cos(angle) - (tmpY - 400) * Math.Sin(angle) + 800));
            //Console.WriteLine((int)(Vector2.Distance(new Vector2(tmpX, tmpY), new Vector2(800, 400)) * (float)Math.Sin(5 * Math.PI / 4) + tmpY));
            // Vector2.Distance()

        }
        public void Load()
        {
            //InitContent.Content
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
