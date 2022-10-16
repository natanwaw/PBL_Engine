using System;
using System.Collections.Generic;
using System.Text;


namespace Silnik.GamingBox
{
    class TestMenu : World
    {
        lista2 ob_lista;
        public TestMenu()
        {
            ob_lista = new lista2();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }
    }
}
