using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class MeshRenderer : GameObjectComponent
    {
        Mesh mesh;
        Transform transform;
        public override void Init()
        {

        }
        public override void Update(GameTime gameTime)
        {

        }
        public void Set(Mesh Mesh)
        {
            mesh = Mesh;
            transform = mesh.gameObject.GetComponent<Transform>();
        }
        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            world = Matrix.CreateTranslation(transform.position);
            

            //view = Matrix.CreateLookAt(Game1.MainCam.)
            //world.Translation += transform.position;
            foreach (ModelMesh modelMesh in mesh.model.Meshes)
            {
                foreach (BasicEffect effect in modelMesh.Effects)
                {
                    effect.World = world;

                    effect.View = view;
                    effect.Projection = projection;

                    //effect.TextureEnabled = false;
                    //effect.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f);
                }
                modelMesh.Draw();
            }
        }
    }
}
