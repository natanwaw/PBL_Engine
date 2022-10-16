using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class Music : GameObjectComponent
    {
        Boolean hasntStarted = true;

        public float musicVolume;
        public override void Update(GameTime gameTime)
        {
            if (hasntStarted)
            {
                gameObject.GetComponent<AudioComponent>().Play3DSound("music", true);
                gameObject.GetComponent<AudioComponent>().Instance.Volume = musicVolume;
                hasntStarted = false;
            }
        }

        public Music(float myVolume)
        {
            musicVolume = myVolume;
        }

        public override void Init()
        {

        }
    }
}
