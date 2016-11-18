using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Projet_01
{
    
class GameObject
    {
        public bool estVivant;
        public Texture2D sprite;
        public Vector2 position;
        public Vector2 origine;
        public Vector2 vitesse;
        public float rotationAngle;
        public bool changementDirection;

        public Rectangle rectColision = new Rectangle();


        public GameObject(float positionX, float positionY, float origineX, 
            float origineY, bool estVivant)
        {
            //this.sprite = new Game1().Content.Load<Texture2D>(lienImage); //encore en erreur
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = estVivant;
        }
        public GameObject()
        {
            //this.sprite = new Game1().Content.Load<Texture2D>(lienImage); // Encore en erreur

        }
        public GameObject(float positionX, float positionY, bool estVivant)
        {
           // this.sprite = new Game1().Content.Load<Texture2D>(lienImage);//Encore en erreur
            this.position.X = positionX;
            this.position.Y = positionY;
            this.estVivant = true;
        }

        /// <summary>
        /// Pour connaitre l'endoit de la colision et la rencontre des corps
        /// </summary>
        /// <returns></returns>
        public Rectangle GetRect() //1 Pour sauver plusieurs lignes de code
        {
            rectColision.X = (int) this.position.X;
            rectColision.Y = (int) this.position.Y;
            rectColision.Height = this.sprite.Height;
            rectColision.Width = this.sprite.Width;

            return rectColision;
        }


        public void InScreen(Rectangle cetteFenetre)
        {
            //Gérer ici les déplacements en fonction de la fenêtre
            if ((this.position.Y) > (cetteFenetre.Height - (this.origine.Y)))
            {
                this.position.Y = (cetteFenetre.Height - (this.sprite.Height/2));
            }
            else if ((this.position.Y-(this.sprite.Height/2)< 0))
            {
                this.position.Y = 0 + (this.sprite.Height / 2);
            }
            if (this.position.X + (this.sprite.Width/2) > cetteFenetre.Width)
            {
                this.position.X = cetteFenetre.Width - (this.sprite.Width / 2);
            }
            else if (this.position.X - (this.sprite.Width/2)< 0)
            {
                this.position.X = 0 + (this.sprite.Width / 2);
            }
        }
        
    }
}
