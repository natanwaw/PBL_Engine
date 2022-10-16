using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class Prefab : GameObject
    {
        public static GameObject GrappleHook()
        {
            GameObject ob = new GameObject();
            ob.name = "GrappleHook";
            //parent = GameObject.FindFromListUnsafe("weapon_holder");
            //ob.parent = parent;
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("cube");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new Hook());
            return ob;
        }

        public static GameObject fragmentOfTheMap()
        {
            GameObject ob = new GameObject();
            ob.name = "FragmentOfTheMap";
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("map");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<Transform>().scale = new Vector3(0.25f);
            ob.AddComponent(new FragmentOfTheMap());
            return ob;
        }
    }
}
