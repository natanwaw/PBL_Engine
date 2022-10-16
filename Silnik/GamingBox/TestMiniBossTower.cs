using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class TestMiniBossTower : World
    {
        listaMiniBossTower ob_lista;
        public TestMiniBossTower()
        {
            ob_lista = new listaMiniBossTower();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }

    }
}

