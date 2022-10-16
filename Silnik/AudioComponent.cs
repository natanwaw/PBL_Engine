using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    class AudioComponent : GameObjectComponent
    {
        static string[] soundNames =
        {
            "death",
            "attack"
        };
        public SoundEffectInstance Instance;

        public float sfxVolume = 1f;

        private GameObject hero;
        public float volume = 0.01f;
        public override void Start()
        {
            foreach (GameObject GO in World.AllGameObjects)
            {
                if (GO.tag == "hero")
                {
                    hero = GO;
                }
            }
        }
        // The emitter describes an entity which is making a 3D sound.
        AudioEmitter emitter = new AudioEmitter();


        // Store all the sound effects that are available to be played.
        Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        public override void Init()
        {
            // Set the scale for 3D audio so it matches the scale of our game world.
            // DistanceScale controls how much sounds change volume as you move further away.
            // DopplerScale controls how much sounds change pitch as you move past them.
            SoundEffect.DistanceScale = 20000000f;
            SoundEffect.DopplerScale = 0.1f;
            
        }

        public void Load(string name)
        {
            soundEffects.Add(name, InitContent.Content.Load<SoundEffect>(name));
            foreach (GameObject GO in World.AllGameObjects)
            {
                if (GO.tag == "hero")
                {
                    hero = GO;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Instance != null)
            {
                if (Instance.State == SoundState.Stopped)
                {
                    // If the sound has stopped playing, dispose it.
                    Instance.Dispose();

                }
                else
                {
                    // If the sound is still playing, update its 3D settings.
                    Apply3D();
                }
            }
        }

        public SoundEffectInstance Play3DSound(string soundName, bool isLooped)
        {

            // Fill in the instance and emitter fields.
            Instance = soundEffects[soundName].CreateInstance();
            Instance.IsLooped = isLooped;
            Instance.Volume = volume*sfxVolume;
            // Set the 3D position of this sound, and then play it.
            Apply3D();

            Instance.Play();

            

            return Instance;
        }

        public AudioComponent(float myVolume)
        {
            volume = myVolume;
        }

        /// <summary>
        /// Updates the position and velocity settings of a 3D sound.
        /// </summary>
        private void Apply3D()
        {
            emitter.Position = gameObject.GetComponent<Transform>().position;
            emitter.Forward = Vector3.Forward;
            emitter.Up = Vector3.Up;
            emitter.Velocity = Vector3.Zero;

            Instance.Apply3D(hero.GetComponent<Listener>().listner, emitter); ;
        }
    }
}
