using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class Listener : GameObjectComponent
    {
        public AudioListener listner = new AudioListener();

        public AudioListener getListener()
        {
            return listner;
        }
        public override void Update(GameTime gameTime)
        {
            listner.Position = gameObject.GetComponent<Transform>().position;
        }
    }
}
