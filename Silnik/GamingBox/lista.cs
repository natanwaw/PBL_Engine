using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyXmlData;
using System.Xml.Linq;
using Silnik.GameStates;
using System.IO;
using System.Linq;

namespace Silnik.GamingBox
{
    public class lista
    {
        private Camera camera;
        //USUNAC NATYCHMIAST!!
        private BoundingBox box2;
        //lista gameObjects na swiecie
        public static bool pierwszeWejscie = true;
        public static bool poWiezy = false;
        public static bool poSkoczku = false;
        public static bool poGoncu = false;
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

            xdoc = XDocument.Load(filePath);
            var element = xdoc.Descendants("VolumeValue").Single();
            float volumeValue = float.Parse(element.Value);

            //TEMP GAMEOBJECT DO PRZYPISYWANIA RODZICOW
            GameObject parent;

            levelCollection = InitContent.Content.Load<MyLevelsCollection>("UpLevels");


            ob = new GameObject();
            ob.name = "bramaWieza";
            ob.GetComponent<Transform>().position = new Vector3(171, Game1.terrain.getHeight(new Vector2(171, -887)), -887); 
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.2f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("fence");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "bramaSkoczek";
            ob.GetComponent<Transform>().position = new Vector3(358, Game1.terrain.getHeight(new Vector2(358, -869)), -869);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.2f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("fence");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.name = "bramaGoniec";
            ob.GetComponent<Transform>().position = new Vector3(657, Game1.terrain.getHeight(new Vector2(657, -236)), -236);
            ob.GetComponent<Transform>().rotation = new Quaternion(0f, -45f, 0f, 1f);
            ob.GetComponent<Transform>().scale = new Vector3(0.4f);
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("fence");
            ob.AddComponent(new CModel());
            ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
            World.WorldGameObjects.Add(ob);


            //Wczytywanie modeli z XMLa

            foreach (MyLevel obj in levelCollection.levelsCollection)
            {
                int i = 0;
                if(i>=0)
                {
                    GameObject tmp = new GameObject();
                    tmp.tag = "level";
                    //boxCollision = new BoxCollision();
                    //transform = new Transform();
                    //CModel cModel = new CModel();
                    tmp.GetComponent<Transform>().position = new Vector3(obj.PosX, Game1.terrain.getHeight(new Vector2(obj.PosX, -obj.PosZ)), -obj.PosZ); //chwilowo zeby wieksza wydajnosc byla
                    tmp.GetComponent<Transform>().rotation = new Quaternion(obj.RotY, obj.RotX, obj.RotZ, obj.RotW);
                    //tmp.GetComponent<Transform>().scale = new Vector3(obj.ScalX * 0.00022f, obj.ScalY * 0.00022f, obj.ScalZ * 0.00022f);
                    tmp.GetComponent<Transform>().scale = new Vector3(0.006f, 0.006f, 0.006f);
                    tmp.AddComponent(new Mesh());
                    tmp.GetComponent<Mesh>().Load("ModeleOdGrafikow/" + obj.Tag);
                    tmp.AddComponent(new CModel());
                    tmp.GetComponent<CModel>().Set(tmp.GetComponent<Mesh>());

                    tmp.GetComponent<CModel>().NormalMapEnabled = true;
                    tmp.GetComponent<CModel>().normal = InitContent.Content.Load<Texture2D>("Mapping/" + obj.Tag + "/norm");

                    //Przypisuje modelowi nasz wlasny shader -KR
                    tmp.GetComponent<CModel>().SetModelEffect(Game1.effect, true);
                    tmp.AddComponent(new Collision());
                    tmp.GetComponent<Collision>().Load(new Vector3(4, 10, 4));
                    tmp.GetComponent<Collision>().IsStatic = true;

                    World.WorldGameObjects.Add(tmp);
                }
                i++;
            }
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
            if (pierwszeWejscie)
            {
                ob.GetComponent<Transform>().position = new Vector3(200, Game1.terrain.getHeight(new Vector2(200, -200)), -200);
            }
            if (poWiezy)
            {
                ob.GetComponent<Transform>().position = new Vector3(171, Game1.terrain.getHeight(new Vector2(171, -880)), -880);
            }
            if (poSkoczku)
            {
                ob.GetComponent<Transform>().position = new Vector3(358, Game1.terrain.getHeight(new Vector2(358, -860)), -860);
            }
            if (poGoncu)
            {
                ob.GetComponent<Transform>().position = new Vector3(665, Game1.terrain.getHeight(new Vector2(665, -236)), -236);
            }
            
