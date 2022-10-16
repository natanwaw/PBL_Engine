using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class ProjectedTexture
    {
        public Vector3 ProjectorPosition { get; set; }
        public Vector3 ProjectorTarget { get; set; }
        public Texture2D projectedTexture { get; set; }
        public bool ProjectorEnabled { get; set; }
        public float Scale { get; set; }
        float halfWidth, halfHeight;
        public ProjectedTexture(Texture2D Texture,
            GraphicsDevice graphicsDevice)
        {
            ProjectorPosition = new Vector3(1081, 20000, -540.5f);
            ProjectorTarget = new Vector3(540.5f, 0, -540.5f);
            ProjectorEnabled = true;
            projectedTexture = Texture;
            // We determine how large the texture will be based on the
            // texture dimensions and a scaling value
            halfWidth = Texture.Width / 2.0f;
            halfHeight = Texture.Height / 2.0f;
            Scale = 1.0f / Game1.RTscale;
        }
        public void SetEffectParameters(Effect effect)
        {

            // Calculate an orthographic projection matrix for the
            // projector "camera"
            Matrix projection = Matrix.CreateOrthographicOffCenter(
            -halfWidth * Scale, halfWidth * Scale,
            -halfHeight * Scale, halfHeight * Scale,
            -10000, 10000);
            // Calculate view matrix as usual
            Matrix view = Matrix.CreateLookAt(ProjectorPosition,
            ProjectorTarget, Vector3.Up);
            if (effect.Parameters["ProjectorViewProjection"] != null)
                effect.Parameters["ProjectorViewProjection"].SetValue(
                view * projection);
            if (effect.Parameters["ProjectedTexture"] != null)
                effect.Parameters["ProjectedTexture"].SetValue(
                projectedTexture);
        }
    }
}
