using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class TimeCounter
    {
        private float TimeData;
        public float AvgTime = 0;
        public int TickRate = 100;
        private int Tick = 0;
        public TimeCounter()
        {

        }
        public void Update(GameTime gametime)
        {
            Counter(gametime);
        }
        private void Counter(GameTime gametime)
        {
            if(Tick < TickRate)
            {
                Tick++;
                TimeData += (float)gametime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Tick = 0;
                AvgTime = TimeData*1000 / TickRate;
                TimeData = 0;
            }
        }

    }
}
