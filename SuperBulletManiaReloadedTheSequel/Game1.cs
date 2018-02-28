using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.FZT;
using MonoGame.FZT.Assets;
using MonoGame.FZT.Data;
using MonoGame.FZT.Drawing;
using MonoGame.FZT.Input;
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
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 810;
            graphics.PreferredBackBufferWidth = 1440;
        }

        protected override void Initialize()
        {
            inputProfile = new InputProfile(new KeyManager[] { new KeyManager(Keys.Left, "playerLeft"), new KeyManager(Keys.Right, "playerRight"), new KeyManager(Keys.Up, "playerUp"), new KeyManager(Keys.Down, "playerDown") });
            colman = new CollisionManager();

            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureDrawer playerDrawer = new TextureDrawer(Content.Load<Texture2D>("greenBeamChunk"), new HitboxCollection[] { new HitboxCollection(new FRectangle[][] { new FRectangle[] { new FRectangle(0, 0, 16, 16) } }, "collision") }, "default");
            DrawerCollection drawer = new DrawerCollection(new TextureDrawer[] { playerDrawer }, "playerDrawThingy");
            player = new Player(drawer, new Vector2(0, 0), new List<Property>());
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

            colman.DoBounds(player);

            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
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
