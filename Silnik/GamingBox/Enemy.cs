using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class Enemy : GameObjectComponent
    {
        float x;
        float z;
        float speed;
        float visibility;
        float socialDistance;
        GameObject hero;

        public Enemy(GameObject h)
        {
            setHero(h);
            visibility = 1000;
            socialDistance = 10;
            setSpeed(400);
        }

        public override void Init()
        {
            
            
        }
        public override void Start() 
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            
            if (distance(getX(), hero.GetComponent<Transform>().position.X, getZ(), hero.GetComponent<Transform>().position.Z) < visibility)
            {
                if (getX() < hero.GetComponent<Transform>().position.X - socialDistance)
                {
                    moveRight();
                }
                if (getX() > hero.GetComponent<Transform>().position.X + socialDistance)
                {
                    moveLeft();
                }
                if (getZ() < hero.GetComponent<Transform>().position.Z - socialDistance)
                {
                    moveDown();
                }
                if (getZ() > hero.GetComponent<Transform>().position.Z + socialDistance)
                {
                    moveUp();
                }
            }
            gameObject.GetComponent<Transform>().position.X = x;
            gameObject.GetComponent<Transform>().position.Z = z;
        }
        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }
        public float getX()
        {
            return x;
        }
        public float getZ()
        {
            return z;
        }
        public void setHero(GameObject h)
        {
            hero = h;
        }
        public void setSpeed(float s)
        {
            speed = s;
        }
        protected bool moveUp()
        {
            z -= speed * Time.DeltaTime;
            return true;
        }

        protected bool moveDown()
        {
            z += speed * Time.DeltaTime;
            return true;
        }
        protected bool moveLeft()
        {
            x -= speed * Time.DeltaTime;
            return true;
        }
        protected bool moveRight()
        {
            x += speed * Time.DeltaTime;
            return true;
        }
    }
}
