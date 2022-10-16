using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Light
    {
        public Vector3 DLdirection { get; set; }
        public Vector3 DLambient { get; set; }
        public Vector3 DLdiffuse { get; set; }
        public Vector3 DLspecular { get; set; }

        public static Vector3 PLposition { get; set; }
        public float PLconstant { get; set; }
        public float PLlinear { get; set; }
        public float PLquadratic { get; set; }
        public Vector3 PLambient { get; set; }
        public Vector3 PLdiffuse { get; set; }
        public Vector3 PLspecular { get; set; }

        public Vector3 SLposition { get; set; }
        public Vector3 SLdirection { get; set; }
        public float SLcutOff { get; set; }
        public float SLouterCutOff { get; set; }
        public float SLconstant { get; set; }
        public float SLlinear { get; set; }
        public float SLquadratic { get; set; }
        public Vector3 SLambient { get; set; }
        public Vector3 SLdiffuse { get; set; }
        public Vector3 SLspecular { get; set; }

        public Light()
        {
            DLdirection = new Vector3(-1.0f, -1.0f, 1.0f);
            //DLambient = new Vector3(0.0031f, 0.0041f, 0.0053f);
            //DLdiffuse = new Vector3(0.0031f, 0.0041f, 0.0053f);
            //DLambient = new Vector3(0.1031f, 0.1041f, 0.1053f);
            DLambient = new Vector3(0.0f, 0.0f, 0.0f);
            //DLdiffuse = new Vector3(0.4031f, 0.4041f, 0.4053f);
            DLdiffuse = new Vector3(0.0f, 0.0f, 0.0f);
            DLspecular = new Vector3(0.0f, 0.0f, 0.0f);


            //PLposition = new Vector3(60.0f, 35.0f, -40.0f);
            PLconstant = 1.0f;
            PLlinear = 0.007f;
            PLquadratic = 0.0002f;
            PLambient = new Vector3(0.0f, 0.0f, 0.0f);
            PLdiffuse = new Vector3(0.16f, 0.16f, 0.16f);
            PLspecular = new Vector3(0.0f, 0.0f, 0.0f);

            SLposition = new Vector3(6000.0f, 35.0f, -4000.0f);
            SLdirection = new Vector3(1.0f, 1.0f, -1.0f);
            SLcutOff = (float)Math.Cos(0.3d);
            SLouterCutOff = (float)Math.Cos(0.5d);
            SLconstant = 1.0f;
            SLlinear = 0.01f;
            SLquadratic = 0.0003f;
            SLambient = new Vector3(0.0f, 0.0f, 0.0f);
            SLdiffuse = new Vector3(0.3f, 0.27f, 0.25f);
            //SLdiffuse = new Vector3(0.0f, 0.0f, 0.0f);
            //SLspecular = new Vector3(0.003f, 0.003f, 0.003f);
            SLspecular = new Vector3(0.0f, 0.0f, 0.0f);
        }
        public void SetEffectParameters(Effect effect, Camera camera, Matrix world)
        {
            if (effect.Parameters["DLdirection"] != null)
                effect.Parameters["DLdirection"].SetValue(DLdirection);
            if (effect.Parameters["DLambient"] != null)
                effect.Parameters["DLambient"].SetValue(DLambient);
            if (effect.Parameters["DLdiffuse"] != null)
                effect.Parameters["DLdiffuse"].SetValue(DLdiffuse);
            if (effect.Parameters["DLspecular"] != null)
                effect.Parameters["DLspecular"].SetValue(DLspecular);
            if (effect.Parameters["PLposition"] != null)
                effect.Parameters["PLposition"].SetValue(PLposition);
            if (effect.Parameters["PLconstant"] != null)
                effect.Parameters["PLconstant"].SetValue(PLconstant);
            if (effect.Parameters["PLlinear"] != null)
                effect.Parameters["PLlinear"].SetValue(PLlinear);
            if (effect.Parameters["PLquadratic"] != null)
                effect.Parameters["PLquadratic"].SetValue(PLquadratic);
            if (effect.Parameters["PLambient"] != null)
                effect.Parameters["PLambient"].SetValue(PLambient);
            if (effect.Parameters["PLdiffuse"] != null)
                effect.Parameters["PLdiffuse"].SetValue(PLdiffuse);
            if (effect.Parameters["PLspecular"] != null)
                effect.Parameters["PLspecular"].SetValue(PLspecular);
            if (effect.Parameters["SLposition"] != null)
                effect.Parameters["SLposition"].SetValue(camera.Position);
            if (effect.Parameters["SLdirection"] != null)
                effect.Parameters["SLdirection"].SetValue(camera.Forward);
            if (effect.Parameters["SLcutOff"] != null)
                effect.Parameters["SLcutOff"].SetValue(SLcutOff);
            if (effect.Parameters["SLouterCutOff"] != null)
                effect.Parameters["SLouterCutOff"].SetValue(SLouterCutOff);
            if (effect.Parameters["SLconstant"] != null)
                effect.Parameters["SLconstant"].SetValue(SLconstant);
            if (effect.Parameters["SLlinear"] != null)
                effect.Parameters["SLlinear"].SetValue(SLlinear);
            if (effect.Parameters["SLquadratic"] != null)
                effect.Parameters["SLquadratic"].SetValue(SLquadratic);
            if (effect.Parameters["SLambient"] != null)
                effect.Parameters["SLambient"].SetValue(SLambient);
            if (effect.Parameters["SLdiffuse"] != null)
                effect.Parameters["SLdiffuse"].SetValue(SLdiffuse);
            if (effect.Parameters["SLspecular"] != null)
                effect.Parameters["SLspecular"].SetValue(SLspecular);
            if (effect.Parameters["xView"] != null)
                effect.Parameters["xView"].SetValue(camera.View);
            if (effect.Parameters["xProjection"] != null)
                effect.Parameters["xProjection"].SetValue(camera.Projection);
            if (effect.Parameters["xWorld"] != null)
                effect.Parameters["xWorld"].SetValue(world);
            if (effect.Parameters["xCamPos"] != null)
                effect.Parameters["xCamPos"].SetValue(camera.Position);

        }
    }
}
