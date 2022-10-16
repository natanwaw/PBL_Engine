using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class FragmentOfTheMap : GameObjectComponent
    {
        GameObject hero;
        bool collected = false;
        public float increasingSizeTime = 0.4f;
        public float decreasingSizeTime = 0.7f;
        private float timer = 0f;
        public override void Start()
        {
            hero = GameObject.FindByTag("hero");
        }
        public override void Update(GameTime gameTime)
        {
            if (!collected)
            {
                if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 8)
                {
                    if (gameObject.name == "wieza")
                    {
                        Hero.fragmentyMapyWiezy++;
                    }
                    if (gameObject.name == "skoczek")
                    {
                        Hero.fragmentyMapySkoczka++;
                    }
                    if (gameObject.name == "goniec")
                    {
                        Hero.fragmentyMapyGonca++;
                    }
                    collected = true;
                }
            }
            else
            {
                if (timer < increasingSizeTime)
                {
                    timer += Time.DeltaTime;
                    gameObject.GetComponent<Transform>().scale = new Vector3(0.25f) + new Vector3(0.25f) * timer/increasingSizeTime;
                }
                else if (timer < increasingSizeTime + decreasingSizeTime)
                {
                    timer += Time.DeltaTime;
                    gameObject.GetComponent<Transform>().scale = new Vector3(0.5f) - new Vector3(0.5f) * (timer - increasingSizeTime) / decreasingSizeTime;
                }
                else
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }

        public float distance(Vector3 w1, Vector3 w2)
        {
            double dis = 0.0;
            dis = Math.Sqrt((w2.X - w1.X) * (w2.X - w1.X) + (w2.Y - w1.Y) * (w2.Y - w1.Y) + (w2.Z - w1.Z) * (w2.Z - w1.Z));
            return (float)dis;
        }
    }
}
