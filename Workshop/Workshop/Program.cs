using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Workshop
{
    class Program
    {
        private const int NumberOfRepetitions = 1000;
        static string name = "Personne n°";
        static int nbplacesoccupé = 0;
        static DateTime debut;
        static DateTime fin;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Veuillez entrer un argument");
                return;
            }
            Stopwatch stopwatch = new Stopwatch();

            switch (args[0])
            {
                case "1":
                    Console.WriteLine("Question 1:");
                    stopwatch.Start();
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_1(name + i); }
                    stopwatch.Stop();
                    break;
                case "2":
                    Console.WriteLine("Question 2:");
                    stopwatch.Start();
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_2(name + i); }
                    stopwatch.Stop();
                    break;
                case "3":
                    Console.WriteLine("Question 3:");
                    stopwatch.Start();
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_3(name + i); }
                    stopwatch.Stop();
                    break;
                case "4":
                    Console.WriteLine("Question 4:");
                    stopwatch.Start();
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_4(name + i, i); }
                    stopwatch.Stop();
                    break;
                case "5":
                    Console.WriteLine("Question 5:");
                    stopwatch.Start();
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_5(name + i); }
                    stopwatch.Stop();
                    break;
            }
            Console.WriteLine("Temps d'exécution: " + stopwatch.Elapsed.TotalSeconds + " secondes");
        }

        static public void A_1(string nom) // Fonction de base
        {
            Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
            Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
        }

        static public void A_2(string nom) // Fonction avec thread
        {
            Thread t = new Thread(() => A_1(nom));
            t.Start();
        }

        static public void A_3(string nom) // Fonction avec thread et attente
        {
            Thread t = new Thread(() => A_1(nom));
            t.Start();
            t.Join();
        }

        static public void A_4(string nom, int id) // Fonction avec thread et attente pour les pairs
        {
            Thread t = new Thread(() => A_1(nom));
            t.Start();
            if (id % 2 == 0) { t.Join(); }
        }

        static public void A_5(string nom) // Fonction avec protection de la variable nbplacesoccupé
        {
            Thread t = new Thread(() => A_5_2(nom));
            t.Start();
        }

        static public void A_5_2(string nom) // Fonction avec protection de la variable nbplacesoccupé
        {
            Interlocked.Increment(ref nbplacesoccupé);
            Console.WriteLine($"{nom} rentre, {nbplacesoccupé} places occupées");
            Interlocked.Decrement(ref nbplacesoccupé);
            Console.WriteLine($"{nom} sort,   {nbplacesoccupé} places occupées");
        }

    }
}