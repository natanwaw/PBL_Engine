using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class BoxCollision : GameObjectComponent
    {
        public BoundingBox box;
        public BoundingSphere sphere;
        /// <summary>
        /// size of Bounding Box
        /// </summary>
        public Vector3 size = new Vector3(4f, 4f, 4f);
        /// <summary>
        /// static means no transform update and no collsion checking, bounding box is created once
        /// </summary>
        public bool IsStatic = true;
        public bool IsInteracting;
        private bool Collided;
        private bool Interacted;
        private bool Triggered = false;
        Transform transform;
        List<GameObject> world;
        private int choose;
        public bool isActive = true;

        //Vector3 pos;
        Vector3 prevPos;
        public BoxCollision(bool sphere = false)
        {
            //nie pobierac danych w konstruktorze!!! bo errory -BJ
            //transform = gameObject.GetComponent<Transform>();
            //collision();
            if(sphere == true)
            {
                choose = 1;
            }
            else
            {
                choose = 0;
            }
        }
        public void Set()
        {
            transform = gameObject.GetComponent<Transform>();
            CreateBoundingBox();
        }
        public void CreateBoundingBox()
        {
            box = new BoundingBox(new Vector3(transform.position.X - size.X / 2, transform.position.Y - size.Y, transform.position.Z - size.Z / 2),
                new Vector3(transform.position.X + size.X / 2, transform.position.Y + size.Y, transform.position.Z + size.Z / 2));
        }
        public void CreateBoundingSphere()
        {
            //sphere = new BoundingSphere();
            sphere.Center = new Vector3(transform.position.X, transform.position.Y, transform.position.Z);
            sphere.Radius = size.X;
        }
        public bool Collide(BoundingBox otherBox)
        {
            if (box.Intersects(otherBox))
            {
                return true;
            }
            return false;
        }
        private bool CollisionCheck(BoundingBox otherBox)
        {
            if (box.Intersects(otherBox))
            {
                return true;
            }
            return false;
        }
        private bool CollisionCheck(BoundingSphere sphere)
        {
            if (box.Intersects(sphere))
            {
                return true;
            }
            return false;
        }
        private bool TriggerCheck(BoundingBox otherBox)
        {
            if (box.Intersects(otherBox))
            {
                return true;
            }
            return false;
        }
        public override void Update(GameTime gameTime)
        {
            //czy zamienic World.gameObjects na public static?? -BJ
            //to byl wielki błąd jak się okazało -BJ
            Collided = false;
            if (isActive)
            {

                if (IsStatic)
                {

                }
                else
                {
                    CreateBoundingBox();
                    world = World.CollisionObjects;
                    foreach (GameObject gameOb in world)
                    {
                        if (gameOb.GetComponent<BoxCollision>() != null)
                            if (CollisionCheck(gameOb.GetComponent<BoxCollision>().box))
                                if (gameOb != gameObject)
                                    Collided = true;
                    }


                    if (Collided)
                    {
                        //transform.position = prevPos;
                    }
                    else
                    {
                        //prevPos = transform.position;
                    }
                }
                if (IsInteracting)
                {
                    foreach (GameObject gameOb in world)
                    {
                        if (gameOb.GetComponent<BoxCollision>() != null)
                            if (Interacted && gameOb.GetComponent<BoxCollision>().Interacted)
                                if (gameOb != gameObject)
                                    Triggered = TriggerCheck(gameOb.GetComponent<BoxCollision>().box);
                    }
                }
            }
            
        }

        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            //foreach (ModelMesh mesh in Model.Meshes)
            //{
            //    BoundingSphere transformed = mesh.BoundingSphere.Transform(
            //         modelTransforms[mesh.ParentBone.Index]);
            //    sphere = BoundingSphere.CreateMerged(sphere, transformed);
            //}
            //this.boundingSphere = sphere;
        }
    }
}
