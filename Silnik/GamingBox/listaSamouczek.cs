using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Silnik.GamingBox
{
    class listaSamouczek
    {

        XDocument xdoc = new XDocument();
        private string filePath = Path.Combine(Environment.CurrentDirectory, "Content\\GameSettings.xml");

        public void loadLista()
        {
            Sprite sprite = new Sprite();
            GameObject ob = new GameObject();
            GameObject hero = new GameObject();
            //Hero hero = new Hero();
            SpriteRenderer spriteRenderer = new SpriteRenderer();
            Mesh mesh = new Mesh();
            MeshRenderer meshRenderer = new MeshRenderer();
            BoxCollision boxCollision = new BoxCollision();
            //TEMP GAMEOBJECT DO PRZYPISYWANIA RODZICOW
            GameObject parent;

            xdoc = XDocument.Load(filePath);
            var element = xdoc.Descendants("VolumeValue").Single();
            float volumeValue = float.Parse(element.Value);


            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(816, 0.1f, -100);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, -45f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(8, 8, 8); //Y nie istotny, nic nie robi
            ob.AddComponent(new Water());
            ob.GetComponent<Water>().Set();
            World.WorldGameObjects.Add(ob);

            Random rand = new Random(524);
            for (int i = 0; i < 30; i++)
            {
                ob = new GameObject();
                ob.tag = "level";
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(870, 990), 0, -rand.Next(45, 305));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, rand.Next(0, 360), 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.009f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("ModeleOdGrafikow/grzewo1");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(new Vector3(4,10,4));
                ob.GetComponent<Collision>().IsStatic = true;
                World.WorldGameObjects.Add(ob);
            }
            for (int i = 0; i < 40; i++)
            {
                ob = new GameObject();
                ob.tag = "level";
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(774, 800), 0, -rand.Next(234, 282));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, rand.Next(0, 360), 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.006f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("ModeleOdGrafikow/grzewo1");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(new Vector3(4, 10, 4));
                ob.GetComponent<Collision>().IsStatic = true;
                World.WorldGameObjects.Add(ob);
            }
            /*for (int i = 0; i < 40; i++)
            {
                ob = new GameObject();
                ob.tag = "level";
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(821, 840), 0, -rand.Next(259, 309));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, rand.Next(0, 360), 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.006f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("ModeleOdGrafikow/grzewo1");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
                ob.GetComponent<Collision>().IsStatic = true;
                World.WorldGameObjects.Add(ob);
            }*/


            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();

            ob = new GameObject();
            ob.name = "black";
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("Black");
            ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, InitContent.Graphics.PreferredBackBufferWidth, InitContent.Graphics.PreferredBackBufferHeight);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            //BOSS
            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            ob.GetComponent<Transform>().position = new Vector3(916, Game1.terrain.getHeight(new Vector2(916, -80)), -80);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.5f);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("blood");
            ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, InitContent.Graphics.PreferredBackBufferWidth, InitContent.Graphics.PreferredBackBufferHeight);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("king2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            //ob.AddComponent(new BoxCollision());
            //ob.GetComponent<BoxCollision>().Set(ob.GetComponent<Transform>());
            //ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Boss());
            ob.tag = "enemy";
            World.WorldGameObjects.Add(ob);

            LoadPlayer(volumeValue);

            ob = new GameObject();
            ob.name = "grob";
            ob.GetComponent<Transform>().position = new Vector3(970, Game1.terrain.getHeight(new Vector2(970, -234)), -234);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, -45f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.01f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("ModeleOdGrafikow/groby3");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<CModel>().SetModelEffect(Game1.effect, true);
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "brama";
            ob.GetComponent<Transform>().position = new Vector3(840, Game1.terrain.getHeight(new Vector2(840, -244)), -244);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 45f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("fence");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            //ob.AddComponent(new Collision());
            //ob.GetComponent<Collision>().Load(new Vector3(20, 10, 20));
            //ob.GetComponent<Collision>().IsStatic = true;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "brama2";
            ob.GetComponent<Transform>().position = new Vector3(1025, 0, -170);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.17f, 0.2f, 0.08f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("ModeleOdGrafikow/brama");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4, 10, 50));
            ob.GetComponent<Collision>().IsStatic = true;

            World.WorldGameObjects.Add(ob);


            //Skoczek
            ob = new GameObject();
            ob.name = "wrog1";
            ob.GetComponent<Transform>().position = new Vector3(910, Game1.terrain.getHeight(new Vector2(910, -230)), -230);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.25f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4, 10, 4), true);
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Skoczek());
            ob.GetComponent<Skoczek>().samouczek = true;
            ob.AddComponent(new Billboard());
            ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("attack");
            ob.GetComponent<AudioComponent>().Load("death");
            World.WorldGameObjects.Add(ob);

            /*ob = new GameObject();
            //ob.name = "wrog1";
            ob.GetComponent<Transform>().position = new Vector3(940, Game1.terrain.getHeight(new Vector2(910, -230)), -230);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Skoczek());
            ob.GetComponent<Skoczek>().samouczek = true;
            ob.AddComponent(new Billboard());
            ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            ob.AddComponent(new AudioComponent());
            ob.GetComponent<AudioComponent>().Load("attack");
            ob.GetComponent<AudioComponent>().Load("death");
            World.WorldGameObjects.Add(ob);*/

            //Wieza
            ob = new GameObject();
            ob.name = "wrog2";
            ob.GetComponent<Transform>().position = new Vector3(900, Game1.terrain.getHeight(new Vector2(900, -230)), -230);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.25f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("tower2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4,10,4), true);
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Wieza());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "loadingScreen";
            ob.AddComponent(new Sprite());
            Random random = new Random();
            int screen = random.Next(0, 4);          
            ob.GetComponent<Sprite>().Load("loading" + screen.ToString());
            //ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, 1600, 800);
            ob.GetComponent<Sprite>().SetPercentage(0,0,100,100);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "sterowanie";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("samouczek_sterowanie");
            //ob.GetComponent<Sprite>().Load("Sterowanie");
            //ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, 1600, 800);
            ob.GetComponent<Sprite>().SetPercentage(0, 0, 100, 100);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);
            
            ob = new GameObject();
            ob.name = "atakowanie";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("samouczek_walka");
            //ob.GetComponent<Sprite>().rect = new Rectangle(0, 0, 1600, 800);
            ob.GetComponent<Sprite>().SetPercentage(0, 0, 100, 100);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);
            
            /// Muzyka
            ob = new GameObject();
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("music");
            //ob.GetComponent<AudioComponent>().Instance.Volume=0.1f;
            ob.AddComponent(new Music(volumeValue));
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "loser_text";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "YOU DIED";
            ob.GetComponent<Text>().Load("text");
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 60);
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "Hitpoint";
            ob.AddComponent(new TerrainRaycast());
            ob.AddComponent(new QuadTreeComponent());
            ob.AddComponent(new GameManager());
            World.WorldGameObjects.Add(ob);



            //funkcje
            loadGui();

        }

        private void loadGui()
        {
            GameObject ob;


            ob = new GameObject();
            ob.name = "health_stat";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/red");
            ob.GetComponent<Sprite>().SetPercentage(1.5f, 72, 3, 25);
            ob.AddComponent(new CharStats());
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui1";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/HP");
            ob.GetComponent<Sprite>().SetPercentage(0, 70, 6, 30);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui2";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/HookIcon");
            ob.GetComponent<Sprite>().SetPercentage(8f, 92f, 3.5f, 5.5f);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui3";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(7, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui4";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(12, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui5";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(17, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "gui6";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(22, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);
        }
        private void LoadPlayer(float volumeValue)
        {
            GameObject ob = new GameObject();
            GameObject parent = new GameObject();
            //parent of hero
            ob = new GameObject();
            ob.name = "hero_parent";
            ob.tag = "hero";
            ob.GetComponent<Transform>().position = new Vector3(796, Game1.terrain.getHeight(new Vector2(796, -300)), -300);
            ob.AddComponent(new Hero());
            ob.AddComponent(new Listener());
            //ob.AddComponent(new WorldsTiles());
            //ob.GetComponent<WorldsTiles>().Set(ob.GetComponent<Transform>());
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("dude/dude");
            ob.AddComponent(new SamouczekScript());
            ob.AddComponent(new Dialog());
            ob.GetComponent<Dialog>().Text = "Bohater: Nie powinienem się tu tak zakradać po zmroku... Jednak matka kazała mi obiecać, że nigdy nie przyjdę na ten cmentarz, " +
                "więc lepiej żeby nigdy nie dowiedziała się ze tu byłem.\n"
                + "Bohater: Wiec to tutaj pochowany jest moj dziadek... Ale dlaczego jego mogiła jest taka zaniedbana? W koncu cała moja rodzina to bohaterowie wojenni, " +
                "a on jest najwiekszym z nich - osobiscie zabil Złego Generała! Powinien miec wielką kryptę tak jak moi wujkowie!\n"
                + "Bohater: O nie, to Zły Generał! Powrocił z martwych aby znowu siać zniszczenie!\n"
                + "Zły Generał: Buahahaha! To ja Zły Generał! Powróciłem z martwych aby znowu siać zniszczenie!\n"
                + "Zły Generał: Chetnie bym z toba pogawedził młody wojowniku, ale mam pilniejsze sprawy na głowie. Sługusy, zajmijcie sie nim!\n"
                + "Bohater: Powinienem jak najszybciej stąd uciekać! Niestety wygląda na to że brama jest magicznie zamknięta... Mam nadzieję że jest tam jakieś inne wyjście.\n"
                + "Potrzebne aby currentPage odpowiednio ustawic";
            ob.GetComponent<Dialog>().Load("dialog");
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "Aby pominac samouczek nacisnij Q\n"
                + "Podejdz do grobu\n"
                + "Pokonaj przeciwnikow\n"
                + "Znajdz wyjscie\n"
                + "Potrzebne aby currentPage odpowiednio ustawic";
            ob.GetComponent<Text>().Load("text");
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4, 4, 4));
            ob.GetComponent<Collision>().IsStatic = false;
            //ob.GetComponent<Collision>().size = new Vector3(5, 5, 5);
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("damage");
            ob.GetComponent<AudioComponent>().Load("dying");
            World.WorldGameObjects.Add(ob);
            // hero
            ob = new GameObject();
            ob.name = "hero";
            parent = GameObject.FindFromListUnsafe("hero_parent");
            ob.parent = parent;
            //ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().position = new Vector3(0, -1.2f, 0);
            ob.GetComponent<Transform>().scale = new Vector3(0.03f);
            ob.AddComponent(new Animation());
            ob.GetComponent<Animation>().Load("animation/hero");
            ob.GetComponent<Animation>().Set("Take 001");
            World.WorldGameObjects.Add(ob);
            // children of hero
            ob = new GameObject();
            ob.name = "weapon_holder";
            parent = GameObject.FindFromListUnsafe("hero_parent");
            ob.parent = parent;
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("cube");
            World.WorldGameObjects.Add(ob);
            //children of weapon holder
            ob = new GameObject();
            ob.name = "weapon";
            parent = GameObject.FindFromListUnsafe("weapon_holder");
            ob.parent = parent;
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("cube");
            //ob.AddComponent(new CModel());
            //ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<Transform>().position = new Vector3(10, 0, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            ob.GetComponent<Transform>().scale = new Vector3(2, 1, 4);
            ob.AddComponent(new PlayerAttack());
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("hit");
            ob.GetComponent<AudioComponent>().Load("miss");
            World.WorldGameObjects.Add(ob);
            //hook
            ob = new GameObject();
            ob.name = "hook";
            parent = GameObject.FindFromListUnsafe("weapon_holder");
            ob.parent = parent;
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("cube");
            //ob.AddComponent(new CModel());
            //ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<Transform>().position = new Vector3(3, 0, 2);
            ob.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //ob.AddComponent(new PlayerAttack());
            World.WorldGameObjects.Add(ob);
        }
    }
}
