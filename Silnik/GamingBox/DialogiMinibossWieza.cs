using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class DialogiMinibossWieza : GameObjectComponent
    {
        GameObject miniboss;
        GameObject bramaWieza;
        bool ucieklPrzedBossem = false;
        bool cameraExit = false;
        bool firstEnter = true;
       // bool
        bool czyPoWalce = false;

        Vector3 cameraPositionTarget;
        Vector3 cameraForwardTarget;
        Vector3 cameraForward;
        Vector3 cameraPositionStart;
        Vector3 cameraForwardStart;

        float x = 0f;
        float time = 1.5f;
        public override void Start()
        {
            bramaWieza = GameObject.Find("bramaWieza");
            miniboss = GameObject.Find("miniboss");
            miniboss.GetComponent<BossTower>().isMovable = false;

            /*cameraPositionStart.X = gameObject.GetComponent<Transform>().position.X - 100;
            cameraPositionStart.Y = gameObject.GetComponent<Transform>().position.Y + 10;
            cameraPositionStart.Z = gameObject.GetComponent<Transform>().position.Z + 100;*/
            cameraForward = Game1.MainCam().Forward;
            /*Game1.MainCam().Position = cameraPositionStart;
            Game1.MainCam().targetX = cameraPositionStart.X;
            Game1.MainCam().targetY = cameraPositionStart.Y;
            Game1.MainCam().targetZ = cameraPositionStart.Z;
            Game1.MainCam().Forward = cameraForwardStart;*/
            gameObject.GetComponent<Text>()._textBox.Show();
        }
        public override void Update(GameTime gameTime)
        {
            if (miniboss.GetComponent<EnemiesStats>().health < 11)
            {
                czyPoWalce = true;
            }
            if (czyPoWalce)
            {
                miniboss.GetComponent<BossTower>().isMovable = false;
            }
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
            
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7 && !czyPoWalce)
            {
                miniboss.GetComponent<BossTower>().isMovable = true;
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 7 && czyPoWalce)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 8)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 9)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 10)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 11)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 12)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 13)
            {
                gameObject.GetComponent<Text>()._textBox.Hide();
                GameObject.Destroy(miniboss);
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if ((gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 14 && distance(bramaWieza.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 10) || cameraExit)
            {
                gameObject.GetComponent<Hero>().isMovable = false;
                cameraExit = true;
                if (firstEnter)
                {
                    firstEnter = false;
                    cameraPositionStart = Game1.MainCam().Position;
                    cameraForwardStart = Game1.MainCam().Forward;
                    cameraPositionTarget = new Vector3(171f, 10.3f, -1010f);
                    cameraForwardTarget = new Vector3(-0.050278064f, -0.5278733f, -0.84783363f);
                }
                if (cameraExit)
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
                        EnemiesStats.chanceToDropMapWieza = 20;
                        EnemiesStats.chanceToDropMapSkoczek = 80;
                        lista.pierwszeWejscie = false;
                        lista.poWiezy = true;
                        Game1.MainCam().Forward = cameraForward;
                        Game1.MainCam().hero = null;
                        WorldManager.LoadMap(new TestWorld());
                    }
                }
            }





            if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L))
            {
                miniboss.GetComponent<EnemiesStats>().health = 10;
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
