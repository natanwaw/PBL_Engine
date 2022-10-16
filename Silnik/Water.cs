using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Water : GameObjectComponent
    {
        public CModel waterMesh { get; set; }
        Effect waterEffect;
        public GameTime gameTime { get; set; }
        Transform transform;

        public void Set()
        {
            transform = gameObject.GetComponent<Transform>();
            waterMesh = new CModel(InitContent.Content.Load<Model>("plane"), transform.position,
                transform.rotation, new Vector3(transform.scale.X, 1, transform.scale.Z));
            waterEffect = InitContent.Content.Load<Effect>("WaterEffect");
            waterMesh.SetModelEffect(waterEffect, false);
            waterEffect.Parameters["WaterNormalMap"].SetValue(InitContent.Content.Load<Texture2D>("water_normal"));
        }

        public void renderReflection(List<GameObject> gameObjects)
        {
            // Reflect the camera's properties across the water plane
            Vector3 reflectedCameraPosition = Game1.MainCam().Position;
            reflectedCameraPosition.Y = -reflectedCameraPosition.Y +
            waterMesh.Position.Y * 2;
            Vector3 reflectedCameraTarget = Game1.MainCam().LookAtDirection;
            reflectedCameraTarget.Y = -reflectedCameraTarget.Y;
            // Create a temporary camera to render the reflected scene
            Camera reflectionCamera = new Camera(InitContent.Graphics.GraphicsDevice, Game1.GejmWindo);
            reflectionCamera.reflectedCamera = true;
            reflectionCamera.Position = reflectedCameraPosition;
            reflectionCamera.LookAtDirection = reflectedCameraTarget;
            reflectionCamera.Update(new GameTime());
            // Set the reflection camera's view matrix to the water effect
            waterEffect.Parameters["ReflectedView"].SetValue(
            reflectionCamera.View);
            // Create the clip plane
            Vector4 clipPlane = new Vector4(0, 1, 0, -waterMesh.Position.Y);
            // Set the render target
            InitContent.Graphics.GraphicsDevice.SetRenderTarget(Game1.reflectionTarg);
            InitContent.Graphics.GraphicsDevice.Clear(Color.Black);
            // Draw all objects with clip plane
            RasterizerState originalRasterizerState = InitContent.Graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            InitContent.Graphics.GraphicsDevice.RasterizerState = rasterizerState;

            Game1.sky.Draw(reflectionCamera.View, reflectionCamera.Projection, reflectedCameraPosition);

            InitContent.Graphics.GraphicsDevice.RasterizerState = originalRasterizerState;
            
            foreach (GameObject GO in gameObjects)
            {
                if (GO.GetComponent<CModel>() != null)
                {
                    if (reflectionCamera.BoundingVolumeIsInView(GO.GetComponent<CModel>().BoundingSphere))
                    {
                        GO.GetComponent<CModel>().SetClipPlane(clipPlane);
                        GO.GetComponent<CModel>().Draw(reflectionCamera.View, reflectionCamera.Projection, reflectedCameraPosition);
                        GO.GetComponent<CModel>().SetClipPlane(null);
                    }
                        
                }
            }
            InitContent.Graphics.GraphicsDevice.SetRenderTarget(null);
            // Set the reflected scene to its effect parameter in
            // the water effect
            waterEffect.Parameters["ReflectionMap"].SetValue(Game1.reflectionTarg);
            waterEffect.Parameters["xWorld"].SetValue(Game1.MainCam().world);
            waterEffect.Parameters["xView"].SetValue(Game1.MainCam().View);
            waterEffect.Parameters["xProjection"].SetValue(Game1.MainCam().Projection);
            waterEffect.Parameters["Time"].SetValue(Time.TotalGameTime);
            waterMesh.Draw(Game1.MainCam().View, Game1.MainCam().Projection, Game1.MainCam().Position);
        }
        public void PreDraw(List<GameObject> gameObjects)
        {
            renderReflection(gameObjects);
        }
    }
}
