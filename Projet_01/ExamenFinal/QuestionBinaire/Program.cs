using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionBinaire
{
    class Program
    {
        static bool[] octet = new bool[8];
        static String[] motPasseInvalide= new String[3];
        static String action = "";

        static void Main(string[] args)
        {
            //Déclaration
            String motDePasseUsager = "qwerty1234";
            string motDePasseSaisi = "";
            int tentative = 0;
            bool loginSuccess = false;


            //Comparaison des mots de passe
            for (int i = 0; i < 3 && loginSuccess == false; i++)
            {
                
                //Lire le mot de passe à l'écran
                Console.Clear();
                Console.Write("Votre mot de passe: ");
                motDePasseSaisi = Console.ReadLine();
                
                if (motDePasseSaisi != motDePasseUsager)
                {
                    motPasseInvalide[i] = motDePasseSaisi;
                }
                else
                {
                    loginSuccess = true;
                }
            }
            if (loginSuccess == false)
            {
                Console.Clear();
                Console.WriteLine("le Prgramme va fermer après 3 tentatives" +
                    "\nTentative 1: " + motPasseInvalide[0] +
                    "\nTentative 2: " + motPasseInvalide[1] +
                    "\nTentative 2: " + motPasseInvalide[2]);

                AfficherMenu();
                Console.ReadLine();
            }
            else
            {
                while (action != "quitter")
                {
                    AfficherMenu();
                }
                
            }




        }

        static void AfficherMenu()
        {
            //Déclaration
            int choix = 0;
            Console.Clear();
            Console.WriteLine("CHOIX = 1 - Lire un octet, 2 - afficher un octet, "+
                "3 - Traduire l'octet en décimal, 4 - Quitter");
            choix =int.Parse( Console.ReadLine());
            switch (choix)
            {
                case 1:
                    LireOctet();
                    break;
                case 2:
                    AfficherOctet();
                    break;
                case 3:
                    TraduireOctetDecimal();
                    break;
                case 4 :
                    action = "quitter";
                    break;

                default:
                    break;
            }
        }

        static void LireOctet()
        {
            //Déclaration
            String octetLu = "";
            
           
            
            Console.Write("Votre octet? (8 valeurs comprises entre 0 et 1): ");
            octetLu =Console.ReadLine();
           
            for (int i = 0; i < octet.Length; i++)
            {
                char lettre = octetLu[i];
                lettre = octetLu[i];
                if (lettre == '0')
                {
                    octet[i] = false;
                }
                else
                {
                    octet[i] = true;
                }

               // Console.WriteLine(octet[i]);
               // Console.ReadLine();

            }

        }

        static void AfficherOctet()
        {

            for (int i = 0; i < octet.Length; i++)
            {
                if (i== 4)
                {
                    Console.Write(" ");
                }
                if (octet[i])
                {
                    Console.Write("1");
                }
                else
                {
                    Console.Write("0");
                }
               
               
            }
            Console.ReadLine();

        }

        static void TraduireOctetDecimal()
        {
            //Déclaration
            double somme = 0;
            double valeur = 0;
            for (int i = 0; i <8; i++)
            {
                if (octet[i])
                {
                    valeur = 2;
          
                }
                else
                {
                    valeur = 0;
                }
                switch (i)
                {
                    case 0:
                        somme += Math.Pow(valeur,7);
                        break;
                    case 1:
                        somme += Math.Pow(valeur, 6);
                        break;
                    case 2:
                        somme += Math.Pow(valeur, 5);
                        break;
                    case 3:
                        somme += Math.Pow(valeur, 4);
                        break;
                    case 4:
                        somme += Math.Pow(valeur, 3);
                        break;
                    case 5:
                        somme += Math.Pow(valeur, 2);
                        break;
                    case 6:
                        somme += Math.Pow(valeur, 1);
                        break;
                    case 7:
                        somme += Math.Pow(valeur, 0);
                        break;
                }
                
            }
            Console.WriteLine("Réponse : " + somme);
            Console.ReadLine();
        }

    }
}
