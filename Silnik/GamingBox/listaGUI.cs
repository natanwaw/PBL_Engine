using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class listaGUI : GameObject
    {
        public void loadLista()
        {

            GameObject ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("HP");
            ob.GetComponent<Sprite>().SetPercentage(90, 10, 10, 20);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);
        }
    }
}
