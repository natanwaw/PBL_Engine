using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silnik.GamingBox;
using Silnik.States;

namespace Silnik.GameStates
{
    public class AuthorsState : State
    {
        private List<Component> components;

        private Texture2D authorsBackGroundTexture;

        public AuthorsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("ButtonTexture");
            var buttonFont = _content.Load<SpriteFont>("fonts/ButtonFont");

            var backGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 250),    
                Text = "Go back",
            };
            backGameButton.Click += Button_Back_Click;

            components = new List<Component>()
            {    
                backGameButton,
            };
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        

        public override void LoadContent()
        {
            authorsBackGroundTexture = _content.Load<Texture2D>("authors");
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

            spriteBatch.Draw(authorsBackGroundTexture,new Rectangle(0,0,Game1.screenWidth,Game1.screenHeight), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
