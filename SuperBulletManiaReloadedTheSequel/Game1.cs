using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using MonoGame.FZT.UI;
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
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Button button = new Button("goToGame", new Rectangle(300, 200, 200, 100), new TextureDrawer(Content.Load<Texture2D>("button")));
            UIs = new UISystem[] { new UISystem(new List<Button>(1) {button}, "Menu"), new UISystem(new List<Button>(), "Game") };
            currentUI = UIs[0];
        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            mouseMan.Update();

            currentUI.HandleMouseInput(mouseMan);
            if (currentUI.IssuedCommand("goToGame"))
            { currentUI = UIs[1]; phase = GamePhase.Gameplay; }

            if (phase == GamePhase.Gameplay)
            {
                EntityCollection.UpdateAll((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            currentUI.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
