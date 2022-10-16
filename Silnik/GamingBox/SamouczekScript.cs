using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class SamouczekScript : GameObjectComponent
    {
        GameObject grob;
        GameObject boss;
        GameObject wrog1;
        GameObject wrog2;
        GameObject brama;
        GameObject brama2;
        GameObject black;
        GameObject sterowanie;
        GameObject atakowanie;
        GameManager manager;
        bool timer2 = false;
        float timer2f = 0f;
        int loadingScreen = 0;
        Vector3 target = new Vector3(-1, 0, 0);
        Vector3 cameraPositionTarget;
        Vector3 cameraForwardTarget;
        Vector3 cameraForward;
        Vector3 cameraPositionStart;
        Vector3 cameraForwardStart;
        bool cameraExitMap = false;
        bool firstEnterPage5 = true;
        bool SamouczekSterowania = false;
        bool SamouczekAtakowania = false;
        bool czyPokonaniPrzeciwnicy = false;
        bool pierwszePodejscieDoZlejBramy = true;
        bool pierwszePodejscieDoZlejBramy1 = false;
        bool tempBool1 = false;
        public override void Init()
        {

        }
        public void Set()
        {

        }
        public override void Start()
        {
            manager = GameObject.Find("Hitpoint").GetComponent<GameManager>();
            grob = GameObject.Find("grob");
            boss = GameObject.FindByTag("enemy");
            wrog1 = GameObject.Find("wrog1");
            wrog2 = GameObject.Find("wrog2");
            brama = GameObject.Find("brama");
            brama2 = GameObject.Find("brama2");
            black = GameObject.Find("black");
            sterowanie = GameObject.Find("sterowanie");
            atakowanie = GameObject.Find("atakowanie");
            boss.GetComponent<Boss>().isMovable = false;
            boss.GetComponent<Boss>().samouczek = false;
            boss.GetComponent<Boss>().foundHero = true;
            boss.isActive = false;
            boss.GetComponent<Boss>().movingSpeed = 35;

            //gameObject.GetComponent<Hero>().x = 735;
            //gameObject.GetComponent<Hero>().y = Game1.terrain.getHeight(new Vector2(735,-177));
            //gameObject.GetComponent<Hero>().z = -177;
            gameObject.GetComponent<Hero>().up = true;
            gameObject.GetComponent<Hero>().setSpeed(5);

            //cameraPositionStart = new Vector3(923, 11, -89);
            cameraPositionStart.X = gameObject.GetComponent<Transform>().position.X - 100;
            cameraPositionStart.Y = gameObject.GetComponent<Transform>().position.Y+10;
            cameraPositionStart.Z = gameObject.GetComponent<Transform>().position.Z + 100;
            cameraForwardStart = new Vector3(0.88284624f, -0.2601737f, 0.39101425f);
            cameraForwardTarget = Game1.MainCam().Forward;
            cameraForward = Game1.MainCam().Forward;
            Game1.MainCam().Position = cameraPositionStart;
            Game1.MainCam().targetX = cameraPositionStart.X;
            Game1.MainCam().targetY = cameraPositionStart.Y;
            Game1.MainCam().targetZ = cameraPositionStart.Z;
            Game1.MainCam().Forward = cameraForwardStart;
            cameraPositionTarget.X = gameObject.GetComponent<Transform>().position.X-70;
            cameraPositionTarget.Y = gameObject.GetComponent<Transform>().position.Y+40;
            cameraPositionTarget.Z = gameObject.GetComponent<Transform>().position.Z+70;
            // na początku nie ma u mnie dialogów to od startu są ukryte
            gameObject.GetComponent<Dialog>()._dialogBox.Hide();
            gameObject.GetComponent<Text>()._textBox.Hide();
            black.GetComponent<SpriteRenderer>().ChangeColor(Color.White * 0);


        }
        float x = 0;
        float time = 9.17f;
        float x2 = 0;
        float time2 = 5f;
        float x3 = 0;
        float time3 = 3.34f;
        float timerCollision = 0;
        public override void Update(GameTime gameTime)
        {
            timerCollision += Time.DeltaTime;
            if (timerCollision >= 5f)
                gameObject.GetComponent<Collision>().IsPushable(true);
            if (wrog1.GetComponent<EnemiesStats>().health<=0 && wrog2.GetComponent<EnemiesStats>().health <= 0)
            {
                czyPokonaniPrzeciwnicy = true;
            }
            //Console.WriteLine("cam position" + Game1.MainCam().Position);
            //Console.WriteLine(gameObject.GetComponent<Transform>().position);
            //Console.WriteLine("cam forward" + Game1.MainCam().Forward);

                    /*Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x / time * 1.57079632679f);
                    Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x / time * 1.57079632679f);
                    Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x / time * 1.57079632679f);
                    Game1.MainCam().Forward = cameraForwardStart + (cameraForwardTarget - cameraForwardStart) * (float)Math.Sin(x / time * 1.57079632679f);*/
                    
            //jeżeli nie wyświetlamy żadnego dialogu to pozwalamy graczowi poruszać się
            if (gameObject.GetComponent<Dialog>()._dialogBox.Active == false)
            {
                gameObject.GetComponent<Hero>().isMovable = true;
                
            }
            else //w przeciwnym wypadku blokujemy taką możliwość
            {
                gameObject.GetComponent<Hero>().isMovable = false;
            }
            if(gameObject.GetComponent<Dialog>()._dialogBox.Active == false && pierwszePodejscieDoZlejBramy == false)
            {
                //manager.GuiSetOn();
            }

            //pierwszy ruch kamery - jeszcze nie ma dialogboxów
            if (x<time)
            {
                manager.GuiSetOff();
                gameObject.GetComponent<Text>()._textBox.Show();
                gameObject.GetComponent<Hero>().isMovable = false;
                x+=Time.DeltaTime;
                gameObject.GetComponent<Hero>().moveRight();
                Game1.MainCam().Position = new Vector3(796.82f, 19.25f, -267.43f);
                Game1.MainCam().Forward = new Vector3(0.9594049f+ (float)Math.Sin((x / time * 1.57079632679f)*5)/10.0f, -0.23050702f - (float)Math.Sin((x / time * 1.57079632679f) * 4) / 20.0f, 0.16250822f);
            }//nadal nie
            else if(x < time+0.34f)
            {
                gameObject.GetComponent<Text>()._textBox.Hide();
                //gameObject.GetComponent<Hero>().setSpeed(25);
                x += Time.DeltaTime;
                gameObject.GetComponent<Hero>().isMovable = false;
                Game1.MainCam().Position = new Vector3(796.82f, 19.25f, -267.43f);
                Game1.MainCam().Forward = new Vector3(0.9594049f + (float)Math.Sin((x / time * 1.57079632679f) * 5) / 10.0f, -0.23050702f - (float)Math.Sin((x / time * 1.57079632679f) * 4) / 20.0f, 0.16250822f);
                black.GetComponent<SpriteRenderer>().ChangeColor(Color.White * ((x-time)/ 0.34f));
            }//nadal nie
            else if (x < time + 0.67f)
            {
                gameObject.GetComponent<Hero>().isMovable = false;
                Game1.MainCam().Position = new Vector3(831.53f, 10.03f, -253.57f);
                Game1.MainCam().Forward = new Vector3(0.9529796f + (float)Math.Sin((x / time * 1.57079632679f) * 5) / 10.0f, -0.25068212f - (float)Math.Sin((x / time * 1.57079632679f) * 4) / 20.0f, 0.1702596f);
                black.GetComponent<SpriteRenderer>().ChangeColor(Color.White * (1f-((x - time- 0.34f) / 0.34f)));
                x += Time.DeltaTime;
            }
            // gracz po podejściu do bramy dostaje pierwszy dialog
            else if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 0)
            {
                x += Time.DeltaTime;
                gameObject.GetComponent<Hero>().isMovable = false;
                //i go wyświetlamy
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
                Game1.MainCam().Position = new Vector3(831.53f, 10.03f, -253.57f);
                Game1.MainCam().Forward = new Vector3(0.9529796f + (float)Math.Sin((x / time * 1.57079632679f) * 5) / 10.0f, -0.25068212f - (float)Math.Sin((x / time * 1.57079632679f) * 4) / 20.0f, 0.1702596f);
                cameraPositionStart = Game1.MainCam().Position;
                cameraForwardStart = Game1.MainCam().Forward;
            }
            //teraz animacja przejscia przez brame
            else if (x2<time2)
            {
                x2 += Time.DeltaTime;
                gameObject.GetComponent<Hero>().isMovable = false;
                
                if (x2 < 1.5f)
                {
                    brama.GetComponent<Transform>().rotation = new Quaternion(0f, 45f-x2*60, 0f, 1f);
                }
                else if (x2>3.34f && x2< 4.67f)
                {
                    brama.GetComponent<Transform>().rotation = new Quaternion(0f, -45f + x2*60-200f, 0f, 1f);
                }
                if (x2 < (time2 - 1.67f))
                {
                    gameObject.GetComponent<Hero>().moveRight();
                    cameraPositionTarget.X = gameObject.GetComponent<Transform>().position.X - 35;
                    cameraPositionTarget.Y = gameObject.GetComponent<Transform>().position.Y + 40;
                    cameraPositionTarget.Z = gameObject.GetComponent<Transform>().position.Z + 35;
                    Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x2 / (time2 - 1.67f) * 1.57079632679f);
                    Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x2 / (time2 - 1.67f) * 1.57079632679f);
                    Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x2 / (time2 - 1.67f) * 1.57079632679f);
                    Game1.MainCam().Forward = cameraForwardStart + (cameraForward - cameraForwardStart) * (float)Math.Sin(x2 / (time2 - 1.67f) * 1.57079632679f);
                    Game1.MainCam().Position = new Vector3(Game1.MainCam().targetX, Game1.MainCam().targetY, Game1.MainCam().targetZ);
                }

            }
            else if (Math.Abs(x2 - time2)<0.1f)
            {
                gameObject.GetComponent<Hero>().setSpeed(25);
                x2 += Time.DeltaTime;
                gameObject.GetComponent<Text>()._textBox._currentPage = 1;
                gameObject.GetComponent<Text>()._textBox.Show();
                manager.GuiSetOn();
            }
            

            //teraz gracz może sobie chodzic po mapie, dopiero gdy zbliży się do grobu to zostanie wyświetlony dialog z currentPage == 1
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 1 && distance(gameObject.GetComponent<Transform>().position, grob.GetComponent<Transform>().position) < 10)
            {
                manager.GuiSetOff();
                gameObject.GetComponent<Text>()._textBox.Hide();
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            //scena z podejściem bossa do gracza
            //teraz gdy currentPage==2, ale timer2f nie zakończył odliczania to boss podchodzi, ale dialogu jeszcze nie wyświetlamy
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2 && !timer2)
            {
                boss.isActive = true;

                boss.GetComponent<Transform>().position = new Vector3(916, 0, -80) + (new Vector3(960, 0, -213) - new Vector3(916, 0, -80)) * timer2f / 4f;
                boss.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(boss.GetComponent<Transform>().position.X, boss.GetComponent<Transform>().position.Z));
                gameObject.GetComponent<Hero>().isMovable = false;
                //boss.GetComponent<Boss>().isMovable = true;
                timer2f += Time.DeltaTime;
                if (timer2f > 4.0f)
                {
                    timer2 = true;
                }
            }
            //dopiero teraz wyświetlamy dialog z currentPage == 2 - gdy boss podejdzie do nas
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 2 && timer2)
            {
                boss.GetComponent<Boss>().isMovable = false;
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            //teraz kilka dialogów 
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 3)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
            }
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 4)
            {
                gameObject.GetComponent<Dialog>()._dialogBox.Show();
                SamouczekSterowania = true;
            }

            //wyświetlenie samouczka atakowania
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && SamouczekAtakowania)
            {
                sterowanie.isActive = false;
                atakowanie.isActive = true;
                if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    atakowanie.isActive = false;
                    SamouczekAtakowania = false;
                }
            }

            //wyświetlenie samouczka sterowania
            if (gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 && SamouczekSterowania)
            {
                sterowanie.isActive = true;
                if (Input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) )
                {
                    //sterowanie.isActive = false;
                    SamouczekSterowania = false;
                    SamouczekAtakowania = true;
                }
            }

            

            //teraz jesteśmy już po wszytskich dialogach. W listaSamouczek jest dodany ostatni teskt  "Potrzebne aby currentPage odpowiednio ustawic"; nie pamiętam już co dokładnie było nie tak, ale z tym działa
            if ((gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 5 || gameObject.GetComponent<Dialog>()._dialogBox._currentPage == 6) && !SamouczekAtakowania && !SamouczekSterowania)
            {
                if(!tempBool1)
                {
                    manager.GuiSetOn();
                    tempBool1 = true;
                }
                    
                if (!czyPokonaniPrzeciwnicy)
                {
                    gameObject.GetComponent<Text>()._textBox._currentPage = 2;
                    gameObject.GetComponent<Text>()._textBox.Show();
                }
                else
                {
                    gameObject.GetComponent<Text>()._textBox._currentPage = 3;
                    gameObject.GetComponent<Text>()._textBox.Show();
                }
                
                boss.GetComponent<Transform>().position += target * Time.DeltaTime * boss.GetComponent<Boss>().movingSpeed;
                boss.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(boss.GetComponent<Transform>().position.X, boss.GetComponent<Transform>().position.Z));
                if ((distance(gameObject.GetComponent<Transform>().position, brama.GetComponent<Transform>().position) < 10 ) && czyPokonaniPrzeciwnicy && pierwszePodejscieDoZlejBramy)
                {
                    manager.GuiSetOff();
                    //gameObject.GetComponent<Hero>().isMovable = false;
                    //cameraExitMap = true;
                    gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    pierwszePodejscieDoZlejBramy = false;
                    /*if (firstEnterPage5)
                    {

                        firstEnterPage5 = false;
                        cameraPositionStart = Game1.MainCam().Position;
                        cameraForwardStart = Game1.MainCam().Forward;
                        cameraPositionTarget = new Vector3(847, 12, -244);
                        cameraForwardTarget = new Vector3(-0.72511816f, -0.22286955f, -0.65156186f);
                    }*/
                }
                if(!pierwszePodejscieDoZlejBramy && !gameObject.GetComponent<Dialog>()._dialogBox.Active && !pierwszePodejscieDoZlejBramy1)
                {
                    manager.GuiSetOn();
                    pierwszePodejscieDoZlejBramy1 = true;
                }
                if ((distance(gameObject.GetComponent<Transform>().position, brama2.GetComponent<Transform>().position) < 10 || cameraExitMap) && czyPokonaniPrzeciwnicy)
                {
                    manager.GuiSetOff();
                    gameObject.GetComponent<Hero>().isMovable = false;
                    cameraExitMap = true;
                    //gameObject.GetComponent<Dialog>()._dialogBox.Show();
                    //pierwszePodejscieDoZlejBramy = false;
                    if (firstEnterPage5)
                    {

                        firstEnterPage5 = false;
                        cameraPositionStart = Game1.MainCam().Position;
                        cameraForwardStart = Game1.MainCam().Forward;
                        cameraPositionTarget = new Vector3(1012f, 21.3f, -169f);
                        cameraForwardTarget = new Vector3(0.92084056f, -0.38769838f, -0.041747045f);
                    }
                    if (cameraExitMap)
                    {
                        if (x3 < time3)
                        {
                            if (x3 > 0)
                            {
                                Game1.MainCam().targetX = cameraPositionStart.X + (cameraPositionTarget.X - cameraPositionStart.X) * (float)Math.Sin(x3 / time3 * 1.57079632679f);
                                Game1.MainCam().targetY = cameraPositionStart.Y + (cameraPositionTarget.Y - cameraPositionStart.Y) * (float)Math.Sin(x3 / time3 * 1.57079632679f);
                                Game1.MainCam().targetZ = cameraPositionStart.Z + (cameraPositionTarget.Z - cameraPositionStart.Z) * (float)Math.Sin(x3 / time3 * 1.57079632679f);
                                Game1.MainCam().Forward = cameraForwardStart + (cameraForwardTarget - cameraForwardStart) * (float)Math.Sin(x3 / time3 * 1.57079632679f);
                            }
                            x3 += Time.DeltaTime;
                        }
                        else if(x3 < time3 + 2f)
                        {
                            x3 += Time.DeltaTime;
                            Game1.MainCam().Position = cameraPositionTarget;
                        }
                        if (Math.Abs(x3 - time3 - 2f)<0.1f)
                        {
                            GameObject.Find("loadingScreen").isActive = true;
                            Game1.MainCam().Forward = cameraForward;
                            if (loadingScreen == 1)
                            {
                                Game1.MainCam().hero = null;
                                WorldManager.LoadMap(new TestWorld());
                            }
                            loadingScreen = 1;
                        }
                    }
                }
            }
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.Q) || loadingScreen==2)
            {
                GameObject.Find("loadingScreen").isActive = true;
                Game1.MainCam().Forward = cameraForward;
                if (loadingScreen == 2)
                {
                    Game1.MainCam().hero = null;
                    WorldManager.LoadMap(new TestWorld());
                }
                loadingScreen = 2;
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
