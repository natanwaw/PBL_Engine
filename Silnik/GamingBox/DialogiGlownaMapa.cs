using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class DialogiGlownaMapa : GameObjectComponent
    {
        GameObject boss;
        GameObject bramaWieza;
        GameObject bramaSkoczek;
        GameObject bramaGoniec;
        bool ucieklPrzedBossem = false;
        public static bool dialogFragmentyMapyWiezy = true;
        public static bool dialogFragmentyMapySkoczka = true;
        public static bool dialogFragmentyMapyGonca = true;
        bool cameralLoadWieza = false;
        bool cameralLoadSkoczek = false;
        bool cameralLoadGoniec = false;
        bool firstEnterWieza = true;
        bool firstEnterSkoczek = true;
        bool firstEnterGoniec = true;

        Vector3 cameraPositionTarget;
        Vector3 cameraForwardTarget;
        Vector3 cameraForward;
        Vector3 cameraPositionStart;
        Vector3 cameraForwardStart;

        float x = 0f;
        float time = 1.5f;

        public override void Start()
        {
            boss = GameObject.FindByTag("enemy");
            bramaWieza = GameObject.Find("bramaWieza");
            bramaSkoczek = GameObject.Find("bramaSkoczek");
            bramaGoniec = GameObject.Find("bramaGoniec");
            boss.GetComponent<Boss>().isMovable = false;
            boss.GetComponent<Transform>().Position = gameObject.GetComponent<Transform>().position;
            boss.GetComponent<Transform>().Position = new Vector3(boss.GetComponent<Transform>().Position.X - 12, boss.GetComponent<Transform>().Position.Y, boss.GetComponent<Transform>().Position.Z);
            cameraForward = Game1.MainCam().Forward;

            //cameraPositionStart.X = gameObject.GetComponent<Transform>().position.X - 100;
            //cameraPositionStart.Y = gameObject.GetComponent<Transform>().position.Y + 10;
            //cameraPositionStart.Z = gameObject.GetComponent<Transform>().position.Z + 100;
            cameraForward = Game1.MainCam().Forward;
            /*Game1.MainCam().Position = cameraPositionStart;
            Game1.MainCam().targetX = cameraPositionStart.X;
            Game1.MainCam().targetY = cameraPositionStart.Y;
            Game1.MainCam().targetZ = cameraPositionStart.Z;
            Game1.MainCam().Forward = cameraForwardStart;*/
        }
        public override void Update(GameTime gameTime)
        {
            //Console.WriteLine("cam forward" + Game1.MainCam().Forward);
            //jeżeli nie wyświetlamy żadnego dialogu to pozwalamy graczowi poruszać się
            if (gameObject.GetComponent<Dialog>()._dialogBox.Active == false)
            {
                gameObject.GetComponent<Hero>().isMovable = true;
            }
            else //w przeciwnym wypadku blokujemy taką możliwość
            {
                gameObject.GetComponent<Hero>().isMovable = false;
                Time.Pause();
            }
            if (lista.pierwszeWejscie)
            {
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 0)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 1)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 3)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 4)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                //Console.WriteLine(gameObject.GetComponent<Dialog>()._dialogBox._currentPage);
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && !ucieklPrzedBossem)
                {
                    boss.GetComponent<Boss>().isMovable = true;
                    if ((distance(gameObject.GetComponent<Transform>().position, boss.GetComponent<Transform>().position) > 80))
                    {
                        ucieklPrzedBossem = true;
                    }
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 6)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (Hero.fragmentyMapyWiezy >= 5 && dialogFragmentyMapyWiezy && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 9;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapyWiezy = false;
                }
                if (Hero.fragmentyMapySkoczka >= 5 && dialogFragmentyMapySkoczka && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 10;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapySkoczka = false;
                }
                if (Hero.fragmentyMapyGonca >= 5 && dialogFragmentyMapyGonca && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 11;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapyGonca = false;
                }
                if ((Hero.fragmentyMapyWiezy >= 5 && distance(bramaWieza.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 10) || cameralLoadWieza)
                {
                    gameObject.GetComponent<Hero>().isMovable = false;
                    cameralLoadWieza = true;
                    if (firstEnterWieza)
                    {
                        firstEnterWieza = false;
                        cameraPositionStart = Game1.MainCam().Position;
                        cameraForwardStart = Game1.MainCam().Forward;
                        cameraPositionTarget = new Vector3(170f, 3.41f, -883f);
                        cameraForwardTarget = new Vector3(-0.27251384f, -0.31021452f, -0.9107706f);
                    }
                    if (cameralLoadWieza)
                    {
                        x += Time.DeltaTime;
                        if (x < time)
                        {
                            Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().Forward = cameraForwardStart + (cameraForwardTarget - cameraForwardStart) * (float)Math.Sin(x / time * 1.57079632679f);
                        }
                        else if (x < time + 1f)
                        {
                            Game1.MainCam().Position = cameraPositionTarget;
                        }
                        else
                        {
                            Game1.MainCam().Forward = cameraForward;
                            Game1.MainCam().hero = null;
                            WorldManager.LoadMap(new TestMiniBossTower());
                        }

                    }
                }
            }
            if (lista.poWiezy)
            {
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 0)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 1)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 3)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 4)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 6)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                //teraz zaraza
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7)
                {
                    //ktos podchodzi i zostaje zarazony przez generala
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8 && !ucieklPrzedBossem)
                {
                    boss.GetComponent<Boss>().afterFirstMiniBoss = true;
                    boss.GetComponent<Boss>().isMovable = true;
                    if ((distance(gameObject.GetComponent<Transform>().position, boss.GetComponent<Transform>().position) > 80))
                    {
                        ucieklPrzedBossem = true;
                    }
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8 && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (Hero.fragmentyMapySkoczka >= 5 && dialogFragmentyMapySkoczka && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 10;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapySkoczka = false;
                }
                if (Hero.fragmentyMapyGonca >= 5 && dialogFragmentyMapyGonca && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 11;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapyGonca = false;
                }
                if ((Hero.fragmentyMapySkoczka >= 5 && distance(bramaSkoczek.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 10) || cameralLoadWieza)
                {
                    gameObject.GetComponent<Hero>().isMovable = false;
                    cameralLoadSkoczek = true;
                    if (firstEnterSkoczek)
                    {
                        firstEnterSkoczek = false;
                        cameraPositionStart = Game1.MainCam().Position;
                        cameraForwardStart = Game1.MainCam().Forward;
                        cameraPositionTarget = new Vector3(358, 6.16f, -868f);
                        cameraForwardTarget = new Vector3(-0.048526958f, -0.7287702f, -0.68303674f);
                    }
                    if (cameralLoadSkoczek)
                    {
                        x += Time.DeltaTime;
                        if (x < time)
                        {
                            Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().Forward = cameraForwardStart + (cameraForwardTarget - cameraForwardStart) * (float)Math.Sin(x / time * 1.57079632679f);
                        }
                        else if (x < time + 1f)
                        {
                            Game1.MainCam().Position = cameraPositionTarget;
                        }
                        else
                        {
                            Game1.MainCam().Forward = cameraForward;
                            Game1.MainCam().hero = null;
                            WorldManager.LoadMap(new TestMiniBossSkoczek());
                        }

                    }
                }
            }
            if (lista.poSkoczku)
            {
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 0)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 1)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 3)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 4)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && !ucieklPrzedBossem)
                {
                    boss.GetComponent<Boss>().afterFirstMiniBoss = true;
                    boss.GetComponent<Boss>().isMovable = true;
                    if ((distance(gameObject.GetComponent<Transform>().position, boss.GetComponent<Transform>().position) > 80))
                    {
                        ucieklPrzedBossem = true;
                    }
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }

                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 6)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                
                /*if (Hero.fragmentyMapySkoczka >= 5 && dialogFragmentyMapySkoczka && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 10;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapySkoczka = false;
                }*/
                if (Hero.fragmentyMapyGonca >= 5 && dialogFragmentyMapyGonca && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox._currentPage = 9;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    dialogFragmentyMapyGonca = false;
                }

                if ((Hero.fragmentyMapyGonca >= 5 && distance(bramaGoniec.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 10) || cameralLoadGoniec)
                {
                    gameObject.GetComponent<Hero>().isMovable = false;
                    cameralLoadGoniec = true;
                    if (firstEnterGoniec)
                    {
                        firstEnterGoniec = false;
                        cameraPositionStart = Game1.MainCam().Position;
                        cameraForwardStart = Game1.MainCam().Forward;
                        cameraPositionTarget = new Vector3(657, 15f, -236);
                        cameraForwardTarget = new Vector3(-0.427141f, -0.6475338f, -0.6310711f);
                    }
                    if (cameralLoadGoniec)
                    {
                        x += Time.DeltaTime;
                        if (x < time)
                        {
                            Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x / time * 1.57079632679f);
                            Game1.MainCam().Forward = cameraForwardStart + (cameraForwardTarget - cameraForwardStart) * (float)Math.Sin(x / time * 1.57079632679f);
                        }
                        else if (x < time + 1f)
                        {
                            Game1.MainCam().Position = cameraPositionTarget;
                        }
                        else
                        {
                            Game1.MainCam().Forward = cameraForward;
                            Game1.MainCam().hero = null;
                            WorldManager.LoadMap(new TestMiniBossGoniec());
                        }

                    }
                }
            }
            if (lista.poGoncu)
            {
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 0)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 1)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 3)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 4)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 6)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8 && !ucieklPrzedBossem)
                {
                    boss.GetComponent<Boss>().afterFirstMiniBoss = true;
                    boss.GetComponent<Boss>().isMovable = true;
                    if ((distance(gameObject.GetComponent<Transform>().position, boss.GetComponent<Transform>().position) > 80))
                    {
                        ucieklPrzedBossem = true;
                    }
                }
                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8 && ucieklPrzedBossem)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }

                if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 9)
                {
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                }
            }



        }
        public float distance(Vector3 w1, Vector3 w2)
        {
            double dis = 0.0;
            dis = Math.Sqrt((w2.X - w1.X) * (w2.X - w1.X) + (w2.Y - w1.Y) * (w2.Y - w1.Y) + (w2.Z - w1.Z) * (w2.Z - w1.Z));
            return (float)dis;
        }
    }
}
