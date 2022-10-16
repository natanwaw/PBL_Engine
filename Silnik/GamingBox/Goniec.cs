using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class Goniec : GameObjectComponent
    {
        public float skillChargeDuration = 1.5f;
        //w sekundach
        public float specialAttackDistance = 0.2f;
        public float specialAttackDistance2 = 0.4f;
        public float specialAttackSpeed = 80.0f;
        public float attackRange = 5.0f; //specialAttackDistance * specialAttackSpeed / 2f
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
                gameObject.GetComponent<Transform>().scale = new Vector3(3.3f);
            }
            else
            {
                gameObject.GetComponent<Transform>().scale = new Vector3(0.1f);
            }
            if (isInfected && isMovable)
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
                            hero.GetComponent<Hero>().dealDamage(50);
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
                        isTheSpecialAttackCharging = true;
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
               // gameObject.GetComponent<Collision>().IsPushable(true);
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



/*
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class Goniec : GameObjectComponent
    {
        public float attackRange = 10.0f;
        public float movingSpeed = 25.0f;
        public float timeToLoseHero = 2.0f;
        public float range = 100.0f;
        public float timeBetweenChangesOfDirection = 3.0f;
        public bool isMovable = true;
        public bool isInfected = true;
        GameObject boss;

        private GameObject hero;
        // wektor wskazujacy od pozycji bossa do gracza
        private Vector3 targetHeroPosition;
        private float count;

        public bool foundHero = false;
        private float timerToLoseTheHero = 0.0f;
        private float timerToChangeDirection = 0.0f;
        //jakis tam losowy kierunek poruszania sie bossa
        private Vector3 target = new Vector3(1.0f, 0.0f, 0.0f);
        Random rand = new Random(123);

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
                count++;
                if (hero.GetComponent<BoxCollision>().Collide(gameObject.GetComponent<Collision>().box))
                {
                    if (count > 80)
                    {
                        count = 0;
                        hero.GetComponent<Hero>().dealDamage(50);
                    }
                }
                if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) > 175)
                {
                    timerToLoseTheHero = 0.0f;
                    foundHero = true;
                }
                if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) <= range)
                {
                    foundHero = true;
                    timerToLoseTheHero = 0.0f;
                }

                if (foundHero)
                {
                    timerToLoseTheHero += Time.DeltaTime;
                    if (timerToLoseTheHero > timeToLoseHero)
                    {
                        foundHero = false;
                    }
                }
                if (foundHero)
                {
                    targetHeroPosition = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                    //normalizujemy, bo wazny jest tylko kierunek a nie dlugosc wektora
                    targetHeroPosition.Normalize();
                    if (isMovable)
                    {
                        gameObject.GetComponent<Transform>().position += targetHeroPosition * Time.DeltaTime * movingSpeed;
                    }
                }
                else
                {
                    timerToChangeDirection += Time.DeltaTime;
                    if (timerToChangeDirection > timeBetweenChangesOfDirection)
                    {
                        //zeby troche czesciej boss na nas natrafial to jednak jego kierunek poruszania sie nie moze byc calkowicie losowy.
                        //do wylosowanego kierunku zostanie dodana czesc wektora ktory wskazuje na gracza
                        targetHeroPosition = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                        targetHeroPosition.Normalize();

                        target = new Vector3((float)rand.NextDouble() - 0.5f, 0.0f, (float)rand.NextDouble() - 0.5f);
                        target.Normalize();
                        target += targetHeroPosition * 0.6f;
                        target.Normalize();
                        timerToChangeDirection = 0.0f;
                    }
                    //nie chcemy aby poza mapa sie krecil
                    if (gameObject.GetComponent<Transform>().position.X <= 0.0f || gameObject.GetComponent<Transform>().position.X >= Game1.terrain._terrainWidth ||
                        gameObject.GetComponent<Transform>().position.Z <= -(Game1.terrain._terrainHeight) || gameObject.GetComponent<Transform>().position.Z >= 0.0f)
                    {
                        target = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                        target.Normalize();
                    }
                    if (isMovable)
                    {
                        gameObject.GetComponent<Transform>().position += target * Time.DeltaTime * movingSpeed;
                    }
                }
                gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
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
*/