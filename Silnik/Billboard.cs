using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Silnik.GamingBox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class Billboard : GameObjectComponent
    {

        public Vector3 height = new Vector3(0, 10, 0);
        GraphicsDevice graphicsDevice;
        AlphaTestEffect effect,redEffect;
        VertexPositionTexture[] vertices;

        Texture2D texture;
        Texture2D redTexture;
        float witdh=1;
        public Billboard()
        {
            graphicsDevice = Game1._graphics.GraphicsDevice;

            effect = new AlphaTestEffect(Game1._graphics.GraphicsDevice);

            effect.AlphaFunction = CompareFunction.Greater;
            effect.ReferenceAlpha = 128;




            redEffect = new AlphaTestEffect(Game1._graphics.GraphicsDevice);

            redEffect.AlphaFunction = CompareFunction.Greater;
            redEffect.ReferenceAlpha = 128;

            // Preallocate an array of four vertices.
            vertices = new VertexPositionTexture[4];

            vertices[0].Position = new Vector3(1, 1, 0);
            vertices[1].Position = new Vector3(-1, 1, 0);
            vertices[2].Position = new Vector3(1, -1, 0);
            vertices[3].Position = new Vector3(-1, -1, 0);
        }

        public void Load(string path)
        {
            texture = InitContent.Content.Load<Texture2D>(path);
            redTexture = InitContent.Content.Load<Texture2D>("enemy_red_bar");
        }

        public override void Update(GameTime gameTime)
        {
            witdh = gameObject.GetComponent<EnemiesStats>().health / gameObject.GetComponent<EnemiesStats>().maxHealth;
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
            if (witdh < 1) { 
            Matrix world2 = Matrix.CreateTranslation(0, 1, 0) *
                           Matrix.CreateScale(3) *
                           Matrix.CreateConstrainedBillboard(gameObject.GetComponent<Transform>().position+ height, Game1.MainCam().Position,
                                                             Vector3.Up, null, null);
            //Console.WriteLine("a");
            //quadDrawer.DrawQuad(texture, 1, world2, view, projection);



            effect.Texture = texture;

            effect.World = world2;
            effect.View = view;
            effect.Projection = projection;

            // Update our vertex array to use the specified number of texture repeats.
            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(1, 0);
            vertices[2].TextureCoordinate = new Vector2(0, 1);
            vertices[3].TextureCoordinate = new Vector2(1, 1);

            // Draw the quad.
            effect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);



            //////////// red bar
            //
            Matrix worldRed = Matrix.CreateTranslation(0, 1, 0) *
                           Matrix.CreateScale(3*witdh,3,3) *
                           Matrix.CreateConstrainedBillboard(gameObject.GetComponent<Transform>().position + height, Game1.MainCam().Position,
                                                             Vector3.Up, null, null);
            //Console.WriteLine("a");
            //quadDrawer.DrawQuad(texture, 1, world2, view, projection);



            redEffect.Texture = redTexture;

            redEffect.World = worldRed;
            redEffect.View = view;
            redEffect.Projection = projection;

            // Draw the quad.
            redEffect.CurrentTechnique.Passes[0].Apply();

            graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices, 0, 2);
            }
        }
    }
}
