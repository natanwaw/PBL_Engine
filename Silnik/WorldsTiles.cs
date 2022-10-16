using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class WorldsTiles : GameObjectComponent
    {
        int terrainWidth = 1024;
        int terrainHeight = 1024;
        private float partX, partY;
        private float X, Y;
        private List<GameObject> world;
        private bool start = false;
        private Transform transform;
        /// <summary>
        /// integer - must be divided by 2
        /// </summary>
        private const int numberOfSlices = 8;
        private Vector2[,] tileOffset = new Vector2[numberOfSlices,numberOfSlices];
        private List<GameObject>[,] tileObjects = new List<GameObject>[numberOfSlices,numberOfSlices];

        

        public WorldsTiles()
        {
            
        }
        public void Set(Transform Transform)
        {
            transform = Transform;
            world = World.CollisionObjects;
            UpdateGameObjectInTiles();
        }
        public override void Init()
        {
            partX = terrainWidth / numberOfSlices;
            partY = terrainHeight / numberOfSlices;
            for(int i = 0; i<numberOfSlices; i++)
                for(int j = 0; j<numberOfSlices; j++)
                {
                    tileOffset[i, j] = new Vector2(i * partX, j * partY);
                    tileObjects[i, j] = new List<GameObject>();
                }
        }
        public void UpdateGameObjectInTiles()
        {
            for (int i = 0; i < numberOfSlices; i++)
                for (int j = 0; j < numberOfSlices; j++)
                {
                    tileObjects[i, j].Clear();
                    //tileObjects[i, j] = new List<GameObject>();
                }
            foreach (GameObject gameObject in world)
            {
                float x = gameObject.GetComponent<Transform>().position.X;
                //float x2 = x + partX;
                float y = gameObject.GetComponent<Transform>().position.Z * -1;
                //float y2 = y + partY;

                for (int i = 0; i < numberOfSlices; i++)
                    for (int j = 0; j < numberOfSlices; j++)
                    {
                        if(x>=tileOffset[i,j].X && x<tileOffset[i,j].X+partX)
                            if(y>=tileOffset[i,j].Y && y<tileOffset[i,j].Y+partY)
                            {
                                tileObjects[i, j].Add(gameObject);
                            }
                    }
            }
        }
        public void UpdateTiles()
        {
            for (int i = 0; i < numberOfSlices; i++)
                for (int j = 0; j < numberOfSlices; j++)
                {
                    if (X >= tileOffset[i, j].X - partX && X < tileOffset[i, j].X + partX*2)
                    {
                        if (Y >= tileOffset[i, j].Y - partY && Y < tileOffset[i, j].Y + partY*2)
                        {
                            foreach (GameObject gameObject in tileObjects[i, j])
                            {
                                gameObject.isActive = true;
                            }
                        }
                        else
                        {
                            foreach (GameObject gameObject in tileObjects[i, j])
                            {
                                gameObject.isActive = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (GameObject gameObject in tileObjects[i, j])
                        {
                            gameObject.isActive = false;
                        }
                    }


                }

        }
        public override void Start()
        {
            //world = World.gameObjects;
            if (world.Count != 0)
            {
                UpdateGameObjectInTiles();
            }
        }
        public override void Update(GameTime gameTime)
        {

            X = transform.position.X;
            Y = transform.position.Z*-1;
            UpdateGameObjectInTiles();
            UpdateTiles();

        }
    }
}
