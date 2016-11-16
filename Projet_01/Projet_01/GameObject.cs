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

        public Rectangle rectColision = new Rectangle();


        public GameObject(float positionX, float positionY, float origineX, 
            float origineY, bool estVivant)
        {
            //this.sprite = new Game1().Content.Load<Texture2D>(lienImage); //encore en erreur
            this.position.X = positionX;
            this.position.Y = positionY;
            this.origine.X = origineX;
            this.origine.Y = origineY;
            this.estVivant = true;
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


        public void Position(float x, float y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        public void InScreen(Rectangle fenetre)
        {
            //Gérer ici les déplacements en fonction de la fenêtre
            if ((this.position.Y) > (fenetre.Height - (this.Height/2)))
            {
                this.position.Y = (fenetre.Height - (this.Height/2));
            }
            else if ((this.position.Y-(this.Height/2)< 0))
            {
                this.position.Y = 0 + (this.Height / 2);
            }
            if (this.position.X + (this.Width/2) > fenetre.Width)
            {
                this.position.X = fenetre.Width - (this.Width / 2);
            }
            else if (this.position.X - (this.Width/2)< 0)
            {
                this.position.X = 0 + (this.Width / 2);
            }
        }


        public int Width { get; internal set; }
        public int Height { get; internal set; }
    }
}
