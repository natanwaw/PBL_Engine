using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class CModel : GameObjectComponent
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        private Transform transform;
        public Model Model { get; private set; }
        private Matrix[] modelTransforms;
        private BoundingSphere boundingSphere;
        public bool NormalMapEnabled = false;
        public bool shouldBeColored = false;
        public bool isInfected = false;
        public Texture2D normal;
        public CModel()
        {

        }
        public CModel(Model Model, Vector3 Position, Quaternion Rotation, Vector3 Scale)
        {
            this.Model = Model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            buildBoundingSphere();
            generateTags();
            this.Position = Position;
            this.Rotation = Rotation;
            this.Scale = Scale;
        }
        public void Set(Mesh mesh)
        {

            Model = mesh.model;
            modelTransforms = new Matrix[Model.Bones.Count];
            Model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            buildBoundingSphere();
            generateTags();
            transform = gameObject.GetComponent<Transform>();
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.scale;
            gameObject.GetComponent<CModel>().SetModelEffect(Game1.effect, true);
        }
        public override void Update(GameTime gameTime)
        {
            //transform = gameObject.GetComponent<Transform>();
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.scale;

            //Vector2 mouseLocation = new Vector2(Input.GetMouse.X, Input.GetMouse.Y);
            //Viewport viewport = InitContent.Graphics.GraphicsDevice.Viewport;

            //if(Intersects(mouseLocation, transform.AbsoluteTransform, Game1.MainCam().View, Game1.MainCam().Projection, viewport))
            //{
            //    Console.WriteLine("X: {0} Z: {1}", transform.position.X, transform.position.Z);
            //}

        }

        //public Material Material { get; set; }
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            // Calculate the base transformation by combining
            // translation, rotation, and scaling
            Matrix baseWorld;
            if(transform == null)
            {
                baseWorld = Matrix.CreateScale(Scale)
                * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z)
                * Matrix.CreateTranslation(Position);
            }
            else
            {
                baseWorld = transform.AbsoluteTransform;
            }
            //    Matrix.CreateScale(Scale)
            //* Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X,
            //Rotation.Z)
            //* Matrix.CreateTranslation(Position);
            foreach (ModelMesh mesh in Model.Meshes)
            {
                int i = -1;
                Matrix localWorld = modelTransforms[mesh.ParentBone.Index] *
                baseWorld;
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    i++;
                    Effect effect = meshPart.Effect;
                    if (effect is BasicEffect)
                    {
                        ((BasicEffect)effect).World = localWorld;
                        ((BasicEffect)effect).View = View;
                        ((BasicEffect)effect).Projection = Projection;
                        ((BasicEffect)effect).EnableDefaultLighting();
                        ((BasicEffect)effect).DiffuseColor = new Vector3(0, 0, 0);
                        i++;
                        //Console.WriteLine(i);
                        //Console.WriteLine("Hello");
                    }
                    else
                    {
                        setEffectParameter(effect, "xWorld", localWorld);
                        setEffectParameter(effect, "xView", View);
                        setEffectParameter(effect, "xProjection", Projection);
                        setEffectParameter(effect, "xCamPos", CameraPosition);
                        setEffectParameter(effect, "NormalMapEnabled", NormalMapEnabled);
                        if (NormalMapEnabled)
                        {
                            setEffectParameter(effect, "NormalMap", normal);
                        }
                    }
                    if (shouldBeColored)
                        setEffectParameter(effect, "red", true);
                    else
                        setEffectParameter(effect, "red", false);
                    if (isInfected)
                        setEffectParameter(effect, "isInfected", true);
                    else
                        setEffectParameter(effect, "isInfected", false);
                }
                
                mesh.Draw();
            }
        }
        private void buildBoundingSphere()
        {
            BoundingSphere sphere = new BoundingSphere(Vector3.Zero, 0);
            // Merge all the model's built in bounding spheres
            foreach (ModelMesh mesh in Model.Meshes)
            {
                BoundingSphere transformed = mesh.BoundingSphere.Transform(
                     modelTransforms[mesh.ParentBone.Index]);
                sphere = BoundingSphere.CreateMerged(sphere, transformed);
            }
            this.boundingSphere = sphere;
        }

        public BoundingSphere BoundingSphere
        {
            get
            {
                // No need for rotation, as this is a sphere
                Matrix worldTransform;
                if (transform == null)
                {
                    worldTransform = Matrix.CreateScale(Scale)
                * Matrix.CreateTranslation(Position);
                }
                else
                {
                    worldTransform = transform.AbsoluteTransform;
                }
                BoundingSphere transformed = boundingSphere;
                transformed = transformed.Transform(worldTransform);
                return transformed;
            }
        }

        private void generateTags()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (part.Effect is BasicEffect)
                    {
                        BasicEffect effect = (BasicEffect)part.Effect;
                        MeshTag tag = new MeshTag(effect.DiffuseColor, effect.Texture,
                        effect.SpecularPower);
                        part.Tag = tag;
                    }
        }
        public void CacheEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    ((MeshTag)part.Tag).CachedEffect = part.Effect;
        }
        public void RestoreEffects()
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                    if (((MeshTag)part.Tag).CachedEffect != null)
                        part.Effect = ((MeshTag)part.Tag).CachedEffect;
        }
        public void SetModelEffect(Effect effect, bool CopyEffect)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    Effect toSet = effect;
                    
                    // Copy the effect if necessary
                    if (CopyEffect)
                        toSet = effect.Clone();
                    MeshTag tag = ((MeshTag)part.Tag);
                    // If this ModelMeshPart has a texture, set it to the effect
                    if (tag.Texture != null)
                    {
                        setEffectParameter(toSet, "BasicTexture", tag.Texture);
                        setEffectParameter(toSet, "TextureEnabled", true);

                    }
                    else
                        setEffectParameter(toSet, "TextureEnabled", false);
                    // Set our remaining parameters to the effect
                    setEffectParameter(toSet, "DiffuseColor", tag.Color);
                    setEffectParameter(toSet, "SpecularPower", tag.SpecularPower);
                    part.Effect = toSet;
                    //setEffectParameter(toSet, "ShadowBias", 1.0f / 5000.0f);
                }
        }
       
        public void setEffectParameter(Effect effect, string paramName, object val)
        {
            if (effect.Parameters[paramName] == null)
                return;
            if (val is Vector3)
                effect.Parameters[paramName].SetValue((Vector3)val);
            if (val is bool)
                effect.Parameters[paramName].SetValue((bool)val);
            if (val is Matrix)
                effect.Parameters[paramName].SetValue((Matrix)val);
            if (val is Texture2D)
                effect.Parameters[paramName].SetValue((Texture2D)val);

        }

        public void SetClipPlane(Vector4? Plane)
        {
            foreach (ModelMesh mesh in Model.Meshes)
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    if (part.Effect.Parameters["ClipPlaneEnabled"] != null)
                        part.Effect.Parameters["ClipPlaneEnabled"].
                        SetValue(Plane.HasValue);
                    if (Plane.HasValue)
                        if (part.Effect.Parameters["ClipPlane"] != null)
                            part.Effect.Parameters["ClipPlane"].SetValue(Plane.Value);
                }
        }
        //creating sphere for raycasting:
        public bool Intersects(Vector2 mouseLocation, Matrix world, Matrix view, Matrix projection, Viewport viewport)
        {
            for (int i = 0; i < Model.Meshes.Count; i++)
            {
                BoundingSphere sphere = Model.Meshes[i].BoundingSphere;
                sphere = sphere.Transform(world);
                float? distance = Raycast.RayDistance(sphere, mouseLocation, view, projection, viewport);
                if (distance != null)
                    return true;
            }
            return false;
        }
    }
}
