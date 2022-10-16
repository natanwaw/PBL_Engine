using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class TestFinalBoss : World
    {
        listaFinalBoss ob_lista;
        public TestFinalBoss()
        {
            ob_lista = new listaFinalBoss();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }

    }
}

