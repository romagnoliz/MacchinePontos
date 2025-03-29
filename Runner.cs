using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MacchinePontos
{
    internal class Runner
    {
        private const int maxVeicoliPontos = 3;

        private static SemaphoreSlim bridgeSemaphore = new SemaphoreSlim(maxVeicoliPontos, maxVeicoliPontos);

        private Queue<string> leftQueue = new Queue<string>();
        private Queue<string> rightQueue = new Queue<string>();
        private bool isShipCrossing = false;

        private static string bridge = 
            $"                         -------------------------------\n\n\n\n" +
            $"                         -------------------------------";
        private static string ship = @"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

        public void Ponte()
        {
            Console.Clear();
            Console.WriteLine("==== Ponte Luca Pulga ====");
            Console.WriteLine("Cliccare uno dei seguenti tasti per eseguire un comando");
            Console.WriteLine("L → Aggiungi un'auto a sinistra");
            Console.WriteLine("R → Aggiungi un'auto a destra");
            Console.WriteLine("P → Avvia il passaggio delle auto");
            Console.WriteLine("S → Simula il passaggio di una nave");
            Console.WriteLine("E → Esce dal simulatore");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Auto a destra: " + string.Join(" ", rightQueue));
            Console.WriteLine("Auto a sinistra: " + string.Join(" ", leftQueue) + $"\n\n                                      PONTOS: \n" + (isShipCrossing ? "Nave in transito..." : bridge));
            Console.WriteLine();
        }
        public void Run()
        {

            bool running = true;

            while (running)
            {
                this.Ponte();
                var input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.L:
                        AggiungiMacchina("Sinistra");
                        break;
                    case ConsoleKey.R:
                        AggiungiMacchina("Destra");
                        break;
                    case ConsoleKey.P:
                        Passaggio();
                        break;
                    case ConsoleKey.S:
                        PassaggioBarca();
                        break;
                    case ConsoleKey.E:
                        running = false;
                        break;
                }
            }
        }

        private void AggiungiMacchina(string side)
        {
            if (side == "Sinistra")
                leftQueue.Enqueue("Aut sx ");
            else if (side == "Destra")
                rightQueue.Enqueue("Aut dx ");
        }

        private void Passaggio()
        {

            if (isShipCrossing)
            {
                Console.WriteLine("Attendere che la nave abbia attraversato il ponte.");
                return;
            }

            for (int i = 0; i < 3; i++) //faccio passare 3 nacchine alla volta
            {
                if (leftQueue.Count > 0 && leftQueue.Count >= rightQueue.Count)
                {
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine("Passaggio auto dalla sinistra...");
                    PassaggioMacchina("Sinistra");
                    Console.SetCursorPosition(10, 15);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(30, 15);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(50, 15);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(70, 15);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(90, 15);
                    Console.WriteLine("auto");
                    this.Ponte();
                }
                else if (rightQueue.Count > 0 && leftQueue.Count <= rightQueue.Count)
                {
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine("Passaggio auto dalla destra...");
                    PassaggioMacchina("Destra");
                    Console.SetCursorPosition(70, 14);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(50, 14);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(30, 14);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    this.Ponte();
                    Console.SetCursorPosition(10, 14);
                    Console.WriteLine("auto");
                    Thread.Sleep(500);
                    Console.SetCursorPosition(5, 14);
                    Console.WriteLine("auto");
                    this.Ponte();
                }
                else
                {
                    Console.WriteLine("Non c'è nessuna auto da far passare");
                }
            }
        }

        private void PassaggioMacchina(string side)
        {
            string car = side == "Sinistra" ? leftQueue.Dequeue() : rightQueue.Dequeue();
            Console.WriteLine($"Passaggio di {car}");


            bridgeSemaphore.Wait();
            Thread.Sleep(1000);
            bridgeSemaphore.Release();
        }

        private void PassaggioBarca()
        {
            if (isShipCrossing)
            {
                Console.WriteLine("La nave sta già attraversando il ponte.");
                return;
            }

            Console.WriteLine("Nave in transito...");
            isShipCrossing = true;


            Thread.Sleep(3000);
            isShipCrossing = false;
        }
    }
}
