using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class BossTower : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        //w sekundach
        public float specialAttackDistance = 1.5f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 60.0f;
        public float movingSpeed = 40.0f;
        public bool isInfected = false;
        public bool isMovable = true;
        public bool samouczek = false;
        GameObject boss;


        private GameObject hero;
        private GameObject weapon;
        // wektor wskazujacy od pozycji wiezy do gracza
        public Vector3 targetHeroPosition;
        public float chargingTime = 0.0f;
        public float actualSpecialAttackDistance = 0.0f;
        //jezeli atak specjalny sie laduje to wieza nic nie robi
        public bool isTheSpecialAttackCharging = false;
        //jak wieza wykonuje swoj atak to nic innego nie robi
        public bool isDuringSpecialAttack = false;
        //liczy coldown dla ataku
        private float count;

        float health = 300;

        Random rand = new Random(124);
        private int destinationNum;
        private float destinationX;
        private float destinationZ;
        private float speed = 10;
        //private float widthOfView = 20; // szerokosc widzenia - pol szerokosci korytarza / obszaru w ktorym wieza widzi gracza
        private float widthOfView = 11; // szerokosc widzenia - pol szerokosci korytarza / obszaru w ktorym wieza widzi gracza
        //private bool lookAround = false; // czy ma se rozgladac, ustawiane na true gdy wieza zatrzymuje sie na ktoryms z waypointow
        private int lookAroundTimeMax = 100; // ile czasu ma sie rozgladac
        private int lookAroundTime = 0; // ile czasu ma sie jeszcze rozgladac
        private bool drawNewWaypoint = false; // czy ma losowac nowy waypoint
        private int direction = 0; // w ktora strone sie patrzy jak idzie (0 - N, 1 - S, 2 - E, 3 - W)
        public bool invincible = false; // gdy aktywne to nie mozna zadawac mu obrazen
        private bool dizzy = false; // oszolomienie - gdy aktywne nie moze sie poruszyc
        private int dizzyTime = 0; // czas do konca oczolomienia
        private int dizzyTimeMax = 100; // ile czasu ma byc oszolomiony
        private bool seeHeroLong = false; // czy widzi bohatera daleko
        private bool seeHeroShort = false; // czy widzi bohatera blisko
        private bool isAttackLong = false; // czy atakuje daleko
        private bool isAttackShort = false; // czy atakuje blisko

        /// <summary>
        /// ///////////////////
        /// </summary>
        private bool goToWP = true;
        private bool attackLong = false;
        private bool canAttack = false;
        private bool lookAroundShort = false;
        private bool lookAroundLong = false;
        private bool wasAttacked = false;
        private bool pauseAfterLongAttack = false;
        private bool pauseAfterWasAttacked = false;

        public bool vulnerable = false;
        private float valyouOfHeroAttack;
        private float lastTimeCheckHealth;


        //private float rangeOfShortAttack = 30;
        private float rangeOfShortAttack = 22;
        private float dizzyTimeAfterShortAttack = 100;
        private float dizzyTimeBeforeLongAttack = 100; // jednak nie?
        private float dizzyTimeAfterLongAttack = 100;

        struct BossTowerWaypoint
        {
            public float x;
            public float z;
            public bool N;
            public bool S;
            public bool E;
            public bool W;
        }

        BossTowerWaypoint[] waypoints = new BossTowerWaypoint[12];


        public override void Init()
        {

        }
        public override void Start()
        {
            
            foreach (GameObject GO in World.AllGameObjects)
            {
                if (GO.tag == "hero")
                {
                    hero = GO;
                }
            }

            weapon = GameObject.Find("weapon");

            boss = GameObject.FindByTag("enemy");

            for(int i = 0; i < 12; i++)
            {
                waypoints[i].N = true;
                waypoints[i].S = true;
                waypoints[i].E = true;
                waypoints[i].W = true;
            }

            for (int i = 0; i < 12; i++)
            {
                if (i <= 3) waypoints[i].N = false;
                if (i >= 8) waypoints[i].S = false;
                if (i == 3 || i == 7 || i == 11) waypoints[i].E = false;
                if (i == 0 || i == 4 || i == 8) waypoints[i].W = false;
            }
            for (int i = 0; i < 12; i++)
            {
                if (i == 0 || i == 4 || i == 8) waypoints[i].x = 70;
                else if (i == 1 || i == 5 || i == 9) waypoints[i].x = 110;
                else if (i == 2 || i == 6 || i == 10) waypoints[i].x = 150;
                else if (i == 3 || i == 7 || i == 11) waypoints[i].x = 190;

                if (i <= 3) waypoints[i].z = -1010;
                else if (i >= 4 && i <= 7) waypoints[i].z = -970;
                else if (i >= 8) waypoints[i].z = -930;
            }
            /*
            // waypointy z poprzedniej mapy
            for (int i = 0; i < 12; i++)
            {
                if (i == 0 || i == 4 || i == 8) waypoints[i].x = 180;
                else if (i == 1 || i == 5 || i == 9) waypoints[i].x = 290;
                else if (i == 2 || i == 6 || i == 10) waypoints[i].x = 400;
                else if (i == 3 || i == 7 || i == 11) waypoints[i].x = 510;

                if (i <= 3) waypoints[i].z = -975;
                else if (i >=4 && i <=7) waypoints[i].z = -895;
                else if (i >= 8) waypoints[i].z = -815;
            }
            */

            gameObject.GetComponent<Transform>().position.X = 150;
            gameObject.GetComponent<Transform>().position.Z = -970;
            gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
            destinationNum = 6;
            destinationX = waypoints[destinationNum].x;
            destinationZ = waypoints[destinationNum].z;

            lastTimeCheckHealth = gameObject.GetComponent<EnemiesStats>().health;

            gameObject.GetComponent<EnemiesStats>().maxHealth = 40;
            gameObject.GetComponent<EnemiesStats>().health = 40;
        }
        public override void Update(GameTime gameTime)
        {
            positionCheck();
            //Console.WriteLine(gameObject.GetComponent<Transform>().position);
            //Console.WriteLine("Jestem miniboss wieża!");
            if (isMovable)
            {
                // atakowanie bossa ma byc mozliwe tylko i wylacznie gdy goToWP == true
                if (goToWP == true)// && weapon.GetComponent<PlayerAttack>().damage != 0)
                {
                    vulnerable = true;
                    //valyouOfHeroAttack = weapon.GetComponent<PlayerAttack>().damage;
                    //weapon.GetComponent<PlayerAttack>().damage = 0;
                }
                else
                {
                    vulnerable = false;
                    //weapon.GetComponent<PlayerAttack>().damage = valyouOfHeroAttack;
                }

                // 1. ------------------------------------------------
                // idz do WayPointa
                if (goToWP == true)
                {
                    // czy widzi hero przed soba?
                    switch (direction)
                    {
                        case 0:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z) // "Jestem na 90% przekonany ze znak nierownosci pomiedzy wspolrzednymi Z powinien byc odwrotnie, ale jakims cudem dziala tylko jak jest tak, wiec trudno, zostawiam jak jest." - M
                            {
                                //Console.WriteLine("Widze cie! :D idac w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                goToWP = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( idac w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 1:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z) // "Jestem na 90% przekonany ze znak nierownosci pomiedzy wspolrzednymi Z powinien byc odwrotnie, ale jakims cudem dziala tylko jak jest tak, wiec trudno, zostawiam jak jest." - M
                            {
                                //Console.WriteLine("Widze cie! :D idac w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                goToWP = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( idac w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 2:
                            if (hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView)
                            {
                                //Console.WriteLine("Widze cie! :D idac w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                goToWP = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( idac w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 3:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView)
                            {
                                //Console.WriteLine("Widze cie! :D idac w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                goToWP = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( idac w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                    }
                    // czy zostal zaatakowany?
                    if (gameObject.GetComponent<EnemiesStats>().health != lastTimeCheckHealth)
                    {
                        //Console.WriteLine("Zostalem zaatakowany! AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!");
                        lastTimeCheckHealth = gameObject.GetComponent<EnemiesStats>().health;
                        pauseAfterWasAttacked = true;
                        goToWP = false;
                    }
                    // czy doszedl do WP?
                    if (goToWP == true && (gameObject.GetComponent<Transform>().position.X < destinationX + 2 && gameObject.GetComponent<Transform>().position.X > destinationX - 2 && gameObject.GetComponent<Transform>().position.Z < destinationZ + 2 && gameObject.GetComponent<Transform>().position.Z > destinationZ - 2))
                    {
                        //Console.WriteLine("Doszedlem do waypointa nr {0}", destinationNum);
                        lookAroundShort = true;
                        goToWP = false;
                    }

                    // idzie do WP
                    if (goToWP == true)
                    {
                        destinationNumCheck();
                        destinationX = waypoints[destinationNum].x;
                        destinationZ = waypoints[destinationNum].z;
                        // Porusza sie w kierunku doceowego waypointa (jednego z 12 skrzyzowan)
                        if (gameObject.GetComponent<Transform>().position.X < destinationX)
                        {
                            gameObject.GetComponent<Transform>().position.X += speed * Time.DeltaTime;
                        }
                        else if (gameObject.GetComponent<Transform>().position.X > destinationX)
                        {
                            gameObject.GetComponent<Transform>().position.X -= speed * Time.DeltaTime;
                        }

                        if (gameObject.GetComponent<Transform>().position.Z < destinationZ)
                        {
                            gameObject.GetComponent<Transform>().position.Z += speed * Time.DeltaTime;
                        }
                        else if (gameObject.GetComponent<Transform>().position.Z > destinationZ)
                        {
                            gameObject.GetComponent<Transform>().position.Z -= speed * Time.DeltaTime;
                        }
                    }
                }
                // 2. ------------------------------------------------
                // zosatl zaatakowany, wiec chwile czeka ogluszony czy cos
                else if (pauseAfterWasAttacked)
                {
                    // czeka chwile
                    chargingTime += Time.DeltaTime;
                    if (chargingTime >= skillChargeDuration)
                    {
                        chargingTime = 0.0f;
                        wasAttacked = true;
                        pauseAfterWasAttacked = false;
                    }
                }
                // 3. ------------------------------------------------
                // zosatl zaatakowany, wiec odwraca sie na chwile a potem idzie dalej
                else if (wasAttacked == true)
                {
                    // zmienia WP na przeciwny
                    switch (direction)
                    {
                        case 0:
                            destinationNum += 4;
                            direction = 1;
                            break;
                        case 1:
                            destinationNum -= 4;
                            direction = 0;
                            break;
                        case 2:
                            destinationNum -= 1;
                            direction = 3;
                            break;
                        case 3:
                            destinationNum += 1;
                            direction = 2;
                            break;
                    }
                    destinationNumCheck();
                    // sprawdza czy widzi hero przed soba
                    switch (direction)
                    {
                        case 0:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z) // "Jestem na 90% przekonany ze znak nierownosci pomiedzy wspolrzednymi Z powinien byc odwrotnie, ale jakims cudem dziala tylko jak jest tak, wiec trudno, zostawiam jak jest." - M
                            {
                                //Console.WriteLine("Widze cie! :D odwracajac sie w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                wasAttacked = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( odwracajac sie w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 1:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z) // "Jestem na 90% przekonany ze znak nierownosci pomiedzy wspolrzednymi Z powinien byc odwrotnie, ale jakims cudem dziala tylko jak jest tak, wiec trudno, zostawiam jak jest." - M
                            {
                                //Console.WriteLine("Widze cie! :D odwracajac sie w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                wasAttacked = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( odwracajac sie w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 2:
                            if (hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView)
                            {
                                //Console.WriteLine("Widze cie! :D odwracajac sie w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                wasAttacked = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( odwracajac sie w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                        case 3:
                            if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X && hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView)
                            {
                                //Console.WriteLine("Widze cie! :D odwracajac sie w kierunku nr {0}", direction);
                                attackLong = true;
                                canAttack = true;
                                wasAttacked = false;
                            }
                            else
                            {
                                //Console.WriteLine("Nie widze cie! :( odwracajac sie w kierunku nr {0}", direction);
                                //attackLong = false;
                            }
                            break;
                    }
                    // zmienia WP z powrotem
                    if (wasAttacked == true)
                    {
                        switch (direction)
                        {
                            case 0:
                                destinationNum += 4;
                                direction = 1;
                                break;
                            case 1:
                                destinationNum -= 4;
                                direction = 0;
                                break;
                            case 2:
                                destinationNum -= 1;
                                direction = 3;
                                break;
                            case 3:
                                destinationNum += 1;
                                direction = 2;
                                break;
                        }
                        destinationNumCheck();
                        goToWP = true;
                        wasAttacked = false;
                    }
                }

                // 4. ------------------------------------------------
                // rozglada sie blisko bo jest na WP
                else if (lookAroundShort)
                {
                    // czy hero jest blisko?
                    if (distance(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z, hero.GetComponent<Transform>().position.X, hero.GetComponent<Transform>().position.Z) <= rangeOfShortAttack)
                    {
                        chargingTime += Time.DeltaTime;
                        if (chargingTime >= skillChargeDuration)
                        {
                            isTheSpecialAttackCharging = false;
                            chargingTime = 0.0f;
                            isDuringSpecialAttack = true;
                            //Console.WriteLine("Atakuje cie z bliska!");
                            hero.GetComponent<Hero>().dealDamage(75);
                        }
                    }
                    else
                    {
                        lookAroundLong = true;
                        lookAroundShort = false;
                    }
                }
                // 5. ------------------------------------------------
                // rozglada sie daleko bo jest na WP a blisko juz sie rozejrzal
                else if (lookAroundLong == true)
                {
                    if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView || hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView)
                    {
                        // ustawia odpowiedni WP
                        if (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView)
                        {
                            if (hero.GetComponent<Transform>().position.Z < gameObject.GetComponent<Transform>().position.Z)
                            {
                                direction = 0;
                                destinationNum -= 4;
                                attackLong = true;
                                canAttack = true;
                                lookAroundLong = false;
                            }
                            else
                            {
                                direction = 1;
                                destinationNum += 4;
                                attackLong = true;
                                canAttack = true;
                                lookAroundLong = false;
                            }
                        }
                        else
                        {
                            if (hero.GetComponent<Transform>().position.X < gameObject.GetComponent<Transform>().position.X)
                            {
                                direction = 3;
                                destinationNum -= 1;
                                attackLong = true;
                                canAttack = true;
                                lookAroundLong = false;
                            }
                            else
                            {
                                direction = 2;
                                destinationNum += 1;
                                attackLong = true;
                                canAttack = true;
                                lookAroundLong = false;
                            }
                        }
                        //Console.WriteLine("Widze cie! :D zaatakuje w kierunku nr {0}", direction);
                        destinationNumCheck();
                    }
                    // losuje nowy WP i idzie do niego
                    else
                    {
                        int tmp = rand.Next(0, 4); // rand od 0 do 3
                        while (true)
                        {
                            //Console.WriteLine("Ustawiam nowy tmp na {0}", tmp);
                            if (tmp == 0) //TODO zmienic to na switcha
                            {
                                if (waypoints[destinationNum].N == true)
                                {
                                    destinationNum = destinationNum - 4;
                                    direction = tmp;
                                    Console.WriteLine("Ustawiam nowy waypoint na {0}", destinationNum);
                                    drawNewWaypoint = false;
                                    break;
                                }
                            }
                            else if (tmp == 1)
                            {
                                if (waypoints[destinationNum].S == true)
                                {
                                    destinationNum = destinationNum + 4;
                                    direction = tmp;
                                    //Console.WriteLine("Ustawiam nowy waypoint na {0}", destinationNum);
                                    drawNewWaypoint = false;
                                    break;
                                }
                            }
                            else if (tmp == 2)
                            {
                                if (waypoints[destinationNum].E == true)
                                {
                                    destinationNum = destinationNum + 1;
                                    direction = tmp;
                                    //Console.WriteLine("Ustawiam nowy waypoint na {0}", destinationNum);
                                    drawNewWaypoint = false;
                                    break;
                                }
                            }
                            else if (tmp == 3)
                            {
                                if (waypoints[destinationNum].W == true)
                                {
                                    destinationNum = destinationNum - 1;
                                    direction = tmp;
                                    //Console.WriteLine("Ustawiam nowy waypoint na {0}", destinationNum);
                                    drawNewWaypoint = false;
                                    break;
                                }
                            }
                            tmp = rand.Next(0, 4); // rand od 0 do 3

                        }
                        destinationNumCheck();
                        //Console.WriteLine(destinationNum);
                        destinationX = waypoints[destinationNum].x;
                        destinationZ = waypoints[destinationNum].z;
                        goToWP = true;
                        lookAroundLong = false;
                    }
                }

                // 6. ------------------------------------------------
                // atakuje gracza szarzujac
                else if (attackLong == true)
                {
                    destinationNumCheck();
                    destinationX = waypoints[destinationNum].x;
                    destinationZ = waypoints[destinationNum].z;
                    // przemieszcza sie
                    //Console.WriteLine("Atakuje w kierunku nr {0}", direction);
                    switch (direction)
                    {
                        case 0:
                            gameObject.GetComponent<Transform>().position.Z -= 2 * speed * Time.DeltaTime;
                            break;
                        case 1:
                            gameObject.GetComponent<Transform>().position.Z += 2 * speed * Time.DeltaTime;
                            break;
                        case 2:
                            gameObject.GetComponent<Transform>().position.X += 2 * speed * Time.DeltaTime;
                            break;
                        case 3:
                            gameObject.GetComponent<Transform>().position.X -= 2 * speed * Time.DeltaTime;
                            break;
                    }
                    // Sprawdza czy nie dotarl juz czasem do gracza
                    if (canAttack)
                    {
                        switch (direction)
                        {
                            case 0:
                                if (gameObject.GetComponent<Transform>().position.Z <= hero.GetComponent<Transform>().position.Z && (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView))
                                {
                                    //Console.WriteLine("Dotarlem do hero");
                                    hero.GetComponent<Hero>().dealDamage(100);
                                    canAttack = false;
                                }
                                break;
                            case 1:
                                if (gameObject.GetComponent<Transform>().position.Z >= hero.GetComponent<Transform>().position.Z && (hero.GetComponent<Transform>().position.X <= gameObject.GetComponent<Transform>().position.X + widthOfView && hero.GetComponent<Transform>().position.X >= gameObject.GetComponent<Transform>().position.X - widthOfView))
                                {
                                    //Console.WriteLine("Dotarlem do hero");
                                    hero.GetComponent<Hero>().dealDamage(100);
                                    canAttack = false;
                                }
                                break;
                            case 2:
                                if (gameObject.GetComponent<Transform>().position.X >= hero.GetComponent<Transform>().position.X && (hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView))
                                {
                                    //Console.WriteLine("Dotarlem do hero");
                                    hero.GetComponent<Hero>().dealDamage(100);
                                    canAttack = false;
                                }
                                break;
                            case 3:
                                if (gameObject.GetComponent<Transform>().position.X <= hero.GetComponent<Transform>().position.X && (hero.GetComponent<Transform>().position.Z <= gameObject.GetComponent<Transform>().position.Z + widthOfView && hero.GetComponent<Transform>().position.Z >= gameObject.GetComponent<Transform>().position.Z - widthOfView))
                                {
                                    //Console.WriteLine("Dotarlem do hero");
                                    hero.GetComponent<Hero>().dealDamage(100);
                                    canAttack = false;
                                }
                                break;
                        }
                    }
                    /*
                    if (canAttack == true && distance(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z, hero.GetComponent<Transform>().position.X, hero.GetComponent<Transform>().position.Z) < 2 * widthOfView)
                    {
                        Console.WriteLine("Dotarlem do hero");
                        //TODO zamiast promienia widthofwiew musi byc odcinek polozony poziomo lub pionowo w zaleznosci od kierunku poruszania sie
                        hero.GetComponent<Hero>().dealDamage(50);
                        canAttack = false;
                    }
                    */
                    // Sprawdza czy nie dotarl juz czasem do skrzyzowania
                    if (gameObject.GetComponent<Transform>().position.X < destinationX + 2 && gameObject.GetComponent<Transform>().position.X > destinationX - 2 && gameObject.GetComponent<Transform>().position.Z < destinationZ + 2 && gameObject.GetComponent<Transform>().position.Z > destinationZ - 2)
                    {
                        //Console.WriteLine("Dotarlem do skrzyzowania");
                        pauseAfterLongAttack = true;
                        attackLong = false;
                    }
                }
                // 7. ------------------------------------------------
                // odpoczywa chwile po szarzy
                else if (pauseAfterLongAttack == true)
                {

                    chargingTime += Time.DeltaTime;
                    if (chargingTime >= skillChargeDuration)
                    {
                        chargingTime = 0.0f;
                        lookAroundShort = true;
                        pauseAfterLongAttack = false;
                    }
                }
            }
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection) { }
        /*
        public float distance(Vector3 w1, Vector3 w2)
        {
            double dis = 0.0;
            dis = Math.Sqrt((w2.X - w1.X) * (w2.X - w1.X) + (w2.Y - w1.Y) * (w2.Y - w1.Y) + (w2.Z - w1.Z) * (w2.Z - w1.Z));
            return (float)dis;
        }
        */
        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }
        public void destinationNumCheck()
        {
            // Jezleli destinationNum wychodzi poza przedzial nastepuje reset parametrow minibossa
            // Teoretycznie nie powinno sie to nigdy wydarzyc, ale gdyby jakims cudem do tego doszlo to program by sie wywalil,
            // wiec lepiej dawac ta funkcje wszedzie gdzie destinationNum sie zmienia, tak na wszelki wypadek
            if (destinationNum < 0 || destinationNum >= 12)
            {
                gameObject.GetComponent<Transform>().position.X = 150;
                gameObject.GetComponent<Transform>().position.Z = -970;
                gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
                destinationNum = 6;
                destinationX = waypoints[destinationNum].x;
                destinationZ = waypoints[destinationNum].z;

                goToWP = true;
                attackLong = false;
                canAttack = false;
                lookAroundShort = false;
                lookAroundLong = false;
                wasAttacked = false;
                pauseAfterLongAttack = false;
                pauseAfterWasAttacked = false;
            }
        }
        public void positionCheck()
        {
            // Jezleli miniboss wychodzi poza przedzial to to teleportuje go z powrotem
            if (gameObject.GetComponent<Transform>().position.X < 50 || gameObject.GetComponent<Transform>().position.X > 210 || gameObject.GetComponent<Transform>().position.Z < -1030 || gameObject.GetComponent<Transform>().position.Z > -910)
            {
                gameObject.GetComponent<Transform>().position.X = 150;
                gameObject.GetComponent<Transform>().position.Z = -970;
                gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
                destinationNum = 6;
                destinationX = waypoints[destinationNum].x;
                destinationZ = waypoints[destinationNum].z;

                goToWP = true;
                attackLong = false;
                canAttack = false;
                lookAroundShort = false;
                lookAroundLong = false;
                wasAttacked = false;
                pauseAfterLongAttack = false;
                pauseAfterWasAttacked = false;
            }
        }
    }
}
