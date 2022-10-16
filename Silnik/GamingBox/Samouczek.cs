using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class Samouczek : World
    {
        listaSamouczek ob_lista;
        public Samouczek()
        {
            ob_lista = new listaSamouczek();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }
    }
}
