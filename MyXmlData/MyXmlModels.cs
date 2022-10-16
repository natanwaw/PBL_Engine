using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyXmlData
{
    public class MyLevel
    {

        public string Tag;

        public float PosX;
        public float PosY;
        public float PosZ;

        public float RotX;
        public float RotY;
        public float RotZ;
        public float RotW;

        public float ScalX;
        public float ScalY;
        public float ScalZ;

    }

    public class MyLevelsCollection
    {
        public List<MyLevel> levelsCollection;
    }
}
