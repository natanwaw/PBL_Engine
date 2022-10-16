using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class TerrainRaycast : GameObjectComponent
    {
        //public BoundingBox TerrainBox;
        private Transform transform;
        private Transform player;
        public Vector3? HitPoint;
        Plane plane;
        //Init - wykonywane zaraz po dodaniu tego komponentu, ale przed dodaniem całego gameObjectu do świata gry
        public override void Init()
        {
            //TerrainBox = new BoundingBox(Vector3.Zero, new Vector3(Game1.terrain._terrainWidth, 10, -Game1.terrain._terrainHeight));
            transform = gameObject.GetComponent<Transform>();
            plane = new Plane(Vector3.Zero, Vector3.Up);
            
        }
        //Start - wykonywane 1 raz tuż przed pierwszym Update
        public override void Start()
        {
            player = GameObject.Find("hero_parent").GetComponent<Transform>();
        }
        //Update - wiadomo co i jak
        public override void Update(GameTime gameTime)
        {
            Vector2 mouseLocation = new Vector2(Input.GetMouse.X, Input.GetMouse.Y);
            Viewport viewport = InitContent.Graphics.GraphicsDevice.Viewport;
            plane = new Plane(player.position, Vector3.Up);

            if (Intersects(mouseLocation, transform.AbsoluteTransform, Game1.MainCam().View, Game1.MainCam().Projection, viewport))
            {
                //Console.WriteLine("Hit: {0} ", HitPoint);
            }
        }
        private bool Intersects(Vector2 mouseLocation, Matrix world, Matrix view, Matrix projection, Viewport viewport)
        {
            //float? distance = Raycast.RayDistance(TerrainBox, mouseLocation, view, projection, viewport);
            float? distance = Raycast.RayDistance(plane, mouseLocation, view, projection, viewport);
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X, mouseLocation.Y, 0f), projection, view, Matrix.Identity);
            HitPoint = (nearPoint + Raycast.direction * distance);
            if (distance != null)
                return true;
            return false;
        }

    }
}

