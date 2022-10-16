using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class PlayerAttack : GameObjectComponent
    {
        private BoundingBox box;
        private BoundingSphere sphere;
        private Transform transform;
        private Vector3 size;
        private float sizeF;
        private List<GameObject> enemies = new List<GameObject>();
        private GameObject HitPoint;
        private GameObject Hook;
        private GameObject Player;
        private int timer = 0;
        public float damage;
        private float timerHook;
        private float timerAttack;
        private int numberAttack;
        private float timerUpdateAttack;

        private float coldownAttack = 0.1f;

        private Vector3 startingPosition;
        private Vector3 targerPosition;
        private float speed = 100;
        //Init - wykonywane zaraz po dodaniu tego komponentu, ale przed dodaniem całego gameObjectu do świata gry
        public override void Init()
        {
            timerHook = 0;
        }
        //Start - wykonywane 1 raz tuż przed pierwszym Update
        public override void Start()
        {
            HitPoint = GameObject.Find("Hitpoint");
            Hook = GameObject.Find("hook");
            Player = GameObject.Find("hero_parent");
            transform = gameObject.GetComponent<Transform>();
            size = new Vector3(8f, 90f, 8f);
            sizeF = 8f;
            //damage
            damage = 10;
            //
            BoxUpdate();
            foreach(GameObject go in World.CollisionObjects)
            {
                if (go.GetComponent<EnemiesStats>() != null)
                    enemies.Add(go);
            }
        }
        //Update - wiadomo co i jak
        public override void Update(GameTime gameTime)
        {
            if(HitPoint.GetComponent<TerrainRaycast>().HitPoint.HasValue)
            {
                BoxUpdate();
                PerformAttack();
                UpdateAfterAttack();
                Transform.LookAtTarget(gameObject.parent.parent.GetComponent<Transform>(), gameObject.parent.GetComponent<Transform>(), HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value, 0f);
                hook();
            }
            
            if (timer > 0) timer--;
            
        }
        private void BoxUpdate()
        {
            //box = new BoundingBox(new Vector3(transform.AbsoluteTransform.Translation.X - size.X / 2, transform.AbsoluteTransform.Translation.Y - size.Y, transform.AbsoluteTransform.Translation.Z - size.Z / 2),
                //new Vector3(transform.AbsoluteTransform.Translation.X + size.X / 2, transform.AbsoluteTransform.Translation.Y + size.Y, transform.AbsoluteTransform.Translation.Z + size.Z / 2));
            sphere = new BoundingSphere(transform.AbsoluteTransform.Translation, sizeF);
            //RotateBox();
        }
        private void CheckCollision()
        {
            foreach(GameObject go in enemies)
            {
                if (box.Intersects(go.GetComponent<Collision>().box))
                {
                    go.GetComponent<EnemiesStats>().DealDamage(damage);
                    
                    gameObject.GetComponent<AudioComponent>().Play3DSound("hit",false);

                }
                else
                {
                    if(timer==0)
                    gameObject.GetComponent<AudioComponent>().Play3DSound("miss", false);
                    timer = 5;
                }
            }
        }
        private void hook()
        {
            timerHook += Time.DeltaTime;
            if(Input.IsKeyDown(1))
            {
                if(timerHook >= 1.0f)
                {
                    timerHook = 0;
                    float hero_height = 2;
                    GameObject hook = Prefab.GrappleHook();
                    hook.GetComponent<Hook>().pointTowards = HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value;
                    hook.GetComponent<Hook>().pointTowards.Y += hero_height;
                    hook.GetComponent<Transform>().position = gameObject.parent.parent.GetComponent<Transform>().Position;
                    GameObject.Instantiate(hook);
                }
                

            }
        }
        private void PerformAttack()
        {
            timerAttack += Time.DeltaTime;
            if (Input.IsKeyDown(0))
            { 
                if (timerAttack > coldownAttack)
                {
                    timerAttack = 0;
                    attack(0);

                }
            }
            
            
        }
        private void attack(int numberAttack)
        {
            if(numberAttack == 1)
            {
                Player.GetComponent<Hero>().isMovable = false;
                numberAttack = 1;
                timerUpdateAttack = 0.5f;
            }
            foreach (GameObject go in enemies)
            {
                if (sphere.Intersects(go.GetComponent<Collision>().box))
                {
                    go.GetComponent<EnemiesStats>().DealDamage(damage);

                    gameObject.GetComponent<AudioComponent>().Play3DSound("hit", false);

                }
                else
                {
                    if (timer == 0)
                        gameObject.GetComponent<AudioComponent>().Play3DSound("miss", false);
                    timer = 5;
                }
            }
        }
        private void UpdateAfterAttack()
        {
            timerUpdateAttack -= Time.DeltaTime;
            if(timerUpdateAttack > 0)
            {
                if(numberAttack == 1)
                {
                    Player.GetComponent<Transform>().Position += Transform.PositionToMoveTowards(startingPosition, targerPosition, 0.2f) * speed * Time.DeltaTime;
                }
                
            }
            else
            {
                numberAttack = 0;
            }
            
        }

        public Vector3 GetHitPoint()
        {
            if (HitPoint.GetComponent<TerrainRaycast>().HitPoint.HasValue)
            {
                Transform.LookAtTarget(gameObject.parent.parent.GetComponent<Transform>(), gameObject.parent.GetComponent<Transform>(), HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value, 0f);

            }
            return HitPoint.GetComponent<TerrainRaycast>().HitPoint.Value;
        }

        private void RotateBox()
        {
            Vector3[] corners = box.GetCorners();
            for(int i=0;i<8;i++)
            {
                Matrix worldTransform;
                worldTransform = transform.AbsoluteTransform;
                //transformed = transformed.Transform(worldTransform);
                //corners[i].
                worldTransform *= Matrix.CreateTranslation(corners[i]);
                corners[i] = worldTransform.Translation;
            }
        }
    }
}
