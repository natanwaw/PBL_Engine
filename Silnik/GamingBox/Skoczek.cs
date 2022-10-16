using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class Skoczek : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 80.0f;
        public float movingSpeed = 30.0f;
        public float maxHeight = 20.0f;
        public bool isInfected = false;
        public bool isMovable = true;
        public bool samouczek = false;
        GameObject boss;

        public GameObject hero;
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
        }
        public override void Update(GameTime gameTime)
        {
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

                        gameObject.GetComponent<AudioComponent>().Play3DSound("attack", false);

                        isTheSpecialAttackCharging = true;
                    }
                    else
                    {
                        Vector3 heroPos = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                        heroPos.Normalize();
                        gameObject.GetComponent<Transform>().position += heroPos * Time.DeltaTime * movingSpeed;
                        gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
                        
                    }
                }
                if (isTheSpecialAttackCharging)
                {
                    gameObject.GetComponent<Transform>().position.Y += maxHeight / skillChargeDuration * Time.DeltaTime;
                    chargingTime += Time.DeltaTime;
                    if (chargingTime >= skillChargeDuration)
                    {
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
                if (isDuringSpecialAttack)
                {
                    if (Math.Abs((heroPosition - gameObject.GetComponent<Transform>().position).Length()) < 1.0f || gameObject.GetComponent<Transform>().position.Y < Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z)))
                    {
                        isDuringSpecialAttack = false;
                    }
                    gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * specialAttackSpeed;
                }
                //gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
                if (hero.GetComponent<Collision>().Collide(gameObject.GetComponent<Collision>().box))
                {
                    if (count > 80)
                    {
                        count = 0;
                        hero.GetComponent<Hero>().dealDamage(10);
                    }
                }
                count++;
            }
            else if(!isInfected)
            {
                if(distance(boss.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) < 25)
                {
                    gameObject.GetComponent<Collision>().isActive = true;
                    isInfected = true;
                    gameObject.GetComponent<CModel>().isInfected = true;
                }
            }
            //if(isInfected)
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
