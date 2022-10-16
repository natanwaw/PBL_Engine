using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class TestWorld : World
    {
        lista ob_lista;
        public TestWorld()
        {
            ob_lista = new lista();
            Settings.Game1.IsMouseVisible = true;
            ob_lista.loadLista();
        }
        
    }
}
