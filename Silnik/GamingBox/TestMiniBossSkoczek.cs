using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class TestMiniBossSkoczek : World
    {
        listaMiniBossSkoczek ob_lista;
        public TestMiniBossSkoczek()
        {
            ob_lista = new listaMiniBossSkoczek();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }

    }
}

