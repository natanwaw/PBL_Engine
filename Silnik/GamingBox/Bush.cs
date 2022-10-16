using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
	public class Bush : GameObjectComponent
	{
        GameObject hero;
        bool visited = false;
        float range = 10;
        public bool hide = false;
        public Bush()
		{

		}
        public override void Init()
        {


        }
        public override void Start()
        {
            hero = GameObject.Find("hero");
        }
        public override void Update(GameTime gameTime)
        {
            
                if (distance(hero.GetComponent<Transform>().position.X, hero.GetComponent<Transform>().position.Z, gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z) < range)
                {
                    visited = true;
                    hide = true;
                }
                else
                {
                    visited = false;
                    hide = false;
                }
            
            
        }

        public float getRange()
        {
            return range;
        }
        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }

    }
}
