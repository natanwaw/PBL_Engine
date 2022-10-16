using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Input
    {
        //private GameWindow gameWindow = Game1.GejmWindo;
        public static MouseState GetMouse;
        private static MouseState prevState;
        public static KeyboardState GetKey;
        private static KeyboardState prevKey;
        //public static ButtonState buttonState;
        GameWindow window;
        public Input(GameWindow gameWindow)
        {
            window = gameWindow;
            //state = Mouse.GetState(gameWindow);
        }
        public void Update()
        {
            prevKey = GetKey;
            prevState = GetMouse;
            GetMouse = Mouse.GetState(window);
            GetKey = Keyboard.GetState();            
        }
        public static bool IsKeyUp(Keys key)
        {
            if (prevKey.IsKeyDown(key) && GetKey.IsKeyUp(key))
                return true;
            return false;
        }
        public static bool IsKeyDown(Keys key)
        {
            if (GetKey.IsKeyDown(key) && prevKey.IsKeyUp(key))
                return true;
            return false;
        }
        public static bool IsKeyPressed(Keys key)
        {
            if (GetKey.IsKeyDown(key))
                return true;
            return false;
        }
        public static bool IsKeyPressed(int mouseButton)
        {
            switch(mouseButton)
            {
                case 0:
                    if (GetMouse.LeftButton == ButtonState.Pressed)
                        return true;
                    break;
                case 1:
                    if (GetMouse.RightButton == ButtonState.Pressed)
                        return true;
                    break;
                case 2:
                    if (GetMouse.MiddleButton == ButtonState.Pressed)
                        return true;
                    break;
            }
            return false;
        }
        public static bool IsKeyDown(int mouseButton)
        {
            switch (mouseButton)
            {
                case 0:
                    if (GetMouse.LeftButton == ButtonState.Pressed && prevState.LeftButton == ButtonState.Released)
                        return true;
                    break;
                case 1:
                    if (GetMouse.RightButton == ButtonState.Pressed && prevState.RightButton == ButtonState.Released)
                        return true;
                    break;
                case 2:
                    if (GetMouse.MiddleButton == ButtonState.Pressed && prevState.MiddleButton == ButtonState.Released)
                        return true;
                    break;
            }
            return false;
        }
        public static bool IsKeyUp(int mouseButton)
        {
            switch (mouseButton)
            {
                case 0:
                    if (prevState.LeftButton == ButtonState.Pressed && GetMouse.LeftButton == ButtonState.Released)
                        return true;
                    break;
                case 1:
                    if (prevState.RightButton == ButtonState.Pressed && GetMouse.RightButton == ButtonState.Released)
                        return true;
                    break;
                case 2:
                    if (prevState.MiddleButton == ButtonState.Pressed && GetMouse.MiddleButton == ButtonState.Released)
                        return true;
                    break;
            }
            return false;
        }
    }
}