            ob.AddComponent(new Hero());
            ob.AddComponent(new MapCounter());
            ob.AddComponent(new DialogiGlownaMapa());
            ob.AddComponent(new AudioComponent(volumeValue));
            ob.GetComponent<AudioComponent>().Load("damage");
            ob.GetComponent<AudioComponent>().Load("dying");
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(new Vector3(4, 4, 4));
            ob.GetComponent<Collision>().IsStatic = false;
            //ob.GetComponent<Collision>().IsPushable(true);
            ob.AddComponent(new Listener());
            ob.AddComponent(new WorldsTiles());
            ob.GetComponent<WorldsTiles>().Set(ob.GetComponent<Transform>());
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("dude/dude");
            ob.AddComponent(new Dialog());
            if (pierwszeWejscie)
            {
                ob.GetComponent<Dialog>().Text = "Zły Generał: Och, widzę że nadal tu jesteś. W czasie gdy ty bawiłeś się z moimi minionami, ja odwiedziłem kilku starych znajomych... Powiedzmy że udało mi się namówić dawnych dowódców aby tym razem już na zawsze stanęli po mojej stronie.\n"
                    + "Bohater: Ty potworze! Moich wujków też przemieniłeś w chodzące marionetki? Nieważne! Znajdę sposób by ich uratować! Ale najpierw zabiję ciebie, tak jak wcześniej zabił cię mój dziadek! Matka może i kazała mi tu nie przychodzić, \n"
                    + "ale będzie ze mnie dumna gdy dowie się że poszedłem w jego ślady!\n"
                    + "Zły Generał: Och więc ty musisz być synem Masei. To by wiele wyjaśniało... Więc twa matka powiedziała ze jej ojciec osobiście zabił Wielkiego, Potężnego, Złego Generała? Cóż miała rację, nie mogę temu zaprzeczyć.\n"
                    + "W takim wypadku bardzo chętnie zgładził bym cię osobiście, jednak moje moce nie są jeszcze na tyle potężne - mogę jedynie werbować umarłych do mojej nowej, wspaniałej armii. Jaka szkoda. Ale nieważne - oni też mogą cię załatwić.\n"
                    + "Bohater: Chyba udało mi się uciec, ale nie na długo. Czy Zły Generał mnie znał? Co to znaczy że to by wiele wyjaśniało? Może to dzięki mnie udało mu się powrócić do życia. W końcu Zły Generał znany był ze swojej czarnej magii, \n"
                    + "a ja jestem potomkiem wojownika, który go zabił. To pewnie dlatego matka zabroniła mi tu przychodzić. Co ja narobiłem ? !\n"
                    + "Bohater: Co teraz? Brama zamknięta jest na trzy magiczne zamki, na pewno nie zdołam jej otworzyć. "
                    + "Chyba że... Zły Generał powiedział że przemienił też moich wujków. Ich też było trzech! To musi być w jakiś sposób powiązane. Szkoda że nie wiem gdzie są ich krypty... \n"
                    + "Może powinienem poszukać pozostałych fragmentów mapy?\n"
                    //wieza 9
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Matasuyamę, dowódcę piechoty. Słyszałem że uwielbiał on bronić wysokich twierdz - co za ironia że pochowano go w tak głębokiej krypcie.\n"
                    //skoczek 10
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Teshimę, dowódcę skrytobójców. Dobrze że te ożywieńce nie mogą się skradać, inaczej pewnie już dawno by mnie załatwiły. Oby z wujkiem było podobnie.\n"
                    //goniec 11
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Nakayamę, dowódcę elitarnych żołnierzy. Nie wiem czemu, ale mam wrażenie że nie pokonam go tak łatwo jak pozostałych...\n"
                    + "dodatkowy\n";
            }
            if (poWiezy)
            {
                ob.GetComponent<Dialog>().Text = "Zły Generał: Och jaka szkoda, wygląda na to że Matasuyama opuścił nas po raz kolejny. Ale tym razem dla odmiany to Ty go zabiłeś...\n"
                    + "Bohater: Ty potworze! Wyrwałeś duszę z jego ciała i zmusiłeś do walik w imię swojego największego wroga!\n"
                    + "Szogun: Jakiego wroga? Miał walczyć w moim imieniu, nie kogoś innego! Ach zapomniałem, sądzisz że to ja jestem jego \"największym wrogiem\".\n"
                    + "Bohater: Co... co masz na myśli?\n"
                    /*
                    + "Zły Generał: To proste. Widzisz, zanim twój wujek i jego bracia zaczęli walczyć przeciwko mnie, byli oni...\n"
                    + "Strażnik: Haj wy! W imieniu Straży Miejskiej rozkazuję wam się zatrzymać! Co tutaj robicie? To miejsce jest zamknięte o tej porze!\n"
                    + "Zły Generał: Jak śmiesz przerywać mi mój złowieszczy monolog?!\n"
                    //Zły Generał zaraza straznika
                    + "Zły Generał: O proszę, a więc jednak mogę przemieniać też żywych. Nie miałem o tym pojęcia! To zmienia postać rzeczy. Podejdź tu na chwilę. Spokojnie, to nie zaboli... chyba.\n"
                    */
                    + "Zły Generał: To proste. Widzisz, twoi wujkowie i ja zostaliśmy śmiertelnymi wrogami przyjaźniliśmy się i to nawet bardzo.\n"
                    + "Bohater: Nieprawda! Nikt z mojej rodziny nie miał z Tobą nic wspólnego, to niemożliwe!\n"
                    + "Zły Generał: Nie masz pojęcia co jest, a co nie jest możliwe. Przykładowo: sądzisz że mogę przemieniać w swe sługi tylko martwych wojowników?\n"
                    //Zły Generał NIE zaraza straznika
                    + "Zły Generał: Zaraz sprawdzę na Tobie czy na żywych ludzi też to zadziała. Podejdź tu na chwilę. Spokojnie, to nie zaboli... chyba.\n"

                    //po ucieczce
                    + "Bohater: \"Sądzę\" że jest jego największym wrogiem? Zły Generał chce mi namieszać w głowie, nie mogę na to pozwolić! Muszę zrobić to co powiedział wujek - uwolnić dusze pozostałych.\n"
                    + " I przy okazji pozwoli mi to uciec gdy brama wreszcie się otworzy.\n"
                    //wieza 9
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Matasuyamę, dowódcę piechoty. Słyszałem że uwielbiał on bronić wysokich twierdz - co za ironia że pochowano go w tak głębokiej krypcie.\n"
                    //skoczek 10
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Teshimę, dowódcę skrytobójców. Dobrze że te ożywieńce nie mogą się skradać, inaczej pewnie już dawno by mnie załatwiły. Oby z wujkiem było podobnie.\n"
                    //goniec 11
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Nakayamę, dowódcę elitarnych żołnierzy. Nie wiem czemu, ale mam wrażenie że nie pokonam go tak łatwo jak pozostałych...\n"
                    + "dodatkowy\n";
            }
            if (poSkoczku)
            {
                ob.GetComponent<Dialog>().Text = "Zły Generał: Zabić swojego własnego wujka, masz tupet nie ma co... Wiesz, jak tak o tym pomyślę to wcale nie różnimy się tak bardzo Ty i ja.\n"
                + "Bohater: Nie jestem ani trochę taki jak ty! Ja im pomagam! Uwalniam ich dusze!\n"
                + "Zły Generał: A ja zsyłam ich dusze z powrotem na ten świat i daję im nowe życie! Pozwalam im znowu wojować razem jak za starych dobrych czasów! Czy to nie tego właśnie chcieli?\n"
                + "Bohater: Może wcześniej, ale ostatecznie chcieli cię pokonać!\n"
                + "Zły Generał: Fakt, młodzi zazwyczaj sami nie wiedzą czego chcą. Na szczęście pomogłem im się ostatecznie zdecydować... Tak jak zaraz pomogę zdecydować się Tobie...\n"
                //ucieka
                + "Bohater: Znowu miesza mi w głowie... Ale ja wiem czego chcę. Uwolnić ostatniego z moich wujków i uciec stąd jak najdalej. \n"
                + "Jeżeli Złmu Generałowi nie uda się mnie dorwać to zostanie tu zamknięty już na zawsze!\n"
                    //wieza 7
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Matasuyamę, dowódcę piechoty. Słyszałem że uwielbiał on bronić wysokich twierdz - co za ironia że pochowano go w tak głębokiej krypcie.\n"
                    //skoczek 8
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Teshimę, dowódcę skrytobójców. Dobrze że te ożywieńce nie mogą się skradać, inaczej pewnie już dawno by mnie załatwiły. Oby z wujkiem było podobnie.\n"
                    //goniec 9
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Nakayamę, dowódcę elitarnych żołnierzy. Nie wiem czemu, ale mam wrażenie że nie pokonam go tak łatwo jak pozostałych...\n"
                    + "dodatkowy\n";
            }
            if (poGoncu)
            {
                ob.GetComponent<Dialog>().Text = "Zły Generał: Do trzech razy sztuka! Zaczynam Cię lubić wiesz? Naprawdę szkoda że muszę Cię zabić...\n"
                + "Bohater: Od początku tego chciałeś! Ale dlaczego? Czemu nie ożywiłeś jej tak jak reszty?\n"
                + "Zły Generał: Że co? Słuchaj, jedyne co próbuję od początku zrobić to zabić Ciebie!\n"
                + " Gdybym miał jakiś genialny plan to już dawno bym Ci go opowiedział, widzisz przecież jak uwielbiam złowieszcze monologi! I kogo niby chcę ożywić?\n"
                + "Bohater: Moją babcię! Po co chcesz ją ożywić? Co ona takiego Ci zrobiła? Też była buntowniczką jak wujkowie i dziadek, prawda?\n"
                + "Zły Generał: Czekaj co? Ty naprawdę nie wiesz kim ona była? W zasadzie... ty nic nie wiesz! Zabawne...\n"
                + " Wygląda na to że twoja matka jednak nic Ci nie powiedziała. W sumie sam mógłbym to zrobić tu i teraz, ale nie...\n"
                + " Chcę żebyś miał ten zdezorientowany wyraz twarzy gdy będziesz umierał.\n"
                //ucieczka
                + "Bohater: On... ma rację. Zły Generał ma rację! Moja matka nic mi nie powiedziała, a ja nie wiem nic o swojej rodzinie.\n"
                + " Zaraz... Czy to znaczy że on też nie wiedział że zaklęcie przywróci moją babcię? Może ona będzie świadoma? Tak czy siak muszę się z nią spotkać skoro czeka za bramą.\n"
                    //wieza 10
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Matasuyamę, dowódcę piechoty. Słyszałem że uwielbiał on bronić wysokich twierdz - co za ironia że pochowano go w tak głębokiej krypcie.\n"
                    //skoczek 11
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Teshimę, dowódcę skrytobójców. Dobrze że te ożywieńce nie mogą się skradać, inaczej pewnie już dawno by mnie załatwiły. Oby z wujkiem było podobnie.\n"
                    //goniec 12
                    + "Bohater: Mam już całą mapę. Pora wreszcie odwiedzić wujka Nakayamę, dowódcę elitarnych żołnierzy. Nie wiem czemu, ale mam wrażenie że nie pokonam go tak łatwo jak pozostałych...\n"

                + "dodatkowy\n";
            }
            ob.GetComponent<Dialog>().Load("dialog");
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
            ob.AddComponent(new Collision());
            ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>());
            ob.GetComponent<Collision>().IsStatic = false;
            World.WorldGameObjects.Add(ob);
            //children of weapon holder
            ob = new GameObject();
            ob.name = "weapon";
            parent = GameObject.FindFromListUnsafe("weapon_holder");
            ob.parent = parent;
            ob.AddComponent(new Mesh());
            ob.GetComponent<Mesh>().Load("cube");
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
            
