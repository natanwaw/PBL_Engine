using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class EnemiesStats : GameObjectComponent
    {
        public float maxHealth = 30;
        public float health;
        private int timer=0;
        Boolean notChecked=true;
        public bool canDropMap = false;
        public static int chanceToDropMapWieza = 80;
        public static int chanceToDropMapSkoczek = 20;
        public static int chanceToDropMapGoniec = 20;

        public override void Start()
        {
            health = maxHealth;
            
        }
        public override void Update(GameTime gameTime)
        {
            checkDeath();
            if (timer > 0)
            {
                timer--;
            }
            if (timer == 0 && notChecked)
            {
                notChecked = false;
                gameObject.GetComponent<CModel>().shouldBeColored = false;
            }
        }
        public void DealDamage(float Damage)
        {
            if (gameObject.GetComponent<Skoczek>() != null)
            {
                if (gameObject.GetComponent<Skoczek>().isInfected)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }
            if (gameObject.GetComponent<Wieza>() != null)
            {
                if (gameObject.GetComponent<Wieza>().isInfected)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }
            if (gameObject.GetComponent<Goniec>() != null)
            {
                if (gameObject.GetComponent<Goniec>().isInfected)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }
            if (gameObject.GetComponent<BossTower>() != null)
            {
                if (gameObject.GetComponent<BossTower>().vulnerable)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }
            if (gameObject.GetComponent<BossSkoczek>() != null)
            {
                if (gameObject.GetComponent<BossSkoczek>().vulnerable)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }
            if (gameObject.GetComponent<BossGoniec>() != null)
            {
                if (gameObject.GetComponent<BossGoniec>().vulnerable)
                {
                    gameObject.GetComponent<CModel>().shouldBeColored = true;
                    health -= Damage;
                    timer = 10;
                    notChecked = true;
                }
            }

        }
        public void checkDeath()
        {
            if (health <= 0)
            {
                if (canDropMap)
                {
                    int chanceToDropMap = 0;
                    if(gameObject.name == "skoczek")
                    {
                        chanceToDropMap = chanceToDropMapSkoczek;
                    }
                    if (gameObject.name == "wieza")
                    {
                        chanceToDropMap = chanceToDropMapWieza;
                    }
                    if (gameObject.name == "goniec")
                    {
                        chanceToDropMap = chanceToDropMapGoniec;
                    }
                    if (InitContent.Random.Next(0, 100) < chanceToDropMap)
                    {
                        GameObject fragmentOfTheMap = Prefab.fragmentOfTheMap();
                        fragmentOfTheMap.GetComponent<Transform>().position = gameObject.GetComponent<Transform>().position;
                        fragmentOfTheMap.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(fragmentOfTheMap.GetComponent<Transform>().position.X, fragmentOfTheMap.GetComponent<Transform>().position.Z));
                        fragmentOfTheMap.name = gameObject.name;
                        GameObject.Instantiate(fragmentOfTheMap);
                    }
                }
                GameObject.Destroy(gameObject);               
            }         
        }
    }
}
