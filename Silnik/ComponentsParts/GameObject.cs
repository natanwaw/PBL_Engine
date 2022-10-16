using Microsoft.Xna.Framework;
using Silnik.GamingBox;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class GameObject
    {
        public string name;
        public string tag;
        public bool isActive = true;
        private bool startBool = true;
        public GameObject parent;
        List<GameObjectComponent> components = new List<GameObjectComponent>();
        public List<GameObject> children = new List<GameObject>();

        public GameObject()
        {
            AddComponent(new Transform());
        }
        public Comp GetComponent<Comp>() where Comp : GameObjectComponent
        {
            foreach (GameObjectComponent comp in components)
                if(comp.GetType().Equals(typeof(Comp)))
                {
                    return (Comp)comp;
                }
            return null;
        }
        public Comp GetComponentById<Comp>(int ID) where Comp : GameObjectComponent
        {
            foreach (GameObjectComponent comp in components)
                if (comp.GetType().Equals(typeof(Comp)))
                {
                    if(comp.id == ID)
                    {
                        return (Comp)comp;
                    }                  
                }
            return null;
        }
        public Comp GetComponentInParent<Comp>() where Comp : GameObjectComponent
        {
            foreach (GameObjectComponent comp in parent.components)
                if (comp.GetType().Equals(typeof(Comp)))
                {
                    return (Comp)comp;
                }
            return null;
        }
        public Comp GetComponentInParentById<Comp>(int ID) where Comp : GameObjectComponent
        {
            foreach (GameObjectComponent comp in parent.components)
                if (comp.GetType().Equals(typeof(Comp)))
                {
                    if (comp.id == ID)
                    {
                        return (Comp)comp;
                    }
                }
            return null;
        }
        public List<Comp> GetComponentsOfType<Comp>() where Comp : GameObjectComponent
        {
            List<Comp> lista = new List<Comp>();
            bool temp = false;
            foreach (GameObjectComponent comp in components)
                if (comp.GetType().Equals(typeof(Comp)))
                {
                    lista.Add((Comp)comp);
                    temp = true;
                }
            if (temp)
                return lista;
            return null;
        }


        public void AddComponent(GameObjectComponent gameObjectComponent)
        {
            components.Add(gameObjectComponent);
            gameObjectComponent.gameObject = this;
            gameObjectComponent.Init();
        }
        public virtual void Start()
        {
            if(startBool)
            {
                foreach (GameObjectComponent comp in components)
                {
                    comp.Start();
                }
                SetParents();
            }
            startBool = false;
            
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObjectComponent comp in components)
            {
                if(isActive)
                {
                    if(comp.isActive)
                    {
                        comp.Update(gameTime);
                    }
                }
                
            }
        }
        public static void Instantiate(GameObject gameObject)
        {
            World.WorldGameObjects.Add(gameObject);
        }
        public static void Destroy(GameObject gameObject)
        {
            World.DeletedGameObjects.Add(gameObject);
        }
        public static GameObject Find(string Name)
        {
            return World.AllGameObjects.Find(x => x.name == Name);
        }
        public static GameObject FindFromListUnsafe(string Name)
        {
            return World.WorldGameObjects.Find(x => x.name == Name);
        }
        public static GameObject FindByTag(string Tag)
        {
            return World.AllGameObjects.Find(x => x.tag == Tag);
        }
        public virtual void Draw(Matrix world, Matrix view, Matrix projection)
        {
            foreach (GameObjectComponent comp in components)
            {
                if (isActive)
                    if (comp.isActive)
                    {
                        comp.Draw(world, view, projection);
                    }
            }
        }
        private void SetParents()
        {
            if(parent!=null)
            {
                parent.children.Add(this);
            }
        }

    }
}
