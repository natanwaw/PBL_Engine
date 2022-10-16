using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Time
    {
        private static float time;
        private static float totalTime;

        private static bool pause;
        public static float DeltaTime
        {
            get
            {
                return time;
            }
        }
        public static float TotalGameTime
        {
            get
            {
                return totalTime;
            }
        }
        public Time()
        {
            
        }
        public void Update(GameTime gameTime)
        {
            time = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (pause)
                time = 0;
            totalTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }
        public static void Pause()
        {
            pause = true;
        }
        public static void Resume()
        {
            pause = false;
        }
    }
}
