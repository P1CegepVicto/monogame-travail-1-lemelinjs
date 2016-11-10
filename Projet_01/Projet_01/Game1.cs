using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/* Initialiser les variables dans public class Game1: Game
 * GameObject hero;
 * dans LoadContent()
 * hero = new GameObjet;
 * On initialise la grosseur de la fenete dans fonction Initialise()
 * On trace un rectangle transparent au dessus de la fenetre
 * Rectangle fenetre à mettre sur le jeu
 * L'action se passe dans Update()
 * Dans Draw() on dessine ce qu'on veut voir apparaitre;
 * tHÉORIE DU 10
 * Récap
 * Étape 1
 * Créer l'objet globalement
 * Loadcontent : on met des trucs dans notre gameobject
 * On ne met presque rien dans update, on appelle des fonctions
 */
namespace Projet_01
{
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameObject hero;
        GameObject ennemi;
        GameObject fond;
        GameObject screen;
        Rectangle fenetre;
        public static int maxX;
        public static int maxY;




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
            // Demander à Windows, la résolution maximal
            // La largeur de l'ordi dans Windows
            this.graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height;

            maxY = graphics.GraphicsDevice.DisplayMode.Height;
            maxX = graphics.GraphicsDevice.DisplayMode.Width;

            // Met toi en plein écran
            this.graphics.ToggleFullScreen();
            //this.graphics.ApplyChanges();

            fenetre = new Rectangle(0, 0, maxX, maxY);
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
            hero = new GameObject("avionHero.png",300, 300, 150, 96, true);
            ennemi = new GameObject("wildcat-top.png",500,500,75,100, true);
            fond = new GameObject();

            //screen = new GameObject();
            fond.sprite = Content.Load<Texture2D>("fond.jpg");

            hero.sprite = Content.Load<Texture2D>("avionHero.png");
            ennemi.sprite = Content.Load<Texture2D>("wildcat-top.png");


            hero.rotationAngle = 0;

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                hero.position.Y -= 20;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                hero.position.Y += 20;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                hero.position.X -= 5;
            }
            //Recule rapide
            if (Keyboard.GetState().IsKeyDown(Keys.CapsLock))
            {
                hero.position.X -= 30;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                hero.position.X += 5;
            }

            //Détection des limites

            if ((hero.position.Y) > (maxY-96))
            {
                hero.position.Y = -96;
            }
            else if ((hero.position.Y) < -96)
            {
                hero.position.Y = maxY-96;
            }
            if ((hero.position.X + 300) >= (int)(maxX/2))
            {
                hero.position.X -= 5;
            }
            else if (hero.position.X < 150)
            {
                hero.position.X = 150;
            }

            // rotation essais
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                hero.rotationAngle -= 0.03f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                hero.rotationAngle += 0.03f;
            }
            if (hero.rotationAngle < -1.0f)
            {
                hero.rotationAngle = -1.0f;
            }
            if (hero.rotationAngle > 1.2f)
            {
                hero.rotationAngle = 1.2f;
            }


            // TODO: Add your update logic here
            UpdateHero(); // un update pour le vaisseau
            base.Update(gameTime);
        }

        ///
        public void UpdateHero()
        {

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // fond bleu de base
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // L'objet qui permet d'afficher toutes les images 
            spriteBatch.Begin();
            spriteBatch.Draw(fond.sprite, new Rectangle(0, 0, maxX,maxY), Color.White);
            //spriteBatch.Draw(hero.sprite, hero.position, Color.White);
            spriteBatch.Draw(hero.sprite,hero.position, null, Color.White, hero.rotationAngle, hero.origine, 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(ennemi.sprite, ennemi.position, Color.White);



            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
