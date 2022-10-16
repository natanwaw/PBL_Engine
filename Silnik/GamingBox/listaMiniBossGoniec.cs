using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyXmlData;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace Silnik.GamingBox
{
    public class listaMiniBossGoniec
    {
        private Camera camera;
        //USUNAC NATYCHMIAST!!
        private BoundingBox box2;
        //lista gameObjects na swiecie

        XDocument xdoc = new XDocument();
        private string filePath = Path.Combine(Environment.CurrentDirectory, "Content\\GameSettings.xml");

        MyLevelsCollection levelCollection;

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

            xdoc = XDocument.Load(filePath);
            var element = xdoc.Descendants("VolumeValue").Single();
            float volumeValue = float.Parse(element.Value);

            //TEMP GAMEOBJECT DO PRZYPISYWANIA RODZICOW
            GameObject parent;

            //levelCollection = InitContent.Content.Load<MyLevelsCollection>("UpLevels");


            ob = new GameObject();
            ob.name = "bramaGoniec";
            ob.GetComponent<Transform>().position = new Vector3(337, Game1.terrain.getHeight(new Vector2(337, -1050)), -1050);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.2f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("fence");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            World.WorldGameObjects.Add(ob);


            //Wczytywanie modeli z XMLa
            /*
            foreach (MyLevel obj in levelCollection.levelsCollection)
            {

                GameObject tmp = new GameObject();
                tmp.tag = "level";
                //boxCollision = new BoxCollision();
                //transform = new Transform();
                //CModel cModel = new CModel();
                tmp.GetComponent<Transform>().position = new Vector3(obj.PosX, Game1.terrain.getHeight(new Vector2(obj.PosX, -obj.PosZ)), -obj.PosZ); //chwilowo zeby wieksza wydajnosc byla
                tmp.GetComponent<Transform>().rotation = new Quaternion(obj.RotY, obj.RotX, obj.RotZ, obj.RotW);
                //tmp.GetComponent<Transform>().scale = new Vector3(obj.ScalX * 0.00022f, obj.ScalY * 0.00022f, obj.ScalZ * 0.00022f);
                tmp.GetComponent<Transform>().scale = new Vector3(0.004f, 0.004f, 0.004f);
                tmp.AddComponent(new Mesh());
                tmp.GetComponent<Mesh>().Load("ModeleOdGrafikow/" + obj.Tag);
                tmp.AddComponent(new CModel());
                tmp.GetComponent<CModel>().Set(tmp.GetComponent<Mesh>());

                tmp.GetComponent<CModel>().NormalMapEnabled = true;
                tmp.GetComponent<CModel>().normal = InitContent.Content.Load<Texture2D>("Mapping/" + obj.Tag + "/norm");

                /*
                if (obj.Tag.Equals("true"))
                {
                    tmp.GetComponent<CModel>().NormalMapEnabled = true;
                    tmp.GetComponent<CModel>().normal = InitContent.Content.Load<Texture2D>("Mapping/" + obj.Tag + "norm");
                }
                */

            /*
            if (obj.Tag.Equals("grzewo1"))
            {
                tmp.GetComponent<CModel>().NormalMapEnabled = true;
                tmp.GetComponent<CModel>().normal = InitContent.Content.Load<Texture2D>("Mapping/" + obj.Tag + "norm");
            }
            */
            /*

            //Przypisuje modelowi nasz wlasny shader -KR
            tmp.GetComponent<CModel>().SetModelEffect(Game1.effect, true);
            tmp.AddComponent(new Collision());
            tmp.GetComponent<Collision>().Load(tmp.GetComponent<Mesh>());
            tmp.GetComponent<Collision>().IsStatic = true;

            World.WorldGameObjects.Add(tmp);
        }
        */
            //Woda
            //Renderuje się tylko 1 woda na raz, to lepiej nie kłaść ich obok siebie
            //zmieniając skalę zmnieniają się też parametry wody - najlepiej nie schodzić poniżej 3.
            //i nie ustawiać zbyt dużej, bo tworzy się ogramna sfera otaczająca przez co są problemy ze znikaniem wody (10 to dużo za dużo)
            //pozycja wskazuje na środek wody
            //na razie takie błędy znalazłem, ale pewnie będzie ich więcej
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(150, 0.6f, -350);
            ob.GetComponent<Transform>().scale = new Vector3(5, 5, 5); //Y nie istotny, nic nie robi
            ob.AddComponent(new Water());
            ob.GetComponent<Water>().Set();
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(550, 0.6f, -900);
            ob.GetComponent<Transform>().scale = new Vector3(5, 5, 5); //Y nie istotny, nic nie robi
            ob.AddComponent(new Water());
            ob.GetComponent<Water>().Set();
            World.WorldGameObjects.Add(ob);

            Animation riggedAnimation = new Animation();
            // Billboard billboard = new Billboard();
            ob = new GameObject();
            ob.name = "dude1";
            ob.GetComponent<Transform>().position = new Vector3(20, Game1.terrain.getHeight(new Vector2(20, -20)), -20);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 90f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.01f);
            ob.AddComponent(riggedAnimation);
            ob.GetComponent<Animation>().Load("dude/dude");
            ob.GetComponent<Animation>().Set("Take 001");

            ob.AddComponent(new TestScript());
            World.WorldGameObjects.Add(ob);


            //parent of hero
            ob = new GameObject();
            ob.name = "hero_parent";
            ob.tag = "hero";
            ob.GetComponent<Transform>().position = new Vector3(340, Game1.terrain.getHeight(new Vector2(340, -980)), -980);
            ob.AddComponent(new Hero());
            ob.AddComponent(new Listener());
            ob.AddComponent(new WorldsTiles());
            ob.GetComponent<WorldsTiles>().Set(ob.GetComponent<Transform>());
            ob.AddComponent(new Mesh());
            ob.AddComponent(new DialogiMinibossGoniec());
            ob.GetComponent<Mesh>().Load("dude/dude");
            ob.AddComponent(new Dialog());
            ob.GetComponent<Dialog>().Text = "Bohater: Wujku Nakayama?\n"
                //Generał elitarnych żołnierzy NIE wyskakuje znienacka i NIE zabiera graczowi połowy HP.
                + "Dowódca elitarnych żołnierzy: Inni dali Ci się zajść znienacka, ale ze mną nie pójdzie Ci tak łatwo! Nie ma czasu na rozmowy. Stawwaj do walki!\n"
                //walka
                + "Bohater: Uff, mało brakowało, myślałem że to już mój koniec.\n"
                + "Dowódca elitarnych żołnierzy: Co? Hamasaki? Czemu chciałem Cię zabić? Co mnie opętało?\n"
                + "Bohater: Zły Generał. Powrócił i zniewolił ciała swoich wrogów! Ale spokojnie uwolniłem już dusze twoich braci.\n"
                + " Wujek Matasuyama powiedział że jeśli gdy już to zrobię, brama się otworzy, a wujek Teshima że muszę nie dać mu się złapać żeby nie był niepowstrzymany, dlatego zaraz stąd uciekam.\n"
                + "Dowódca elitarnych żołnierzy: Matasuyama? Teshima? Ci idioci ledwo co znają się na czarnej magii! Fakt, brama się otworzy i fakt jeżeli Zły Generał Cię złapie będzie niepowstrzymany,\n"
                + " ale tak naprawdę od początku chciał właśnie tego! Chciał abyś nas zabił, żebyśmy znowu zginęli. Chciał żeby historia się powtórzyła! To jedyny sposób żeby ona powróciła!\n"
                + "Bohater: Kto? Kto powróci?\n"
                + "Dowódca elitarnych żołnierzy: Nasza matka... Znaczy twoja babcia... Na pewno czeka za bramą... Znajdź ją zanim on to zrobi i... i...\n"
                + "Bohater: Babcia? Czemu Zły Generał miałby ożywiać babcię? Wujku?\n"
                //miniboss umiera
                + "Potrzebne aby currentPage odpowiednio ustawic";
            ob.GetComponent<Dialog>().Load("dialog");
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("damage");
            ob.GetComponent<AudioComponent>().Load("dying");
            World.WorldGameObjects.Add(ob);
            // hero
            ob = new GameObject();
            ob.name = "hero";
            parent = GameObject.FindFromListUnsafe("hero_parent");
            ob.parent = parent;
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
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
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
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.GetComponent<Transform>().position = new Vector3(3, 0, 2);
            ob.GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 1);
            //ob.AddComponent(new PlayerAttack());
            World.WorldGameObjects.Add(ob);


            //Goniec
            ob = new GameObject();
            ob.name = "miniboss";
            ob.GetComponent<Transform>().position = new Vector3(180, 0, -900);
            ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("king2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().isActive = true;
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new BossGoniec());
            World.WorldGameObjects.Add(ob);
 

            /// Muzyka
            ob = new GameObject();
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("music");
            //ob.GetComponent<AudioComponent>().Instance.Volume=0.1f;
            ob.AddComponent(new Music(volumeValue));
            World.WorldGameObjects.Add(ob);


            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(90f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("skoczek");
            ob.GetComponent<Sprite>().rect = new Rectangle(1300, 20, 75, 150);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(90f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("goniec");
            ob.GetComponent<Sprite>().rect = new Rectangle(1375, 20, 75, 150);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            ob.GetComponent<Transform>().position = new Vector3(1, 1, 0);
            ob.GetComponent<Transform>().rotation = new Quaternion(90f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(1);
            ob.AddComponent(sprite);
            ob.GetComponent<Sprite>().Load("wieza");
            ob.GetComponent<Sprite>().rect = new Rectangle(1450, 20, 75, 150);
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "Hitpoint";
            ob.AddComponent(new TerrainRaycast());
            ob.AddComponent(new QuadTreeComponent());
            World.WorldGameObjects.Add(ob);


            /*ob = new GameObject();
            Dialog dialog = new Dialog();
            ob.AddComponent(dialog);
            ob.GetComponent<Dialog>().Load("dialog");
            World.WorldGameObjects.Add(ob);*/


            ob = new GameObject();
            ob.name = "loser_text";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "YOU DIED";
            ob.GetComponent<Text>().Load("text");
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();

            ob.AddComponent(sprite);
            ob.tag = "map";
            ob.AddComponent(spriteRenderer);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.GetComponent<Sprite>().Load("mapa");
            ob.GetComponent<Sprite>().rect = new Rectangle(800, 590, 500, 500);
            //ob.GetComponent<SpriteRenderer>().Rotate(225);

            //ob.GetComponent<Sprite>().rect = new Rectangle(800, 565, 500, 500);

            ob.GetComponent<SpriteRenderer>().ChangeOrigin(new Vector2(250, 250));
            ob.GetComponent<SpriteRenderer>().Rotate(225);
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);


            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            Map map = new Map();
            ob.AddComponent(map);
            ob.AddComponent(sprite);
            ob.tag = "hero_on_map";
            ob.AddComponent(spriteRenderer);
            ob.isActive = false;
            ob.GetComponent<Sprite>().Load("heroSprite");
            ob.GetComponent<Sprite>().rect = new Rectangle((int)ob.GetComponent<Map>().X * 100, (int)ob.GetComponent<Map>().Y * 100, 30, 30);
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            //ob.GetComponent<SpriteRenderer>().ChangeSize()
            World.WorldGameObjects.Add(ob);

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
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/HP");
            ob.GetComponent<Sprite>().SetPercentage(0, 70, 6, 30);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/HookIcon");
            ob.GetComponent<Sprite>().SetPercentage(8f, 92f, 3.5f, 5.5f);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(7, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(12, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(17, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("GUI/slot");
            ob.GetComponent<Sprite>().SetPercentage(22, 91, 5, 8);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            World.WorldGameObjects.Add(ob);
        }
    }
}
