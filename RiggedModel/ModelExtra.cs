﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiggedModel
{

    public class ModelExtra
    {

        private List<int> skeleton = new List<int>();
        public List<AnimationClip> clips = new List<AnimationClip>();
        public List<int> Skeleton { get { return skeleton; } set { skeleton = value; } }
        public List<AnimationClip> Clips { get { return clips; } set { clips = value; } }
    }
}
