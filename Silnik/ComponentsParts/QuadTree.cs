using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class QuadTree
    {
        private const int MAX_OBJECTS = 50;
        private const int MAX_DEPTH = 15;
        private int level;
        private List<GameObject> objects;
        private Rectangle bounds;
        private QuadTree[] nodes;
        public int Count
        {
            get { return objects.Count; }
        }
        public QuadTree(int Level, Rectangle Bounds)
        {
            level = Level;
            objects = new List<GameObject>();
            bounds = Bounds;
            nodes = new QuadTree[4];
        }
        public void clear()
        {
            objects.Clear();
            for (int i = 0; i < 4; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].clear();
                }
            }
        }
        private void split()
        {
            int subWidth = bounds.Width / 2;
            int subHeight = bounds.Height / 2;
            int x = bounds.X;
            int y = bounds.Y;
            
            nodes[0] = new QuadTree(level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            nodes[1] = new QuadTree(level + 1, new Rectangle(x, y, subWidth, subHeight));
            nodes[2] = new QuadTree(level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            nodes[3] = new QuadTree(level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }
        private int getIndex(Rectangle Rect)
        {
            int index = -1;
            float verticalMidPoint = bounds.X + (bounds.Width / 2);
            float horizontalMidPoint = bounds.Y + (bounds.Height / 2);

            bool topQuadrant = (Rect.Y < horizontalMidPoint && Rect.Y + Rect.Height < horizontalMidPoint);
            bool bottomQuadrant = (Rect.Y > horizontalMidPoint);
            if (Rect.X < verticalMidPoint && Rect.X + Rect.Width < verticalMidPoint)
            {
                if (topQuadrant)
                    index = 1;
                else if (bottomQuadrant)
                    index = 2;
            }
            else if (Rect.X > verticalMidPoint)
            {
                if (topQuadrant)
                    index = 0;
                else if (bottomQuadrant)
                    index = 3;
            }
            return index;
        }
        public void Insert(GameObject ob)
        {
            if (nodes[0] != null)
            {
                int index = getIndex(ob.GetComponent<Collision>().Rect);
                if (index != -1)
                {
                    nodes[index].Insert(ob);
                    return;
                }
            }
            objects.Add(ob);
            if (objects.Count > MAX_OBJECTS && level < MAX_DEPTH)
            {
                if (nodes[0] == null)
                {
                    split();
                }
                List<GameObject> save = new List<GameObject>();
                foreach (GameObject go in objects)
                {
                    int index = getIndex(go.GetComponent<Collision>().Rect);
                    if (index != -1)
                    {
                        nodes[index].Insert(go);
                    }
                    else
                    {
                        save.Add(go);
                    }
                }
                objects = save;
            }
        }
        private void Remove(GameObject ob)
        {
            if (objects != null && objects.Contains(ob))
            {
                objects.Remove(ob);
            }
        }
        public void delete(GameObject ob)
        {
            bool objectRemoved = false;
            if (objects != null && objects.Contains(ob))
            {
                Remove(ob);
                objectRemoved = true;
            }
            if (nodes[0] != null && !objectRemoved)
            {
                nodes[0].delete(ob);
                nodes[1].delete(ob);
                nodes[2].delete(ob);
                nodes[3].delete(ob);
            }
            if (nodes[0] != null)
            {
                if (nodes[0].Count == 0 && nodes[1].Count == 0 && nodes[2].Count == 0 && nodes[3].Count == 0)
                {
                    nodes[0] = null;
                    nodes[1] = null;
                    nodes[2] = null;
                    nodes[3] = null;
                }
            }
        }
        public List<GameObject> Retrieve(Rectangle Rect)
        {
            int index = getIndex(Rect);
            List<GameObject> returnObjects = new List<GameObject>(objects);
            if (nodes[0] != null)
            {
                if (index != -1)
                {
                    returnObjects.AddRange(nodes[index].Retrieve(Rect));
                }
                else
                {
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        returnObjects.AddRange(nodes[i].Retrieve(Rect));
                    }
                }
            }
            return returnObjects;
        }
    }
}
