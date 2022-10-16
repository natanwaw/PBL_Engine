using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Silnik
{
    public class NormalMapping : GameObjectComponent
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Model theMesh;

        Texture2D diffuseTexture;
        Texture2D normalTexture;

        // Shader container
        Effect effect;

        // Parameters for the shader 
        EffectParameter projectionParameter;
        EffectParameter viewParameter;
        EffectParameter worldParameter;
        EffectParameter ambientIntensityParameter;
        EffectParameter ambientColorParameter;

        // New parameters for diffuse light
        EffectParameter diffuseIntensityParameter;
        EffectParameter diffuseColorParameter;
        EffectParameter lightDirectionParameter;

        EffectParameter eyePosParameter;
        EffectParameter specularColorParameter;

        EffectParameter colorMapTextureParameter;
        EffectParameter normalMapTextureParameter;

        Matrix rotateMatrix;
        Matrix world, view, projection;
        float ambientLightIntensity;
        Vector4 ambientLightColor;
        Vector3 eyePos;

        double rotateCamera = 0.0f;

        public override void Update(GameTime gameTime)
        {
            // To do wywalenia 
            ambientLightIntensity = 0.1f;
            ambientLightColor = Color.White.ToVector4();

            rotateCamera += gameTime.ElapsedGameTime.Milliseconds / 1000.0;
            eyePos = new Vector3(0.0f, 2.0f, 5.0f);
            view = Matrix.CreateLookAt(eyePos, new Vector3(0, 2, 0), Vector3.Up);

            rotateMatrix = Matrix.CreateRotationY((float)rotateCamera);

            base.Update(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            ModelMesh mesh = theMesh.Meshes[0];
            ModelMeshPart meshPart = mesh.MeshParts[0];

            // Set parameters
            projectionParameter.SetValue(projection);
            viewParameter.SetValue(view);
            worldParameter.SetValue(world * rotateMatrix);
            ambientIntensityParameter.SetValue(ambientLightIntensity);
            ambientColorParameter.SetValue(ambientLightColor);
            diffuseColorParameter.SetValue(Color.White.ToVector4());
            diffuseIntensityParameter.SetValue(0.2f);
            specularColorParameter.SetValue(Color.White.ToVector4());
            eyePosParameter.SetValue(eyePos);
            colorMapTextureParameter.SetValue(diffuseTexture);
            normalMapTextureParameter.SetValue(normalTexture);

            Vector3 lightDirection = new Vector3(0, 1, -1);

            // Ensure the light direction is normalized, or the shader will give some weird results
            lightDirection.Normalize();
            lightDirectionParameter.SetValue(lightDirection);

            // Set the vertex source to the mesh's vertex buffer
            _graphics.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);

            // Set the current index buffer to the sample mesh's index buffer
            _graphics.GraphicsDevice.Indices = meshPart.IndexBuffer;

            effect.CurrentTechnique = effect.Techniques["Technique1"];

            for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
            {
                // EffectPass.Apply will update the device to begin using the state information defined in the current pass
                effect.CurrentTechnique.Passes[i].Apply();

                // Mesh contains all of the information required to draw the current mesh
                _graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
            }
        }

        public void Load(string path, string diffPath, string normPath)
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the model
            theMesh = InitContent.Content.Load<Model>(path);

            // Load the shader
            effect = InitContent.Content.Load<Effect>("NormalMapping");

            // Set up the parameters
            SetupShaderParameters();

            // calculate matrixes
            float aspectRatio = (float)_graphics.GraphicsDevice.Viewport.Width / (float)_graphics.GraphicsDevice.Viewport.Height;
            float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
            projection = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, 0.1f, 1000.0f);

            // Load dif and norm map
            diffuseTexture = InitContent.Content.Load<Texture2D>(diffPath);
            normalTexture = InitContent.Content.Load<Texture2D>(normPath);

            //create a default world matrix
            world = Matrix.Identity;
        }

        public void SetupShaderParameters()
        {
            // Bind the parameters with the shader.
            worldParameter = effect.Parameters["World"];
            viewParameter = effect.Parameters["View"];
            projectionParameter = effect.Parameters["Projection"];

            ambientColorParameter = effect.Parameters["AmbientColor"];
            ambientIntensityParameter = effect.Parameters["AmbientIntensity"];

            diffuseColorParameter = effect.Parameters["DiffuseColor"];
            diffuseIntensityParameter = effect.Parameters["DiffuseIntensity"];
            lightDirectionParameter = effect.Parameters["LightDirection"];

            eyePosParameter = effect.Parameters["EyePosition"];
            specularColorParameter = effect.Parameters["SpecularColor"];

            colorMapTextureParameter = effect.Parameters["ColorMap"];
            normalMapTextureParameter = effect.Parameters["NormalMap"];
        }

    }
}
