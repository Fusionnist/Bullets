using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using MonoGame.FZT.UI;
using MonoGame.FZT.XML;
using MonoGame.FZT.Sound;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using static SuperBulletManiaReloadedTheSequel.Enums;
using System.Xml;
using System.Linq;

namespace SuperBulletManiaReloadedTheSequel
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputProfile inputProfile;
        CursorManager mouseMan;
        GamePhase phase, nextPhase;
        UISystem currentUI;
        UISystem[] UIs;
        TDEntityBuilder ebuilder;
        Point virtualDims, windowDims, TDdims, TAdims;
        SceneCollection scenes;
        TextHandler handler;
        Rectangle TDFrame, TAFrame;
        Event[] currentQueue;
        Map gameMap;
        InputProfile ipp;
        List<Entity> availableTurrets;
        TextureDrawer status, cursor, transitiontex;
        Timer waveTimer, transitionTimer;
        int waveNumber, money, health, waveAmt, currentEventNo, turretIndex, income;
        bool lost, transition, countdown;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ebuilder = new TDEntityBuilder();
            windowDims = new Point(1200, 821);
            graphics.PreferredBackBufferHeight = windowDims.Y;
            graphics.PreferredBackBufferWidth = windowDims.X;
        }

        protected override void Initialize()
        {
            transitionTimer = new Timer(2f);
            ipp = new InputProfile(new KeyManager[]
            {
                new KeyManager(Keys.Right, "right"),
                new KeyManager(Keys.Left, "left"),
                new KeyManager(Keys.R, "restart"),
            });

            inputProfile = new InputProfile(new KeyManager[] { new KeyManager(Keys.Left, "playerLeft"), new KeyManager(Keys.Right, "playerRight"), new KeyManager(Keys.Up, "playerUp"), new KeyManager(Keys.Down, "playerDown") });
            phase = GamePhase.Menu;
            mouseMan = new CursorManager();
            IsMouseVisible = false;
            EntityCollection.CreateGroup("turret", "turrets");
            EntityCollection.CreateGroup("rock", "rocks");
            EntityCollection.CreateGroup("enemy", "enemies");
            EntityCollection.CreateGroup("bgElement", "bgElements");
            EntityCollection.CreateGroup(new Property("isEnt", "isEnt", "isEnt"), "entities");
            virtualDims = new Point(400, 272);

            TDdims = new Point(240, 240);
            TAdims = new Point(160, 272);

            TDFrame = new Rectangle(0, 0, TDdims.X, TDdims.Y);
            TAFrame = new Rectangle(240, 0, TAdims.X, TAdims.Y);            

            scenes = new SceneCollection();
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, TAdims.X, TAdims.Y),"text"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y), "menu"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, TDdims.X, TDdims.Y), "td"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y), "game"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, TDdims.X, virtualDims.Y - TDdims.Y), "status"));
            base.Initialize();

            waveTimer = new Timer(10);

            money = 100;
            income = 0;
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Button button = new Button("goToGame", new Rectangle(virtualDims.X / 2 - 100, virtualDims.Y / 2 - 50, 200, 100), new TextureDrawer(Content.Load<Texture2D>("button")));
            UIs = new UISystem[] { new UISystem(new List<Button>(1) {button}, "Menu"), new UISystem(SetupGameButtons(), "Game") };
            currentUI = UIs[0];
            //LOAD ALL XML
            ElementCollection.ReadDocument(XDocument.Load("Content/Entities.xml"));
            ElementCollection.ReadDocument(XDocument.Load("Content/TurretSheet.xml"));
            XElement e = ElementCollection.GetSpritesheetRef("turrets");
            SpriteSheetCollection.LoadSheet(ElementCollection.GetSpritesheetRef("turrets"), Content);
            
            FontDrawer drawer = new FontDrawer();
            TextureDrawer[] letters = GetLettersFromSource();
            drawer.fonts.Add(new DrawerCollection(letters, "aaa"));
            handler = new TextHandler(drawer, virtualDims);

            SoundManager.AddEffect(Content.Load<SoundEffect>("classic_hurt"), "shoot");
            SoundManager.AddSong(Content.Load<Song>("ld41tracktest2"), "song");

            ChangeToQueue(0);

            //LOAD MAP AND ENTS
            gameMap = new Map(
                new Vector2[] {
                    new Vector2(150, 0),
                    new Vector2(153, 138),
                    new Vector2(222, 139),
                    new Vector2(222, 184),
                    new Vector2(113, 185),
                    new Vector2(100, 100),
                    new Vector2(-30, 100)},
                new TextureDrawer(Content.Load<Texture2D>("envtest3")),
                new FRectangle[] {
                new FRectangle(0,0,141,81),
                new FRectangle(119,82,26,58),
                new FRectangle(135,144,78,23),
                new FRectangle(162,0,79,123),
                new FRectangle(230,123,9,67),
                new FRectangle(102,191,138,49),
                new FRectangle(0,104,100,138)});

            availableTurrets = new List<Entity>();
            availableTurrets.Add(Assembler.GetEnt(ElementCollection.GetEntRef("machinegun 1"), new Vector2(24,16), Content, ebuilder, false));
            status = new TextureDrawer(Content.Load<Texture2D>("statusbar"));

            cursor = new TextureDrawer(Content.Load<Texture2D>("cursor"), new TextureFrame(new Rectangle(0, 0, 8, 8), new Point(4, 4)));
            transitiontex = new TextureDrawer(Content.Load<Texture2D>("LOADINGOWO"));
        }

        void SetupGame()
        {
            lost = false;
            health = 100;
            money = 100;
            waveNumber = 1;
            waveAmt = 0;
            EntityCollection.RemoveAll();
            PlaceRocks();

            SoundManager.PlaySong("song");
        }

        void PlaceRocks()
        {
            Assembler.GetEnt(ElementCollection.GetEntRef("rock1"),new Vector2(14,122), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock1"), new Vector2(5, 160), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock1"), new Vector2(5, 167), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock1"), new Vector2(76, 214), Content, ebuilder);

            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(100, 64), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(93, 57), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(130, 48), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(185, 24), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(230, 78), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("rock2"), new Vector2(240, 16), Content, ebuilder);

            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(44, 44), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(54, 44), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(64, 44), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(48, 52), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(58, 52), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("tree1"), new Vector2(100, 32), Content, ebuilder);

        }

        void PlaceTurret(Vector2 pos_)
        {
            if(money >= availableTurrets[turretIndex].IntProperty("price"))
            {
                bool canPlace = false;
                foreach(FRectangle r in gameMap.buildableAreas)
                {
                    if (r.Contains(scenes.GetScene("game").ToVirtualPos(scenes.GetScene("td").ToVirtualPos(mouseMan.ClickPos()))))
                    {
                        canPlace = true;
                    }
                }
                foreach(Entity t in EntityCollection.GetGroup("turrets"))
                {
                    if((t.pos - scenes.GetScene("game").ToVirtualPos(scenes.GetScene("td").ToVirtualPos(mouseMan.ClickPos()))).Length()< 15)
                    {
                        canPlace = false;
                    }
                }
                foreach (Entity t in EntityCollection.GetGroup("rocks"))
                {
                    if ((t.pos - scenes.GetScene("game").ToVirtualPos(scenes.GetScene("td").ToVirtualPos(mouseMan.ClickPos()))).Length() < 5)
                    {
                        canPlace = false;
                    }
                }
                if (canPlace)
                {
                    money -= availableTurrets[turretIndex].IntProperty("price");
                    Assembler.GetEnt(ElementCollection.GetEntRef(availableTurrets[turretIndex].Name), pos_, Content, ebuilder);
                }
                
            }
            
        }

        void TransitionTo(GamePhase dest_)
        {
            nextPhase = dest_;
            transition = true;
            transitionTimer.Reset();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbs = Keyboard.GetState();
            GamePadState gps = GamePad.GetState(0);
            ipp.Update(kbs, gps);
            float es = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            mouseMan.Update();

            if (!transition)
            {
                if (phase == GamePhase.Menu)
                {
                    UpdateMenu(es);
                }
                if (phase == GamePhase.Gameplay)
                {
                    UpdateGame(es);
                }
                if (phase == GamePhase.urded)
                {
                    Updateurded(es);
                }
            }
            else
            {
                UpdateTransition(es);
            }

            base.Update(gameTime);
        }
        void UpdateTD(float es)
        {
            if (mouseMan.JustPressed() && TDFrame.Contains( scenes.GetScene("game").ToVirtualPos(mouseMan.ClickPos())))
            {
                PlaceTurret(scenes.GetScene("game").ToVirtualPos(mouseMan.ClickPos()));
            }
            EntityCollection.MoveAll();
            EntityCollection.UpdateAll(es);
        }
        void UpdateTA(float es)
        {

        }
        void UpdateTS(float es)
        {
            if (ipp.JustPressed("right"))
            {
                turretIndex++;
                if (turretIndex > availableTurrets.Count - 1)
                {
                    turretIndex = 0;
                }
            }
            if(ipp.JustPressed("left"))
            {
                turretIndex--;
                if (turretIndex < 0)
                {
                    turretIndex = availableTurrets.Count - 1;
                }
            }
        }
        void Updateurded(float es)
        {
            if (ipp.JustPressed("restart"))
            {
                TransitionTo(GamePhase.Gameplay);
                SetupGame();
            }
        }
        void UpdateHealth()
        {
            foreach(Entity e in EntityCollection.GetGroup("enemies"))
            {
                if(e.exists)
                if (gameMap.ReachedFinish(e.pos))
                {
                    health--;
                    e.exists = false;
                }
            }
            if (health <= 0)
            {
                lost = true;
                TransitionTo(GamePhase.urded);
            }
        }
        void UpdateMenu(float es)
        {
            if (currentUI.IssuedCommand("goToGame"))
            {
                currentUI = UIs[1];
                TransitionTo(GamePhase.Gameplay);
                SetupGame();
            }
            currentUI.HandleMouseInput(mouseMan, scenes.GetScene("game").ToVirtualPos(mouseMan.RawPos()));
        }
        void UpdateGame(float es)
        {

            UpdateHealth();
            for (int x = 0; x < 10; x++) { EntityCollection.OrderGroup(EntityCollection.entities, DrawOrder.SmallToBigY); }
            currentUI.HandleMouseInput(mouseMan, scenes.GetScene("text").ToVirtualPos(scenes.GetScene("game").ToVirtualPos(mouseMan.ClickPos())));
            handler.Update(es);
            EntityCollection.RecycleAll();

            gameMap.Update(es);
            UpdateTD(es);
            UpdateTA(es);
            UpdateTS(es);

            foreach (Entity e in EntityCollection.GetGroup("enemies"))
            {
                money += e.GetValue("loot") * 10;
            }

            if(countdown)
            waveTimer.Update(es);
            if (waveTimer.Complete())
            {
                SendWave(waveNumber * 10);
                waveTimer.Reset();
                money += income;
                if (money < 0)
                    money = 0;
            }
            if (waveAmt > 0)
            {
                waveAmt--;
                SpawnEnemy(1);
            }

            if (!mouseMan.Pressed())
            {
                if (currentUI.IssuedCommand("sayYes"))
                    HandleEventConsequences(currentQueue[currentEventNo].outcomeIfYes);
                else if (currentUI.IssuedCommand("sayNo"))
                    HandleEventConsequences(currentQueue[currentEventNo].outcomeIfNo);
            }
            if (handler.wasIgnored)
            { HandleEventConsequences(currentQueue[currentEventNo].outcomeIfIgnored); handler.wasIgnored = false; }
        }
        void UpdateTransition(float es)
        {
            transitionTimer.Update(es);
            if (transitionTimer.Complete())
            {
                transition = false;
                phase = nextPhase;
            }
        }

        void SendWave(int amt_)
        {
            waveAmt += amt_;
            waveNumber++;
        }
       

        protected List<Button> SetupGameButtons()
        {
            TextureDrawer temp = new TextureDrawer(Content.Load<Texture2D>("button"));
            List<Button> gameButtons = new List<Button>()
            {
                new Button("sayYes", new Rectangle(20, 230, 49 ,21), new TextureDrawer(Content.Load<Texture2D>("yesnpressed")), new TextureDrawer(Content.Load<Texture2D>("yespressed"))),
                new Button("sayNo", new Rectangle(91, 230, 49, 21), new TextureDrawer(Content.Load<Texture2D>("nonpressed")), new TextureDrawer(Content.Load<Texture2D>("nopressed")))
            };
            return gameButtons;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            if (phase == GamePhase.Gameplay)
            {
                DrawTD();
                DrawTA();
                DrawTS();
                DrawGameScenes();
                

                
            }
            else
            {
                scenes.SelectScene("game");
                scenes.CurrentScene.CreateInput(new Rectangle(0, 0, virtualDims.X, virtualDims.Y), 1);
                scenes.CurrentScene.CreateOutput(new Rectangle(0, 0, windowDims.X, windowDims.Y), true, false);
                scenes.SetupScene(spriteBatch, GraphicsDevice);

                currentUI.Draw(spriteBatch);
                cursor.Draw(spriteBatch, scenes.CurrentScene.ToVirtualPos(mouseMan.RawPos()));
                if (transition)
                {
                    transitiontex.Draw(spriteBatch, Vector2.Zero);
                }
                spriteBatch.End();
            }
            spriteBatch.Begin(samplerState: SamplerState.PointWrap);
            GraphicsDevice.SetRenderTarget(null);
            scenes.DrawScene(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        void DrawTD()
        {
            scenes.SelectScene("td");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, TDdims.X, TDdims.Y),1f);
            scenes.CurrentScene.CreateOutput(TDFrame, true, true);
            scenes.SetupScene(spriteBatch, GraphicsDevice);

            gameMap.Draw(spriteBatch);
            EntityCollection.DrawAll(spriteBatch);

            spriteBatch.End();
        }
        void DrawTA()
        {
            scenes.SelectScene("text");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, TAdims.X, TAdims.Y), 1f);
            scenes.CurrentScene.CreateOutput(TAFrame, true, true);
            scenes.SetupScene(spriteBatch, GraphicsDevice);

            //draw
            GraphicsDevice.Clear(Color.Beige);
            currentUI.Draw(spriteBatch);
            handler.Draw(spriteBatch);

            spriteBatch.End();
        }
        void DrawTS()
        {
            scenes.SelectScene("status");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, TDdims.X, virtualDims.Y - TDdims.Y), 1f);
            scenes.CurrentScene.CreateOutput(new Rectangle(0,240,240,32), true, true);
            scenes.SetupScene(spriteBatch, GraphicsDevice);

            status.Draw(spriteBatch, new Vector2(0, 0));
            availableTurrets[turretIndex].Draw(spriteBatch);
            string moneystring = money.ToString();
            while(moneystring.Length > 5) { moneystring =  moneystring.Remove(moneystring.Length - 3); moneystring += "k"; }
            handler.drawer.DrawText("aaa", "money:"+moneystring, new Rectangle(114, 3, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "next:"+ Math.Round(waveTimer.timer,1) + "", new Rectangle(114, 19, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "wave:"+ waveNumber + "", new Rectangle(194, 3, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "health:" + health + "", new Rectangle(164, 19, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "price:"+availableTurrets[turretIndex].IntProperty("price").ToString(), new Rectangle(50, 19, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", availableTurrets[turretIndex].Name, new Rectangle(50, 3, 500, 200), spriteBatch);
            spriteBatch.End();
        }
        void DrawGameScenes()
        {
            scenes.SelectScene("game");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, virtualDims.X, virtualDims.Y), 1);
            scenes.CurrentScene.CreateOutput(new Rectangle(0, 0, windowDims.X, windowDims.Y),true,false);
            scenes.SetupScene(spriteBatch, GraphicsDevice);
            GraphicsDevice.Clear(Color.Blue);

            scenes.DrawScene(spriteBatch, "text");
            scenes.DrawScene(spriteBatch, "td");
            scenes.DrawScene(spriteBatch, "status");
            cursor.Draw(spriteBatch, scenes.CurrentScene.ToVirtualPos(mouseMan.RawPos()));
            if (transition)
            {
                transitiontex.Draw(spriteBatch, Vector2.Zero);
            }
            spriteBatch.End();
        }


        protected TextureDrawer[] GetLettersFromSource()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789?!:;,.-_@'><)( /abcdefghijklmnopqrstuvwxyz";
            Texture2D ogTex = Content.Load<Texture2D>("font");
            TextureDrawer[] letterTexes = new TextureDrawer[78];
            for (int i = 0; i < 52; i++)
            {
                Rectangle rect = new Rectangle(6 * i, 0, 6, 12);
                Texture2D letter = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                Color[] data = new Color[rect.Width * rect.Height];
                ogTex.GetData(0, rect, data, 0, data.Length);
                letter.SetData(data);
                letterTexes[i] = new TextureDrawer(letter, null, alphabet[i].ToString());
            }
            for (int i = 0; i < 26; i++)
            {
                Rectangle rect = new Rectangle(6 * i, 12, 6, 12);
                Texture2D letter = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                Color[] data = new Color[rect.Width * rect.Height];
                ogTex.GetData(0, rect, data, 0, data.Length);
                letter.SetData(data);
                letterTexes[i + 44] = new TextureDrawer(letter,null, alphabet[i+44].ToString());
            }
            return letterTexes;
        }

        protected void HandleEventConsequences(string[] relevantVariable)
        {
            for (int i = 0; i < relevantVariable.Length; i++)
            {
                if (relevantVariable[i].StartsWith("getQueueEvent"))
                    ChangeToEventInQueue(int.Parse(relevantVariable[i].Substring(13)));
                else if (relevantVariable[i].StartsWith("sendWave"))
                    SendWave(int.Parse(relevantVariable[i].Substring(8)));
                else if (relevantVariable[i].StartsWith("breakTurret"))
                    BreakTurret(int.Parse(relevantVariable[i].Substring(11)));
                else if (relevantVariable[i].StartsWith("getQueue"))
                    ChangeToQueue(int.Parse(relevantVariable[i].Substring(8)));
                else if (relevantVariable[i].StartsWith("changeIncome"))
                    income += int.Parse(relevantVariable[i].Substring(12));
            }
        }
        
        protected void ChangeToEventInQueue(int eventNo)
        {
            currentEventNo = eventNo;
            handler.RemoveText();
            handler.AddTextToScroll(currentQueue[currentEventNo].text);
        }

        protected void ChangeToQueue(int queueNo)
        {
            currentQueue = GetEventQueue(queueNo);
            currentEventNo = 0;
            handler.RemoveText();
            handler.AddTextToScroll(currentQueue[0].text);
        }

        protected void SpawnEnemy(int enemyNo_)
        {
            for (int i = 0; i < enemyNo_; i++)
            {
                Assembler.GetEnt(ElementCollection.GetEntRef("enemy1"), new Vector2(150, -20), Content, ebuilder);
            }
        }

        protected void BreakTurret(int turretNo)
        {
            Random r = new Random();
            for (int i = 0; i < turretNo; i++)
            {
                bool done = false;
                int n = 0;
                while (!done && n < EntityCollection.GetGroup("turrets").Count)
                {
                    int k = r.Next(0, EntityCollection.GetGroup("turrets").Count);
                    if (EntityCollection.GetGroup("turrets")[k].exists)
                    {
                        EntityCollection.GetGroup("turrets")[k].exists = false; done = true; }
                    n++;
                }
                
            }
        }

        protected Event[] GetEventQueue(int nb)
        {
            XDocument xdoc = XDocument.Load("Content\\eventStuff.xml");
            XElement el = xdoc.Root.Element("EventQueue" + nb.ToString());
            int cap = (int)el.Attribute("cap");
            Event[] eQueue = new Event[cap];
            IEnumerable<XElement> els = el.Elements("Event");
            string yes, no, ignore;
            for (int i = 0; i < cap; i++)
            {
                yes = (string)els.ElementAt(i).Element("yes");
                no = (string)els.ElementAt(i).Element("no");
                ignore = (string)els.ElementAt(i).Element("ignore");
                eQueue[i] = new Event((string)els.ElementAt(i).Element("Text"), yes.Split(' '), no.Split(' '), ignore.Split(' '));
            }
            return eQueue;
        }

    }
}
