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
        Vector2 virtualDims;
        TDEntityBuilder ebuilder;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ebuilder = new TDEntityBuilder();
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
            virtualDims = new Vector2(800, 480);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Button button = new Button("goToGame", new Rectangle(300, 200, 200, 100), new TextureDrawer(Content.Load<Texture2D>("button")));
            UIs = new UISystem[] { new UISystem(new List<Button>(1) {button}, "Menu"), new UISystem(new List<Button>(), "Game") };
            currentUI = UIs[0];
            //LOAD ALL XML
            ElementCollection.ReadDocument(XDocument.Load("Content/Entities.xml"));
            ElementCollection.ReadDocument(XDocument.Load("Content/TurretSheet.xml"));
            XElement e = ElementCollection.GetSpritesheetRef("turrets");
            SpriteSheetCollection.LoadSheet(ElementCollection.GetSpritesheetRef("turrets"), Content);
            Assembler.GetEnt(ElementCollection.GetEntRef("turret1"), new Vector2(100, 100), Content, ebuilder);

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
            }

            base.Update(gameTime);
        }

        void UpdateTD(float es)
        {
            EntityCollection.UpdateAll(es);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            currentUI.Draw(spriteBatch);
            if (phase == GamePhase.Gameplay)
                DrawTD();
            spriteBatch.End();

            base.Draw(gameTime);
        }
        void DrawTD()
        {
            EntityCollection.DrawAll(spriteBatch);
        }
    }
}
