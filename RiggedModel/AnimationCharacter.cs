using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RiggedModel;
using System.ComponentModel;
using Microsoft.Xna.Framework;


namespace RiggedModel
{
    public class AnimationCharacter
    {
        private float position = 0;

        bool blendingAnimation = false;
        private AnimationClip clip = null;
        private AnimationClip clip2 = null;
        private BoneInfo[] boneInfos;
        private BoneInfo[] boneInfos2;

        private int boneCnt;
        private AnimatedModel model = null;
        private bool looping = false;
        [Browsable(false)]
        public float Position
        {
            get { return position; }
            set
            {
                if (value > Duration)
                    value = Duration;

                position = value;
                foreach (BoneInfo bone in boneInfos)
                {
                    bone.SetPosition(position);
                }
            }
        }
        [Browsable(false)]
        public AnimationClip Clip { get { return clip; } }
        public AnimationClip Clip2 { get { return clip2; } }
        [Browsable(false)]
        public float Duration { get { return (float)clip.Duration; } }
        [Browsable(false)]
        public AnimatedModel Model { get { return model; } }
        public bool Looping { get { return looping; } set { looping = value; } }
        public AnimationCharacter(AnimationClip clip, AnimatedModel model)
        {
            this.clip = clip;
            this.model = model;

            boneCnt = clip.Bones.Count;
            boneInfos = new BoneInfo[boneCnt];

            for (int b = 0; b < boneInfos.Length; b++)
            {
                boneInfos[b] = new BoneInfo(clip.Bones[b]);

                boneInfos[b].SetModel(model);
            }

            Rewind();
        }
        public AnimationCharacter(AnimationClip clip, AnimationClip clip2, AnimatedModel model)
        {
            this.clip = clip;
            this.clip2 = clip2;
            this.model = model;
            blendingAnimation = true;
            boneCnt = clip.Bones.Count;

            boneInfos = new BoneInfo[boneCnt];
            boneInfos2 = new BoneInfo[boneCnt];


            for (int b = 0; b < boneInfos.Length; b++)
            {
                boneInfos[b] = new BoneInfo(clip.Bones[b]);
                boneInfos2[b] = new BoneInfo(clip2.Bones[b]);
                boneInfos[b].SetModel(model);
                boneInfos2[b].SetModel(model);
            }

            Rewind();
        }

        public void Rewind()
        {
            Position = 0f;
        }
        public void Update(GameTime gameTime)
        {
            Position = Position + (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (looping && Position >= Duration)
                Position = 0.04f;
        }
        private class BoneInfo
        {
            private int currentKeyframe = 0;
            private Bone assignedBone = null;
            public bool valid = false;
            private Quaternion rotation;
            public Vector3 translation;
            public AnimationClip.Keyframe Keyframe1;
            public AnimationClip.Keyframe Keyframe1a;
            public AnimationClip.Keyframe Keyframe2;
            public AnimationClip.Keyframe Keyframe2a;

            public AnimationClip.Bone ClipBone { get; set; }
            public AnimationClip.Bone ClipBone2 { get; set; }
            public Bone ModelBone { get { return assignedBone; } }
            public BoneInfo(AnimationClip.Bone bone)
            {
                this.ClipBone = bone;
                SetKeyframes();
                SetPosition(0);
            }
            public BoneInfo(AnimationClip.Bone bone, AnimationClip.Bone bone2)
            {
                this.ClipBone = bone;
                this.ClipBone2 = bone2;
                SetKeyframes();
                SetPosition(0);
            }
            public void SetPosition(float position)
            {
                List<AnimationClip.Keyframe> keyframes = ClipBone.Keyframes;
                if (keyframes.Count == 0)
                    return;

                while (position < Keyframe1.Time && currentKeyframe > 0)
                {
                    currentKeyframe--;
                    SetKeyframes();
                }

                while (position >= Keyframe2.Time && currentKeyframe < ClipBone.Keyframes.Count - 2)
                {
                    currentKeyframe++;
                    SetKeyframes();
                }

                if (Keyframe1 == Keyframe2)
                {
                    rotation = Keyframe1.Rotation;
                    translation = Keyframe1.Translation;
                }
                else
                {
                    float t = (float)((position - Keyframe1.Time) / (Keyframe2.Time - Keyframe1.Time));
                    rotation = Quaternion.Slerp(Keyframe1.Rotation, Keyframe2.Rotation, t);
                    translation = Vector3.Lerp(Keyframe1.Translation, Keyframe2.Translation, t);
                }

                valid = true;
                if (assignedBone != null)
                {
                    Matrix m = Matrix.CreateFromQuaternion(rotation);
                    m.Translation = translation;
                    assignedBone.SetCompleteTransform(m);
                }
            }
            /*
            public void SetPosition(float position)
            {
                List<AnimationClip.Keyframe> keyframes = ClipBone.Keyframes;
                List<AnimationClip.Keyframe> keyframes1 = ClipBone2.Keyframes;
                if (keyframes.Count == 0 || keyframes1.Count == 0)
                    return;
                while (position < Keyframe1.Time && currentKeyframe > 0 || position < Keyframe1.Time && currentKeyframe > 0)
                {
                    currentKeyframe--;
                    SetKeyframes();
                }
                while (position >= Keyframe2.Time && currentKeyframe < ClipBone.Keyframes.Count - 2)
                {
                    currentKeyframe++;
                    SetKeyframes();
                }

                if (Keyframe1 == Keyframe2)
                {
                    rotation = Keyframe1.Rotation;
                    translation = Keyframe1.Translation;
                }
                else
                {
                    float t = (float)((position - Keyframe1.Time) / (Keyframe2.Time - Keyframe1.Time));
                    rotation = Quaternion.Slerp(Keyframe1.Rotation, Keyframe2.Rotation, t);
                    translation = Vector3.Lerp(Keyframe1.Translation, Keyframe2.Translation, t);
                }

                valid = true;
                if (assignedBone != null)
                {
                    Matrix m = Matrix.CreateFromQuaternion(rotation);
                    m.Translation = translation;
                    assignedBone.SetCompleteTransform(m);
                }
            }
            */

            private void SetKeyframes()
            {
                if (ClipBone.Keyframes.Count > 0)
                {
                    Keyframe1 = ClipBone.Keyframes[currentKeyframe];
                    if (currentKeyframe == ClipBone.Keyframes.Count - 1)
                        Keyframe2 = Keyframe1;
                    else
                        Keyframe2 = ClipBone.Keyframes[currentKeyframe + 1];
                }
                else
                {
                    Keyframe1 = null;
                    Keyframe2 = null;
                }
            }
            public void SetModel(AnimatedModel model)
            {
                assignedBone = model.FindBone(ClipBone.Name);
            }
        }

    }
}
