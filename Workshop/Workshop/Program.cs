using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Workshop
{
    class Program
    {
        private static Mutex mutexWS = new Mutex();
        private static Mutex mutexWS_job = new Mutex();
        private const int NumberOfRepetitions = 10000;
        static string name = "Personne n°";
        static int nbplacesoccupé = 0;
        static object __lock = new object();
        static bool __lockTaken = false;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("Veuillez entrer un argument");
                return;
            }
            mutexWS.WaitOne();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            switch (args[0])
            {
                case "1":
                    Console.WriteLine("Question 1:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_1(name + i); }
                    break;
                case "2":
                    Console.WriteLine("Question 2:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_2(name + i); }
                    break;
                case "3":
                    Console.WriteLine("Question 3:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_3(name + i); }
                    break;
                case "4":
                    Console.WriteLine("Question 4:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_4(name + i, i); }
                    break;
                case "5":
                    Console.WriteLine("Question 5:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_5(name + i); }
                    break;
                case "6":
                    Console.WriteLine("Question 6:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_6(name + i); }
                    break;
                case "7":
                    Console.WriteLine("Question 7:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_7(name + i); }
                    break;
            }
            mutexWS.ReleaseMutex();
            stopwatch.Stop();
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

        static public void A_5_2(string nom) // Fonction avec protection de la variable nbplacesoccupé grâce à interlocked
        {
            Console.WriteLine($"{nom} rentre, {Interlocked.Increment(ref nbplacesoccupé)} places occupées");
            Console.WriteLine($"{nom} sort,   {Interlocked.Decrement(ref nbplacesoccupé)} places occupées");
        }

        static public void A_6(string nom) // Fonction avec protection de la variable nbplacesoccupé grâce à lock
        {
            // Si le lock est déjà pris, on attend
            Thread t = new Thread(() => A_6_2(nom));
            while (__lockTaken) { }
            t.Start();
        }

        static public void A_6_2(string nom) // Fonction avec protection de la variable nbplacesoccupé grâce à lock
        {
            lock (__lock)
            {
                __lockTaken = true;
                Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
                Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
            }
            __lockTaken = false;
        }

        static public void A_7(string nom) // Fonction avec le mutex
        {
            Thread t = new Thread(() => A_7_2(nom));
            t.Start();
        }

        static public void A_7_2(string nom)
        {
            mutexWS_job.WaitOne();
            Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
            Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
            mutexWS_job.ReleaseMutex();
        }

    }
}