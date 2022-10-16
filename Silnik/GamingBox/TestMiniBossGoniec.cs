using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class TestMiniBossGoniec : World
    {
        listaMiniBossGoniec ob_lista;
        public TestMiniBossGoniec()
        {
            ob_lista = new listaMiniBossGoniec();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }

    }
}

