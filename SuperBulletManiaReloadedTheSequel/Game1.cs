using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
using System;
using System.Collections.Generic;

namespace SuperBulletManiaReloadedTheSequel
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        InputProfile inputProfile;
        CollisionManager colman;
        BulletShooter testShooter;
        int boundWidth, boundHeight;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            boundHeight = 810;
            boundWidth = 1440;
            graphics.PreferredBackBufferHeight = boundHeight;
            graphics.PreferredBackBufferWidth = boundWidth;
        }

        protected override void Initialize()
        {
            inputProfile = new InputProfile(new KeyManager[] { new KeyManager(Keys.Left, "playerLeft"), new KeyManager(Keys.Right, "playerRight"), new KeyManager(Keys.Up, "playerUp"), new KeyManager(Keys.Down, "playerDown") });
            colman = new CollisionManager(boundWidth, boundHeight);

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HitboxCollection tempColHb = new HitboxCollection(new FRectangle[][] { new FRectangle[] { new FRectangle(0, 0, 16, 16) } }, "collision");
            HitboxCollection tempHurtHb = new HitboxCollection(new FRectangle[][] { new FRectangle[] { new FRectangle(0, 0, 16, 16, properties_: new List<Property>() { new Property("damage", "1", "damage") }) } }, "hurt");

            TextureDrawer playerDrawer = new TextureDrawer(Content.Load<Texture2D>("greenBeamChunk"), new HitboxCollection[] { tempColHb }, "default");
            DrawerCollection drawer = new DrawerCollection(new TextureDrawer[] { playerDrawer }, "playerDrawThingy");
            player = new Player(drawer, new Vector2(0, 0), new List<Property>());

            TextureDrawer testBulletDrawer = new TextureDrawer(Content.Load<Texture2D>("dot"), new HitboxCollection[] { tempColHb, tempHurtHb }, "default");
            DrawerCollection testBulletDrawerCol = new DrawerCollection(new TextureDrawer[] { testBulletDrawer }, "bulletDrawThingy");
            Bullet baseBullet = new Bullet(testBulletDrawerCol, new Vector2(boundWidth / 2, boundHeight / 2), new List<Property>(), 0);
            testShooter = new BulletShooter(drawer, new Vector2(boundWidth / 2, boundHeight / 2), new List<Property>(), 0, baseBullet, 0.1f, 0.5f);
        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputProfile.Update(Keyboard.GetState(), GamePad.GetState(1));

            player.Input(ConvertInput());
            player.MultMov((float)gameTime.ElapsedGameTime.TotalSeconds);
            colman.TileAndPlayer(testShooter, player);

            foreach (var testBullet in testShooter.bullets)
            {
                testBullet.MultMov((float)gameTime.ElapsedGameTime.TotalSeconds);
                colman.TileAndBullet(testBullet, testShooter);
            }
            colman.DoBounds(player, testShooter.bullets.ToArray());

            foreach (var testBullet in testShooter.bullets)
                testBullet.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            testShooter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            testShooter.Draw(spriteBatch);
            foreach (var testBullet in testShooter.bullets)
                testBullet.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected Vector2 ConvertInput()
        {
            Vector2 inputVec = Vector2.Zero;
            if (inputProfile.Pressed("playerLeft"))
                inputVec.X -= 1;
            else if (inputProfile.Pressed("playerRight"))
                inputVec.X += 1;
            if (inputProfile.Pressed("playerUp"))
                inputVec.Y -= 1;
            else if (inputProfile.Pressed("playerDown"))
                inputVec.Y += 1;
            return inputVec;
        }
    }
}