            Random rand = new Random(124);
            for (int i = 0; i < 210; i++)
            {
                //Skoczek
                ob = new GameObject();
                ob.name = "skoczek";
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
                
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(new Vector3(4, 10, 4), true);
                ob.GetComponent<Collision>().isActive=false;
                ob.GetComponent<Collision>().IsStatic = false;
                ob.AddComponent(new EnemiesStats());
                ob.GetComponent<EnemiesStats>().canDropMap = true;
                ob.AddComponent(new Skoczek()); 
                ob.AddComponent(new Billboard());
                ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
                ob.AddComponent(new AudioComponent(volumeValue));
                ob.GetComponent<AudioComponent>().Load("attack");
                ob.GetComponent<AudioComponent>().Load("death");
                World.WorldGameObjects.Add(ob);
            }

            rand = new Random(1234);
            for (int i = 0; i < 240; i++)
            {
                //Wieza
                ob = new GameObject();
                ob.name = "wieza";
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(0, 1081), 0, -rand.Next(0, 1081));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(0.3f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("tower2");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(new Vector3(4, 10, 4), true);
                ob.GetComponent<Collision>().isActive = false;
                ob.GetComponent<Collision>().IsStatic = false;
                ob.AddComponent(new EnemiesStats());
                ob.GetComponent<EnemiesStats>().canDropMap = true;
                ob.AddComponent(new Wieza());
                ob.AddComponent(new Billboard());
                ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
                ob.GetComponent<Billboard>().height= new Vector3(0, 15, 0);
                World.WorldGameObjects.Add(ob);
            }

