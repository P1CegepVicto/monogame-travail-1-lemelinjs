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
        private bool gameOver, youWin;
        int tempsFrappe = 0, tempsActuel = 0, tempsEnMinutes = 0;
        double tempsPartie=60, tempsDernierAjoutEnnemi=20, tempsDebutGame = 0;

        // Déclaration pour les angles
        int angleNombre = 0; // Combien se segments
        float angleIncrement = 0; // Le cacul d'incrément
        private float[] angle; // angles
        private float rotationGlobale;
        GameObject rapporteur;
        private GameObject barre;


        GameObject menu;
        GameObject gameOverStamp;
        GameObject youWinStamp;
        GameObject fond;
        GameObject fondMiniature;
        GameObject heroMiniature;
        GameObject[] ennemiMiniature;
        GameObject[] nuage;
        GameObject hero;
        GameObject heroEnFeu;
        GameObject[] heroEnFume;
        GameObject[] ennemi;
        GameObject[] missile;
        SpriteFont font;

        // Les sons
        SoundEffect son;
        SoundEffectInstance bombe;
        SoundEffectInstance bombe1;
        // Parametres de la game

        int nombreEnnemisVivant = 0, nombreEnnemiMax = 16;
        int ennemiQuiTire;
        int missileEnCourse = 0, nbMissilesSup = 0;

        String message;

        // les limites du jeu
        //float limiteMinX, limiteMaxX, limiteMinY, limiteMaxY;

        private int margeDeSecurite = 0;

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

            fenetre = new Rectangle(0, 0,
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

            // Attribuer les angles
            angleNombre = 360; // 360 angles 
            angleIncrement = (float)(3.14159265358979323846264338327950288419716939937510582 * 2) / angleNombre;
            angle = new float[angleNombre];
            for (int i = 0; i < angleNombre; i++)
            {

                if (i == 0) // On intitialise la première valeur
                {
                    angle[i] = 0;
                }
                else if (i < angleNombre) // On addition l'incrément
                {
                    angle[i] = angle[i - 1] + angleIncrement;
                }
            }

            // Le rapporteur
            // Dans cette exemple, je le place au centre de l'écran
            rapporteur = new GameObject(fenetre.Width / 2,
               fenetre.Height / 2, true);
            rapporteur.sprite = Content.Load<Texture2D>("rapporteur.png");
            rapporteur.angleRotation = 1.5708f;

            //Je place son centre sur l'objet fixe
            rapporteur.origine.X = rapporteur.sprite.Width / 2;
            rapporteur.origine.Y = rapporteur.sprite.Height / 2;

            //Je le place sur l'objet amovible
            barre = new GameObject(fenetre.Width / 2,
               fenetre.Height / 2, true);
            barre.sprite = Content.Load<Texture2D>("barre.png");
            barre.origine.Y = barre.sprite.Height / 2;
            //            barre.origine.X = barre.sprite.Width / 2;


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
            hero = new GameObject(fenetre.Width / 2,
               fenetre.Height / 2, true);
            hero.sprite = Content.Load<Texture2D>("Hero/avionHero.png");
            // Le centre de l'image
            hero.origine.X = hero.sprite.Width / 2;
            hero.origine.Y = hero.sprite.Height / 2;
            hero.angleRotation = angle[0];

            //Le héro en feu
            heroEnFeu = new GameObject(hero.position.X,
                hero.position.Y, false);
            heroEnFeu.sprite = Content.Load<Texture2D>("Hero/avionHeroFeu.png");

            // Le héro en fumé
            heroEnFume = new GameObject[3];

            heroEnFume[0] = new GameObject(hero.position.X,
                hero.position.Y, false);
            heroEnFume[0].sprite = Content.Load<Texture2D>("Hero/fume.png");
            heroEnFume[0].angleRotation = hero.angleRotation - angle[20];

            heroEnFume[1] = new GameObject(hero.position.X,
               hero.position.Y, false);
            heroEnFume[1].sprite = Content.Load<Texture2D>("Hero/fume.png");

            heroEnFume[2] = new GameObject(hero.position.X,
               hero.position.Y, false);
            heroEnFume[2].sprite = Content.Load<Texture2D>("Hero/fume.png");
            heroEnFume[2].angleRotation = hero.angleRotation + angle[20];
            
            // Le menu
            menu = new GameObject();
            menu.sprite = Content.Load<Texture2D>("Fond/menu.png");
            menu.estVivant = true;

            // Le sticker gameOver
            gameOverStamp = new GameObject();
            gameOverStamp.sprite = Content.Load<Texture2D>("Fond/gameOver.png");
            gameOverStamp.estVivant = false;

            // Le sticker youWin
            youWinStamp = new GameObject();
            youWinStamp.sprite = Content.Load<Texture2D>("Fond/youWin.png");
            youWinStamp.estVivant = false;

            // Le fond
            fond = new GameObject();
            fond.sprite = Content.Load<Texture2D>("Fond/fond.jpg");
            fond.position.X = hero.position.X;
            fond.position.Y = hero.position.Y;

            fond.origine.X = fond.sprite.Width / 2;
            fond.origine.Y = fond.sprite.Height / 2;

            // miniature
            //fond
            fondMiniature = new GameObject();
            fondMiniature.position.X = 0;
            fondMiniature.position.Y = 0;
            //héro
            heroMiniature = new GameObject();
            heroMiniature.sprite = Content.Load<Texture2D>("Miniature/Airplane_Green.png");
            heroMiniature.position.X = fond.origine.X * 0.05f;
            heroMiniature.position.Y = fond.origine.Y * 0.05f;
            heroMiniature.origine.X = heroMiniature.sprite.Width / 2;
            heroMiniature.origine.Y = heroMiniature.sprite.Height/ 2;

            


                // Les nuages
                nuage = new GameObject[10];
            for (int i = 0; i < 10; i++)
            {
                nuage[i] = new GameObject(nombre.Next(0 - (fenetre.Width * 2), fenetre.Width * 2),
                nombre.Next(0 - (fenetre.Height * 2), fenetre.Height * 2), true);
                nuage[i].sprite = Content.Load<Texture2D>("Fond/nuage.png");
                nuage[i].tag = "nuage";
            }

            // Les ennemis
            ennemi = new GameObject[16];
            missile = new GameObject[32];
            int missileIndividuel = 0;

            for (int i = 0; i < ennemi.Length; i++)
            {
                ennemi[i] = new GameObject(nombre.Next(0, fenetre.Width),
                nombre.Next(0, fenetre.Height), false);
                ennemi[i].sprite = Content.Load<Texture2D>("ennemis/ennemi" + (i + 1) + ".png");
                ennemi[i].origine.X = ennemi[i].sprite.Width / 2;
                ennemi[i].origine.Y = ennemi[i].sprite.Height / 2;

                //tag pour l'ennemi
                ennemi[i].tag = "ennemi";
                // 2 missiles pour chaque ennemi
                // Missile du haut
                missile[i * 2] = new GameObject(ennemi[i].position.X,
                    ennemi[i].position.Y, true);
                missile[i * 2].sprite = Content.Load<Texture2D>("missileEnnemi.png");
                missile[i * 2].origine.Y = missile[i * 2].sprite.Height / 2;
                missile[i * 2].origine.X = missile[i * 2].sprite.Width / 2;
                missile[i * 2].isLaunched = false;

                //              Missile du bas
                missile[(i * 2) + 1] = new GameObject(ennemi[i].position.X,
                    ennemi[i].position.Y, true);
                missile[(i * 2) + 1].sprite = Content.Load<Texture2D>("missileEnnemi.png");
                missile[(i * 2) + 1].origine.Y = missile[(i * 2) + 1].sprite.Height / 2;
                missile[(i * 2) + 1].origine.X = missile[(i * 2) + 1].sprite.Width / 2;
                missile[(i * 2) + 1].isLaunched = false;
            }

            //les ennemis miniatures
            ennemiMiniature = new GameObject[16];
            for (int i = 0; i < ennemiMiniature.Length; i++)
            {
                ennemiMiniature[i] = new GameObject();
                ennemiMiniature[i].position.X = ((fond.origine.X - (fenetre.Width/2) + ennemi[i].position.X) * 0.05f);
                ennemiMiniature[i].position.Y = ((fond.origine.Y - (fenetre.Height/2) + ennemi[i].position.Y) * 0.05f);


                ennemiMiniature[i].sprite = Content.Load<Texture2D>("Miniature/Airplane_Red.png");

                ennemiMiniature[i].origine.X = ennemiMiniature[i].sprite.Width / 2;
                ennemiMiniature[i].origine.Y = ennemiMiniature[i].sprite.Height / 2;
                ennemiMiniature[i].estVivant = false;
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
            
            tempsActuel = gameTime.TotalGameTime.Seconds;
            tempsEnMinutes = gameTime.TotalGameTime.Minutes;
            tempsPartie = gameTime.TotalGameTime.Seconds;
            if (tempsDebutGame == 0)
            {
                tempsDebutGame = tempsEnMinutes;
            }
            if (tempsActuel - tempsFrappe > 5 && gameOver == true)
            {
                menu.estVivant = true;
                gameOverStamp.estVivant = true;
            }
            if ((tempsEnMinutes - tempsDebutGame) >= 5)
            {
                menu.estVivant = true;
                youWinStamp.estVivant = true;
            }
           
                  

            if (menu.estVivant == true)
            {
                 if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    ResetGame();
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();

                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    // j'ai abandonné le projet de rotation car j'avais du mal à 
                    // Faire rotationner les ennemis autour de moi.
                    UpdateRotationVitesse(gameTime);
                   
                }

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    if (fond.vitesse.X > 0)
                    {
                        fond.vitesse.X -= 0.07f;
                    }
                    else
                    {
                        fond.vitesse.X += 0.07f;
                    }

                    if (fond.vitesse.Y > 0)
                    {
                        fond.vitesse.Y -= 0.07f;
                    }
                    else
                    {
                        fond.vitesse.Y += 0.07f;
                    }


                    UpdateNuages(2);
                }
                //                if (Keyboard.GetState().IsKeyDown(Keys.A))
                //                {
                //                    fond.origine.X -= 15;
                //
                //                    fond.vitesse.X -= 0.1f;
                //                    
                //                    UpdateNuages(3);
                //                }
                //
                //                if (Keyboard.GetState().IsKeyDown(Keys.D))
                //                {
                //
                //                    fond.origine.X -= 15;
                //                    fond.vitesse.X += 0.1f;
                //                    
                //                    UpdateNuages(4);
                //                 }

                // Rotation de l'écran
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    hero.angleRotation += -0.05f;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    hero.angleRotation += 0.05f;


                }
                fond.origine += fond.vitesse;
                //Les 16 ennemis
                for (int i = 0; i < 16; i++)
                {
                    //int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].position += ennemi[i].vitesse;
                    }
                }


                // TODO: Add your update logic here
                //UpdateMaquette();
                UpdateHero();
                UpdateEnnemi();
                UpdateMissiles();
                UpdateFond();
                UpdateMissiles(gameTime);

                UpdateEnnemi(gameTime);
                UpdateHero(gameTime);
                base.Update(gameTime);
            }
            

           
        }

        public void UpdateRotationVitesse(GameTime gametime)
        {
            if (hero.angleRotation >= angle[0] && hero.angleRotation < angle[45])
            {
                fond.ThisRotation(angle[0], angle[45], "xHighRightYno");
                ////                Les 16 ennemis
                ////                Éventuellement je vais faire traverser mes ennemis vivant au début 
                ////                du tableau et sorter seulement les premiers

                for (int i = 0; i < 16; i++)
                    {
                        int missileIndividuel = 0;
                        if (ennemi[i].estVivant)
                        {
                            ennemi[i].ThisRotation(angle[0], angle[45], "xHighRightYno");
                        }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[0], angle[45], "xHighRightYno");
                }

            }
            else if (hero.angleRotation >= angle[45] && hero.angleRotation < angle[60])
            {
                fond.ThisRotation(angle[45], angle[60], "xSlowRightYSlowDown");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[45], angle[60], "xSlowRightYSlowDown");
                    }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[45], angle[60], "xSlowRightYSlowDown");
                }
            }
            else if (hero.angleRotation >= angle[60] && hero.angleRotation < angle[135])
            {
                fond.ThisRotation(angle[60], angle[135], "xNoYHighDown");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[60], angle[135], "xNoYHighDown");
                    }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[60], angle[135], "xNoYHighDown");
                }
            }
            else if (hero.angleRotation >= angle[135] && hero.angleRotation < angle[150])
            {
                fond.ThisRotation(angle[135], angle[150], "xSlowLeftYSlowDown");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[135], angle[150], "xSlowLeftYSlowDown");
                    }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[135], angle[150], "xSlowLeftYSlowDown");
                }

            }
            else if (hero.angleRotation >= angle[150] && hero.angleRotation < angle[225])
            {
                fond.ThisRotation(angle[150], angle[225], "xHighLeftYNo");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers

               
                    for (int i = 0; i < 16; i++)
                    {
                        int missileIndividuel = 0;
                        if (ennemi[i].estVivant)
                        {
                            ennemi[i].ThisRotation(angle[150], angle[225], "xHighLeftYNo");
                        }
                    }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[150], angle[225], "xHighLeftYNo");
                }

            }
            else if (hero.angleRotation >= angle[225] && hero.angleRotation < angle[240])
            {
                fond.ThisRotation(angle[225], angle[240], "xSlowLeftYSlowUp");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[225], angle[240], "xSlowLeftYSlowUp");
                    }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[225], angle[240], "xSlowLeftYSlowUp");
                }
            }
            else if (hero.angleRotation >= angle[240] && hero.angleRotation < angle[315])
            {
                fond.ThisRotation(angle[240], angle[315], "xNoYHighUp");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[240], angle[315], "xNoYHighUp");
                        
                    }

                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[240], angle[315], "xNoYHighUp");
                }
                
            }
            else if (hero.angleRotation >= angle[315] && hero.angleRotation < angle[330])
            {
                fond.ThisRotation(angle[315], angle[330], "xSlowLeftYSlowUp");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[315], angle[330], "xSlowRightYSlowUp");
                    }
                }
                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[315], angle[330], "xSlowRightYSlowUp");
                }
            }
            // Nouvelle condition car elle existe aussi dans les conditions précédentes
            if (hero.angleRotation >= angle[330] && hero.angleRotation < angle[359])
            {
                fond.ThisRotation(angle[330], angle[359], "xHighRightYNo1");
                //                Les 16 ennemis
                //                Éventuellement je vais faire traverser mes ennemis vivant au début 
                //                du tableau et sorter seulement les premiers
                for (int i = 0; i < 16; i++)
                {
                    int missileIndividuel = 0;
                    if (ennemi[i].estVivant)
                    {
                        ennemi[i].ThisRotation(angle[330], angle[359], "xHighRightYNo1");
                    }
                }

                for (int i = 0; i < nuage.Length; i++)
                {
                    nuage[i].ThisRotation(angle[330], angle[359], "xHighRightYNo1");
                }
            }
        }

        public void UpdateFond()
        {
            if (fond.origine.X < 0 + fenetre.Width/2)
            {
                fond.origine.X = (fenetre.Width/2);
                // On tourne pour éviter l'arrêt
                if (hero.angleRotation>=angle[180])
                {
                     hero.angleRotation++;
                }
                else
                {
                    hero.angleRotation--;
                }
               
                // On réduit la vitesse
                fond.vitesse.X = 0.03f;
            }
            else if (fond.origine.X > fond.sprite.Width - (fenetre.Width / 2))
            {
                fond.origine.X = fond.sprite.Width - (fenetre.Width / 2);
                // On tourne pour éviter l'arrêt
                if (hero.angleRotation >= angle[0] && hero.angleRotation < angle[90])
                {
                    hero.angleRotation++;
                }
                else if (hero.angleRotation > angle[270] && hero.angleRotation<angle[359])
                {
                    hero.angleRotation--;
                }
                // On réduit la vitesse
                fond.vitesse.X = -0.03f;
            }
            if (fond.origine.Y < 0 + (fenetre.Height / 2))
            {
                fond.origine.Y = (fenetre.Height / 2);
                // On tourne pour éviter l'arrêt
                if (hero.angleRotation >= angle[270] && hero.angleRotation < angle[359])
                {
                    hero.angleRotation++;
                }
                else if (hero.angleRotation >= angle[180] && hero.angleRotation < angle[270])
                {
                    hero.angleRotation--;
                }
                // On réduit la vitesse
                fond.vitesse.Y = +0.03f;
            }
            else if (fond.origine.Y > fond.sprite.Height - (fenetre.Height / 2))
            {
                fond.origine.Y = fond.sprite.Height - (fenetre.Height / 2);
                // On tourne pour éviter l'arrêt
                if (hero.angleRotation >= angle[90] && hero.angleRotation < angle[180])
                {
                    hero.angleRotation++;
                }
                else if (hero.angleRotation >= angle[0] && hero.angleRotation < angle[90])
                {
                    hero.angleRotation--;
                }
                // On réduit la vitesse
                fond.vitesse.Y = -0.03f;
            }

           
        }

        public void UpdateHero()
        {
            //hero.InScreen(fenetre,limiteMinY,limiteMaxY,limiteMinX, limiteMaxX);
            // Rétablir le compteur de rotation après un tour complet
            if (hero.angleRotation >= angle[359])
            {
                hero.angleRotation = angle[0];
            }
            else if (hero.angleRotation <= angle[0])
            {
                hero.angleRotation = angle[359];
            }
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
                    ennemiMiniature[ennemiAleatoire].estVivant = true;

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
                ennemi[i].InMaquette(fond.sprite.Bounds,fenetre,fond.origine.X,fond.origine.Y);
                float[] progression = new float[4];
                float quelleProgression = 0;
                float maxVitesse = 1f;
                progression[0] =  0.03f; 
                progression[1] = 0.02f;
                progression[2] = -0.01f;
                progression[3] = -0.02f;
                quelleProgression = progression[nombre.Next(0,4)];

                if (ennemi[i].position.X > hero.position.X)
                {

                    ennemi[i].vitesse.X -= quelleProgression;
                    ennemi[i].angleRotation = 3.1416f;
                }
                else
                {

                    ennemi[i].vitesse.X += quelleProgression;
                    ennemi[i].angleRotation = 0f;
                }
                // on rebrasse en Y
                quelleProgression = progression[nombre.Next(0, 4)];
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
//              par rapport au héro si le missile n'est pas launché
                
                if (ennemi[i].position.X > hero.position.X)
                {
                    if (!missile[i * 2].isLaunched)
                    {
                        missile[i*2].position.X = ennemi[i].position.X - 10;
                        missile[i * 2].position.Y = ennemi[i].position.Y -50;
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
                        missile[i * 2].position.Y = ennemi[i].position.Y +50;
                        missile[i*2].angleRotation = ennemi[i].angleRotation;
                    }
                    if (!missile[(i*2)+1].isLaunched)
                    {
                        missile[i * 2 + 1].position.X = ennemi[i].position.X + 10;
                        missile[(i * 2) + 1].position.Y = ennemi[i].position.Y - 20;
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
                // Est-ce que l'ennemi est dans l'éran
                ennemi[ennemiQuiTire].EnnemiDansEcran(fenetre);

                // Est-ce que l'ennemi est vivant et son missile non lancé
                if (ennemi[ennemiQuiTire].estVivant && 
                    !missile[ennemiQuiTire * 2].isLaunched &&
                    ennemi[ennemiQuiTire].dansEcran == true)
                {
                    
                    // Lancer un missile
                    if (ennemi[ennemiQuiTire].position.X > hero.position.X)
                    {
                        // Dire au programme que ce missile est lancé
                        missile[ennemiQuiTire * 2].isLaunched = true;
                        missile[ennemiQuiTire * 2].vitesse.X = -15 ;
                        missileEnCourse++;
                    }
                    else
                    {
                        // Vérifier que l'avion est dans la fentre pour lancer
                        if (hero.position.X - ennemi[ennemiQuiTire].position.X < fenetre.Width / 2 &&
                            hero.position.Y - ennemi[ennemiQuiTire].position.Y < fenetre.Height / 2)
                        {
                            missile[ennemiQuiTire * 2].vitesse.X = +15 ;
                            missileEnCourse++;
                        }

                    }
                    
                    
                   
                }
            }
            //Si le nombre de missile ne peut se permettre un missile supplémentaire
            else
            {

                //Donner l'accélération au missile 
                if (missile[ennemiQuiTire * 2].isLaunched)
                {
                    missile[ennemiQuiTire * 2].position.X
                        += missile[ennemiQuiTire * 2].vitesse.X;
                    // Garder le missile à la même vitesse en Y que le fond
                    missile[ennemiQuiTire * 2].position.Y += - 30 /100 * fond.vitesse.Y; // facteur de vitesse dans This.RotationVitesse (GO)
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
            // Ajouter un ennemi à tous les 20 secondes
            tempsDernierAjoutEnnemi = gameTime.TotalGameTime.TotalMilliseconds;
            if (tempsDernierAjoutEnnemi % 20000 <= 1)
            {
                nombreEnnemiMax++;
                nbMissilesSup++;
                tempsDernierAjoutEnnemi = 0;
            }
        }

        public void UpdateMissiles(GameTime gameTime)
        {

            tempsActuel  = gameTime.TotalGameTime.Seconds;
            if (gameOver == true && tempsFrappe == 0)
            {
                tempsFrappe = tempsActuel;
            }
           
        }

        public void ResetGame()
        {
            tempsDebutGame = 0;
            menu.estVivant = false;
            gameOverStamp.estVivant = false;
            youWinStamp.estVivant = false;
            gameOver = false;
            tempsActuel = 0;
            tempsPartie = 0;
            tempsFrappe = 0;
            nombreEnnemisVivant = 0;
            nombreEnnemiMax = 3;
            heroEnFume[0].estVivant = false;
            heroEnFume[1].estVivant = false;
            heroEnFume[2].estVivant = false;
            for (int i = 0; i < ennemi.Length; i++)
            {
                ennemi[i].estVivant = false;
                ennemiMiniature[i].estVivant = false;
                missile[i].isLaunched = false;
                missile[i * 2].isLaunched = false;

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
            // Le menu
            if (menu.estVivant == true)
            {
                spriteBatch.Draw(menu.sprite, new Rectangle(0,0,fenetre.Width, 
                    fenetre.Height), Color.White);
                if (gameOverStamp.estVivant == true)
                {
                    spriteBatch.Draw(gameOverStamp.sprite, gameOverStamp.position, 
                        null, Color.White, gameOverStamp.angleRotation, 
                        gameOverStamp.origine, 1.0f, SpriteEffects.None, 0f);
                }
                if (youWinStamp.estVivant == true)
                {
                    spriteBatch.Draw(youWinStamp.sprite, youWinStamp.position,
                        null, Color.White, youWinStamp.angleRotation,
                        youWinStamp.origine, 1.0f, SpriteEffects.None, 0f);
                }
            }
            else
            {
                //  Le fond
                spriteBatch.Draw(fond.sprite, fond.position, null,  Color.White,
                 fond.angleRotation,fond.origine,1.0f, SpriteEffects.None,0f);

                // Le fond miniature
                spriteBatch.Draw(fond.sprite, fondMiniature.position, null, 
                    Color.White, fond.angleRotation, fondMiniature.origine, 
                    0.050f, SpriteEffects.None, 0f);
                // Le héro miniture
                spriteBatch.Draw(heroMiniature.sprite, (fond.origine * 0.05f), 
                    null, Color.White,hero.angleRotation, heroMiniature.origine, 
                    0.1f, SpriteEffects.None, 0f);

                // Les ennemis miniatures
                for (int i = 0; i < 16; i++)
                {
                    if (ennemiMiniature[i].estVivant)
                    {
                        ennemiMiniature[i].position.X = (ennemi[i].position.X +
                            fond.origine.X - (fenetre.Width / 2)) * 0.05f;
                        ennemiMiniature[i].position.Y = (ennemi[i].position.Y +
                            fond.origine.Y - (fenetre.Height / 2)) * 0.05f;

                        spriteBatch.Draw(ennemiMiniature[i].sprite, 
                            ennemiMiniature[i].position, null, Color.White,
                            ennemi[i].angleRotation, ennemiMiniature[i].origine, 
                            0.2f, SpriteEffects.None, 0f);
                    }
                }

                // Le barre
                spriteBatch.Draw(barre.sprite, barre.position, null, Color.White,
                fond.angleRotation, barre.origine, 2.0f, SpriteEffects.None, 0f);

                // Le rapporteur
                spriteBatch.Draw(rapporteur.sprite, rapporteur.position, null,
                    Color.White, rapporteur.angleRotation, rapporteur.origine, 
                    2.0f, SpriteEffects.None, 0f);

                // LEs 10 nuages
                for (int i = 0; i < 10; i++)
                {
                    spriteBatch.Draw(nuage[i].sprite, nuage[i].position, null, 
                        Color.White,nuage[i].angleRotation, nuage[i].origine, 
                        1.0f, SpriteEffects.None, 0f);
                }

                // Le héro

                spriteBatch.Draw(hero.sprite, hero.position, null, Color.White,
                    hero.angleRotation, hero.origine, 1f, SpriteEffects.None, 0f);
                // Le feu si frappé
                for (int i = 0; i < heroEnFume.Length; i++)
                {
                    if (heroEnFume[i].estVivant)
                    {
                        spriteBatch.Draw(heroEnFume[i].sprite, 
                            heroEnFume[i].position, null, Color.White,
                            hero.angleRotation, hero.origine, 0.4f, 
                            SpriteEffects.None, 1.0f);
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
                        spriteBatch.Draw(missile[i * 2].sprite, missile[i * 2].position, null, Color.White,
                            ennemi[i].angleRotation, ennemi[i].origine, 0.5f,
                            SpriteEffects.None, 0f);
                        spriteBatch.Draw(missile[(i * 2) + 1].sprite, missile[(i * 2) + 1].position, null, Color.White,
                            ennemi[i].angleRotation, ennemi[i].origine, 0.5f,
                            SpriteEffects.None, 0f);
                    }

                }

                // Écrire un texte duant la game
                //            if (!gameOver)
                //            {
                //                spriteBatch.DrawString(font, message, new Vector2(100, 100), Color.Black);
                //            }

                // Écrire le message du Game Over
                //            else if (tempsActuel - tempsFrappe > 5 && gameOver == true) //  si 5 seconde se sont écoulés depuis gameOver
                //            {
                //message = "Temps de la partie : " + tempsActuel +  " secondes \nVotre rotation est de " + hero.angleRotation + "( en float )";
                if (tempsPartie < 10)
                {
                    message = "Votre temps de jeu = 0" + tempsEnMinutes + ":0"
                    + tempsPartie + ")";
                }
                else
                {
                    message = "Votre temps de jeu = 0" + tempsEnMinutes + ":" 
                    + tempsPartie + "(Debut : " + tempsDebutGame  + ")";
                }
                
                message += "\n" + nombreEnnemiMax +
                    ") ennemis en jeu";

                spriteBatch.DrawString(font, message, new Vector2(fenetre.Width/2, 50), Color.Black);
                //Exit();
                //            }

            }




            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
