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
        GameObject heroEnFeu;
        GameObject[] ennemi;
        GameObject[] missile;
        SpriteFont font;
        // Parametres de la game

        int nombreEnnemisVivant = 0, nombreEnnemiMax = 4;
        int ennemiQuiTire;
        private int missileEnCourse = 0, nbMissilesSup = 0;


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
            this.graphics.PreferredBackBufferWidth = 
                graphics.GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = 
                graphics.GraphicsDevice.DisplayMode.Height;

            fenetre = new Rectangle(0,0, 
                graphics.GraphicsDevice.DisplayMode.Width, 
                graphics.GraphicsDevice.DisplayMode.Height);
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
            //--------
            hero = new GameObject(nombre.Next(0,fenetre.Width),
                nombre.Next(0, fenetre.Height), true);
            hero.sprite = Content.Load<Texture2D>("Hero/avionHero.png");
            // Le centre de l'image
            hero.origine.X = hero.sprite.Width/2;
            hero.origine.Y = hero.sprite.Height/2;
            hero.angleRotation = 0f;

            //Le héro en feu
            heroEnFeu = new GameObject(hero.position.X,
                hero.position.Y, false);
            heroEnFeu.sprite = Content.Load<Texture2D>("Hero/avionHeroFeu.png");

            // Le fond
            fond = new GameObject();
            fond.sprite = Content.Load<Texture2D>("Fond/water.jpg");

            // Les nuages
            nuage = new GameObject[10];
            for (int i = 0; i < 10; i++)
            {
                nuage[i] = new GameObject(nombre.Next(0, fenetre.Width),
                nombre.Next(0, fenetre.Height), true);
                nuage[i].sprite = Content.Load<Texture2D>("Fond/nuage.png");
            }

            // Les ennemis
            ennemi = new GameObject[16];
            missile = new GameObject[32];
            int missileIndividuel = 0;

            for (int i = 0; i < ennemi.Length; i++)
            {
                ennemi[i] = new GameObject(nombre.Next(0, fenetre.Width),
                nombre.Next(0, fenetre.Height), false);
                ennemi[i].sprite = Content.Load<Texture2D>("ennemis/ennemi" + (i+1) + ".png");
                ennemi[i].origine.X = ennemi[i].sprite.Width/2;
                ennemi[i].origine.Y = ennemi[i].sprite.Height / 2;
                // 2 missiles pour chaque ennemi
                // Missile du haut
                missile[i * 2] = new GameObject(ennemi[i].position.X, 
                    ennemi[i].position.Y,true);
                missile[i * 2].sprite = Content.Load<Texture2D>("missileEnnemi.png");
                missile[i * 2].origine.Y = missile[i*2].sprite.Height/2;
                missile[i * 2].origine.X = missile[i * 2].sprite.Width / 2;
                missile[i*2].isLaunched = false;

//              Missile du bas
                missile[(i*2)+1] = new GameObject(ennemi[i].position.X, 
                    ennemi[i].position.Y,true);
                missile[(i*2)+1].sprite = Content.Load<Texture2D>("missileEnnemi.png");
                missile[(i * 2)+1].origine.Y = missile[(i * 2)+1].sprite.Height / 2;
                missile[(i * 2)+1].origine.X = missile[(i * 2)+1].sprite.Width / 2;
                missile[(i * 2)+1].isLaunched = false;
            }

            // Le texte
            font = Content.Load<SpriteFont>("Font");
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

          if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    hero.position.Y -= 20;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    hero.position.Y += 20;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    hero.position.X -= 20;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    hero.position.X += 20;
                }

                // Rotation de l'avion
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    hero.angleRotation += -0.1f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    hero.angleRotation += 0.1f;
                }


            // TODO: Add your update logic here
            UpdateHero();
            UpdateEnnemi();
            UpdateMissiles();
            UpdateEnnemi(gameTime);
            UpdateHero(gameTime);
            base.Update(gameTime);
        }

        public void UpdateHero()
        {
            hero.InScreen(fenetre);
        }

        public void UpdateEnnemi()
        {
            //Faire apparaitre 4 ennemis seulements
            // Définir un seul qui tire
            
            for (int i = 0; nombreEnnemisVivant < nombreEnnemiMax & 
                i < ennemi.Length; i++)
            {
                int ennemiAleatoire = nombre.Next(0, 16);
                if (ennemi[ennemiAleatoire].estVivant == false)
                {
                    ennemi[ennemiAleatoire].estVivant = true;
                    nombreEnnemisVivant++;
                    // Apparaitre soit de la gauche (0) soit de la droite (1)
                    // soit d'en haut (3), soit d'en bas (4)
                    int positionRandom = nombre.Next(0, 5);
                    
                    //De la gauche
                    if (positionRandom == 0)
                    {
                        ennemi[i].position.X = 0 + ennemi[i].origine.X;
                        ennemi[i].position.Y = nombre.Next(0, fenetre.Height);
                    }
                    // de la droite
                    else if(positionRandom == 1)
                    {
                        ennemi[i].position.X = fenetre.Width -  ennemi[i].origine.X;
                        ennemi[i].position.Y = nombre.Next(0, fenetre.Height);
                    }
                    // d'en haut 
                    else if (positionRandom == 2)
                    {
                        ennemi[i].position.Y = 0 + ennemi[i].origine.Y;
                        ennemi[i].position.X = nombre.Next(0, fenetre.Width);
                    }
                    // D'en bas
                    else if (positionRandom == 3)
                    {
                        ennemi[i].position.Y = fenetre.Height - ennemi[i].origine.Y;
                        ennemi[i].position.X = nombre.Next(0, fenetre.Width);
                    }
                }
                
            }
            
            //L'ennemi reste dans la fenêtre et se bouge en fonction du héro
            for (int i = 0; i < ennemi.Length; i++)
            {
                ennemi[i].InScreen(fenetre);
                float[] progression = new float[4];
                float quelleProgression = 0;
                
                 progression[0] =  0.05f; 
                progression[1] = 0.02f;
                progression[2] = 0.00f;
                progression[3] = -0.01f;
                quelleProgression = progression[nombre.Next(0,4)];

                if (ennemi[i].position.X > hero.position.X)
                {
                    ennemi[i].vitesse.X -= quelleProgression;
                    ennemi[i].angleRotation = 3.1416f;
                }
                else
                {
                    ennemi[i].vitesse.X += quelleProgression ;
                    ennemi[i].angleRotation = 0f;
                }
                if (ennemi[i].position.Y > hero.position.Y)
                {
                    ennemi[i].vitesse.Y -= quelleProgression;

                }
                else
                {
                    ennemi[i].vitesse.Y += quelleProgression;


                }
                ennemi[i].position += ennemi[i].vitesse;
//              Ajuster le missile a l'ennemi
//              -----------------------------'             
//              Donner un offset au missile dépendant de l'orientation 
//              par rapport au héro si le missile n'est pas lauché
                
                if (ennemi[i].position.X > hero.position.X)
                {
                    if (!missile[i * 2].isLaunched)
                    {
                        missile[i*2].position.X = ennemi[i].position.X - 10;
                        missile[i * 2].position.Y = ennemi[i].position.Y - 20;
                        missile[i * 2].angleRotation = ennemi[i].angleRotation;
                    }
                    if (!missile[(i*2) + 1].isLaunched)
                    {
                        missile[(i*2) + 1].position.X = ennemi[i].position.X - 10;
                        missile[(i * 2) + 1].position.Y = ennemi[i].position.Y + 20;
                        missile[(i * 2) + 1].angleRotation = ennemi[i].angleRotation;
                    }
                }
                else
                {
                    if (!missile[i*2].isLaunched)
                    {
                        missile[i*2].position.X = ennemi[i].position.X + 10;
                        missile[i * 2].position.Y = ennemi[i].position.Y - 20;
                        missile[i*2].angleRotation = ennemi[i].angleRotation;
                    }
                    if (!missile[(i*2)+1].isLaunched)
                    {
                        missile[i * 2 + 1].position.X = ennemi[i].position.X + 10;
                        missile[(i * 2) + 1].position.Y = ennemi[i].position.Y + 20;
                        missile[(i * 2)+1].angleRotation = ennemi[i].angleRotation;
                    }
                    
                }
            }
           
        }
        /// <summary>
        /// Comportement des missiles
        /// </summary>
        public void UpdateMissiles()
        {
            // si le nombre de missile en course permet un missile supplémentaire
/*          AJOUTER : 
            - Missile de gauche
            - Possibilité de mettre plus de missiles dans l'écran pour augmenter
              Le stress.
            - mettre des balles de courte porté, mitraillettes 
/*/
             
            if (missileEnCourse <= nbMissilesSup)
            {
                ennemiQuiTire = nombre.Next(0, 16);

                if (ennemi[ennemiQuiTire].estVivant && 
                    !missile[ennemiQuiTire * 2].isLaunched)
                {
                    
                    // Dire au programme que ce missile est lancé
                    missile[ennemiQuiTire * 2].isLaunched = true;

                    // Lancer un missile
                    if (ennemi[ennemiQuiTire].position.X > hero.position.X)
                    {
                        missile[ennemiQuiTire * 2].vitesse.X = -15;

                        missileEnCourse++;
                    }
                    else
                    {
                       missile[ennemiQuiTire * 2].vitesse.X = +15;
                       missileEnCourse++;
                    }
                }
            }
            //Si le nombre de missile ne peut se permettre un missile suppl.mentaire
            else
            {

                //Donner l'accélération au missile lancé
                missile[ennemiQuiTire * 2].position.X 
                += missile[ennemiQuiTire * 2].vitesse.X;

                // Vérifier la collision
                if (missile[ennemiQuiTire * 2].GetRect().Intersects(hero.GetRect()))
                {
                    heroEnFeu.estVivant = true;
                }
              
                if (missile[ennemiQuiTire * 2].isLaunched)
                {
                    missile[ennemiQuiTire * 2].position.X 
                        += missile[ennemiQuiTire * 2].vitesse.X;
                }
                if (missile[ennemiQuiTire * 2].isLaunched &
                    missile[ennemiQuiTire * 2].position.X < 0)
                {
                    missile[ennemiQuiTire * 2].isLaunched = false;
                    missileEnCourse--;
                }
                else if (missile[ennemiQuiTire * 2].isLaunched & 
                    missile[ennemiQuiTire * 2].position.X > fenetre.Width)
                {
                    missile[ennemiQuiTire * 2].isLaunched = false;
                    missileEnCourse--;
                }   
            }
           

        }

        public void UpdateHero(GameTime gameTime)
        {

        }

        public void  UpdateEnnemi(GameTime gameTime)
        {

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

            // Le héro
            
            spriteBatch.Draw(hero.sprite, hero.position, null, Color.White,
                hero.angleRotation, hero.origine, 1.0f, SpriteEffects.None, 0f);
            // Le feu si frappé
            if (heroEnFeu.estVivant)
            {
                spriteBatch.Draw(heroEnFeu.sprite, hero.position, null, Color.White,
                hero.angleRotation, hero.origine, 1.0f, SpriteEffects.None, 0.75f);
            }
            //Les 16 ennemis
            for (int i = 0; i < 16; i++)
            {
                int missileIndividuel = 0;
                if (ennemi[i].estVivant)
                {
                    spriteBatch.Draw(ennemi[i].sprite, ennemi[i].position, null, Color.White,
                        ennemi[i].angleRotation, ennemi[i].origine, 1.0f, 
                        SpriteEffects.None, 0f);
                    spriteBatch.Draw(missile[i*2].sprite,missile[i*2].position, null, Color.White,
                        missile[i*2].angleRotation, missile[i * 2].origine, 0.5f,
                        SpriteEffects.None, 0f);
                    spriteBatch.Draw(missile[(i * 2)+1].sprite, missile[(i * 2)+1].position, null, Color.White,
                        missile[(i * 2)+1].angleRotation, missile[(i * 2)+1].origine, 0.5f,
                        SpriteEffects.None, 0f);
                }
               
            }
            
            // Écrire un texte
            spriteBatch.DrawString(font, "Hello Mono!", new Vector2(100, 100), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
