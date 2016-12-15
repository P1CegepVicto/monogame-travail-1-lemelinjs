using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    class Program
    {
        static int[] motSecret;
        static int[] tabSecret;
        static int[] premier;
        static String msg = "";
        static void Main(string[] args)
        {
            int choix = 0;
            while (choix != 3)
            {
                Console.WriteLine("Voulez-vous 1- encrypter un message, 2-Décrypter un message? (3-quitter)");
                choix = int.Parse(Console.ReadLine());
                if (choix == 1)
                {
                    CrypteMessage();
                }
                else if (choix == 2)
                {
                    DecrypteMessage();
                }
            }


        }

        static void Premier()
        {
            // Trouver les n premiers nombres entiers en dessous de 100
            premier = new int[100];
            bool nombreOrdinaire = false;
            int compteur = 0;

            for (int j = 2; j < 100; j++)
            {
                for (int k = 2; k < j; k++)
                {

                    if ((j % k) == 0)
                    {
                        nombreOrdinaire = true;
                        k = j;
                    }

                }
                if (nombreOrdinaire == false)
                {
                    premier[compteur] = j;
                    compteur++;
                    if (compteur == msg.Length)
                    {
                        j = 100;
                    }

                }
                nombreOrdinaire = false;

            }
        }

        static void CrypteMessage()
        {
            //Déclaration
            
            int codeASCII = 0;

            Premier();

            Console.Write("Votre message: ");
            msg = Console.ReadLine();

            motSecret = new int[msg.Length];
            tabSecret = new int[msg.Length];
            
           
            
           

            for (int i = 0; i < msg.Length; i++)
            {
                codeASCII = (int)msg[i];
                motSecret[i] = codeASCII;
                tabSecret[i] = codeASCII - premier[i];
                Console.Write(tabSecret[i] + " " );
            }

            Console.ReadLine();
        }
        static void DecrypteMessage()
        {
            char lettre;
            int bonNombre = 0;
            for (int i = 0; i < tabSecret.Length; i++)
            {
                bonNombre = tabSecret[i]+premier[i];
                lettre = (char)bonNombre;
                Console.Write(lettre);
            }
            Console.WriteLine();
        }
    }
}
