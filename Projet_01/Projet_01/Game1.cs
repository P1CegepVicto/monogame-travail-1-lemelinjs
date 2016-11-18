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
        GameObject hero, heroEnFeu;
        GameObject ennemi, missileEnnemi1;
        GameObject fond;
        GameObject screen;
        Rectangle fenetre;

        public static int maxX, maxY;
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
            heroEnFeu = new GameObject(hero.position.X, hero.position.Y,150,96, false);
            ennemi = new GameObject(maxY -200,maxX/2,101,79, true);
            missileEnnemi1 = new GameObject(ennemi.position.X,ennemi.position.Y,9,40,false);
            fond = new GameObject();
            
            // Intialiser propriétés dans l'espace 
            ennemi.changementDirection = true;
            hero.rotationAngle = 0;
            ennemi.rotationAngle = 0;

            // Loader les images
            fond.sprite = Content.Load<Texture2D>("fond.jpg");
            hero.sprite = Content.Load<Texture2D>("avionHero.png");
            heroEnFeu.sprite = Content.Load<Texture2D>("avionHeroFeu.png");
            ennemi.sprite = Content.Load<Texture2D>("wildcat-top.png");
            missileEnnemi1.sprite = Content.Load<Texture2D>("missileEnnemi.png");



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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                hero.position.Y -= 40;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                hero.position.Y += 40;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                hero.position.X -= 40;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                hero.position.X += 40; 
            }

            // Rotation de l'avion
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                hero.rotationAngle += -0.1f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                hero.rotationAngle += 0.1f;
            }

            // Attaque de l'ennemi
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (!missileEnnemi1.estVivant)
                {
                    missileEnnemi1.vitesse.X = -10;
                    missileEnnemi1.estVivant = true;
                }
               

            }
            
            // TODO: Add your update logic here
            UpdateHero(); // un update pour le vaisseau
            UpdateEnnemi();
            UpdateMissile();
            UpdateEnnemi(gameTime);
            base.Update(gameTime);
        }

        ///
        public void UpdateHero()
        {
            hero.InScreen(fenetre);
            if (missileEnnemi1.GetRect().Intersects(hero.GetRect()))
            {
                heroEnFeu.estVivant = true;
            }

        }
        public void UpdateEnnemi()
        {
            ennemi.InScreen(fenetre);
            //Vitesse Y pour la prochaine x/60 seconde (tempsDeVol)
            
            if (ennemi.changementDirection == true)
            {
                
                vitY = nbRand.Next(-1, 2);
                if ( vitY == -1)
                {
                    if (ennemi.position.Y > hero.position.Y)// on va vers l'Héro
                    {
                         ennemi.vitesse.Y -= 0.5f;
                    }
                    else
                    {
                        ennemi.vitesse.Y += 0.5f;
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
                    if (ennemi.position.X > hero.position.X)
                    {
                        ennemi.vitesse.X -= 0.5f;
                        
                    }
                    else
                    {
                        ennemi.vitesse.X += 0.5f;
                    }
                    
                }
                else if (vitX == 0)
                {
                    ennemi.vitesse.Y -= 0.0f; // reste pareil

                }
                else if (vitX == 1)
                {
                    if (ennemi.position.X > hero.position.X)
                    {
                        ennemi.vitesse.X -= 0.2f;
                    }
                    else
                    {
                        ennemi.vitesse.X += 0.2f;
                    }

                }
            }

            ennemi.position += ennemi.vitesse;
            // Vérifier si l'ennemi est à droite ou à gauche pour mettre le missile du bon côté
            // de l'avion et dans la bonne direction
            if (ennemi.position.X > hero.position.X)
            {
                if (!missileEnnemi1.estVivant)
                {
                    missileEnnemi1.position.X = ennemi.position.X - 22;
                    missileEnnemi1.position.Y = ennemi.position.Y + 30;
                    missileEnnemi1.rotationAngle = 3.1416F;
                }
                
                ennemi.rotationAngle = 0f;
                
            }
            else
            {
                if (!missileEnnemi1.estVivant)
                {
                    missileEnnemi1.position.X = ennemi.position.X + 22;
                    missileEnnemi1.position.Y = ennemi.position.Y - 30;
                    missileEnnemi1.rotationAngle = 0f;
                    ennemi.rotationAngle = 3.1416f;
                }
            }
           


        }

        public void UpdateEnnemi(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 3000 == 0)
            {
                ennemi.changementDirection = false;
                
            }
            else
            {
                ennemi.changementDirection = true;
              
            }
        }

        public void UpdateMissile()
        {
            if (!missileEnnemi1.estVivant)
            {
                if (ennemi.position.X>hero.position.X)
                {
                    missileEnnemi1.vitesse.X = -20;
                    missileEnnemi1.estVivant = true;
                }
                else
                {
                    missileEnnemi1.vitesse.X = +20;
                    missileEnnemi1.estVivant = true;
                }
                
            }

            if (missileEnnemi1.position.X < 0 || missileEnnemi1.position.X > fenetre.Width)// AJOUTER UN OU POUR 
            {
                missileEnnemi1.estVivant = false;
            }
            missileEnnemi1.position += missileEnnemi1.vitesse;
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


            // Sprite du héro
            spriteBatch.Draw(hero.sprite,hero.position, null, Color.White, hero.rotationAngle, hero.origine, 
                1.0f, SpriteEffects.None, 0f);

            // Sprite de feu
            if (heroEnFeu.estVivant) // Si touché, mettre le feu
            {
                spriteBatch.Draw(heroEnFeu.sprite, hero.position, null, Color.White, hero.rotationAngle, 
                hero.origine, 1.0f, SpriteEffects.None, 0f);
                missileEnnemi1.estVivant = false;
            }
            

            // Sprite de l'ennemi
            spriteBatch.Draw(ennemi.sprite, ennemi.position,null, Color.White, ennemi.rotationAngle, ennemi.origine, 
                1.0f, SpriteEffects.None, 0f);
            // Sprite du missile de l'ennemi
            spriteBatch.Draw(missileEnnemi1.sprite, missileEnnemi1.position, null, Color.White, 
                missileEnnemi1.rotationAngle, missileEnnemi1.origine, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
