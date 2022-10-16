using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RiggedModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Animation : GameObjectComponent
    {
        Model model;
        AnimatedModel animatedModel;
        AnimationClip clip;
        AnimationCharacter character;

        Transform transform;
        Matrix modelPosition;
        TimeSpan animationSpeed;
        public bool shouldBeColored=false;
        public uint timer=0;
        //Control the flow of animation
        bool isPlaying;

        Texture2D texture;

        public Animation()
        {
            isPlaying = true;
        }
        public void Load(string path)
        {
            transform = gameObject.GetComponent<Transform>();
            animatedModel = new AnimatedModel(path);
            animatedModel.LoadContent(InitContent.Content);
            texture = InitContent.Content.Load<Texture2D>("animation/white");
        }
        public void Set(string Path, bool loop = true, int AnimationSpeed = 20)
        {
            Console.WriteLine(animatedModel.Clips.Count);
            Console.WriteLine(animatedModel.Clips[0].Name);
            clip = animatedModel.Clips[0];
        }
        public void Set(int startFrame = 0, int stopFrame = 0, bool loop = true)
        {

        }
        public override void Start()
        {
            character = animatedModel.PlayClip(clip);
            character.Looping = true;
        }
        public override void Update(GameTime gameTime)
        {
            modelPosition = transform.AbsoluteTransform;

            animatedModel.Update(gameTime);        
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
            animatedModel.Draw(InitContent.Graphics.GraphicsDevice, view, projection, modelPosition, texture);

           /*Matrix[] bones = animationCharacter.GetSkinTransforms();

            // Render the skinned mesh.
            foreach (ModelMesh mesh in model.Meshes)
            {
                Vector3 color = new Vector3(1);
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    effect.View = view;
                    effect.Projection = projection;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                    if (timer > 0)
                    {
                        color = effect.DiffuseColor;
                        effect.DiffuseColor = new Vector3(11, 0, 0);
                        timer--;
                    }
                }

                mesh.Draw();
                if (timer == 0)
                {

                
                     foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.DiffuseColor = color;
                    }
                }
            }*/
        }
        public void Resume()
        {
            isPlaying = true;
        }
        public void Pause()
        {
            isPlaying = false;
        }



    }
}
