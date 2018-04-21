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
        Point virtualDims;
        SceneCollection scenes;
        TextHandler handler;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ebuilder = new TDEntityBuilder();
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1200;
        }

        protected override void Initialize()
        {
            inputProfile = new InputProfile(new KeyManager[] { new KeyManager(Keys.Left, "playerLeft"), new KeyManager(Keys.Right, "playerRight"), new KeyManager(Keys.Up, "playerUp"), new KeyManager(Keys.Down, "playerDown") });
            phase = GamePhase.Menu;
            mouseMan = new CursorManager();
            IsMouseVisible = true;
            EntityCollection.CreateGroup("turret", "turrets");
            EntityCollection.CreateGroup("enemy", "enemies");
            EntityCollection.CreateGroup("bgElement", "bgElements");
            EntityCollection.CreateGroup(new Property("isEnt", "isEnt", "isEnt"), "entities");
            virtualDims = new Point(1200, 720);

            scenes = new SceneCollection();
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y),"text"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y), "menu"));
            scenes.scenes.Add(new Scene(new RenderTarget2D(GraphicsDevice, virtualDims.X, virtualDims.Y), "td"));
            base.Initialize();
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
            Assembler.GetEnt(ElementCollection.GetEntRef("turret1"), new Vector2(100, 100), Content, ebuilder);
            FontDrawer drawer = new FontDrawer();
            TextureDrawer[] letters = GetLettersFromSource();
            drawer.fonts.Add(new DrawerCollection(letters, "aaa"));
            handler = new TextHandler(drawer, virtualDims);
            handler.AddTextToScroll("zfsf sdfdftrhtsd dqdqsd");

            //LOAD MAP AND ENTS 
            Assembler.GetEnt(ElementCollection.GetEntRef("turret1"), new Vector2(30, 30), Content, ebuilder);
            Assembler.GetEnt(ElementCollection.GetEntRef("enemy1"), new Vector2(300, 300), Content, ebuilder);
            //LOAD MAP AND ENTS

        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            float es = (float)gameTime.ElapsedGameTime.TotalSeconds; 
            mouseMan.Update();

            currentUI.HandleMouseInput(mouseMan);
            if (currentUI.IssuedCommand("goToGame"))
            { currentUI = UIs[1]; phase = GamePhase.Gameplay; }

            if (phase == GamePhase.Gameplay)
            {
                UpdateTD(es);
                UpdateTA(es);
            }

            handler.Update(es);

            base.Update(gameTime);
        }

        void UpdateTD(float es)
        {
            EntityCollection.UpdateAll(es);
        }
        void UpdateTA(float es)
        {

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            currentUI.Draw(spriteBatch);

            if (phase == GamePhase.Gameplay)
            {
                handler.Draw(spriteBatch);
                spriteBatch.End();
                DrawTD();
                DrawTA();
                DrawGameScenes();
            }
            else if (phase == GamePhase.Menu)
                spriteBatch.End();

            base.Draw(gameTime);
        }

        protected List<Button> SetupGameButtons()
        {
            TextureDrawer temp = new TextureDrawer(Content.Load<Texture2D>("button"));
            List<Button> gameButtons = new List<Button>()
            {
                new Button("sayYes", new Rectangle(virtualDims.X * 3/5, virtualDims.Y * 7/8, virtualDims.X / 6, virtualDims.X / 8), temp),
                new Button("sayNo", new Rectangle(virtualDims.X * 4/5, virtualDims.Y * 7/8, virtualDims.X / 6, virtualDims.X / 8), temp),
                new Button("selectTurret1", new Rectangle(0, 0, virtualDims.X / 6, virtualDims.X / 6), temp),
                new Button("selectTurret2", new Rectangle(virtualDims.X / 6, 0, virtualDims.X / 6, virtualDims.X / 6), temp),
            };
            return gameButtons;
        }
        void DrawTD()
        {
            scenes.SelectScene("td");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, 100, virtualDims.Y),1f);
            scenes.CurrentScene.CreateOutput(new Rectangle(0, 0, 1000, 1000), true, true);
            scenes.SetupScene(spriteBatch, GraphicsDevice);

            EntityCollection.DrawAll(spriteBatch);

            spriteBatch.End();
        }
        void DrawTA()
        {
            scenes.SelectScene("text");
            scenes.CurrentScene.CreateInput(new Rectangle(0, 0, 100, virtualDims.Y), 1f);
            scenes.CurrentScene.CreateOutput(new Rectangle(0, 0, 200, 200), true, true);
            scenes.SetupScene(spriteBatch, GraphicsDevice);

            //draw

            spriteBatch.End();
        }
        void DrawGameScenes()
        {
            spriteBatch.Begin();
            GraphicsDevice.SetRenderTarget(null);
            scenes.DrawScene(spriteBatch, "text");
            scenes.DrawScene(spriteBatch, "td");
            spriteBatch.End();
        }

        protected TextureDrawer[] GetLettersFromSource()
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789?!:;,.-_abcdefghijklmnopqrstuvwxyz";
            Texture2D ogTex = Content.Load<Texture2D>("font");
            TextureDrawer[] letterTexes = new TextureDrawer[70];
            for (int i = 0; i < 44; i++)
            {
                Rectangle rect = new Rectangle(16 * i, 0, 16, 16);
                Texture2D letter = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                Color[] data = new Color[rect.Width * rect.Height];
                ogTex.GetData(0, rect, data, 0, data.Length);
                letter.SetData(data);
                letterTexes[i] = new TextureDrawer(letter, null, alphabet[i].ToString());
            }
            for (int i = 0; i < 26; i++)
            {
                Rectangle rect = new Rectangle(16 * i, 16, 16, 16);
                Texture2D letter = new Texture2D(GraphicsDevice, rect.Width, rect.Height);
                Color[] data = new Color[rect.Width * rect.Height];
                ogTex.GetData(0, rect, data, 0, data.Length);
                letter.SetData(data);
                letterTexes[i + 44] = new TextureDrawer(letter,null, alphabet[i+44].ToString());
            }
            return letterTexes;
        }
    }
}
