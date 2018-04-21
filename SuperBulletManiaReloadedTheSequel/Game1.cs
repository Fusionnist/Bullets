using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using MonoGame.FZT.UI;
using MonoGame.FZT.XML;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using static SuperBulletManiaReloadedTheSequel.Enums;

namespace SuperBulletManiaReloadedTheSequel
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputProfile inputProfile;
        CursorManager mouseMan;
        GamePhase phase;
        UISystem currentUI;
        UISystem[] UIs;
        TDEntityBuilder ebuilder;
        Point virtualDims, windowDims;
        SceneCollection scenes;
        TextHandler handler;
        Rectangle TDFrame, TAFrame;
        Point TDdims, TAdims;
        Event[] allEvents;
        Event currentEvent;
        Map gameMap;
        InputProfile ipp;
        List<Entity> availableTurrets;
        int turretIndex;
        TextureDrawer status;
        Timer waveTimer;
        int waveNumber, money, health;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ebuilder = new TDEntityBuilder();
            windowDims = new Point(800, 544);
            graphics.PreferredBackBufferHeight = windowDims.Y;
            graphics.PreferredBackBufferWidth = windowDims.X;
        }

        protected override void Initialize()
        {
            ipp = new InputProfile(new KeyManager[]
            {
                new KeyManager(Keys.Right, "right"),
                new KeyManager(Keys.Left, "left"),
            });

            inputProfile = new InputProfile(new KeyManager[] { new KeyManager(Keys.Left, "playerLeft"), new KeyManager(Keys.Right, "playerRight"), new KeyManager(Keys.Up, "playerUp"), new KeyManager(Keys.Down, "playerDown") });
            phase = GamePhase.Menu;
            mouseMan = new CursorManager();
            IsMouseVisible = true;
            EntityCollection.CreateGroup("turret", "turrets");
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


            allEvents = LoadUpEvents();

            ChangeToEvent(0);

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
            availableTurrets.Add(Assembler.GetEnt(ElementCollection.GetEntRef("turret1"), new Vector2(24,16), Content, ebuilder, false));
            status = new TextureDrawer(Content.Load<Texture2D>("statusbar"));
        }

        void PlaceTurret(Vector2 pos_)
        {
            if(money > availableTurrets[turretIndex].IntProperty("price"))
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
                    if((t.pos - scenes.GetScene("game").ToVirtualPos(scenes.GetScene("td").ToVirtualPos(mouseMan.ClickPos()))).Length()< 20)
                    {
                        canPlace = false;
                    }
                }
                if (canPlace)
                {
                    money -= availableTurrets[turretIndex].IntProperty("price");
                    Assembler.GetEnt(ElementCollection.GetEntRef("turret1"), pos_, Content, ebuilder);
                }
                
            }
            
        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            float es = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            mouseMan.Update();

            if (phase == GamePhase.Menu)
            {
                if (currentUI.IssuedCommand("goToGame"))
                { currentUI = UIs[1]; phase = GamePhase.Gameplay; }
                currentUI.HandleMouseInput(mouseMan);
            }
            if (phase == GamePhase.Gameplay)
            {
                EntityCollection.OrderGroup(EntityCollection.entities, DrawOrder.SmallToBigY);
                currentUI.HandleMouseInput(mouseMan, scenes.GetScene("text").ToVirtualPos(scenes.GetScene("game").ToVirtualPos(mouseMan.ClickPos())));
                handler.Update(es);
                EntityCollection.RecycleAll();

                gameMap.Update(es);
                UpdateTD(es);
                UpdateTA(es);
                UpdateTS(es);

                foreach(Entity e in EntityCollection.GetGroup("enemies"))
                {
                    money += e.GetValue("loot");
                }

                waveTimer.Update(es);
                if (waveTimer.Complete())
                {
                    SendWave(waveNumber * 4);
                    waveTimer.Reset();
                }

                if (currentUI.IssuedCommand("sayYes"))
                    HandleEventConsequences(currentEvent.outcomeIfYes);
                else if (currentUI.IssuedCommand("sayNo"))
                    HandleEventConsequences(currentEvent.outcomeIfNo);
                else if (handler.wasIgnored)
                { HandleEventConsequences(currentEvent.outcomeIfIgnored); handler.wasIgnored = false; }
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
                spriteBatch.Begin();
                currentUI.Draw(spriteBatch);
                spriteBatch.End();
            }


            base.Draw(gameTime);
        }

        protected List<Button> SetupGameButtons()
        {
            TextureDrawer temp = new TextureDrawer(Content.Load<Texture2D>("button"));
            List<Button> gameButtons = new List<Button>()
            {
                new Button("sayYes", new Rectangle(0, 212, 100 ,60), new TextureDrawer(Content.Load<Texture2D>("yes"))),
                new Button("sayNo", new Rectangle(60, 212, 100, 60), new TextureDrawer(Content.Load<Texture2D>("no")))
            };
            return gameButtons;
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
            while(moneystring.Length > 8) { moneystring =  moneystring.Remove(moneystring.Length - 2); moneystring += "k"; }
            handler.drawer.DrawText("aaa", "money:"+moneystring, new Rectangle(114, 3, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "next:"+ Math.Round(waveTimer.timer,1) + "", new Rectangle(114, 19, 500, 200), spriteBatch);
            handler.drawer.DrawText("aaa", "wave:"+ waveNumber + "", new Rectangle(194, 11, 500, 200), spriteBatch);
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
            spriteBatch.End();

            spriteBatch.Begin(samplerState: SamplerState.PointWrap);
            GraphicsDevice.SetRenderTarget(null);
            scenes.DrawScene(spriteBatch);
            spriteBatch.End();
        }

        protected TextureDrawer[] GetLettersFromSource()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789?!:;,.-_abcdefghijklmnopqrstuvwxyz";
            Texture2D ogTex = Content.Load<Texture2D>("font");
            TextureDrawer[] letterTexes = new TextureDrawer[70];
            for (int i = 0; i < 44; i++)
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
            ChangeToEvent((int)char.GetNumericValue(relevantVariable[0][8]));
            for (int i = 1; i < relevantVariable.Length; i++)
            {
                if (relevantVariable[i].StartsWith("sendWave"))
                    SendWave((int)char.GetNumericValue(relevantVariable[i][8]));
            }
            for (int i = 1; i < relevantVariable.Length; i++)
            {
                if (relevantVariable[i].StartsWith("breakTurret"))
                    BreakTurret((int)char.GetNumericValue(relevantVariable[i][11]));
            }
        }
        
        protected void ChangeToEvent(int eventNo)
        {
            currentEvent = allEvents[eventNo];
            handler.RemoveText();
            handler.AddTextToScroll(currentEvent.text);
        }

        protected void SendWave(int enemyNo_)
        {
            waveNumber++;
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

        protected Event[] LoadUpEvents()
        {
            Event[] events = new Event[2]
            {
                new Event(
                    "this is the first dialogue text wow",
                    new string[] { "getEvent1" },
                    new string[] { "getEvent1", "breakTurret1" },
                    new string[] { "getEvent1", "sendWave8" }),
                new Event(
                    "and this is the second dialogue text!",
                    new string[] { "getEvent0" },
                    new string[] { "getEvent0", "breakTurret3" },
                    new string[] { "getEvent0", "sendWave8" })
            };
            return events;
        }
    }
}
