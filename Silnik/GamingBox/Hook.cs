using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Silnik.GamingBox
{
    public class Hook:GameObjectComponent
    {
        Transform transform;
        public float speed = 250;
        public Vector3 pointTowards;
        private Vector3 startingPosition;
        private BoundingSphere sphereCollider;
        /// <summary>
        /// 0 = nothing, 1 = level object, 2 = enemy
        /// </summary>
        private int collided = 0;
        private QuadTreeComponent tree;
        private Rectangle rect;
        private Transform player;
        private GameObject Enemy;
        private float distance_old;
        private float timeAlive =0;
        private BasicEffect basicEffect;

        public override void Start()
        {
            transform = gameObject.GetComponent<Transform>();
            tree = GameObject.Find("Hitpoint").GetComponent<QuadTreeComponent>();
            startingPosition = transform.position;
            player = GameObject.Find("hero_parent").GetComponent<Transform>();
            basicEffect = new BasicEffect(InitContent.Graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            
        }
        public override void Update(GameTime gameTime)
        {
            if (collided==0)
            {
                timeAlive += Time.DeltaTime;
                if (timeAlive >= 0.3f)
                    GameObject.Destroy(gameObject);
                transform.position += Transform.PositionToMoveTowards(startingPosition, pointTowards, 0.2f) * speed * Time.DeltaTime;

                //i += Time.DeltaTime;
                //function = (-5*(i*i) + i);
                //Console.WriteLine(function);
                //transform.position.Y += function;
                    //Game1.terrain.getHeight(new Vector2(transform.position.X, transform.position.Z)) + 1;
                
                foreach (GameObject ob in tree.CollideObjects(gameObject))
                {
                    if(collided==0)
                        if (gameObject.GetComponent<Collision>().BoundingSphere.Intersects(ob.GetComponent<Collision>().box))
                        {
                            if (ob.GetComponent<EnemiesStats>() != null)
                            {
                                if(ob.GetComponent<Wieza>() != null && !ob.GetComponent<Wieza>().isInfected)
                                {

                                }
                                else if(ob.GetComponent<Skoczek>() != null && !ob.GetComponent<Skoczek>().isInfected)
                                {

                                }
                                else if(ob.GetComponent<Goniec>() != null && !ob.GetComponent<Goniec>().isInfected)
                                {

                                }
                                else
                                {
                                    Collided(ob);
                                }
                                
                            }
                            else if(ob.tag == "level")
                            {
                                CollidedTree(ob);
                            }
                        }
                }
            }
            else if(collided == 1)
            {
                AfterCollidedTree();
            }
            else if(collided == 2)
            {
                AfterCollided();
            }
        }

        private void CollidedTree(GameObject ob)
        {
            collided = 1;
            Enemy = ob;
            startingPosition = player.position;
            pointTowards = transform.position;
            player.gameObject.GetComponent<Hero>().isMovable = false;
            distance_old = Vector3.Distance(player.position, transform.position);
        }
        private void Collided(GameObject ob)
        {
            collided = 2;
            Enemy = ob;
            startingPosition = ob.GetComponent<Transform>().Position;
            pointTowards = player.position;
            distance_old = Vector3.Distance(ob.GetComponent<Transform>().Position, player.position);
        }
        private void AfterCollidedTree()
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance >= 10.0f)
            {
                player.Position += Transform.PositionToMoveTowards(startingPosition, pointTowards, 0.1f) * speed * Time.DeltaTime;
                if (distance > distance_old)
                {
                    player.gameObject.GetComponent<Hero>().isMovable = true;
                    GameObject.Destroy(gameObject);
                }
                distance_old = distance;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
        private void AfterCollided()
        {
            float distance = Vector3.Distance(Enemy.GetComponent<Transform>().Position, player.position);
            if (distance >= 10.0f)
            {
                Enemy.GetComponent<Transform>().Position += Transform.PositionToMoveTowards(startingPosition, pointTowards, 0.1f) * speed * Time.DeltaTime;
                if (distance > distance_old)
                {
                    GameObject.Destroy(gameObject);
                }
                distance_old = distance;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        public override void Draw(Matrix world, Matrix view, Matrix projection)
        {
            Color color1 = new Color(0.2f, 0.11f, 0.15f);
            Color color2 = new Color(0.11f, 0.01f, 0.05f);
            if (collided == 2)
            {
                
            }
            basicEffect.Projection = projection;
            basicEffect.View = view;
            basicEffect.World = Matrix.Identity;

            basicEffect.CurrentTechnique.Passes[0].Apply();
            
            var vertices = new VertexPositionColor[4];
            if (collided == 2)
            {
                vertices[0].Position = Enemy.GetComponent<Transform>().position;
                vertices[0].Color = color1;
            }
            else
            {
                vertices[0].Position = transform.position;
                vertices[0].Color = color1;
            }
            vertices[1].Position = player.position+new Vector3(0,2,0);
            vertices[1].Color = color2;
            if(collided == 2)
            {
                vertices[2].Position = Enemy.GetComponent<Transform>().position + new Vector3(0.1f, 2.1f, 0.1f);
                vertices[2].Color = color1;
            }
            else
            {
                vertices[2].Position = transform.position + new Vector3(0.1f, 0.1f, 0.1f);
                vertices[2].Color = color1;
            }
            vertices[3].Position = player.position+new Vector3(0.1f,2.1f,0.1f);
            vertices[3].Color = color2;

            InitContent.Graphics.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 2);
        }
    }
}
