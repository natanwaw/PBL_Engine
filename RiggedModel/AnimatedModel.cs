using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using RiggedModel;

namespace RiggedModel
{
    public class AnimatedModel
    {
        private Model model = null;
        private ModelExtra modelExtra = null;
        private List<Bone> bones = new List<Bone>();
        private string assetName = "";
        private AnimationCharacter player = null;
        public Model Model
        {
            get { return model; }
        }
        public List<Bone> Bones { get { return bones; } }
        public List<AnimationClip> Clips { get { return modelExtra.Clips; } }
        public AnimatedModel(string assetName)
        {
            this.assetName = assetName;

        }
        public void LoadContent(ContentManager content)
        {
            this.model = content.Load<Model>(assetName);
            modelExtra = model.Tag as ModelExtra;
            System.Diagnostics.Debug.Assert(modelExtra != null);

            ObtainBones();
        }
        private void ObtainBones()
        {
            bones.Clear();
            foreach (ModelBone bone in model.Bones)
            {
                Bone newBone = new Bone(bone.Name, bone.Transform, bone.Parent != null ? bones[bone.Parent.Index] : null);
                bones.Add(newBone);
            }
        }
        public Bone FindBone(string name)
        {
            foreach (Bone bone in Bones)
            {
                if (bone.Name == name)
                    return bone;
            }

            return null;
        }
        public AnimationCharacter PlayClip(AnimationClip clip)
        {
            player = new AnimationCharacter(clip, this);
            return player;
        }
        public void Update(GameTime gameTime)
        {
            if (player != null)
            {
                player.Update(gameTime);
            }
        }
        public void Draw(GraphicsDevice graphics, Matrix view, Matrix projection, Matrix world, Texture2D texture)
        {
            if (model == null)
                return;

            Matrix[] boneTransforms = new Matrix[bones.Count];

            for (int i = 0; i < bones.Count; i++)
            {
                Bone bone = bones[i];
                bone.ComputeAbsoluteTransform();

                boneTransforms[i] = bone.AbsoluteTransform;
            }

            Matrix[] skeleton = new Matrix[modelExtra.Skeleton.Count];
            for (int s = 0; s < modelExtra.Skeleton.Count; s++)
            {
                Bone bone = bones[modelExtra.Skeleton[s]];
                skeleton[s] = bone.SkinTransform * bone.AbsoluteTransform;
            }
            Vector3 SpecularColor = new Vector3(0.5f, 0.5f, 0.5f);
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (Effect effect in modelMesh.Effects)
                {
                    if (effect is BasicEffect)
                    {
                        BasicEffect beffect = effect as BasicEffect;
                        beffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        beffect.View = view;
                        beffect.Projection = projection;
                        //beffect.EnableDefaultLighting();
                    }

                    if (effect is SkinnedEffect)
                    {
                        SkinnedEffect seffect = effect as SkinnedEffect;
                        seffect.World = boneTransforms[modelMesh.ParentBone.Index] * world;
                        seffect.View = view;
                        seffect.Projection = projection;
                        seffect.Texture = texture;
                        seffect.SpecularColor = SpecularColor;
                        seffect.SpecularPower = 22.25f;
                        //seffect.DiffuseColor = new Vector3(0.03f, 0.03f, 0.03f);
                        seffect.EnableDefaultLighting();
                        seffect.SetBoneTransforms(skeleton);
                    }
                }

                modelMesh.Draw();
            }
        }

    }
}
