using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Projet_02
{
    class GameObject
    {
        public Texture2D sprite;
        public bool estVivant;
        public bool isLaunched;
        public bool estTireur;
        public bool directionChange;
        public bool dansEcran;
        public float angleRotation;
        public string tag;

        public Vector2 position;
        public Vector2 origine;
        public Vector2 vitesse;

        Rectangle collisionHappen = new Rectangle();

        public GameObject(float positionx, float positiony, bool estVivant)
        {
            this.position.X = positionx;
            this.position.Y = positiony;
            this.estVivant = estVivant;
        }

        public GameObject() // Utile pour une image de fond ou une image 
            //pleine largeur
        {

        }

        public Rectangle GetRect()
        {
            collisionHappen.X = (int)this.position.X - (int)this.origine.X;
            collisionHappen.Y = (int) this.position.Y - (int)this.origine.Y;
            collisionHappen.Height = this.sprite.Height;
            collisionHappen.Width = this.sprite.Width;
            return collisionHappen;
        }

        public void InMaquette(Rectangle cetteMaquette, Rectangle cetteFenetre, 
            float margeX, float margeY)
        {
            float facteurPermis = 1f;
            float minX, maxX, minY, maxY;
            // On sort de l'écran en bas d'un facteur de 1
            minX = (cetteFenetre.Width/2)-margeX ;
            maxX = cetteMaquette.Width - margeX + cetteFenetre.Width/2;
            minY = (cetteFenetre.Height / 2)-margeY;
            maxY = cetteMaquette.Height - margeY + cetteFenetre.Height/2;
            if (this.position.X <  minX)
            {
                this.position.X =  minX;
                this.vitesse.X = 0;
            }
            else if (this.position.X  > maxX)
            {
                this.position.X = maxX;
                this.vitesse.X = 0;
            }

            if (this.position.Y < minY)
            {
                this.position.Y =minY;
                this.vitesse.Y = 0;
            }
            else if (this.position.Y > maxY)
            {
                this.position.Y = maxY;
                this.vitesse.Y = 0;
            }
        }
        public void InScreen(Rectangle cetteFenetre)
        {
            float facteurPermis = 1f;
            float minX, maxX, minY, maxY;
            // On sort de l'écran en bas d'un facteur de 1
            minX = 0 - cetteFenetre.Width * (facteurPermis - 1);
            maxX = cetteFenetre.Width * facteurPermis;
            minY = 0 - cetteFenetre.Height * (facteurPermis - 1);
            maxY = cetteFenetre.Height * facteurPermis;
            if (this.position.X < minX)
            {
                this.position.X = minX;
                this.vitesse.X = 0;
            }
            else if (this.position.X > maxX)
            {
                this.position.X = maxX;
                this.vitesse.X = 0;
            }

            if (this.position.Y < minY)
            {
                this.position.Y = minY;
                this.vitesse.Y = 0;
            }
            else if (this.position.Y > maxY)
            {
                this.position.Y = maxY;
                this.vitesse.Y = 0;
            }
        }
        public void EnnemiDansEcran(Rectangle cetteFenetre)
        {
            if (this.position.X > 0 && this.position.X < cetteFenetre.Width )
            {
                if (this.position.Y > 0 && this.position.Y < cetteFenetre.Height)
                {
                    this.dansEcran = true;
                }
            }
        }
        public void ThisRotation(float min ,float max, String vitesse)
        {
            float vitesseLente = 0.03f;
            float vitesseRapide = 0.07f;
            int pCentEnnemi = 30; // pourcentage l'ennemi en rapport avec le fond

            switch (vitesse)
            {
                case "xHighLeftYNo":
                    if (this.tag == "ennemi")
                    {
                            this.vitesse.X += (vitesseRapide * pCentEnnemi / 100); 
                    }
                    else
                    {

                        this.vitesse.X -= vitesseRapide;
                    }
                    break;
                case "xNoYHighUp":
                    if (this.tag == "ennemi")
                    {
                        this.vitesse.Y += (vitesseRapide*pCentEnnemi/100);
                    }
                    else
                    {
                       
                        this.vitesse.Y -= vitesseRapide;
                    }
                   break; 
                case "xSlowLeftYSlowUp":
                    if (this.tag == "ennemi")
                    {
                        this.vitesse.X += vitesseLente * pCentEnnemi / 100;
                        this.vitesse.Y += vitesseLente * pCentEnnemi / 100;
                        
                    }
                    else
                    {
                        this.vitesse.X -= vitesseLente;
                        this.vitesse.Y -= vitesseLente;
                    }
                    break;
                case "xSlowLeftYNo":
                    if (this.tag == "ennemi")
                    {
                        this.vitesse.X += vitesseLente * pCentEnnemi / 100;
                       
                    }
                    else
                    {
                        this.vitesse.X -= vitesseLente;
                    }
                    break;
                case "xSlowLeftYSlowDown":
                    if (this.tag == "ennemi")
                    {
                        this.vitesse.X += vitesseLente * pCentEnnemi / 100;
                        this.vitesse.Y -= vitesseLente * pCentEnnemi / 100;
                    }
                    else
                    {
                        this.vitesse.X -= vitesseLente;
                        this.vitesse.Y += vitesseLente;
                    }
                    break;
                case "xNoYHighDown":
                    if (this.tag == "ennemi")
                    {
                        
                        this.vitesse.Y -= vitesseRapide * pCentEnnemi / 100;

                    }
                    else
                    {
                        
                        this.vitesse.Y += vitesseRapide;
                    }
                    break;
                case "xSlowRightYSlowDown":
                    if (this.tag == "ennemi")
                    {

                        this.vitesse.X -= vitesseLente * pCentEnnemi / 100;
                        this.vitesse.Y -= vitesseLente * pCentEnnemi / 100;

                    }
                    else
                    {

                        this.vitesse.X += vitesseLente;
                        this.vitesse.Y += vitesseLente;
                    }
                    break;
                case "xHighRightYno":
                    if (this.tag == "ennemi")
                    {
                      
                            this.vitesse.X -= (vitesseRapide * pCentEnnemi / 100);
                
                    }
                    else
                    {

                        this.vitesse.X += vitesseRapide;

                    }
                    break;
                case "xHighRightYNo1":
                    if (this.tag == "ennemi")
                    {

                        this.vitesse.X -= (vitesseRapide * pCentEnnemi / 100);

                    }
                    else
                    {

                        this.vitesse.X += vitesseRapide;

                    }
                    break;
                case "xSlowRightYSlowUp":

                    if (this.tag == "ennemi")
                    {

                        this.vitesse.X -= vitesseLente * pCentEnnemi / 100;
                        this.vitesse.Y += vitesseLente * pCentEnnemi / 100;

                    }
                    else
                    {

                        this.vitesse.X += vitesseLente;
                        this.vitesse.Y -= vitesseLente;
                    }
                    break;


            }


            //if (this.angleRotation >= min && this.angleRotation < max)
            //{
            //    if (this.tag == "ennemi")
            //    {
            //        this.vitesse.Y += vitesseY - (vitesseY * 80 / 100);
            //        this.vitesse.X += vitesseX - (vitesseX * 80 / 100);
            //    }
            //    else
            //    {
            //        this.vitesse.Y += vitesseY;
            //        this.vitesse.X += vitesseX;
            //    }
                    
            //}
        }

    }
}