            for (int i = 0; i < 240; i++)
            {
                //Goniec
                ob = new GameObject();
                ob.name = "goniec";
                ob.GetComponent<Transform>().position = new Vector3(rand.Next(0, 1081), 0, -rand.Next(0, 1081));
                ob.GetComponent<Transform>().position.Y = Game1.terrain.getHeight(new Vector2(ob.GetComponent<Transform>().position.X, ob.GetComponent<Transform>().position.Z));
                ob.GetComponent<Transform>().rotation = new Quaternion(0f, 0f, 0f, 1f);
                ob.GetComponent<Transform>().scale = new Vector3(3.3f);
                ob.AddComponent(new Mesh());
                ob.GetComponent<Mesh>().Load("cube");
                ob.AddComponent(new CModel());
                ob.GetComponent<CModel>().Set(ob.GetComponent<Mesh>());
                ob.AddComponent(new Collision());
                ob.GetComponent<Collision>().Load(ob.GetComponent<Mesh>(), true);
                ob.GetComponent<Collision>().isActive = false;
                ob.GetComponent<Collision>().IsStatic = false;
                ob.AddComponent(new EnemiesStats());
                ob.GetComponent<EnemiesStats>().canDropMap = true;
                ob.AddComponent(new Goniec());
                ob.AddComponent(new Billboard());
                ob.GetComponent<Billboard>().Load("enemy_health_bar_foreground_004");
                ob.GetComponent<Billboard>().height = new Vector3(0, 15, 0);
                World.WorldGameObjects.Add(ob);
            }

