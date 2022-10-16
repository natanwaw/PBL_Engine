using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Silnik.GamingBox
{
    public class lista_prog
    {
        //lista gameObjects na swiecie
        public void loadLista()
        {
            Sprite sprite = new Sprite();
            Transform transform = new Transform();
            GameObject ob = new GameObject();
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            Mesh mesh = new Mesh();
            MeshRenderer meshRenderer = new MeshRenderer();
            //TestScript testScript = new TestScript();
            BoxCollision boxCollision = new BoxCollision();

            ob.AddComponent(transform);
            ob.GetComponent<Transform>().position = new Vector3(1,1,0);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("gray");
            ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, 1600, 1000);
            //ob.GetComponent<Sprite>().layer = 0;
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            sprite = new Sprite();
            transform = new Transform();
            ob = new GameObject();
            spriteRenderer = new SpriteRenderer();
            ob.name = "button02";
            ob.AddComponent(transform);
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("button_start");
            ob.GetComponent<Sprite>().rect = new Rectangle(700, 300, 200, 120);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.AddComponent(new Button());
            ob.GetComponent<Button>().Set();
            ob.AddComponent(new TestScript());
            ob.GetComponent<TestScript>();
            World.WorldGameObjects.Add(ob);
            sprite = new Sprite();
            transform = new Transform();
            ob = new GameObject();
            spriteRenderer = new SpriteRenderer();
            ob.AddComponent(transform);
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("button_option");
            ob.GetComponent<Sprite>().rect = new Rectangle(700, 430, 200, 120);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);
            sprite = new Sprite();
            transform = new Transform();
            ob = new GameObject();
            spriteRenderer = new SpriteRenderer();
            ob.AddComponent(transform);
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("button_exit");
            ob.GetComponent<Sprite>().rect = new Rectangle(700, 560, 200, 120);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);
            

        }
    }
}
