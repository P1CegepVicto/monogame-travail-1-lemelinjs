using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool directionChange;
        public float angleRotation;

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
            collisionHappen.X = (int)this.position.X;
            collisionHappen.Y = (int) this.position.Y;
            collisionHappen.Height = this.sprite.Height;
            collisionHappen.Width = this.sprite.Width;
            return collisionHappen;
        }

        public void InScreen(Rectangle cetteFenetre)
        {
            if (this.position.X - (this.origine.X) < 0)
            {
                this.position.X = 0 + (this.origine.X);
            }
            else if (this.position.X + (this.origine.X) > cetteFenetre.Width)
            {
                this.position.X = cetteFenetre.Width - this.origine.X;
            }

            if (this.position.Y - this.origine.Y < 0)
            {
                this.position.Y = 0 + this.origine.Y;
            }
            else if(this.position.Y + this.origine.Y > cetteFenetre.Height)
            {
                this.position.Y = cetteFenetre.Height - this.origine.Y;
            }
        }

    }
}
