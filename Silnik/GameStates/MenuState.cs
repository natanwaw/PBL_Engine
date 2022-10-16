using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Silnik.GameStates;
using Silnik.GamingBox;

namespace Silnik.States
{
    public class MenuState : State
    {
        private List<Component> components;

        private Texture2D menuBackGroundTexture;

        XDocument xdoc = new XDocument();
        private int shadowValue;
        private float gammaValue;
        private float volumeValue;
        private string filePath = Path.Combine(Environment.CurrentDirectory, "Content\\GameSettings.xml");

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
      : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("ButtonTexture");
            var buttonFont = _content.Load<SpriteFont>("fonts/ButtonFont");

            var newGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 - 150),
                Text = "New Game",
            };
            newGameButton.Click += Button_NewGame_Click;

            var settingsGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 - 50),
                Text = "Load",
            };
            settingsGameButton.Click += Button_Load_Click;

            var loadGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 50),
                Text = "Settings",
            };
            loadGameButton.Click += Button_Settings_Click;

            var authorsGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 150),
                Text = "Authors",
            };
            authorsGameButton.Click += Button_Authors_Click;

            var quitGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 250),
                Text = "Quit Game",
            };
            quitGameButton.Click += Button_Quit_Click;

            components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                settingsGameButton,
                authorsGameButton,
                quitGameButton,
            };

        }


        public override void LoadContent()
        {
            menuBackGroundTexture = _content.Load<Texture2D>("MainMenuBackground");

            xdoc = XDocument.Load(Path.Combine(Environment.CurrentDirectory, filePath));
            var element1 = xdoc.Descendants("ShadowValue").Single();
            shadowValue = int.Parse(element1.Value);
            var element2 = xdoc.Descendants("GammaValue").Single();
            gammaValue = float.Parse(element2.Value);
            var element3 = xdoc.Descendants("VolumeValue").Single();
            volumeValue = float.Parse(element3.Value);

        }

        
        private void Button_NewGame_Click(object sender, EventArgs e)
        {
            Game1.setGamma(gammaValue);
            Game1.shadowQuality(shadowValue);

            _game.ChangeState(new GameState(_game, _graphicsDevice, _content)); 
        }

        private void Button_Load_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new LoadState(_game, _graphicsDevice, _content));
        }

        private void Button_Settings_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SettingsState(_game, _graphicsDevice, _content));
        }

        private void Button_Authors_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new AuthorsState(_game, _graphicsDevice, _content));
        }

        private void Button_Quit_Click(object sender, EventArgs args)
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuBackGroundTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
