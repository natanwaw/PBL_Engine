using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
	public class Hero : GameObjectComponent
    {
        public static int fragmentyMapySkoczka = 0;
        public static int fragmentyMapyWiezy = 0;
        public static int fragmentyMapyGonca = 0;
        public bool isMovable = true;
        public bool isMovable2 = true;
        public float x = 200;
        public float y;
        public float z;
        float speed;
        public bool up;
        private bool down;
        private bool left;
        private bool right;
        private float maxHealth = 800;
        private float health;
        private bool hide = false;
        private bool hideButVisible = false;
        float hideRange = 30; //minimalna odleglosc w jakiej musimy byc od przeciwnikow zeby sie schowac
        bool start = true;
        private bool alive = true;
        GameObject modelObject;
        private CharStats guiStats;
        private bool canSpecialTowerAttack = false;
        private bool canSpecialKnightAttack = false;
        private bool canSpecialBishopAttack = false;
        private bool isSpecialTowerAttack = false;
        private bool isSpecialKnightAttack = false;
        private bool isSpecialBishopAttack = false;
        private bool isSpecialBishopAttack2 = false;
        private bool isSpecialBishopAttack3 = false;
        private bool isSpecialBishopAttack4 = false;
        private bool isSpecialBishopAttack5 = false;
        private bool isSpecialBishopAttack6 = false;

        //do atakow specjalnych
        private float countTower;
        public float specialAttackDistanceTower = 1.5f;
        public float specialAttackSpeedTower = 80.0f;
        public float actualSpecialAttackDistanceTower = 0.0f;

        private float countKnight;
        private bool isTheSpecialAttackKnightCharging = false;
        private bool isDuringSpecialAttackKnight = false; 
        public float skillChargeDurationKnight = 1.5f;
        public float maxHeightKnight = 200.0f;
        public float chargingTimeKnight = 0.0f;
        public float specialAttackSpeedKnight = 80.0f;
        private bool attackKnightEnd = false;


        private float countBishop;
        public float specialAttackDistanceBishop = 0.2f;
        public float specialAttackDistance2Bishop = 0.4f;
        public float specialAttackSpeedBishop = 80.0f;
        public float actualSpecialAttackDistanceBishop = 0.0f;


        public Vector3 targetPosition;
        private GameObject boss;
        private GameObject HitPoint;
        private GameObject weapon;

        private GameObject HitPoint2;
        public Hero()
        {

        }

        public override void Init()
        {
            

        }
        public override void Start() 
        {
            HitPoint2 = GameObject.Find("Hitpoint");
            setSpeed(25);
            modelObject = GameObject.Find("hero");
            health = maxHealth;
            //x = gameObject.GetComponent<Transform>().position.X - Game1.MainCam().cameraDistance + Game1.MainCam().lookX;
            //y = gameObject.GetComponent<Transform>().position.Y + 40;
            //z = gameObject.GetComponent<Transform>().position.Z + Game1.MainCam().cameraDistance + Game1.MainCam().lookZ;
            x = 0;
            y = 0;
            z = 0;
            Game1.MainCam().targetX = gameObject.GetComponent<Transform>().position.X - Game1.MainCam().cameraDistance + Game1.MainCam().lookX;
            Game1.MainCam().targetY = gameObject.GetComponent<Transform>().position.Y + 40;
            Game1.MainCam().targetZ = gameObject.GetComponent<Transform>().position.Z + Game1.MainCam().cameraDistance + Game1.MainCam().lookZ;

            guiStats = GameObject.Find("health_stat").GetComponent<CharStats>();
            boss = GameObject.FindByTag("enemy");
            HitPoint = GameObject.Find("Hitpoint");
            weapon = GameObject.FindByTag("weapon");
        }
        int w, a, s, d;
        public override void Update(GameTime gameTime)
        {
            Light.PLposition = gameObject.GetComponent<Transform>().position;
            Light.PLposition = new Vector3(Light.PLposition.X, Light.PLposition.Y + 10, Light.PLposition.Z);
            //Console.WriteLine("Jestem hero!");
           // Console.WriteLine(gameObject.GetComponent<Transform>().position);
            //Console.WriteLine("kamera" + Game1.MainCam().Position);
            //Console.WriteLine("forward" + Game1.MainCam().Forward);
            if (hide == false && hideButVisible == false)
            {
                foreach (GameObject GO in World.AllGameObjects)
                {
                    if (GO.tag == "bush")
                    {
                        float bushX = GO.GetComponent<Transform>().position.X;
                        float bushZ = GO.GetComponent<Transform>().position.Z;
                        float bushRange = GO.GetComponent<Bush>().getRange();
                        //Console.WriteLine("X = {0}, Z = {1}, range = {2}", bushX, bushZ, bushRange);
                        if (distance(bushX, bushZ, gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z) < bushRange)
                        {
                            foreach (GameObject ob in World.AllGameObjects)
                            {
                                if (ob.tag == "enemy")
                                {
                                    if (distance(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z, gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z) > hideRange)
                                    {
                                        hide = true;
                                    }
                                    else
                                    {
                                        hideButVisible = true;
                                    }
                                    break;
                                }
                                if (hide || hideButVisible)
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                bool stillHide = false;
                foreach (GameObject GO in World.AllGameObjects)
                {
                    if (GO.tag == "bush")
                    {
                        float bushX = GO.GetComponent<Transform>().position.X;
                        float bushZ = GO.GetComponent<Transform>().position.Z;
                        float bushRange = GO.GetComponent<Bush>().getRange();
                        //Console.WriteLine("X = {0}, Z = {1}, range = {2}", bushX, bushZ, bushRange);
                        if (distance(bushX, bushZ, gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z) < bushRange)
                        {
                            stillHide = true;
                            break;
                        }
                    }
                }
                if (stillHide == false)
                {
                    hide = false;
                    hideButVisible = false;
                }
            }
            
            if (isSpecialKnightAttack == false)
            {
                y = gameObject.GetComponent<Transform>().position.Y;
            }
            
            //Console.WriteLine("X = {0}, Y = {1}, Z = {2}", x, y, z);
            if (checkDeath())
            {
                GameObject.Find("loser_text").isActive = true;
            }
            //KeyboardState kstate = Keyboard.GetState();
            /*
            if (Input.GetKey.IsKeyDown(Keys.Up) == true)
            {
                moveUp();
            }
            if (Input.GetKey.IsKeyDown(Keys.Down) == true)
            {
                moveDown();
            }
            if (Input.GetKey.IsKeyDown(Keys.Left) == true)
            {
                moveLeft();
            }
            if (Input.GetKey.IsKeyDown(Keys.Right) == true)
            {
                moveRight();
            }
            */
            if(isMovable2)
            {
                if (isMovable)
                {
                    if (Input.IsKeyPressed(Keys.W) || start)
                    {
                        moveUp();
                        up = true;
                        w++;
                    }
                    else
                    {
                        up = false;
                    }
                    if (Input.IsKeyPressed(Keys.S))
                    {
                        moveDown();
                        down = true;
                        s++;
                    }
                    else
                    {
                        down = false;
                    }
                    if (Input.IsKeyPressed(Keys.A))
                    {
                        moveLeft();
                        left = true;
                        a++;
                    }
                    else
                    {
                        left = false;
                    }
                    if (Input.IsKeyPressed(Keys.D) || start)
                    {
                        start = false;
                        moveRight();
                        right = true;
                        d++;
                    }
                    else
                    {
                        right = false;
                    }
                }
            }
            

            if (Input.GetKey.IsKeyDown(Keys.T) == true && Input.GetKey.IsKeyDown(Keys.LeftControl) == true ) //teleport
            {
                Game1.MainCam().hero = null;
                Time.Resume();
                WorldManager.LoadMap(new TestMiniBossTower());
                //WorldManager.LoadMap(new TestMiniBossSkoczek());
                //WorldManager.LoadMap(new TestMiniBossGoniec());
                //Console.WriteLine(w);
                //Console.WriteLine(a);
                //Console.WriteLine(s);
                //Console.WriteLine(d);
            }

            if (Input.GetKey.IsKeyDown(Keys.Y) == true && Input.GetKey.IsKeyDown(Keys.LeftControl) == true) //teleport
            {
                Game1.MainCam().hero = null;
                Time.Resume();
                //WorldManager.LoadMap(new TestMiniBossTower());
                WorldManager.LoadMap(new TestMiniBossSkoczek());
                //WorldManager.LoadMap(new TestMiniBossGoniec());
                //Console.WriteLine(w);
                //Console.WriteLine(a);
                //Console.WriteLine(s);
                //Console.WriteLine(d);
            }
            if (Input.GetKey.IsKeyDown(Keys.U) == true && Input.GetKey.IsKeyDown(Keys.LeftControl) == true) //teleport
            {
                Game1.MainCam().hero = null;
                Time.Resume();
                //WorldManager.LoadMap(new TestMiniBossTower());
                //WorldManager.LoadMap(new TestMiniBossSkoczek());
                WorldManager.LoadMap(new TestMiniBossGoniec());
                //Console.WriteLine(w);
                //Console.WriteLine(a);
                //Console.WriteLine(s);
                //Console.WriteLine(d);
            }
            if (Input.GetKey.IsKeyDown(Keys.I) == true && false) //teleport
            {
                Game1.MainCam().hero = null;
                Time.Resume();
                //WorldManager.LoadMap(new TestMiniBossTower());
                //WorldManager.LoadMap(new TestMiniBossSkoczek());
                WorldManager.LoadMap(new TestFinalBoss());
                //Console.WriteLine(w);
                //Console.WriteLine(a);
                //Console.WriteLine(s);
                //Console.WriteLine(d);
            }
            if (Input.GetKey.IsKeyDown(Keys.E) == true && canSpecialTowerAttack == true && isSpecialTowerAttack == false) //atak wiezy
            {
                if (HitPoint.GetComponent<TerrainRaycast>().HitPoint.HasValue)
                {
                    //Transform.LookAtTarget(gameObject.parent.parent.GetComponent<Transform>(), gameObject.parent.GetComponent<Transform>(), HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value, 0f);
                    //targetPosition = HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value;
                    targetPosition = Transform.PositionToMoveTowards(gameObject.GetComponent<Transform>().Position, HitPoint2.GetComponent<TerrainRaycast>().HitPoint.Value, 0.1f);
                    //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                    targetPosition.Normalize();
                    actualSpecialAttackDistanceTower = 0.0f;
                    isSpecialTowerAttack = true;
                }
                //targetPosition = boss.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                
            }
            if (Input.GetKey.IsKeyDown(Keys.F ) == true && canSpecialKnightAttack == true && isSpecialKnightAttack == false) //atak konia
            {
                if (HitPoint.GetComponent<TerrainRaycast>().HitPoint.HasValue)
                {
                    targetPosition = Transform.PositionToMoveTowards(gameObject.GetComponent<Transform>().Position, HitPoint2.GetComponent<TerrainRaycast>().HitPoint.Value, 0.1f);
                    //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                    targetPosition.Normalize();
                    chargingTimeKnight = 0.0f;
                    isTheSpecialAttackKnightCharging = true;
                    isSpecialKnightAttack = true;
                }
                    
            }
            if (Input.GetKey.IsKeyDown(Keys.G) == true && canSpecialBishopAttack == true && isSpecialBishopAttack == false) //atak gonca
            {
                isSpecialBishopAttack = true;
            }

            if (up && left) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            else if (down && left) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 90f, 0f, 1f);
            else if (up && right) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 270f, 0f, 1f);
            else if (down && right) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 180f, 0f, 1f);
            else if (up) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 315f, 0f, 1f);
            else if (down) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 135f, 0f, 1f);
            else if (left) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 45f, 0f, 1f);
            else if (right) modelObject.GetComponent<Transform>().rotation = new Quaternion(0f, 225f, 0f, 1f);

            if (up || left || right || down)
            {
                modelObject.GetComponent<Animation>().Resume();
            }
            else
                modelObject.GetComponent<Animation>().Pause();
            if(Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X+x, gameObject.GetComponent<Transform>().position.Z+z)) - Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z)) > 1.2f)
            {
                x = 0;
                z = 0;
            }
            gameObject.GetComponent<Transform>().position.X += x;
            gameObject.GetComponent<Transform>().position.Z += z;
            x = 0;
            z = 0;
            if (gameObject.GetComponent<Transform>().position.X > 0 && gameObject.GetComponent<Transform>().position.X < 1024)
                if (gameObject.GetComponent<Transform>().position.Z < 0 && gameObject.GetComponent<Transform>().position.Z > -1024)
                    gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));

            
            // umiejetnosci specjalne
            if (isSpecialTowerAttack == true)
            {
                actualSpecialAttackDistanceTower += Time.DeltaTime;
                if (actualSpecialAttackDistanceTower >= specialAttackDistanceTower)
                {
                    isSpecialTowerAttack = false;
                    actualSpecialAttackDistanceTower = 0.0f;
                }
                gameObject.GetComponent<Transform>().position += targetPosition * Time.DeltaTime * specialAttackSpeedTower;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countTower > 80)
                            {
                                countTower = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
                
            }
            //gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));

            if (isSpecialKnightAttack == true)
            {
                //attackCount = 0;
                // podskakuje przed atakiem
                if (isTheSpecialAttackKnightCharging)
                {

                    gameObject.GetComponent<Transform>().position.Y += maxHeightKnight / skillChargeDurationKnight * Time.DeltaTime * 5;
                    chargingTimeKnight += Time.DeltaTime * 5;

                    if (chargingTimeKnight >= skillChargeDurationKnight)
                    {
                        //Console.WriteLine("attack still");
                        isTheSpecialAttackKnightCharging = false;
                        chargingTimeKnight = 0.0f;
                        isDuringSpecialAttackKnight = true;
                        //heroPosition = hero.GetComponent<Transform>().position;
                        //targetHeroPosition = heroPosition - gameObject.GetComponent<Transform>().position;
                        //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                        //targetHeroPosition.Normalize();
                        //heroPositon = hero.GetComponent<Transform>().position;
                    }
                }
                // atakuje
                if (isDuringSpecialAttackKnight)
                {

                    foreach (GameObject ob in World.AllGameObjects)
                    {
                        if (ob.GetComponent<EnemiesStats>() != null)
                        {
                            if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                            {
                                if (countKnight > 80)
                                {
                                    ob.GetComponent<EnemiesStats>().DealDamage(50);
                                }
                                attackKnightEnd = true;
                            }
                        }
                    }

                    if (attackKnightEnd == true)
                    {
                        countKnight = 0;
                    }
                    
                    if (Math.Abs((Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X + speed * Time.DeltaTime, gameObject.GetComponent<Transform>().position.Z - speed * Time.DeltaTime)) - gameObject.GetComponent<Transform>().position.Y)) < 0.1f)
                    {
                        isDuringSpecialAttackKnight = false;
                        isSpecialKnightAttack = false;
                    }
                    
                    /*
                    if (Math.Abs((heroPosition - gameObject.GetComponent<Transform>().position).Length()) < 1.0f)
                    {
                        hero.GetComponent<Hero>().dealDamage(10);
                        isDuringSpecialAttackKnight = false;
                        isSpecialKnightAttack = false;
                    }
                    */
                    //heroPosition = hero.GetComponent<Transform>().position;
                    //targetHeroPosition = heroPosition - gameObject.GetComponent<Transform>().position;
                    //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                    //targetPosition.Normalize();
                    //heroPositon = hero.GetComponent<Transform>().position;
                    gameObject.GetComponent<Transform>().position += targetPosition * Time.DeltaTime * specialAttackSpeedKnight;
                }
            }
            if (isSpecialBishopAttack)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistanceBishop)
                {
                    isSpecialBishopAttack = false;
                    isSpecialBishopAttack2 = true;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.X += 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            if (isSpecialBishopAttack2)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistance2Bishop)
                {
                    isSpecialBishopAttack2 = false;
                    isSpecialBishopAttack3 = true;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.X -= 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            if (isSpecialBishopAttack3)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistanceBishop)
                {
                    isSpecialBishopAttack3 = false;
                    isSpecialBishopAttack4 = true;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.X += 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            if (isSpecialBishopAttack4)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistanceBishop)
                {
                    isSpecialBishopAttack4 = false;
                    isSpecialBishopAttack5 = true;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.Z -= 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            if (isSpecialBishopAttack5)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistance2Bishop)
                {
                    isSpecialBishopAttack5 = false;
                    isSpecialBishopAttack6 = true;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.Z += 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            if (isSpecialBishopAttack6)
            {
                actualSpecialAttackDistanceBishop += Time.DeltaTime;
                if (actualSpecialAttackDistanceBishop >= specialAttackDistanceBishop)
                {
                    isSpecialBishopAttack6 = false;
                    actualSpecialAttackDistanceBishop = 0.0f;
                }
                //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                gameObject.GetComponent<Transform>().position.Z -= 1 * Time.DeltaTime * specialAttackSpeedBishop;
                foreach (GameObject ob in World.AllGameObjects)
                {
                    if (ob.GetComponent<EnemiesStats>() != null)
                    {
                        if (ob.GetComponent<Collision>() != null && ob.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (countBishop > 80)
                            {
                                countBishop = 0;
                                ob.GetComponent<EnemiesStats>().DealDamage(50);
                            }
                        }
                    }
                }
            }
            gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));

            countTower++;
            countKnight++;
            countBishop++;
    //}


            //isSpecialTowerAttack = false;
            //isSpecialKnightAttack = false;
            //isSpecialBishopAttack = false;
        }
        public float getX()
        {
            return x;
        }
        public float getZ()
        {
            return z;
        }
        public Hero getHero()
        {
            return this;
        }
        public void setSpeed(float s)
        {
            speed = s;
        }
        public bool moveUp()
        {
            if (checkHight(Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X + speed * Time.DeltaTime, gameObject.GetComponent<Transform>().position.Z - speed * Time.DeltaTime))))
            {
                x += speed * Time.DeltaTime;
                z -= speed * Time.DeltaTime;
                return true;
            }
            else return false;
        }

        public bool moveDown()
        {
            if (checkHight(Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X - speed * Time.DeltaTime, gameObject.GetComponent<Transform>().position.Z + speed * Time.DeltaTime))))
            {
                x -= speed * Time.DeltaTime;
                z += speed * Time.DeltaTime;
                return true;
            }
            else return false;
        }
        public bool moveLeft()
        {
            if (checkHight(Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X - speed * Time.DeltaTime, gameObject.GetComponent<Transform>().position.Z - speed * Time.DeltaTime))))
            {
                x -= speed * Time.DeltaTime;
                z -= speed * Time.DeltaTime;
                return true;
            }
            else return false;
        }
        public bool moveRight()
        {
            if (checkHight(Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X + speed * Time.DeltaTime, gameObject.GetComponent<Transform>().position.Z + speed * Time.DeltaTime))))
            {
                x += speed * Time.DeltaTime;
                z += speed * Time.DeltaTime;
                return true;
            }
            else return false;
        }
        public void dealDamage(float damage)
        {
            health -= damage;
            GameObject go = GameObject.Find("hero");
            if (alive)
            {
                gameObject.GetComponent<AudioComponent>().Play3DSound("damage", false);
                go.GetComponent<Animation>().timer = 10;
            }
            float healthPercentage = health / maxHealth;
            guiStats.Damage(healthPercentage);
        }
        private bool checkDeath()
        {
            if (health <= 0)
            {
                if (alive)
                {
                    gameObject.GetComponent<AudioComponent>().volume = 0.2f;
                    gameObject.GetComponent<AudioComponent>().Play3DSound("dying", false);
                }
                alive = false;
                return true;
            }
                
            return false;
        }

        private bool checkHight(float hight)
        {
            if (hight > 100) return false;
            else return true;
        }

        public bool getHide()
        {
            return hide;
        }
        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }

    }
}