            //Krzaczek
            ob = new GameObject();
            ob.GetComponent<Transform>().position = new Vector3(250, Game1.terrain.getHeight(new Vector2(250, -250)), -250);
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
            ob.AddComponent(new GameManager());
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
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 60);
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            


            //Info o aktalnym zadaniu
            ob = new GameObject();
            ob.name = "info1";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "Pokonuj przeciwników aby zdobyć fragmenty mapy";
            ob.GetComponent<Text>().Load("text");
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 100);
            World.WorldGameObjects.Add(ob);

            //ile zebrano fragmentow mapy skoczka
            ob = new GameObject();
            ob.name = "info2";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "0/5 fragmentów mapy skoczka";
            ob.GetComponent<Text>().Load("text");
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 160);
            World.WorldGameObjects.Add(ob);

            //ile zebrano fragmentow mapy wiezy
            ob = new GameObject();
            ob.name = "info3";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "0/5 fragmentów mapy wieży";
            ob.GetComponent<Text>().Load("text");
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 220);
            World.WorldGameObjects.Add(ob);

            //ile zebrano fragmentow mapy gonca
            ob = new GameObject();
            ob.name = "info4";
            ob.AddComponent(new Text());
            ob.GetComponent<Text>().text = "0/5 fragmentów mapy gońca";
            ob.GetComponent<Text>().Load("text");
            ob.GetComponent<Text>()._textBox.Position = new Vector2(30, 280);
            World.WorldGameObjects.Add(ob);

            loadGui();
            loadMap();
        }

        private void loadMap()
        {
            GameObject ob;
            Sprite sprite;
            SpriteRenderer spriteRenderer;
            sprite = new Sprite();
            spriteRenderer = new SpriteRenderer();

            

            ob = new GameObject();
            ob.AddComponent(new Sprite());
            ob.tag = "map";
            ob.AddComponent(new SpriteRenderer());
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

            ob = new GameObject();
            ob.tag = "map_rook";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("rook");
            //ob.GetComponent<Sprite>().SetPercentage(100, 10, 3, 3);
            ob.GetComponent<Sprite>().rect = new Rectangle(500, 385, 30, 30);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.tag = "map_knight";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("knightmap");
            ob.GetComponent<Sprite>().rect = new Rectangle(1100, 385, 30, 30);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

            ob = new GameObject();
            ob.tag = "map_bishop";
            ob.AddComponent(new Sprite());
            ob.GetComponent<Sprite>().Load("bishop");
            // ob.GetComponent<Sprite>().SetPercentage(9.5f, 3, 50, 3);
            ob.AddComponent(new SpriteRenderer());
            ob.GetComponent<SpriteRenderer>().Set(ob.GetComponent<Sprite>());
            ob.isActive = false;
            World.WorldGameObjects.Add(ob);

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
