using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Raycast
    {
        public static Vector3 direction;
        public static Ray RayCalculation(Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mouseLocation.X, mouseLocation.Y, 0f), projection, view, Matrix.Identity);
            Vector3 farPoint = viewport.Unproject(new Vector3(mouseLocation.X, mouseLocation.Y, 1f), projection, view, Matrix.Identity);
            direction = farPoint - nearPoint;
            direction.Normalize();
            return new Ray(nearPoint, direction);
        }
        public static float? RayDistance(BoundingSphere sphere, Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = RayCalculation(mouseLocation, view, projection, viewport);
            return mouseRay.Intersects(sphere);
        }
        public static float? RayDistance(BoundingBox box, Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = RayCalculation(mouseLocation, view, projection, viewport);           
            float? res = mouseRay.Intersects(box);
            //Console.WriteLine(Hit(res));
            return res;
        }
        public static float? RayDistance(Plane plane, Vector2 mouseLocation, Matrix view, Matrix projection, Viewport viewport)
        {
            Ray mouseRay = RayCalculation(mouseLocation, view, projection, viewport);
            float? res = mouseRay.Intersects(plane);
            //Console.WriteLine(Hit(res));
            return res;
        }
        public static Vector3 Hit(float? res)
        {           
            if (res.HasValue)
            {
                Vector3 Target = Vector3.Zero + (res.Value * direction);
                return Target;
            }
            return Vector3.Zero;
        }
    }
}
