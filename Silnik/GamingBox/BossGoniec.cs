using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class BossGoniec : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        //w sekundach
        public float specialAttackDistance = 0.2f;
        public float specialAttackDistance2 = 0.4f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 5.0f; //specialAttackDistance * specialAttackSpeed / 2f
        public float movingSpeed = 20.0f;
        public bool isInfected = true;
        public bool isMovable = true;
        GameObject boss;


        private GameObject hero;
        // wektor wskazujacy od pozycji wiezy do gracza
        public Vector3 targetHeroPosition;
        public float chargingTime = 0.0f;
        public float actualSpecialAttackDistance = 0.0f;
        //jezeli atak specjalny sie laduje to wieza nic nie robi
        public bool isTheSpecialAttackCharging = false;
        //jak goniec wykonuje swoj atak to nic innego nie robi
        public bool isDuringSpecialAttack = false;
        //jak goniec wykonuje swoj atak2 to nic innego nie robi
        public bool isDuringSpecialAttack2 = false;
        //jak goniec wykonuje swoj atak2 to nic innego nie robi
        public bool isDuringSpecialAttack3 = false;
        //jak goniec wykonuje swoj atak2 to nic innego nie robi
        public bool isDuringSpecialAttack4 = false;
        //jak goniec wykonuje swoj atak2 to nic innego nie robi
        public bool isDuringSpecialAttack5 = false;
        //jak goniec wykonuje swoj atak2 to nic innego nie robi
        public bool isDuringSpecialAttack6 = false;
        //liczy coldown dla ataku
        private float count;

        public bool vulnerable = true;
        private bool isActive = true;
        private int series = 0;




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
            boss = GameObject.FindByTag("enemy");
            gameObject.GetComponent<EnemiesStats>().maxHealth = 100;
            gameObject.GetComponent<EnemiesStats>().health = 100;
        }
        public override void Update(GameTime gameTime)
        {
            if (isMovable)
            {
                if (isActive)
                {
                    if (!isTheSpecialAttackCharging && !isDuringSpecialAttack && !isDuringSpecialAttack2 && !isDuringSpecialAttack3 && !isDuringSpecialAttack4 && !isDuringSpecialAttack5 && !isDuringSpecialAttack6)
                    {
                        if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) <= attackRange)
                        {
                            //isTheSpecialAttackCharging = true;
                            isDuringSpecialAttack = true;
                        }
                        else
                        {
                            Vector3 heroPos = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                            heroPos.Normalize();
                            gameObject.GetComponent<Transform>().position += heroPos * Time.DeltaTime * movingSpeed;
                        }
                    }
                    if (isTheSpecialAttackCharging)
                    {
                        Vector3 heroPos = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                        heroPos.Normalize();
                        gameObject.GetComponent<Transform>().position -= heroPos * Time.DeltaTime * movingSpeed;

                        chargingTime += Time.DeltaTime;
                        targetHeroPosition = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                        //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                        targetHeroPosition.Normalize();
                        if (chargingTime >= skillChargeDuration)
                        {
                            isTheSpecialAttackCharging = false;
                            chargingTime = 0.0f;
                            //isDuringSpecialAttack = true;
                        }
                    }
                    if (isDuringSpecialAttack)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance)
                        {
                            isDuringSpecialAttack = false;
                            isDuringSpecialAttack2 = true;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.X += 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                    if (isDuringSpecialAttack2)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance2)
                        {
                            isDuringSpecialAttack2 = false;
                            isDuringSpecialAttack3 = true;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.X -= 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(100);
                            }
                        }
                    }
                    if (isDuringSpecialAttack3)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance)
                        {
                            isDuringSpecialAttack3 = false;
                            isDuringSpecialAttack4 = true;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.X += 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                    if (isDuringSpecialAttack4)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance)
                        {
                            isDuringSpecialAttack4 = false;
                            isDuringSpecialAttack5 = true;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.Z -= 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                    if (isDuringSpecialAttack5)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance2)
                        {
                            isDuringSpecialAttack5 = false;
                            isDuringSpecialAttack6 = true;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.Z += 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                    if (isDuringSpecialAttack6)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance)
                        {
                            isDuringSpecialAttack6 = false;
                            if (series >= 3)
                            {
                                series = 0;
                                isTheSpecialAttackCharging = true;
                            }
                            series++;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        //gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        gameObject.GetComponent<Transform>().position.Z -= 1 * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                    gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));

                    count++;
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
    }
}
