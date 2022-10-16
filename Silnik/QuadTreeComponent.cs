using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class QuadTreeComponent : GameObjectComponent
    {
        //public List<GameObject> CollisionObjects;
        private QuadTree quadTree;
        public override void Init()
        {

        }
        //Start - wykonywane 1 raz tuż przed pierwszym Update
        public override void Start()
        {
            quadTree = new QuadTree(0, new Rectangle(0, 0, 1080, 1080));
            
        }
        //Update - wiadomo co i jak
        public override void Update(GameTime gameTime)
        {
            quadTree.clear();
            foreach (GameObject ob in World.CollisionObjects)
            {
                quadTree.Insert(ob);
            }
        }
        public List<GameObject> CollideObjects(GameObject ob)
        {
            return quadTree.Retrieve(ob.GetComponent<Collision>().Rect);
        }
        public List<GameObject> CollideObjectRect(Rectangle rect)
        {
            
            return quadTree.Retrieve(rect);
        }
    }
}
