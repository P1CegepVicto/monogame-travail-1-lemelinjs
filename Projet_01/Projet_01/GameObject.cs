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


        public GameObject(int positionX, int positionY, int origineX, 
            int origineY, bool estVivant)
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
        public GameObject(int positionX, int positionY, bool estVivant)
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


        public void Position(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }


        public int Width { get; internal set; }
        public int Height { get; internal set; }
    }
}
