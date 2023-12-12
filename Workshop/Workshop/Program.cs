using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Workshop
{
    class Program
    {
        private static Mutex mutexWS = new Mutex(); // Mutex pour le workshop
        private static Mutex mutexWS_job = new Mutex(); // Mutex pour la question 7

        private static Semaphore semaphoreWS = new Semaphore(1, 50); // Semaphore pour le workshop
        private static Semaphore semaphoreWS_job = new Semaphore(1, 1); // Semaphore pour la question 8

        static Barrier barrier = new Barrier(1); // Barrière pour la question 9

        // ManualResetEvent Class pour la question 10
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        static object __lock = new object(); // Lock pour la question 6
        static bool __lockTaken = false; // Booléen pour savoir si le lock est pris ou non

        private const int NumberOfRepetitions = 5; // Nombre de répétitions pour chaque question
        static string name = "Personne n°"; // Nom des personnes
        static int nbplacesoccupé = 0; // Nombre de places occupées
        static readonly int nbplacesmax = 50; // Nombre de places

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
                case "8":
                    Console.WriteLine("Question 8:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_8(name + i); }
                    break;
                case "9":
                    Console.WriteLine("Question 9:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_9(name + i); }
                    break;
                case "10":
                    Console.WriteLine("Question 10:");
                    for (int i = 0; i < NumberOfRepetitions; i++) { A_10(name + i); }
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

        static public void A_7_2(string nom) //Fonction avec le mutex
        {
            mutexWS_job.WaitOne();
            Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
            Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
            mutexWS_job.ReleaseMutex();
        }

        static public void A_8(string nom) // Fonction avec le semaphore
        {
            Thread t = new Thread(() => A_8_2(nom));
            t.Start();
        }

        static public void A_8_2(string nom) // Fonction avec le semaphore
        {
            semaphoreWS_job.WaitOne();
            Console.WriteLine($"{nom} rentre, {++nbplacesoccupé} places occupées");
            Console.WriteLine($"{nom} sort,   {--nbplacesoccupé} places occupées");
            semaphoreWS_job.Release();
        }

        static public void A_9(string nom) // Fonction avec la barrière
        {
        }

        static public void A_9_2(string nom)
        {
        }

        static public void A_10(string nom)
        {
            Thread t = new Thread(() => A_10_2(nom));
            t.Start();
            Console.WriteLine($"{nom} signale le ManualResetEvent");
            manualResetEvent.Set();
        }

        static public void A_10_2(string nom)
        {
            Console.WriteLine($"{nom} attend le ManualResetEvent");
            manualResetEvent.WaitOne();
            Console.WriteLine($"{nom} a été libéré par le ManualResetEvent");
        }

    }
}