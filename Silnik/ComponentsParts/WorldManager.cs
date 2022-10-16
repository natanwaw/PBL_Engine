using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public static class WorldManager
    {
        static List<World> loadedWorld = new List<World>();
        static List<World> deletedWorld = new List<World>();
        static List<World> activeWorld = new List<World>();
        static World prevWorld;

        public static void LoadMap(World world)
        {
            World.CollisionObjects.Clear();
            World.AllGameObjects.Clear();
            World.ButtonObjects.Clear();
            if(prevWorld != null)
            {
                deletedWorld.Add(prevWorld);
            }
            loadedWorld.Add(world);
            prevWorld = world;
        }
        public static void Update(GameTime gameTime)
        {
            foreach (var map in loadedWorld)
            {
                activeWorld.Add(map);
            }
            loadedWorld.Clear();
            foreach (var map in deletedWorld)
            {
                activeWorld.Remove(map);
            }
            deletedWorld.Clear();
            foreach (var map in activeWorld)
            {
                map.Update(gameTime);
            }
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(var map in activeWorld)
            {
                map.Draw(spriteBatch);
            }
        }
    }
}
