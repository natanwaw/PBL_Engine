using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class BossSkoczek : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        public float vulnerableChargeDuration = 5.5f;
        public float attackChargeDuration = 10.5f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 25.0f;
        public float movingSpeed = 30.0f;
        public float maxHeight = 20.0f;
        public bool isInfected = true;
        public bool isMovable = true;
        public bool samouczek = false;
        GameObject boss;

        public GameObject hero;
        private GameObject weapon;
        // wektor wskazujacy od pozycji wiezy do gracza
        public Vector3 targetHeroPosition;
        public Vector3 heroPositon;
        private Vector3 heroPosition;
        public float chargingTime = 0.0f;
        //jezeli atak specjalny sie laduje to skoczek nic nie robi
        public bool isTheSpecialAttackCharging = false;
        //jak skoczek wykonuje swoj atak to nic innego nie robi
        public bool isDuringSpecialAttack = false;
        //coldown dla ataku
        private float count;

        /// <summary>
        /// /////////////////////
        /// </summary>

        Random rand = new Random(124);
        GameObject[] bushes = new GameObject[100];
        private int bushCount = 0;
        private bool dizzy = false;
        private bool invincible = false;
        private float attackCount = 0;
        private float attackTime = 1000;
        private bool willHide = true;
        private bool isActive = true;

        /// <summary>
        /// /////////////////////////
        /// </summary>
        /// 
        private bool mustHide = true;
        private bool waitInBush = false;
        private bool waitVulnerable = false;
        private bool isAttacking = false;
        private bool waitAfterAttack = false;
        private bool wasAttacked = false;

        public bool vulnerable = false;
        private float valyouOfHeroAttack;
        private float lastTimeCheckHealth;

        private float bushX;
        private float bushZ;


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
            //boss = GameObject.FindByTag("enemy");

            // namierza wszystkie istniejace krzaki
            foreach (GameObject GO in World.AllGameObjects)
            {
                if (GO.tag == "bush")
                {
                    bushes[bushCount] = GO;
                    bushCount++;
                }
            }
            bushCount--;

            weapon = GameObject.Find("weapon");

            lastTimeCheckHealth = gameObject.GetComponent<EnemiesStats>().health;
            gameObject.GetComponent<EnemiesStats>().maxHealth = 40;
            gameObject.GetComponent<EnemiesStats>().health = 40;
        }
        public override void Update(GameTime gameTime)
        {
            if (isMovable)
            {
                // atakowanie bossa ma byc mozliwe tylko i wylacznie gdy waitVulnerable == true
                if (waitVulnerable == true && wasAttacked == false)
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
                if (waitInBush)
                {
                    gameObject.GetComponent<Transform>().scale = new Vector3(0.1f);
                }
                else
                {
                    gameObject.GetComponent<Transform>().scale = new Vector3(0.3f);
                }



                // 1. ------------------------------------------------
                // chowaj sie
                if (mustHide == true)
                {
                    int tmp = rand.Next(0, bushCount + 1); // rand od 0 do bushCount
                    gameObject.GetComponent<Transform>().position.X = bushes[tmp].GetComponent<Transform>().position.X;
                    gameObject.GetComponent<Transform>().position.Y = bushes[tmp].GetComponent<Transform>().position.Y;
                    gameObject.GetComponent<Transform>().position.Z = bushes[tmp].GetComponent<Transform>().position.Z;

                    bushX = bushes[tmp].GetComponent<Transform>().position.X;
                    bushZ = bushes[tmp].GetComponent<Transform>().position.Z;
                    //Console.WriteLine("Chowam sie!");


                    // jezeli schowal sie za blisko to niech sie chowa jeszcze raz
                    if (distance(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z, hero.GetComponent<Transform>().position.X, hero.GetComponent<Transform>().position.Z) <= attackRange)
                    {
                        //Console.WriteLine("Chowam sie jeszcze raz!");
                    }
                    else
                    {
                        //Console.WriteLine("Schowalem sie!");
                        chargingTime = 0.0f;
                        waitInBush = true;
                        mustHide = false;
                    }
                }
                // 2. ------------------------------------------------
                // czekaj w krzaku
                else if (waitInBush == true)
                {
                    // czy dales sie zlapac?
                    if (gameObject.GetComponent<Transform>().position.X != bushX || gameObject.GetComponent<Transform>().position.Z != bushZ)
                    {
                        //Console.WriteLine("O nie, dalem sie zlapac!");
                        chargingTime = 0.0f;
                        waitVulnerable = true;
                        waitInBush = false;
                    }
                    // czy hero podszedl za blisko?
                    else if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) <= attackRange)
                    {
                        //Console.WriteLine("attack start");
                        chargingTime = 0.0f;
                        isTheSpecialAttackCharging = true;
                        isAttacking = true;
                        waitInBush = false;
                    }
                    // czy minelo juz wystarczajaco duzo czasu zeby i tak zaatakowac?
                    else
                    {
                        chargingTime += Time.DeltaTime;
                        if (chargingTime >= attackChargeDuration)
                        {
                            //Console.WriteLine("attack start");
                            chargingTime = 0.0f;
                            isTheSpecialAttackCharging = true;
                            isAttacking = true;
                            waitInBush = false;
                        }
                    }
                }
                // 3. ------------------------------------------------
                // czekaj podatny na atak
                else if (waitVulnerable == true)
                {
                    chargingTime += Time.DeltaTime;
                    if (chargingTime >= vulnerableChargeDuration)
                    {
                        chargingTime = 0.0f;
                        mustHide = true;
                        wasAttacked = false;
                        waitVulnerable = false;
                    }
                    else if (gameObject.GetComponent<EnemiesStats>().health != lastTimeCheckHealth)
                    {
                        //Console.WriteLine("Zostalem zaatakowany! AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!");
                        lastTimeCheckHealth = gameObject.GetComponent<EnemiesStats>().health;
                        wasAttacked = true;
                    }
                }
                // 4. ------------------------------------------------
                // atakuj
                else if (isAttacking == true)
                {
                    attackCount = 0;
                    // podskakuje przed atakiem
                    if (isTheSpecialAttackCharging)
                    {

                        gameObject.GetComponent<Transform>().position.Y += maxHeight / skillChargeDuration * Time.DeltaTime * 5;
                        chargingTime += Time.DeltaTime * 5;

                        if (chargingTime >= skillChargeDuration)
                        {
                            //Console.WriteLine("attack still");
                            isTheSpecialAttackCharging = false;
                            chargingTime = 0.0f;
                            isDuringSpecialAttack = true;
                            heroPosition = hero.GetComponent<Transform>().position;
                            targetHeroPosition = heroPosition - gameObject.GetComponent<Transform>().position;
                            //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                            targetHeroPosition.Normalize();
                            heroPositon = hero.GetComponent<Transform>().position;
                        }
                    }
                    // atakuje
                    if (isDuringSpecialAttack)
                    {

                        if (Math.Abs((heroPosition - gameObject.GetComponent<Transform>().position).Length()) < 1.0f)
                        {
                            hero.GetComponent<Hero>().dealDamage(100);
                            isDuringSpecialAttack = false;
                            //Console.WriteLine("attack stop");
                            waitAfterAttack = true;
                            isAttacking = false;
                        }
                        heroPosition = hero.GetComponent<Transform>().position;
                        targetHeroPosition = heroPosition - gameObject.GetComponent<Transform>().position;
                        //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                        targetHeroPosition.Normalize();
                        heroPositon = hero.GetComponent<Transform>().position;
                        gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                    }

                }
                // 5. ------------------------------------------------
                // odpoczywa chwile po ataku
                else if (waitAfterAttack == true)
                {
                    //Console.WriteLine("wait after attack {0}", chargingTime);
                    chargingTime += Time.DeltaTime;
                    if (chargingTime >= skillChargeDuration)
                    {
                        chargingTime = 0.0f;
                        mustHide = true;
                        waitAfterAttack = false;
                    }
                }
            }

        }
        public override void Draw(Matrix world, Matrix view, Matrix projection) { }
        public float distance(Vector3 w1, Vector3 w2)
        {
            double dis = 0.0;
            dis = Math.Sqrt((w2.X - w1.X) * (w2.X - w1.X) + (w2.Y - w1.Y) * (w2.Y - w1.Y) + (w2.Z - w1.Z) * (w2.Z - w1.Z));
            return (float)dis;
        }

        public float distance(float ax, float az, float bx, float bz)
        {
            double dis = 0;
            dis = Math.Sqrt((ax - bx) * (ax - bx) + (az - bz) * (az - bz));
            return (float)dis;
        }

    }
}
