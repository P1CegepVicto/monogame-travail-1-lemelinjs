using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        public int nbVies = 0;
        private bool gameOver;
        int tempsFrappe = 0;
        GameObject fond;
        GameObject[] nuage;
        GameObject hero;
        GameObject heroEnFeu;
        GameObject [] heroEnFume;
        GameObject[] ennemi;
        GameObject[] missile;
        SpriteFont font;

        // Les sons
        SoundEffect son;
        SoundEffectInstance bombe;
        SoundEffectInstance bombe1;
        // Parametres de la game

        int nombreEnnemisVivant = 0, nombreEnnemiMax = 4;
        int ennemiQuiTire;
        int missileEnCourse = 0, nbMissilesSup = 0;
        String message;


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
            message = "La fenetre fait " + fenetre.Width + " de large \n" +
                "La fenetre fait " + fenetre.Height + " de haut";
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

            // Le sont d'un missile non mortel
            son = Content.Load<SoundEffect>("Sons\\Bombe1");
            bombe1 = son.CreateInstance();

            // Le son du dernier missile mortel
            son = Content.Load<SoundEffect>("Sons\\Bombe");
            bombe = son.CreateInstance();

            

            //La musique de fond
            Song song = Content.Load<Song>("Sons\\BrotherInArms");
            MediaPlayer.Play(song);
            
            // Le héro
            //--------
            hero = new GameObject(fenetre.Width/2,
               fenetre.Height/2, true);
            hero.sprite = Content.Load<Texture2D>("Hero/avionHero.png");
            // Le centre de l'image
            hero.origine.X = hero.sprite.Width/2;
            hero.origine.Y = hero.sprite.Height/2;
            hero.angleRotation = -1.5708f;

            //Le héro en feu
            heroEnFeu = new GameObject(hero.position.X,
                hero.position.Y, false);
            heroEnFeu.sprite = Content.Load<Texture2D>("Hero/avionHeroFeu.png");

            // Le héro en fumé
            heroEnFume = new GameObject[3];

            heroEnFume[0] = new GameObject(hero.position.X-200,
                hero.position.Y, false);
            heroEnFume[0].sprite = Content.Load<Texture2D>("Hero/fume.png");

            heroEnFume[1] = new GameObject(hero.position.X-100,
               hero.position.Y, false);
            heroEnFume[1].sprite = Content.Load<Texture2D>("Hero/fume.png");

            heroEnFume[2] = new GameObject(hero.position.X,
               hero.position.Y, false);
            heroEnFume[2].sprite = Content.Load<Texture2D>("Hero/fume.png");


            // Le fond
            fond = new GameObject();
            fond.sprite = Content.Load<Texture2D>("Fond/fond.jpg");
            fond.position.X = hero.position.X;
            fond.position.Y = hero.position.Y;

            fond.origine.X = fond.sprite.Width/2;
            fond.origine.Y = fond.sprite.Height/ 2;


            // Les nuages
            nuage = new GameObject[10];
            for (int i = 0; i < 10; i++)
            {
                nuage[i] = new GameObject(nombre.Next(0 - (fenetre.Width *2), fenetre.Width  *2),
                nombre.Next(0 - (fenetre.Height *2), fenetre.Height * 2), true);
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

               
                if (fond.angleRotation >= -1.5708 && fond.angleRotation < 0.7854)
                {
                    fond.origine.Y = fond.origine.Y - 3;
                    
                }
                else if (fond.angleRotation >= -0.7854 && fond.angleRotation < 0.7854)
                {
                    fond.origine.Y = fond.origine.Y - 15;
                }
                else if (fond.angleRotation >= -1.5708 && fond.angleRotation < -0.7854)
                {
                    fond.origine.Y = fond.origine.Y - 3;
                }
                
                // reculer le décor au lieu d'avancer condition décor vers vers la gauche (fleche de droite)
                if (fond.angleRotation >= 1.5708 && fond.angleRotation < 2.3562) // Fonctionne
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }
                else if (fond.angleRotation >= 2.3562 && fond.angleRotation < 3.927 ) 
                {
                    fond.origine.Y = fond.origine.Y + 15;
                }
                else if (fond.angleRotation >= 3.927 && fond.angleRotation < 4.7124 )
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }

                // Reculer le décor consition le décor vers la droite (flèche vers la gauche)
                if (fond.angleRotation >= -2.3562 && fond.angleRotation < -1.5708)    // à vérifier
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }
                else if (fond.angleRotation >= 2.3562 && fond.angleRotation <  3.927)
                {
                    fond.origine.Y = fond.origine.Y + 15;
                }
                else if ( fond.angleRotation >= -4.7124 && fond.angleRotation < -3.927) // à vérifier
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }

                UpdateNuages(1);
                }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {

                if (fond.angleRotation > -4.7124 && fond.angleRotation <= -3.927)
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }
                else if (fond.angleRotation >= -2.2562 && fond.angleRotation <= 0.7854)
                {
                    fond.origine.Y = fond.origine.Y + 15;
                }
                else if (fond.angleRotation > 0.7854 && fond.angleRotation < 1.5708)
                {
                    fond.origine.Y = fond.origine.Y + 3;
                }



                //                if (fond.vitesse.Y < 0.1f)
                //                {
                //                    fond.vitesse.Y = 0.4f;
                //                }

                UpdateNuages(2);
             }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    fond.origine.X -= 15;

                    fond.vitesse.X -= 0.1f;
                    
                    UpdateNuages(3);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {

                    fond.origine.X -= 15;
                    fond.vitesse.X += 0.1f;
                    
                    UpdateNuages(4);
                 }

                // Rotation de l'écran
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    fond.angleRotation += 0.01f;
                    if (fond.angleRotation > 4.7124f)
                    {
                        fond.angleRotation = -1.5708f;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    fond.angleRotation += -0.01f;
                    if (fond.angleRotation < -4.7124f)
                    {
                        fond.angleRotation = 1.5708f;
                    }
            }
            //fond.origine += fond.vitesse;

            

            // TODO: Add your update logic here
            UpdateHero();
            UpdateEnnemi();
            UpdateMissiles();
            UpdateMissiles(gameTime);
            UpdateEnnemi(gameTime);
            UpdateHero(gameTime);
            base.Update(gameTime);
        }

        public void UpdateHero()
        {
            hero.InScreen(fenetre);
        }

        public void UpdateNuages(int direction)
        {
            // UP
            if (direction ==1)
            {
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].position.Y += 12;
                }
            }
            // DOWN
            else if (direction == 2)
            {
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].position.Y -= 12;
                }
            }
            else if (direction == 3)
            {
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].position.X += 12;
                }
            }
            else if (direction == 4)
            {
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].position.X -= 12;
                }
            }


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

                //Donner l'accélération au missile 
                if (missile[ennemiQuiTire * 2].isLaunched)
                {
                    missile[ennemiQuiTire * 2].position.X
                        += missile[ennemiQuiTire * 2].vitesse.X;
                }

                // Vérifier la collision
                if (missile[ennemiQuiTire * 2].GetRect().Intersects(hero.GetRect()))
                {

                    //Afficher la fumée
                    for (int i = 0; i < heroEnFume.Length; i++)
                    {
                        if (!heroEnFume[i].estVivant )
                        {
                            if (i==0)
                            {
                                heroEnFume[i].estVivant = true;
                                i = heroEnFume.Length;
                            }
                            else
                            {
                                heroEnFume[i + 1].estVivant = true;
                                i = heroEnFume.Length;
                            }
                        }
                    }

                    // Préparer la fin de la game ou le recommencement
                    if (nbVies == 4)
                    {
                        bombe.Play();
                        gameOver = true;
                        
                    }
                    else
                    {
                        bombe1.Play();
                        nbVies++;    
                    }

                    missile[ennemiQuiTire * 2].isLaunched = false;
                    missileEnCourse--;


                }
              
                
                // Detecter un missile hors de la fenêtre
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

        public void UpdateMissiles(GameTime gameTime)
        {

            if (gameOver = true && tempsFrappe != 0)
            {
                tempsFrappe = gameTime.TotalGameTime.Milliseconds;
                message = "frappé a " + tempsFrappe;
            }
            else if (gameTime.TotalGameTime.Milliseconds - tempsFrappe > 5000  && gameOver == true) //  si 5 seconde se sont écoulés depuis gameOver
            {
                Exit();
            }
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
            spriteBatch.Draw(fond.sprite, fond.position, null,  Color.White,
                fond.angleRotation,fond.origine,1.0f, SpriteEffects.None,0f);

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
            for (int i = 0; i < heroEnFume.Length; i++)
            {
                if (heroEnFume[i].estVivant)
                {
                    spriteBatch.Draw(heroEnFume[i].sprite, heroEnFume[i].position, null, Color.White,
                    hero.angleRotation, hero.origine, 0.4f, SpriteEffects.None, 1.0f);
                }
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
            spriteBatch.DrawString(font, message, new Vector2(100, 100), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
