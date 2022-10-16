using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silnik.GamingBox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Silnik.States;

namespace Silnik
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        public static SpriteBatch _spriteBatch2;

        public const int screenWidth = 1920;
        public const int screenHeight = 1080;

        private Model model;

        //public static AudioListener listener= new AudioListener();

        private State _currentState;
        private State _nextState;

        private static Camera camera;
        private static Input input;
        public static GameWindow GejmWindo;

        public static Terrain terrain;
        Texture2D heightMap;
        AudioManager audioManager;

        private TimeCounter timeCounter = new TimeCounter();
        private Time time = new Time();
        private SpriteFont FontArial;

        //KR
        //Shadery, renderTargety itp potrzebne zeby moc oswietlic GameObjecty majace komponent CModel,
        //a takze do ich shadowmappingu
        public static Effect terrainEffect;
        public static Effect effect;
        public static Effect shadowDepthEffect;
        public static RenderTarget2D shadowDepthTarg;
        public static Light light;
        public static int shadowMapSize = 4096;

        public static SkySphere sky;
        public static RenderTarget2D reflectionTarg;
        //public static Water water;

        public static ProjectedTexture projectedTexture;
        public static RenderTarget2D RTprojectedTexture;
        public static int RTscale = 4;
        public static Texture2D point;
        public static Texture2D circle;

        Settings settings;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            InitContent.Init(Content, _graphics);
            IsMouseVisible = false;

            audioManager = new AudioManager(this);

            Components.Add(audioManager);
            //bill = new EnemyHealthBar();


        }
        public static Camera MainCam()
        {
            return camera;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.IsFullScreen = true;
            //2 linijki ponizej odblokowuja 60+ klatek
            //_graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
            _graphics.ApplyChanges();
            terrain = new Terrain(GraphicsDevice);
            GejmWindo = this.Window;
            light = new Light();
            settings = new Settings(this);

            //Tytuł okna
            Window.Title = "Graveyard Kyōki";

            this.IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _currentState = new MenuState(this, GraphicsDevice, Content);
            _currentState.LoadContent();
            _nextState = null;

            Content = new ContentManager(this.Services, "Content");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spriteBatch2 = new SpriteBatch(GraphicsDevice);
            //healthBar.Texture = Content.Load<Texture2D>("enemy_health_bar_foreground_004");
            model = Content.Load<Model>("lowpoly_tree_sample");

            camera = new Camera(_graphics.GraphicsDevice, this.Window);
            camera.Position = new Vector3(0, 40, 0);
            camera.LookAtDirection = Vector3.Forward;
            input = new Input(this.Window);

            Texture2D heightMap = Content.Load<Texture2D>("heightmap4");
            terrainEffect = Content.Load<Effect>("Effects");
            terrainEffect.Parameters["gamma"].SetValue(2.2f);
            effect = Content.Load<Effect>("EffectModel");
            effect.Parameters["gamma"].SetValue(2.2f);
            shadowDepthEffect = Content.Load<Effect>("ShadowDepthEffect");
            shadowDepthTarg = new RenderTarget2D(GraphicsDevice, shadowMapSize,
                shadowMapSize, false, SurfaceFormat.Single, DepthFormat.Depth24);
            terrain.setUp(terrainEffect, camera, heightMap);
            //WorldManager.LoadMap(new TestMenu());

            FontArial = Content.Load<SpriteFont>("fonts/Arial");

            sky = new SkySphere(Content);
            SkySphere.skyBoxEffect.Parameters["gamma"].SetValue(2.2f);
            reflectionTarg = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            //water = new Water(new Vector3(200, 2.6f, -200), new Vector2(10, 10));

            point = Content.Load<Texture2D>("point");
            circle = Content.Load<Texture2D>("Circle");
            RTprojectedTexture = new RenderTarget2D(GraphicsDevice, 1081 * RTscale, 1081 * RTscale, false, SurfaceFormat.Color, DepthFormat.Depth24);
            projectedTexture = new ProjectedTexture(RTprojectedTexture, GraphicsDevice);
        }

        protected override void UnloadContent()
        {


        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }
        protected override void Update(GameTime gameTime)
        {
            {
                if (_nextState != null)
                {
                    _currentState = _nextState;
                    _currentState.LoadContent();

                    _nextState = null;
                }

                _currentState.Update(gameTime);

                _currentState.PostUpdate(gameTime);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Update(gameTime);
            input.Update();
            time.Update(gameTime);
            //Stopwatch stopwatch = Stopwatch.StartNew();
            WorldManager.Update(gameTime);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);


            //healthBar.Update(gameTime);

            audioManager.Listener.Position = camera.Position;
            audioManager.Listener.Forward = camera.Forward;
            audioManager.Listener.Up = camera.Up;
            //audioManager.Listener.Velocity = camera.Velocity;



            //PreviousKeyState = KeyState;
            //KeyState = Keyboard.GetState();
            //GamePadState = GamePad.GetState(PlayerIndex.One);



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {


            GraphicsDevice.Clear(Color.CornflowerBlue);

            _currentState.Draw(gameTime, _spriteBatch);
            //terrain.Draw();
            //DrawModel(model, Matrix.Identity, camera.View, camera.Projection);
            timeCounter.Update(gameTime);
            var fps = string.Format("Avg TBF: {0:0.00}", timeCounter.AvgTime);
            _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, null);
            //_spriteBatch.Draw(kompas, new Vector2(10, 10), Color.White);
            WorldManager.Draw(_spriteBatch);
            _spriteBatch.End();
            //dodanie głębi
            _graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            /*_spriteBatch.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            _spriteBatch.Draw(RTprojectedTexture, new Rectangle(0, 0, 400, 400), Color.White);
            _spriteBatch.End();*/

            _graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //healthBar.Draw(camera.Position, camera.View, camera.Projection);

            //******************************************************************************************************************
            // _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            //_spriteBatch.End();
            //******************************************************************************************************************


            base.Draw(gameTime);
        }

        public static void setGamma(float gamma)
        {
            terrainEffect.Parameters["gamma"].SetValue(gamma);
            effect.Parameters["gamma"].SetValue(gamma);
            SkySphere.skyBoxEffect.Parameters["gamma"].SetValue(gamma);
        }

        public static void setVolume(float audioVolume)
        {
            Music myVolume = new Music(audioVolume);
            AudioComponent myAudio = new AudioComponent(audioVolume);
        }

        public static void shadowQuality(int i)
        {
            shadowDepthTarg.Dispose();
            if (i == 0)
            {
                Camera.ultraShadow = false;
                shadowDepthTarg = new RenderTarget2D(InitContent.Graphics.GraphicsDevice, 512,
                512, false, SurfaceFormat.Single, DepthFormat.Depth24);
            }
            if (i == 1)
            {
                Camera.ultraShadow = false;
                shadowDepthTarg = new RenderTarget2D(InitContent.Graphics.GraphicsDevice, 1024,
                1024, false, SurfaceFormat.Single, DepthFormat.Depth24);
            }
            if (i == 2)
            {
                Camera.ultraShadow = false;
                shadowDepthTarg = new RenderTarget2D(InitContent.Graphics.GraphicsDevice, 2048,
                2048, false, SurfaceFormat.Single, DepthFormat.Depth24);
            }
            if (i == 3)
            {
                Camera.ultraShadow = false;
                shadowDepthTarg = new RenderTarget2D(InitContent.Graphics.GraphicsDevice, 4096,
                4096, false, SurfaceFormat.Single, DepthFormat.Depth24);
            }
            if (i == 4)
            {
                Camera.ultraShadow = true;
                shadowDepthTarg = new RenderTarget2D(InitContent.Graphics.GraphicsDevice, 4096,
                4096, false, SurfaceFormat.Single, DepthFormat.Depth24);
            }
        }
    }
}
