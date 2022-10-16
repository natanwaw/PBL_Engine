using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Silnik.States;
using MyXmlData;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Silnik.GameStates
{
    public class SettingsState : State
    {
        private SpriteFont font = InitContent.Content.Load<SpriteFont>("fonts/ButtonFont");

        private string infoGamma;
        private string infoShadow;
        private string infoVolume;

        private List<Component> components;

        private string filePath = Path.Combine(Environment.CurrentDirectory, "Content\\GameSettings.xml");

        private Texture2D settingsBackGroundTexture;

        private int shadowValue;
        private float gammaValue;
        private float volumeValue;

        XDocument xdoc = new XDocument();

        public SettingsState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("ButtonTexture");
            var buttonFont = _content.Load<SpriteFont>("fonts/ButtonFont");

            var shadowButton0 = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 400, Game1.screenHeight / 2 - 150),
                Text = "Shadows 0",
            };
            shadowButton0.Click += Button_Shadow0_Click;

            var shadowButton1 = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 200, Game1.screenHeight / 2 - 150),
                Text = "Shadows 1",
            };
            shadowButton1.Click += Button_Shadow1_Click;

            var shadowButton2 = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2), Game1.screenHeight / 2 - 150),
                Text = "Shadows 2",
            };
            shadowButton2.Click += Button_Shadow2_Click;

            var shadowButton3 = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) + 200, Game1.screenHeight / 2 - 150),
                Text = "Shadows 3",
            };
            shadowButton3.Click += Button_Shadow3_Click;

            var shadowButton4 = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) + 400, Game1.screenHeight / 2 - 150),
                Text = "Shadows 4",
            };
            shadowButton4.Click += Button_Shadow4_Click;






            var gammaGameUpButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 400, Game1.screenHeight / 2 - 50),
                Text = "Gamma +",
            };
            gammaGameUpButton.Hold += Button_Gamma_Up_Hold;


            var gammaGameDownButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 200, Game1.screenHeight / 2 - 50),
                Text = "Gamma -",
            };
            gammaGameDownButton.Hold += Button_Gamma_Down_Hold;



            var volumeUpButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 400, Game1.screenHeight / 2 + 50),
                Text = "Volume +",
            };
            volumeUpButton.Hold += Button_Volume_Up_Hold;

            var volumeDownButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2((Game1.screenWidth / 2) - 200, Game1.screenHeight / 2 + 50),
                Text = "Volume -",
            };
            volumeDownButton.Hold += Button_Volume_Down_Hold;







            var backGameButton = new MenuButton(buttonTexture, buttonFont)
            {
                Position = new Vector2(Game1.screenWidth / 2 + 150, Game1.screenHeight / 2 + 250),
                Text = "Go back",
            };
            backGameButton.Click += Button_Back_Click;


            components = new List<Component>()
            {
                shadowButton0,
                shadowButton1,
                shadowButton2,
                shadowButton3,
                shadowButton4,
                gammaGameUpButton,
                gammaGameDownButton,
                volumeUpButton,
                volumeDownButton,
                backGameButton,
            };
        }

        public override void LoadContent()
        {
            settingsBackGroundTexture = _content.Load<Texture2D>("SettingsMenuBackground");

            xdoc = XDocument.Load(filePath);
            var element1 = xdoc.Descendants("ShadowValue").Single();
            shadowValue = int.Parse(element1.Value);
            var element2 = xdoc.Descendants("GammaValue").Single();
            gammaValue = float.Parse(element2.Value);
            var element3 = xdoc.Descendants("VolumeValue").Single();
            volumeValue = float.Parse(element3.Value);
        }

        private void Button_Shadow0_Click(object sender, EventArgs e)
        {
            shadowValue = 0;
            Game1.shadowQuality(shadowValue);


            var element = xdoc.Descendants("ShadowValue").Single();
            element.Value = shadowValue.ToString();
            xdoc.Save(filePath);
        }

        private void Button_Shadow1_Click(object sender, EventArgs e)
        {
            shadowValue = 1;
            Game1.shadowQuality(shadowValue);


            var element = xdoc.Descendants("ShadowValue").Single();
            element.Value = shadowValue.ToString();
            xdoc.Save(filePath);
        }

        private void Button_Shadow2_Click(object sender, EventArgs e)
        {
            shadowValue = 2;
            Game1.shadowQuality(shadowValue);


            var element = xdoc.Descendants("ShadowValue").Single();
            element.Value = shadowValue.ToString();
            xdoc.Save(filePath);
        }

        private void Button_Shadow3_Click(object sender, EventArgs e)
        {
            shadowValue = 3;
            Game1.shadowQuality(shadowValue);


            var element = xdoc.Descendants("ShadowValue").Single();
            element.Value = shadowValue.ToString();
            xdoc.Save(filePath);
        }
        private void Button_Shadow4_Click(object sender, EventArgs e)
        {
            shadowValue = 4;
            Game1.shadowQuality(shadowValue);


            var element = xdoc.Descendants("ShadowValue").Single();
            element.Value = shadowValue.ToString();
            xdoc.Save(filePath);
        }






        private void Button_Gamma_Up_Hold(object sender, EventArgs e)
        {
            gammaValue += 0.005f;
            if (gammaValue >= 3.0f)
            {
                gammaValue = 3.0f;
                Game1.setGamma(gammaValue);


                var element = xdoc.Descendants("GammaValue").Single();
                element.Value = gammaValue.ToString();
                xdoc.Save(filePath);
            }
            else
            {
                Game1.setGamma(gammaValue);


                var element = xdoc.Descendants("GammaValue").Single();
                element.Value = gammaValue.ToString();
                xdoc.Save(filePath);
            }


        }

        private void Button_Gamma_Down_Hold(object sender, EventArgs e)
        {
            gammaValue -= 0.005f;
            if (gammaValue <= 1.0f)
            {
                gammaValue = 1.0f;
                Game1.setGamma(gammaValue);


                var element = xdoc.Descendants("GammaValue").Single();
                element.Value = gammaValue.ToString();
                xdoc.Save(filePath);
            }
            else
            {
                Game1.setGamma(gammaValue);


                var element = xdoc.Descendants("GammaValue").Single();
                element.Value = gammaValue.ToString();
                xdoc.Save(filePath);
            }
        }


        private void Button_Volume_Up_Hold(object sender, EventArgs e)
        {
            volumeValue += 0.005f;
            if (volumeValue >= 1.0f)
            {
                volumeValue = 1.0f;
                Game1.setVolume(volumeValue);
                
                var element = xdoc.Descendants("VolumeValue").Single();
                element.Value = volumeValue.ToString();
                xdoc.Save(filePath);
            }
            else
            {
                Game1.setVolume(volumeValue);

                var element = xdoc.Descendants("VolumeValue").Single();
                element.Value = volumeValue.ToString();
                xdoc.Save(filePath);
            }
        }

        private void Button_Volume_Down_Hold(object sender, EventArgs e)
        {
            volumeValue -= 0.005f;
            if (volumeValue <= 0.0f)
            {
                volumeValue = 0.0f;
                Game1.setVolume(volumeValue);


                var element = xdoc.Descendants("VolumeValue").Single();
                element.Value = volumeValue.ToString();
                xdoc.Save(filePath);
            }
            else
            {
                Game1.setVolume(volumeValue);


                var element = xdoc.Descendants("VolumeValue").Single();
                element.Value = volumeValue.ToString();
                xdoc.Save(filePath);
            }
        }





        private void Button_Back_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        public override void Update(GameTime gameTime)
        {
            StringBuilder builderGamma = new StringBuilder();
            builderGamma.AppendLine("Gamma is set at: " + gammaValue);
            infoGamma = builderGamma.ToString();

            StringBuilder builderShadow = new StringBuilder();
            builderShadow.AppendLine("Shadows are set at: " + shadowValue);
            infoShadow = builderShadow.ToString();

            StringBuilder builderVolume = new StringBuilder();
            builderVolume.AppendLine("Volume is set at: " + volumeValue*100 + "%");
            infoVolume = builderVolume.ToString();

            foreach (var component in components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(settingsBackGroundTexture, new Rectangle(0, 0, Game1.screenWidth, Game1.screenHeight), Color.White);


            spriteBatch.DrawString(font, infoGamma, new Vector2((Game1.screenWidth / 2) - 50, Game1.screenHeight / 2 - 60), Color.White);

            spriteBatch.DrawString(font, infoShadow, new Vector2((Game1.screenWidth / 2) - 50, Game1.screenHeight / 2 - 300), Color.White);

            spriteBatch.DrawString(font, infoVolume, new Vector2((Game1.screenWidth / 2) - 50, Game1.screenHeight / 2 - - 30), Color.White);

            foreach (var component in components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}
