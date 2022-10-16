using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silnik.GamingBox;
using Silnik.States;

namespace Silnik.GameStates
{
    class LoadState : State
    {
        private List<Component> components;

        private Texture2D loadBackGroundTexture;

        private SpriteFont font = InitContent.Content.Load<SpriteFont>("fonts/ButtonFont");

        private int saveSlot;

        XDocument xdoc = new XDocument();

        private string infoEmpty = "Slot Empty";

        private string infoSave1 = "Empty";
        private string infoSave2 = "Empty";
        private string infoSave3 = "Empty";

        private string filePath1 = Path.Combine(Environment.CurrentDirectory, "Content\\GameSave1.xml");
        private string filePath2 = Path.Combine(Environment.CurrentDirectory, "Content\\GameSave2.xml");
        private string filePath3 = Path.Combine(Environment.CurrentDirectory, "Content\\GameSave3.xml");

        public LoadState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("ButtonTexture");
            var buttonFont = _content.Load<SpriteFont>("fonts/ButtonFont");

            var slot1GameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 - 400, Game1.screenHeight / 2 - 150),
                Text = "Slot 1",
            };
            slot1GameButton.Click += Button_Slot1_Click;

            var slot2GameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2 - 150),
                Text = "Slot 2",
            };
            slot2GameButton.Click += Button_Slot2_Click;

            var slot3GameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 400, Game1.screenHeight / 2 - 150),
                Text = "Slot 3",
            };
            slot3GameButton.Click += Button_Slot3_Click;

            var backGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 250),
                Text = "Go back",
            };
            backGameButton.Click += Button_Back_Click;

            components = new List<Component>()
            {
                slot1GameButton,
                slot2GameButton,
                slot3GameButton,
                backGameButton,
            };
        }

        private void Button_Slot1_Click(object sender, EventArgs e)
        {
            //xdoc = XDocument.Load(filePath1);
        }

        private void Button_Slot2_Click(object sender, EventArgs e)
        {
            //xdoc = XDocument.Load(filePath2);
        }

        private void Button_Slot3_Click(object sender, EventArgs e)
        {
            //xdoc = XDocument.Load(filePath3);
        }

        private void Button_Back_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        public override void LoadContent()
        {    
            loadBackGroundTexture = _content.Load<Texture2D>("blank");
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

            spriteBatch.Draw(loadBackGroundTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);

            if (File.Exists(filePath1))
            {
                //infoSave1 = zmienna z datą
                spriteBatch.DrawString(font, infoSave1, new Vector2(Game1.screenWidth / 2 - 400, Game1.screenHeight / 2 - 50), Color.White);
            }
            else
            {
                infoSave1 = infoEmpty;
                spriteBatch.DrawString(font, infoSave1, new Vector2(Game1.screenWidth / 2 - 400, Game1.screenHeight / 2 - 50), Color.White);
            }

            if (File.Exists(filePath2))
            {
                //infoSave2 = zmienna z datą
                spriteBatch.DrawString(font, infoSave2, new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2 - 50), Color.White);
            }
            else
            {
                infoSave2 = infoEmpty;
                spriteBatch.DrawString(font, infoSave2, new Vector2(Game1.screenWidth / 2, Game1.screenHeight / 2 - 50), Color.White);
            }

            if (File.Exists(filePath3))
            {
                //infoSave3 = zmienna z datą
                spriteBatch.DrawString(font, infoSave3, new Vector2(Game1.screenWidth / 2 + 400, Game1.screenHeight / 2 - 50), Color.White);
            }
            else
            {
                infoSave3 = infoEmpty;
                spriteBatch.DrawString(font, infoSave3, new Vector2(Game1.screenWidth / 2 + 400, Game1.screenHeight / 2 - 50), Color.White);
            }

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}