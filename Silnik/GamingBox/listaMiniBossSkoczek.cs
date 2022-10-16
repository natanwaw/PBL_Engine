using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyXmlData;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Silnik.GamingBox
{
    public class listaMiniBossSkoczek
    {
        private Camera camera;
        //USUNAC NATYCHMIAST!!
        private BoundingBox box2;
        //lista gameObjects na swiecie

        MyLevelsCollection levelCollection;

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

            //levelCollection = InitContent.Content.Load<MyLevelsCollection>("UpLevels");

            xdoc = XDocument.Load(filePath);
            var element = xdoc.Descendants("VolumeValue").Single();
            float volumeValue = float.Parse(element.Value);

            ob = new GameObject();
            ob.name = "bramaSkoczek";
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
            ob.GetComponent<Mesh>().Load("dude/dude");
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4, 4, 4));
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new DialogiMinibossSkoczek());
            ob.AddComponent(new Dialog());
            ob.GetComponent<Dialog>().Text = "Bohater: Wujku Teshima?\n"
                ///*
                + "Dowódca skrytobójców: Zaskoczę ich! Zaskoczę ich wszystkich! Nikt nie potrafi skakać tak wysoko jak ja!\n"
                + "Bohater: Proszę powiedz mi że nie jesteś opętany?\n"
                + "Dowódca skrytobójców: Nawet sama Zła Pani Generałowa! Potrafi szarżować i atakować jak moi bracia, ale tylko ja władam mocą końskiego skoku!\n"
                + "Bohater: Zły Generał przejął też twoje ciało. Przepraszam za co zrobię... To dla twojego dobra, naprawdę.\n"
                + "Dowódca skrytobójców: Zły Generał? Muszę zniszczyć jego wrogów! Muszę ich zaskoczyć! Muszę zaskoczyć ich wszystkich!\n"
                //teraz walka
                + "Bohater: Wujku Teshima, słyszysz mnie? Błagam powiedz coś innego niż to że chcesz mnie zaskoczyć!\n"
                + "Dowódca skrytobójców: Czemu? Zaskakiwanie ludzi to moja działka, mój... konik. Ale fakt tym razem nieco przesadziłem. Co się stało? Czemu nie jestem martwy?\n"
                + "Bohater: Zły Generał powrócił i zniewolił ciała swoich wrogów! Bo... byliście wrogami prawda?\n"
                + "Dowódca skrytobójców: Echh... Powiedział Ci prawda? W sumie jestem już na swym łożu śmierci, po śmierci, a i tak wszyscy wiedzą co się wtedy działo,\n"
                + " nie ma sensu żebym to przed Tobą ukrywał. Służyliśmy mu wraz z braćmi. Razem pomogliśmy mu zdobyć władzę.\n"
                + " Byliśmy młodzi i ślepi, chcieliśmy walki nie patrząc na konsekwencje, a on nam ją dał... \n"
                + "W końcu przejrzeliśmy na oczy, zbuntowaliśmy się, ale był zbyt potężny... Nie zdołaliśmy go pokonać.\n"
                + "Bohater: Spokojnie dziadkowi udało się go zabić. A teraz ja powtórzę jego wyczyn!\n"
                + "Dowódca skrytobójców: Dziadkowi? O nie, już rozumiem... Wiem jakiego zaklęcia używa... Nie możesz...\n"
                + " Nie możesz dać mu się złapać... Będzie niepowstrzymany.... jeśli zdobędzie potomka... z krwi...\n"
                + "Bohater: Potomka tego kto go zabił? Czy to moja wina, bo tutaj przyszedłem? Czy to prze ze mnie Zły Generał ożył? Wujku?\n"
                + "Potrzebne aby currentPage odpowiednio ustawic";
            //*/
                /*
                + "Bohater: Daj spokój, wiem że gdzieś tu jesteś, w końcu to twój grobowiec.\n"
                + "Bohater: Ukrywasz się w jednym z tych krzaków?\n"
                + "Bohater: Tak, na pewno jesteś w którymś z krzaków. Nie mam na to czasu, zaraz wyciągnę Cię moim hakiem, powiedz gdzie się schowa...\n"
                //Generał skrytobójców wyskakuje znienacka zabierając graczowi połowę HP.
                + "Bohater: Nani?\n"
                + "Dowódca skrytobójców: No proszę! Jesteś wyjątkowo wytrzymały skoro to przeżyłeś! Ale spokojnie drugi cios zakończy twoje cierpienia!\n"
                //teraz walka
                + "Bohater: Wujku Teshima? Chyba tylko Ty umiesz się tak dobrze skradać nawet jako trup.\n"
                + "Dowódca skrytobójców: Co? Hamasaki? Czemu chciałem Cię zabić? Co się stało? I czemu nie jestem martwy?\n"
                + "Bohater: Zły Generał powrócił i zniewolił ciała swoich wrogów! Bo... byliście wrogami prawda?\n"
                + "Dowódca skrytobójców: Echh... Powiedział Ci prawda? W sumie jestem już na swym łożu śmierci, po śmierci, a i tak wszyscy wiedzą co się wtedy działo,\n"
                + " nie ma sensu żebym to przed Tobą ukrywał. Służyliśmy mu wraz z braćmi. Razem pomogliśmy mu zdobyć władzę.\n"
                + " Byliśmy młodzi i ślepi, chcieliśmy walki nie patrząc na konsekwencje, a on nam ją dał... \n"
                + "W końcu przejrzeliśmy na oczy, zbuntowaliśmy się, ale był zbyt potężny... Nie zdołaliśmy go pokonać.\n"
                + "Bohater: Spokojnie dziadkowi udało się go zabić. A teraz ja powtórzę jego wyczyn!\n"
                + "Dowódca skrytobójców: Dziadkowi? O nie, już rozumiem... Wiem jakiego zaklęcia używa... Nie możesz...\n"
                + " Nie możesz dać mu się złapać... Będzie niepowstrzymany.... jeśli zdobędzie potomka... z krwi...\n"
                + "Bohater: Potomka tego kto go zabił? Czy to moja wina, bo tutaj przyszedłem? Czy to prze ze mnie Zły Generał ożył? Wujku?\n"
                + "Potrzebne aby currentPage odpowiednio ustawic";
                */
            ob.GetComponent<Dialog>().Load("dialog");
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "Wyciągnij go z krzaków!\n"
                + "Potrzebne aby currentPage odpowiednio ustawic";
            ob.GetComponent<Text>().Load("text");
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



            /*
            //BOSS
            ob = new GameObject();
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();
            ob.GetComponent<Transform>().position = new Vector3(50, Game1.terrain.getHeight(new Vector2(50, -50)), -50);
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
            /
            Random rand = new Random(124);
            for (int i = 0; i < 410; i++)
            {
                //Skoczek
                ob = new GameObject();
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(0,1081), 0, -rand.Next(0, 1081));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.3f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("knight2");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                /*foreach(ModelMesh meshTmp in ob.GetComponent<CModel>().Model.Meshes)
                {
                    foreach(ModelMeshPart meshPartTmp in meshTmp.MeshParts)
                    {
                        Effect effect2 = meshPartTmp.Effect;
                        ob.GetComponent<CModel>().setEffectParameter(effect2, "DiffuseColor", new Vector3(255, 0, 0));
                    }
                }
                ob.GetComponent<CModel>().setEffectParameter(ob.GetComponent<CModel>().Model.Meshes[0].MeshParts[0].Effect, "DiffuseColor", new Vector3(255, 0, 0));
                */
            //Effect ef=ob.GetComponent<CModel>().;
            //ef.Parameters["DiffuseColor"].SetValue(new Vector3(255, 0, 0));
            //ob.GetComponent<CModel>().SetModelEffect(ef, true);
            /*
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().isActive=false;
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Skoczek()); 
            ob.AddComponent(new Billboard());
            ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            ob.AddComponent(new AudioComponent());
            ob.GetComponent<AudioComponent>().Load("attack");
            ob.GetComponent<AudioComponent>().Load("death");
            World.WorldGameObjects.Add(ob);
        }

        rand = new Random(1234);
        for (int i = 0; i < 440; i++)
        {
            //Wieza
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(rand.Next(0, 1081), 0, -rand.Next(0, 1081));
            ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("tower2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().isActive = false;
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Wieza());
            World.WorldGameObjects.Add(ob);
        }
        */
            Random rand = new Random(124);
            //Krzaczki
            for (int i = 0; i < 7; i++)
            {
                ob = new GameObject();
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(301, 392), 0, -rand.Next(944, 1026));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.3f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("krzak");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new BoxCollision());
                ob.GetComponent<BoxCollision>().Set();
                ob.GetComponent<BoxCollision>().IsStatic = false;
                ob.AddComponent(new EnemiesStats());
                ob.AddComponent(new Bush());
                ob.AddComponent(new Billboard());
                ob.tag = "bush";
                //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
                World.WorldGameObjects.Add(ob);
            }
                
            /*
            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(180, Game1.terrain.getHeight(new Vector2(180, -950)), -950);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.1f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("tower2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new BoxCollision());
            ob.GetComponent<BoxCollision>().Set();
            ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Bush());
            ob.AddComponent(new Billboard());
            ob.tag = "bush";
            //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            World.WorldGameObjects.Add(ob);

            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(180, Game1.terrain.getHeight(new Vector2(180, -850)), -850);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.1f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("tower2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new BoxCollision());
            ob.GetComponent<BoxCollision>().Set();
            ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Bush());
            ob.AddComponent(new Billboard());
            ob.tag = "bush";
            //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            World.WorldGameObjects.Add(ob);
            */
            //Miniboss Skoczek
            ob = new GameObject();
            ob.name = "miniboss";
            ob.GetComponent<Transform>().position = new Vector3(180, 0, -900);
            ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().isActive = true;
            ob.GetComponent<Collision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new BossSkoczek());
            World.WorldGameObjects.Add(ob);
            /*

            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(200, Game1.terrain.getHeight(new Vector2(200, -200)), -200);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new BoxCollision());
            ob.GetComponent<BoxCollision>().Set();
            ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Bush());
            ob.AddComponent(new Billboard());
            ob.tag = "bush";
            //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            World.WorldGameObjects.Add(ob);

            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(150, Game1.terrain.getHeight(new Vector2(150, -150)), -150);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new BoxCollision());
            ob.GetComponent<BoxCollision>().Set();
            ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Bush());
            ob.AddComponent(new Billboard());
            ob.tag = "bush";
            //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            World.WorldGameObjects.Add(ob);

            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(100, Game1.terrain.getHeight(new Vector2(100, -100)), -100);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.3f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("knight2");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            ob.AddComponent(new BoxCollision());
            ob.GetComponent<BoxCollision>().Set();
            ob.GetComponent<BoxCollision>().IsStatic = false;
            ob.AddComponent(new EnemiesStats());
            ob.AddComponent(new Bush());
            ob.AddComponent(new Billboard());
            ob.tag = "bush";
            //ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
            World.WorldGameObjects.Add(ob);
            */


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
