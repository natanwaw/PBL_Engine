using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Silnik.GamingBox
{
    class TestScript : GameObjectComponent
    {
        private Transform transform;
        //private Button button;
        private GameObject button;
        //private WorldsTiles worldsTiles = new WorldsTiles();
        private bool paused=false;
        private bool mapped = false;
        public override void Init()
        {
            transform = gameObject.GetComponent<Transform>();
        }
        public void Set()
        {
            //worldsTiles.UpdateGameObjectInTiles();
        }
        public override void Start()
        {
            button = World.ButtonObjects.Find(x => x.name == "button01");
        }
        public override void Update(GameTime gameTime)
        {
            //gameObject.GetComponent<BoxCollision>().Collision();

            if (Input.IsKeyPressed(Keys.Up))
            {
                    gameObject.GetComponent<Transform>().position.Z -= 0.5f;
            }
            if(Input.IsKeyPressed(Keys.Down))
            {
                    gameObject.GetComponent<Transform>().position.Z += 0.5f;
            }
            if (Input.IsKeyPressed(Keys.Left))
            {
                    gameObject.GetComponent<Transform>().position.X -= 0.5f;
            }
            if (Input.IsKeyPressed(Keys.Right))
            {
                    gameObject.GetComponent<Transform>().position.X += 0.5f;
            }
            if(transform.position.X>0 && transform.position.X<1024)
                if(transform.position.Z<0 && transform.position.Z>-1024)
                    transform.position.Y = Game1.terrain.getHeight(new Vector2(transform.position.X, transform.position.Z))+8;
            //worldsTiles.UpdateTiles(transform.position.X, transform.position.Z);
            if(Input.IsKeyDown(Keys.T))
            {
                gameObject.GetComponent<Animation>().Pause();
            }
            if(Input.IsKeyUp(Keys.T))
            {
                gameObject.GetComponent<Animation>().Resume();

            }

        }

    }
}
