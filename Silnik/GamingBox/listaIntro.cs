using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class listaIntro
    {
        public void loadLista()
        {
            Sprite sprite = new Sprite();
            Transform transform = new Transform();
            GameObject ob = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();

            ob.AddComponent(transform);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("intro");
            //ob.GetComponent<Sprite>().Load("introscreen");
            //ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, 1600, 800);
            ob.GetComponent<Sprite>().SetPercentage(0, 0, 100, 100);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.AddComponent(new IntroScript());
            World.WorldGameObjects.Add(ob);

        }
    }
}
