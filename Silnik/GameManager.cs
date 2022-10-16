using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Silnik.GamingBox;

namespace Silnik
{
    class GameManager : GameObjectComponent
    {
        private bool PauseClicked;
        private bool mapClicked;

        private GameObject map;
        private GameObject heroMap;
        private GameObject rookMap;
        private GameObject bishopMap;
        private GameObject knightMap;
        private List<GameObject> lista_gui = new List<GameObject>();

        public override void Init()
        {
            PauseClicked = false;
            mapClicked = false;
    }
        //Start - wykonywane 1 raz tuż przed pierwszym Update
        public override void Start()
        {
            map = GameObject.FindByTag("map");
            heroMap = GameObject.FindByTag("hero_on_map");
            rookMap = GameObject.FindByTag("map_rook");
            bishopMap = GameObject.FindByTag("map_bishop");
            knightMap = GameObject.FindByTag("map_knight");
            lista_gui.Add(GameObject.Find("health_stat"));
            lista_gui.Add(GameObject.Find("gui1"));
            lista_gui.Add(GameObject.Find("gui2"));
            lista_gui.Add(GameObject.Find("gui3"));
            lista_gui.Add(GameObject.Find("gui4"));
            lista_gui.Add(GameObject.Find("gui5"));
            lista_gui.Add(GameObject.Find("gui6"));
        }
        //Update - wiadomo co i jak
        public override void Update(GameTime gameTime)
        {
            if(Input.IsKeyDown(Keys.P))
            {
                if (PauseClicked)
                {
                    PauseClicked = false;
                    Time.Resume();
                }
                else
                {
                    PauseClicked = true;
                    Time.Pause();
                }
            }
            ///TESTOWE KLIKANIE
            if(Input.IsKeyDown(2))
            {
                
            }

            if (map!=null&&Input.IsKeyDown(Keys.M))
            {
                if (mapClicked)
                {
                    mapClicked = false;
                    map.isActive = false;
                    heroMap.isActive = false;
                    bishopMap.isActive = false;
                    knightMap.isActive = false;
                    rookMap.isActive = false;

                }
                else
                {
                    mapClicked = true;
                    map.isActive = true;
                    heroMap.isActive = true;
                    Console.WriteLine(Hero.fragmentyMapyGonca);
                    if (Hero.fragmentyMapyGonca >= 5)
                        bishopMap.isActive = true;
                    if (Hero.fragmentyMapySkoczka >= 5)
                        knightMap.isActive = true;
                    if (Hero.fragmentyMapyWiezy >= 5)
                        rookMap.isActive = true;

                }
            }

        }
        public void GuiSetOn()
        {
            foreach(GameObject ob in lista_gui)
            {
                ob.isActive = true;
            }
        }
        public void GuiSetOff()
        {
            foreach (GameObject ob in lista_gui)
            {
                ob.isActive = false;
            }
        }

    }
}
