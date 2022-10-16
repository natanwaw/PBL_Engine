using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class MapCounter : GameObjectComponent
    {
        GameObject info2;
        GameObject info3;
        GameObject info4;
        public override void Start()
        {
            info2 = GameObject.Find("info2");
            info3 = GameObject.Find("info3");
            info4 = GameObject.Find("info4");
        }
        public override void Update(GameTime gameTime)
        {
            if (Hero.fragmentyMapySkoczka < 5)
            {
                info2.GetComponent<Text>()._textBox.Text = Hero.fragmentyMapySkoczka + "/5 fragmentów mapy skoczka";
            }
            else
            {
                info2.GetComponent<Text>()._textBox.Text = "Już wiesz gdzie szukać minibossa skoczka! Otwórz mapę!";
            }
            info2.GetComponent<Text>()._textBox.Show();

            if (Hero.fragmentyMapyWiezy < 5)
            {
                info3.GetComponent<Text>()._textBox.Text = Hero.fragmentyMapyWiezy + "/5 fragmentów mapy wiezy";
            } 
            else
            {
                info3.GetComponent<Text>()._textBox.Text = "Już wiesz gdzie szukać minibossa wieży! Otwórz mapę!";
            }
            info3.GetComponent<Text>()._textBox.Show();

            if (Hero.fragmentyMapyGonca < 5)
            {
                info4.GetComponent<Text>()._textBox.Text = Hero.fragmentyMapyGonca + "/5 fragmentów mapy gońca";
            }
            else
            {
                info4.GetComponent<Text>()._textBox.Text = "Już wiesz gdzie szukać minibossa gońca! Otwórz mapę!";
            }
            info4.GetComponent<Text>()._textBox.Show();
        }
    }
}
