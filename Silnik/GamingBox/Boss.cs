using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    class Boss : GameObjectComponent
    {
        public float attackRange = 10.0f;
        public float movingSpeed = 15.0f;
        public float timeToLoseHero = 2.0f;
        public float range = 100.0f;
        public float timeBetweenChangesOfDirection = 4.0f;
        public bool isMovable = true;
        public bool samouczek = false;
        public bool afterFirstMiniBoss = false;


        private GameObject hero;
        // wektor wskazujacy od pozycji bossa do gracza
        private Vector3 targetHeroPosition;

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
            //gameObject.GetComponent<SpriteRenderer>().ChangeColor(Color.Red * (1.0f - (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) / range)));
            gameObject.GetComponent<SpriteRenderer>().ChangeColor(Color.Red * 0);
        }
        public override void Update(GameTime gameTime)
        {
            if(distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) > 175)
            {
                timerToLoseTheHero = 0.0f;
                foundHero = true;
            }
            if (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) <= range)
            {
                if (afterFirstMiniBoss)
                {
                    gameObject.GetComponent<SpriteRenderer>().ChangeColor(Color.Red * (1.0f - (distance(hero.GetComponent<Transform>().position, gameObject.GetComponent<Transform>().position) / range)));
                    foundHero = true;
                    timerToLoseTheHero = 0.0f;
                }  
            }

                if (foundHero)
                {
                    timerToLoseTheHero += Time.DeltaTime;
                    if (timerToLoseTheHero > timeToLoseHero)
                    {
                        foundHero = false;
                    }
                }
            if (foundHero && hero.GetComponent<Hero>().getHide() == false)
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
                if(timerToChangeDirection > timeBetweenChangesOfDirection)
                {
                    //zeby troche czesciej boss na nas natrafial to jednak jego kierunek poruszania sie nie moze byc calkowicie losowy.
                    //do wylosowanego kierunku zostanie dodana czesc wektora ktory wskazuje na gracza
                    targetHeroPosition = hero.GetComponent<Transform>().position - gameObject.GetComponent<Transform>().position;
                    targetHeroPosition.Normalize();

                    target = new Vector3((float)rand.NextDouble()-0.5f, 0.0f, (float)rand.NextDouble() - 0.5f);
                    target.Normalize();
                    target += targetHeroPosition * 0.5f;
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
            if (samouczek)
            {
                gameObject.GetComponent<Transform>().position.Y = 1000;
            }
            else
            {
                gameObject.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(gameObject.GetComponent<Transform>().position.X, gameObject.GetComponent<Transform>().position.Z));
            }
        }
        public override void Draw(Matrix world, Matrix view, Matrix projection) {}
        public float distance(Vector3 w1, Vector3 w2)
        {
            double dis = 0.0;
            dis = Math.Sqrt((w2.X - w1.X) * (w2.X - w1.X) + (w2.Y - w1.Y) * (w2.Y - w1.Y) + (w2.Z - w1.Z) * (w2.Z - w1.Z));
            return (float)dis;
        }
    }
}
