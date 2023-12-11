using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Workshop
{
    class Program
    {
        static int nbplacesoccupé = 0;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Veuillez entrer un argument");
                return;
            }

            switch (args[0])
            {
                case "1":
                    Console.WriteLine("Question 1:");
                    for (int i = 0; i < 100; i++) { A_1("Personne n°" + i); }
                    break;
                case "2":
                    Console.WriteLine("Question 2:");
                    for (int i = 0; i < 100; i++) { A_2("Personne n°" + i); }
                    break;
            }
        }

        static public void A_1(string nom)
        {
            Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
            Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
        }

        static public void A_2(string nom)
        {
            Thread t = new Thread(() => A_1(nom));
            t.Start();
        }
    }
}