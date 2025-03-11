namespace MacchinePontos;

class Program
{
    static void Main(string[] args)
    {
        var simulator = new DrawbridgeSimulator();
        simulator.Run();
    }

    class DrawbridgeSimulator
    {
        private const int maxVeicoliPontos = 3;
        static string pad = " ";

        private static SemaphoreSlim bridgeSemaphore = new SemaphoreSlim(maxVeicoliPontos, maxVeicoliPontos);
        private static SemaphoreSlim shipSemaphore = new SemaphoreSlim(0, 1);

        private Queue<string> leftQueue = new Queue<string>();
        private Queue<string> rightQueue = new Queue<string>();
        private bool isShipCrossing = false;

        private static string bridge = "\n----------------------------\n\n" + pad + "\n\n----------------------------";
        private static string ship = @"~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";

        public void Run()
        {
            bool running = true;

            while (running)
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

                Console.WriteLine("Auto a sinistra: " + string.Join(", ", leftQueue));
                Console.WriteLine("Auto a destra: " + string.Join(", ", rightQueue));
                Console.WriteLine("Ponte: " + (isShipCrossing ? "Nave in transito..." : bridge));
                Console.WriteLine();

                var input = Console.ReadKey(true).Key;

                switch (input)
                {
                    case ConsoleKey.L:
                        AddCarToQueue("Sinistra");
                        break;
                    case ConsoleKey.R:
                        AddCarToQueue("Destra");
                        break;
                    case ConsoleKey.P:
                        StartCrossingCars();
                        break;
                    case ConsoleKey.S:
                        SimulateShipCrossing();
                        break;
                    case ConsoleKey.E:
                        running = false;
                        break;
                }
            }
        }

        private void AddCarToQueue(string side)
        {
            if (side == "Sinistra")
                leftQueue.Enqueue("Aut sx");
            else if (side == "Destra")
                rightQueue.Enqueue("Aut dx ");
        }

        private void StartCrossingCars()
        {
            if (isShipCrossing)
            {
                Console.WriteLine("Attendere che la nave abbia attraversato il ponte.");
                return;
            }


            if (leftQueue.Count > 0 && leftQueue.Count >= rightQueue.Count)
            {
                Console.WriteLine("Passaggio auto dalla sinistra...");
                CrossCar("Sinistra");
            }
            else if (rightQueue.Count > 0 && leftQueue.Count <= rightQueue.Count)
            {
                Console.WriteLine("Passaggio auto dalla destra...");
                CrossCar("Destra");
            }
            else
            {
                Console.WriteLine("Non c'è nessuna auto da far passare");
            }
        }

        private void CrossCar(string side)
        {
            string car = side == "Sinistra" ? leftQueue.Dequeue() : rightQueue.Dequeue();
            Console.WriteLine($"Passaggio di {car}");


            bridgeSemaphore.Wait();
            Thread.Sleep(1000);
            bridgeSemaphore.Release();
        }

        private void SimulateShipCrossing()
        {
            if (isShipCrossing)
            {
                Console.WriteLine("La nave sta già attraversando il ponte.");
                return;
            }

            Console.WriteLine("Nave in transito...");
            isShipCrossing = true;
            shipSemaphore.Wait();


            Thread.Sleep(3000);
            isShipCrossing = false;
            shipSemaphore.Release();
        }
    }
}