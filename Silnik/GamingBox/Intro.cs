using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class Intro : World
    {
        listaIntro ob_lista;
        public Intro()
        {
            ob_lista = new listaIntro();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }
    }
}
