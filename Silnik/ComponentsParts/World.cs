using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class World
    {
        private List<GameObject> gameObjects = new List<GameObject>();
        public static List<GameObject> WorldGameObjects = new List<GameObject>();
        public static List<GameObject> DeletedGameObjects = new List<GameObject>();
        public static List<GameObject> CollisionObjects = new List<GameObject>();
        public static List<GameObject> AllGameObjects = new List<GameObject>();
        public static List<GameObject> ButtonObjects = new List<GameObject>();

        private bool startBool = true;

        public World()
        {

        }
        /*
        public void AddGameObject(GameObject gameObject)
        {
            if(gameObject.GetComponent<BoxCollision>()!=null)
            {
                CollisionObjects.Add(gameObject);
            }
            if (gameObject.GetComponent<Button>() != null)
                ButtonObjects.Add(gameObject);
            AllGameObjects.Add(gameObject);
            gameObjects.Add(gameObject);
        }*/
        public void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in WorldGameObjects)
            {
                if (gameObject.GetComponent<Collision>() != null)
                {
                    CollisionObjects.Add(gameObject);
                }
                if (gameObject.GetComponent<Button>() != null)
                    ButtonObjects.Add(gameObject);
                AllGameObjects.Add(gameObject);
                gameObjects.Add(gameObject);
            }
            foreach (GameObject gameOb in DeletedGameObjects)
            {
                gameObjects.Remove(gameOb);
            }

            WorldGameObjects.Clear();
            DeletedGameObjects.Clear();
            foreach (GameObject gameOb in gameObjects)
            {
                gameOb.Start();
            }
            foreach (GameObject gameOb in gameObjects)
            {
                gameOb.Update(gameTime);
            }

        }
        //DODAC DRAW!!
        public void Draw(SpriteBatch spriteBatch)
        {
            int nModelsDrawn = 0;


            InitContent.Graphics.GraphicsDevice.SetRenderTarget(Game1.RTprojectedTexture);
            InitContent.Graphics.GraphicsDevice.Clear(Color.Black);
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            InitContent.Graphics.GraphicsDevice.RasterizerState = rasterizerState;

            //Vector2 start = new Vector2(0, 0);
            //Vector2 end = new Vector2(1081, 1081);
            Game1._spriteBatch2.Begin();
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)
                {
                    if (gameObject.GetComponent<GamingBox.Wieza>() != null)
                    {
                        if (gameObject.GetComponent<GamingBox.Wieza>().isDuringSpecialAttack)
                        {
                            float distance = gameObject.GetComponent<GamingBox.Wieza>().specialAttackDistance * gameObject.GetComponent<GamingBox.Wieza>().specialAttackSpeed *
                                (1 - gameObject.GetComponent<GamingBox.Wieza>().actualSpecialAttackDistance / gameObject.GetComponent<GamingBox.Wieza>().specialAttackDistance);
                            Vector2 start = new Vector2(-gameObject.GetComponent<Transform>().position.Z, gameObject.GetComponent<Transform>().position.X);
                            Vector2 end = new Vector2(start.X - gameObject.GetComponent<GamingBox.Wieza>().targetHeroPosition.Z * distance, start.Y + gameObject.GetComponent<GamingBox.Wieza>().targetHeroPosition.X * distance);
                            start *= Game1.RTscale;
                            end *= Game1.RTscale;
                            Game1._spriteBatch2.Draw(Game1.point, start, null, Color.Red,
                                (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                                 new Vector2(0f, 0f),
                                 new Vector2(Vector2.Distance(start, end) / 16f, 1f / 8f),
                                 SpriteEffects.None, 0f);
                        }
                        if (gameObject.GetComponent<GamingBox.Wieza>().isTheSpecialAttackCharging)
                        {
                            float colortmp = gameObject.GetComponent<GamingBox.Wieza>().chargingTime / gameObject.GetComponent<GamingBox.Wieza>().skillChargeDuration;
                            float distance = gameObject.GetComponent<GamingBox.Wieza>().specialAttackDistance * gameObject.GetComponent<GamingBox.Wieza>().specialAttackSpeed;
                            Vector2 start = new Vector2(-gameObject.GetComponent<Transform>().position.Z, gameObject.GetComponent<Transform>().position.X);
                            Vector2 end = new Vector2(start.X - gameObject.GetComponent<GamingBox.Wieza>().targetHeroPosition.Z * distance, start.Y + gameObject.GetComponent<GamingBox.Wieza>().targetHeroPosition.X * distance);
                            start *= Game1.RTscale;
                            end *= Game1.RTscale;
                            Game1._spriteBatch2.Draw(Game1.point, start, null, new Color((colortmp), (colortmp/2f), 0, 1),
                                (float)Math.Atan2(end.Y - start.Y, end.X - start.X),
                                 new Vector2(0f, 0f),
                                 new Vector2(Vector2.Distance(start, end) / 16f, 1f / 8f),
                                 SpriteEffects.None, 0f);
                        }
                    }
                    else if (gameObject.GetComponent<GamingBox.Skoczek>() != null)
                    {
                        if (gameObject.GetComponent<GamingBox.Skoczek>().isDuringSpecialAttack)
                        {
                            Vector2 target = new Vector2(-gameObject.GetComponent<GamingBox.Skoczek>().heroPositon.Z - 3, gameObject.GetComponent<GamingBox.Skoczek>().heroPositon.X - 3) * Game1.RTscale;
                            Game1._spriteBatch2.Draw(Game1.circle, target, null, Color.Red,
                                 0f,
                                 new Vector2(0f, 0f),
                                 new Vector2(1f, 1f) * Game1.RTscale / 5f,
                                 SpriteEffects.None, 0f);
                        }
                        if (gameObject.GetComponent<GamingBox.Skoczek>().isTheSpecialAttackCharging)
                        {
                            float colortmp = gameObject.GetComponent<GamingBox.Skoczek>().chargingTime / gameObject.GetComponent<GamingBox.Skoczek>().skillChargeDuration;
                            Vector2 target = new Vector2(-gameObject.GetComponent<GamingBox.Skoczek>().hero.GetComponent<Transform>().position.Z - 3, gameObject.GetComponent<GamingBox.Skoczek>().hero.GetComponent<Transform>().position.X - 3) * Game1.RTscale;
                            Game1._spriteBatch2.Draw(Game1.circle, target, null, new Color(colortmp, colortmp/2f, 0, 1),
                                 0f,
                                 new Vector2(0f, 0f),
                                 new Vector2(colortmp, colortmp) * Game1.RTscale / 5f,
                                 SpriteEffects.None, 0f);
                        }
                    }
                }
            }
            Game1._spriteBatch2.End();
            InitContent.Graphics.GraphicsDevice.SetRenderTarget(null);

            //KR
            //kazdy GameObject ktory ma komponent CModel moze byc oswietlony przez nasze swiatlo,
            //moze tez rzucac cienie na inne CModele
            //teraz czary beda sie dzialy - KR

            //drawShadowDepthMap
            Vector3 ShadowLightPosition = new Vector3(Game1.MainCam().Position.X, Game1.MainCam().Position.Y, Game1.MainCam().Position.Z);
            //Vector3 ShadowLightPosition = new Vector3(0,30,0);
            Vector3 ShadowLightTarget = new Vector3(Game1.MainCam().Position.X + 30, Game1.MainCam().Position.Y - 30, Game1.MainCam().Position.Z - 20);
            Matrix shadowView = Matrix.CreateLookAt(ShadowLightPosition, ShadowLightTarget, Vector3.Up);
            //Matrix shadowProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 1, 0.1f, 500.0f);
            Matrix shadowProjection = Matrix.CreateOrthographic(250, 250, 0.1f, 500.0f);
            InitContent.Graphics.GraphicsDevice.SetRenderTarget(Game1.shadowDepthTarg);
            InitContent.Graphics.GraphicsDevice.Clear(Color.White);

            InitContent.Graphics.GraphicsDevice.RasterizerState = rasterizerState;
            InitContent.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            Game1.terrain._effect = Game1.shadowDepthEffect;
            //light.SetEffectParameters(shadowDepthEffect, camera, Matrix.Identity);
            Game1.terrain._effect.Parameters["xView"].SetValue(shadowView);
            Game1.terrain._effect.Parameters["xProjection"].SetValue(shadowProjection);
            Game1.terrain._effect.Parameters["xWorld"].SetValue(Matrix.Identity);
            Game1.terrain.Draw();

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)
                {
                    if (gameObject.GetComponent<CModel>() != null)
                    {
                        if (Game1.MainCam().BoundingVolumeIsInView(gameObject.GetComponent<CModel>().BoundingSphere))
                        {
                            gameObject.GetComponent<CModel>().CacheEffects();
                            gameObject.GetComponent<CModel>().SetModelEffect(Game1.shadowDepthEffect, false);
                            gameObject.GetComponent<CModel>().Draw(shadowView, shadowProjection, ShadowLightPosition);
                            gameObject.GetComponent<CModel>().RestoreEffects();
                            nModelsDrawn++;
                        }

                        //gameObject.Draw(Matrix.Identity, Game1.MainCam().View, Game1.MainCam().Projection);
                    }
                }
            }

            // Un-set the render targets
            InitContent.Graphics.GraphicsDevice.SetRenderTarget(null);
            InitContent.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            InitContent.Graphics.GraphicsDevice.RasterizerState = rasterizerState;

            //renderowanie odbicia w wodzie - KR
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)
                {
                    if (gameObject.GetComponent<Water>() != null)
                    {
                        if (Game1.MainCam().BoundingVolumeIsInView(gameObject.GetComponent<Water>().waterMesh.BoundingSphere))
                        {
                            gameObject.GetComponent<Water>().renderReflection(gameObjects);
                        }
                    }
                }
            }



            //RasterizerState originalRasterizerState = InitContent.Graphics.GraphicsDevice.RasterizerState;
            /*RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            InitContent.Graphics.GraphicsDevice.RasterizerState = rasterizerState;*/


            Game1.sky.Draw(Game1.MainCam().View, Game1.MainCam().Projection, Game1.MainCam().Position);

            //InitContent.Graphics.GraphicsDevice.RasterizerState = originalRasterizerState;






            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)
                {
                    if (gameObject.GetComponent<CModel>() != null)
                    {
                        foreach (ModelMesh mesh in gameObject.GetComponent<CModel>().Model.Meshes)
                            foreach (ModelMeshPart part in mesh.MeshParts)
                            {
                                Game1.light.SetEffectParameters(part.Effect, Game1.MainCam(), Matrix.CreateTranslation(gameObject.GetComponent<CModel>().Position));

                                if (part.Effect.Parameters["DoShadowMapping"] != null)
                                    part.Effect.Parameters["DoShadowMapping"].SetValue(true);
                                if (part.Effect.Parameters["ShadowMap"] != null)
                                    part.Effect.Parameters["ShadowMap"].SetValue(Game1.shadowDepthTarg);
                                if (part.Effect.Parameters["ShadowView"] != null)
                                    part.Effect.Parameters["ShadowView"].SetValue(shadowView);

                                if (part.Effect.Parameters["ShadowProjection"] != null)
                                    part.Effect.Parameters["ShadowProjection"].
                                    SetValue(shadowProjection);
                                if (part.Effect.Parameters["ShadowLightPosition"] != null)
                                    part.Effect.Parameters["ShadowLightPosition"].
                                    SetValue(ShadowLightPosition);
                            }
                    }
                }
            }

            //light.SetEffectParameters(effect, camera, Matrix.Identity);
            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)
                {
                    if (gameObject.GetComponent<CModel>() != null)
                    {

                        if (Game1.MainCam().BoundingVolumeIsInView(gameObject.GetComponent<CModel>().BoundingSphere))
                        {
                            nModelsDrawn++;
                            gameObject.GetComponent<CModel>().Draw(Game1.MainCam().View, Game1.MainCam().Projection, Game1.MainCam().Position);
                        }
                    }
                }
            }
            //Console.WriteLine(nModelsDrawn);



            Game1.projectedTexture.SetEffectParameters(Game1.terrainEffect);
            Game1.terrain._effect = Game1.terrainEffect;

            //InitContent.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, new Vector4(0.1f, 0.105f, 0.11f, 1.0f), 1.0f, 0);
            Game1.light.SetEffectParameters(Game1.terrainEffect, Game1.MainCam(), Matrix.Identity);
            Game1.terrain._effect.Parameters["ShadowView"].SetValue(shadowView);
            Game1.terrain._effect.Parameters["ShadowProjection"].SetValue(shadowProjection);
            Game1.terrain._effect.Parameters["ShadowMap"].SetValue(Game1.shadowDepthTarg);
            Game1.terrain.Draw();


            //InitContent.Graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (GameObject gameObject in gameObjects)
            {
                if (gameObject.isActive)// && gameObject.GetComponent<CModel>() == null)
                {
                    gameObject.Draw(Matrix.Identity, Game1.MainCam().View, Game1.MainCam().Projection);
                    if (gameObject.GetComponent<SpriteRenderer>() != null)
                    {
                        gameObject.GetComponent<SpriteRenderer>().Draw(spriteBatch);
                    }

                    if (gameObject.GetComponent<Dialog>() != null)
                    {
                        gameObject.GetComponent<Dialog>().Draw(spriteBatch);
                    }

                    /*
                    if (gameObject.GetComponent<SimpleText>() != null)
                    {
                        gameObject.GetComponent<SimpleText>().Draw(spriteBatch);
                    }
                    */


                    if (gameObject.GetComponent<Text>() != null)
                    {
                        gameObject.GetComponent<Text>().Draw(spriteBatch);
                    }

                }

            }

        }
    }
}
