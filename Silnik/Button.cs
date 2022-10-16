using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Button : GameObjectComponent
    {
        Sprite sprite;
        bool clicked;
        public Button()
        {

        }
        public void Set()
        {
            sprite = gameObject.GetComponent<Sprite>();
        }
        private bool ClickButton()
        {
            if (Input.GetMouse.X > sprite.rect.X && Input.GetMouse.X < sprite.rect.X + sprite.rect.Width
                && Input.GetMouse.Y > sprite.rect.Y && Input.GetMouse.Y < sprite.rect.Y + sprite.rect.Height)
                return true;
            else
                return false;
        }
        public override void Update(GameTime gameTime)
        {
            if(Input.IsKeyDown(MouseButton.Button0)&&ClickButton())
            {
                clicked = true;
            }
            else
            {
                clicked = false;
            }
        }
        public bool ButtonClicked()
        {
            if(clicked == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
