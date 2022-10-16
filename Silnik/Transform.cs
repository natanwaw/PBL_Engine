using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Transform : GameObjectComponent
    {
        public Transform()
        {
            position = Vector3.Zero;
            rotation = new Quaternion(0, 0, 0, 1);
            scale = Vector3.One;
        }
        public override void Init()
        {
            
        }
        public override void Start()
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        private Matrix absoluteTransform;
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                //RecreateMatrix(MatrixPosition, MatrixRotation, MatrixScale);
            }
        }
        public Matrix AbsoluteTransform
        {
            get
            {
                absoluteTransform = ModelMatrix();
                if(gameObject.parent == null)
                {
                    return absoluteTransform;
                }
                else
                {
                    return absoluteTransform * gameObject.parent.GetComponent<Transform>().AbsoluteTransform;
                }
            }
            private set
            {

                if (gameObject.children == null)
                {
                    absoluteTransform = value;
                }
                else
                {
                    absoluteTransform = value * gameObject.parent.GetComponent<Transform>().AbsoluteTransform;
                    foreach(GameObject child in gameObject.children)
                    {
                        child.GetComponent<Transform>().absoluteTransform = child.GetComponent<Transform>().AbsoluteTransform * value;
                    }
                }
                //absoluteTransform = value;
                //Vector3 s, p;
                //Quaternion r;
                //absoluteTransform.Decompose(out s, out r, out p);
                //position = p;
                //rotation = new Quaternion(MathHelper.ToDegrees(r.X), MathHelper.ToDegrees(r.Y), MathHelper.ToDegrees(r.Z), r.W);
                //scale = s;
            }
        }
        public Matrix LocalTransform
        {
            get; set;
        }
        public Matrix MatrixPosition
        {
            get
            {
                return Matrix.CreateTranslation(position);
            }
        }
        public Matrix MatrixRotation
        {
            get
            {
                return Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(rotation.Y), MathHelper.ToRadians(rotation.X), MathHelper.ToRadians(rotation.Z));
            }
        }
        public Matrix MatrixScale
        {
            get
            {
                return Matrix.CreateScale(scale);
            }
        }

        private Matrix ModelMatrix()
        {
            return MatrixScale
                * MatrixRotation
                * MatrixPosition;
        }
        /// <summary>
        /// look at object in X and Z coords
        /// </summary>
        public void LookAt(Vector3 target)
        {
            rotation = 
                new Quaternion(0f, -90 + 180.0f / (float)Math.PI 
                * (float)Math.Atan2(-target.Z + position.Z, target.X - position.X), 0f, 1f);
        }
        /// <summary>
        /// look at object in X and Z coords
        /// </summary>
        public static void LookAtTarget(Transform transformSource, Transform transformTarget, Vector3 target, float angleFix = -90f)
        {
            transformTarget.rotation =
                new Quaternion(0f, angleFix + 180.0f / (float)Math.PI
                * (float)Math.Atan2(-target.Z + transformSource.position.Z, target.X - transformSource.position.X), 0f, 1f);
        }
        private void RecreateMatrix(Matrix p, Matrix r, Matrix s)
        {
            AbsoluteTransform = s * r * p;
        }
        public static Vector3 PositionToMoveTowards(Vector3 Source, Vector3 Target, float Distance)
        {
            Vector3 direction = Target - Source;
            direction.Normalize();
            return direction;
        }
        public void UpdateMatrixInChildren()
        {

        }

    }
}
