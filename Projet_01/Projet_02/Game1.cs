using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Projet_02
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Rectangle fenetre;
        public Random nombre = new Random();
        GameObject fond;
        GameObject[] nuage;
        GameObject hero;
        GameObject[] ennemi;
        GameObject[] missile;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;

            fenetre = new Rectangle(0,0, graphics.GraphicsDevice.DisplayMode.Width, graphics.GraphicsDevice.DisplayMode.Height);
            this.graphics.ToggleFullScreen();
            //this.graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Le héro
            hero = new GameObject(nombre.Next(0,fenetre.Width),
                nombre.Next(0, fenetre.Height), true);
            hero.sprite = Content.Load<Texture2D>("avionHero.png");
            hero.angleRotation = 0f;

            // Le fond
            fond = new GameObject();
            fond.sprite = Content.Load<Texture2D>("water.jpg");

            // Les nuages
            nuage = new GameObject[10];
            for (int i = 0; i < 10; i++)
            {
                nuage[i] = new GameObject(nombre.Next(0, fenetre.Width),
                nombre.Next(0, fenetre.Height), true);
                nuage[i].sprite = Content.Load<Texture2D>("nuage.png");
            }

            // Les ennemis
            ennemi = new GameObject[16];
            for (int i = 0; i < 16; i++)
            {
                ennemi[i] = new GameObject(nombre.Next(0, fenetre.Width),
                nombre.Next(0, fenetre.Height), true);
                ennemi[i].sprite = Content.Load<Texture2D>("ennemi" + (i+1) + ".png");
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // L'eau
            spriteBatch.Draw(fond.sprite, new Rectangle(0, 0, fenetre.Width, fenetre.Height), 
                Color.White);

            // LEs 10 nuages
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw(nuage[i].sprite, nuage[i].position, null, Color.White,
                nuage[i].angleRotation, nuage[i].origine, 1.0f, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(hero.sprite, hero.position, null, Color.White,
                hero.angleRotation, hero.origine, 1.0f, SpriteEffects.None, 0f);

            //Les 16 ennemis
            for (int i = 0; i < 16; i++)
            {
                spriteBatch.Draw(ennemi[i].sprite, ennemi[i].position, null, Color.White,
                ennemi[i].angleRotation, ennemi[i].origine, 1.0f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
