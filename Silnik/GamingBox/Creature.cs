using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{

	public class Creature : GameObjectComponent
	{
        float x;
        float z;
        float speed;

        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }

        public override void Init() 
        {
            
        }
        public override void Start() 
        {
            x = gameObject.GetComponent<Transform>().position.X;
            z = gameObject.GetComponent<Transform>().position.Z;
        }
        public override void Update(GameTime gameTime) 
        {
            setSpeed(5);
            //Console.WriteLine("Cokolwiek", speed, x, z);
            gameObject.GetComponent<Transform>().position.X = x;
            gameObject.GetComponent<Transform>().position.Z = z;
            gameObject.GetComponent<Transform>().position.Z -= 0.5f;
        }
        

        public float getX()
        {
            return x;
        }
        public float getZ()
        {
            return z;
        }
        public Creature getCreature()
        {
            return this;
        }

        protected void setSpeed(float s)
        {
            speed = s;
        }

        protected bool moveUp()
        {
            x += speed;
            return true;
        }

        protected bool moveDown()
        {
            x -= speed;
            return true;
        }
        protected bool moveLeft()
        {
            z += speed;
            return true;
        }
        protected bool moveRight()
        {
            z -= speed;
            return true;
        }


    }
}