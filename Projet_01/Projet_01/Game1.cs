using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public static int vitY=0, vitX=0;
        public static Random nbRand = new Random();
        public static double tempsDeVol = 0, tempsDeVolReel = 0;

       


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
            
            //Loader les items et leur attribuer de valeurs (position x, y, centre x, centre y)
            hero = new GameObject(300, 300, 150, 96, true);
            ennemi = new GameObject(1900,500,75,100, true);
            fond = new GameObject();
            
            // Initialiser la rotation pour les objets qui tournent
            hero.rotationAngle = 0;

            // Loader les images
            fond.sprite = Content.Load<Texture2D>("fond.jpg");
            hero.sprite = Content.Load<Texture2D>("avionHero.png");
            ennemi.sprite = Content.Load<Texture2D>("wildcat-top.png");

            

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

            /*if ((hero.position.Y) > (maxY-(hero.Height/2)))
            {
                hero.position.Y = (maxY - (hero.Height / 2));
            }
            else if ((hero.position.Y) < (hero.Height / 2))
            {
                hero.position.Y = (hero.Height / 2);
            }
            if (hero.position.X + (hero.Width/2) >= maxX)
            {
                hero.position.X -= 5;
            }
            else if (hero.position.X < (hero.Width/2))
            {
                hero.position.X = hero.Width/2;
            }*/

            InScreen("hero");
            InScreen("ennemi");

            // Rotation de l'avion
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                hero.rotationAngle += 0.03f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                hero.rotationAngle += -0.03f;
            }
            /* pas de limitation pour le moment
            if (hero.rotationAngle < -1.0f)
            {
                hero.rotationAngle = -1.0f;
            }
            if (hero.rotationAngle > 1.2f)
            {
                hero.rotationAngle = 1.2f;
            }
            */

            // TODO: Add your update logic here
            UpdateHero(); // un update pour le vaisseau
            UpdateEnnemi();
            base.Update(gameTime);
        }

        ///
        public void UpdateHero()
        {

        }
        public void UpdateEnnemi()
        {
            //Vitesse Y pour la prochaine x/60 seconde (tempsDeVol)
            if (tempsDeVolReel >= tempsDeVol)
            {
                tempsDeVol = 20;
                vitY = nbRand.Next(-1, 2);
                if ( vitY == -1)
                {
                    if (ennemi.position.Y > hero.position.Y)// on va vers l'Héro
                    {
                         ennemi.vitesse.Y -= 0.4f; 
                    }
                    else
                    {
                        ennemi.vitesse.Y += 0.4f; 
                    }
                    
                }
                else if (vitY == 0) // Même si l'action est neutre, je met qqch au cas 
                    //ou je voudrais changer
                {
                    if (ennemi.position.Y > hero.position.Y)
                    {
                       ennemi.vitesse.Y -= 0.0f; // reste pareil 
                    }
                    else
                    {
                        ennemi.vitesse.Y -= 0.0f; // reste pareil      
                    }
                    
                    
                }
                else if (vitY == 1)
                {
                    if (ennemi.position.Y < hero.position.Y) // on s'éloigne du héro
                    {
                        ennemi.vitesse.Y += 0.2f;
                    }
                    else
                    {
                        ennemi.vitesse.Y -= 0.2f;
                    }
                    
                
                }
                vitX = nbRand.Next(-1, 2);
                if ( vitX == -1)
                {
                    ennemi.vitesse.X -= 0.05f;
                }
                else if (vitX == 0)
                {
                    ennemi.vitesse.Y -= 0.0f; // reste pareil

                }
                else if (vitX == 1)
                {
                    ennemi.vitesse.Y += 0.05f; // Augmente de 0.1

                }
            }
            else
            {
                tempsDeVol = 0;
                tempsDeVolReel = 0;
            }
            
            ennemi.position += ennemi.vitesse;


        }

        public void InScreen(string personnage)
        {
            float positionY = 0, positionX  = 0;
            int height = 0, width = 0;
            if (personnage == "hero")
            {
                positionX = hero.position.X;
                positionY = hero.position.Y;
                height = hero.Height;
                width = hero.Width;
            }
            else
            {
                positionX = ennemi.position.X;
                positionY = ennemi.position.Y;
                height = ennemi.Height;
                width = ennemi.Width;
            }
            if ((positionY) > (maxY - (height / 2)))
            {
                positionY = (maxY - (height / 2));
            }
            else if ((positionY) < (height / 2))
            {
                positionY= (height/ 2);
            }
            if (positionX + (width/ 2) >= maxX)
            {
                positionX -= 5;
            }
            else if (positionX< (width / 2))
            {
                positionX = hero.Width / 2;
            }
            if (personnage == "hero")
            {
                hero.position.X = positionX;
                hero.position.Y = positionY;
            }
            else
            {
                ennemi.position.X = positionX;
                ennemi.position.Y = positionY;
            }
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
