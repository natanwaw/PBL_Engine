using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class Wieza : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        private float timeIdle = 0;
        //w sekundach
        public float specialAttackDistance = 1.5f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 60.0f; //specialAttackDistance * specialAttackSpeed / 2f
        public float movingSpeed = 30.0f;
        public bool isInfected = false;
        public bool isMovable = true;
        GameObject boss;


        private GameObject hero;
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


        public override void Init() {

        }
        public override void Start() {
            foreach(GameObject GO in World.AllGameObjects)
            {
                if (GO.tag=="hero")
                {
                    hero = GO;
                }
            }
            boss = GameObject.FindByTag("enemy");
        }
        public override void Update(GameTime gameTime) {
            if (isInfected && isMovable)
            {
                gameObject.GetComponent<Transform>().scale = new Vector3(0.25f);
            }
            else
            {
                gameObject.GetComponent<Transform>().scale = new Vector3(0.01f);
            }
            if (isInfected && isMovable)
            {
                if (!isTheSpecialAttackCharging && !isDuringSpecialAttack)
                {
                    if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) <= attackRange)
                    {
                        isTheSpecialAttackCharging = true;
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
                    chargingTime += Time.DeltaTime;
                    targetHeroPosition = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                    //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                    targetHeroPosition.Normalize();
                    if (chargingTime >= skillChargeDuration)
                    {
                        isTheSpecialAttackCharging = false;
                        chargingTime = 0.0f;
                        isDuringSpecialAttack = true;
                    }
                }
                if(timeIdle<=10f)
                {
                    timeIdle += Time.DeltaTime;
                    if (isDuringSpecialAttack)
                    {
                        actualSpecialAttackDistance += Time.DeltaTime;
                        if (actualSpecialAttackDistance >= specialAttackDistance)
                        {
                            isDuringSpecialAttack = false;
                            actualSpecialAttackDistance = 0.0f;
                        }
                        gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                        if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                        {
                            if (count > 80)
                            {
                                count = 0;
                                hero.GetComponent<Hero>().dealDamage(50);
                            }
                        }
                    }
                }
                else
                {
                    timeIdle = 0;
                }
                
                
                gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
                
                count++;
            }
            else if (!isInfected)
            {
                if (distance(boss.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 25)
                {
                    gameObject.GetComponent<Collision>().isActive = true;
                    isInfected = true;
                    gameObject.GetComponent<CModel>().isInfected = true;
                }
            }
            //if (isInfected)
                //gameObject.GetComponent<Collision>().IsPushable(true);
            //else
                gameObject.GetComponent<Collision>().IsPushable(false);
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
