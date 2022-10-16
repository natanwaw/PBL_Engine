using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik
{
    public class Settings
    {
        static Game game1;
        private Game game;
        public Settings(Game _game)
        {
            game = _game;
            game1 = _game;
        }
        public void MouseVisible(bool checkBool)
        {
            game.IsMouseVisible = checkBool;
        }
        public static Game Game1
        {
            get
            {
                return game1;
            }
        }

    }
}
