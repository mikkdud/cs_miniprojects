using System;
using System.Collections.Generic;
using System.Threading;

// Reprezentuje dane generowane przez producentów
class DataItem
{
    public int ProducerId { get; set; }

    public DataItem(int producerId)
    {
        ProducerId = producerId;
    }
}

// Klasa reprezentująca producenta
class ProducerThread
{
    private int id; // identyfikator producenta
    private int delay; // maksymalne opóźnienie między produkcją danych
    private Queue<DataItem> queue; // współdzielona kolejka na dane
    private object lockObject; // obiekt używany do synchronizacji (sekcja krytyczna)
    private volatile bool running; // flaga sterująca zakończeniem wątku

    public Thread Thread { get; private set; } // właściwy wątek

    public ProducerThread(int id, int delay, Queue<DataItem> queue, object lockObject)
    {
        this.id = id;
        this.delay = delay;
        this.queue = queue;
        this.lockObject = lockObject;
        this.running = true;

        // tworzenie wątku i przypisanie metody Run jako punktu wejścia
        this.Thread = new Thread(Run);
    }

    public void Stop() => running = false; // metoda do zatrzymania wątku

    private void Run()
    {
        Random rand = new Random(id); // generator losowy (unikalny seed)
        while (running)
        {
            Thread.Sleep(rand.Next(delay)); // opóźnienie przed produkcją danych

            // sekcja krytyczna – tylko jeden wątek może naraz modyfikować queue
            lock (lockObject)
            {
                queue.Enqueue(new DataItem(id));
                Console.WriteLine($"[Producent {id}] wygenerował dane");
            }
        }

        Console.WriteLine($"[Producent {id}] zatrzymany.");
    }
}

// Klasa reprezentująca konsumenta
class ConsumerThread
{
    private int id; // identyfikator konsumenta
    private int delay; // maksymalne opóźnienie między próbami pobierania danych
    private Queue<DataItem> queue; // współdzielona kolejka z danymi
    private object lockObject; // obiekt do synchronizacji
    private volatile bool running; // flaga sterująca zakończeniem
    private Dictionary<int, int> consumedCount; // statystyka: ile danych pobrano od danego producenta

    public Thread Thread { get; private set; }

    public ConsumerThread(int id, int delay, Queue<DataItem> queue, object lockObject)
    {
        this.id = id;
        this.delay = delay;
        this.queue = queue;
        this.lockObject = lockObject;
        this.running = true;
        this.consumedCount = new Dictionary<int, int>();

        this.Thread = new Thread(Run);
    }

    public void Stop() => running = false;

    private void Run()
    {
        Random rand = new Random(id + 1000); // inny seed niż u producentów
        while (running)
        {
            Thread.Sleep(rand.Next(delay)); // odczekaj zanim podejmiesz próbę konsumpcji

            DataItem? item = null;

            // sekcja krytyczna – bezpieczne pobieranie z kolejki
            lock (lockObject)
            {
                if (queue.Count > 0)
                    item = queue.Dequeue(); // pobierz pierwszy element
            }

            if (item != null)
            {
                // aktualizacja statystyk
                if (!consumedCount.ContainsKey(item.ProducerId))
                    consumedCount[item.ProducerId] = 0;

                consumedCount[item.ProducerId]++;
                Console.WriteLine($"[Konsument {id}] pobrał dane od Producenta {item.ProducerId}");
            }
        }

        // Po zatrzymaniu wątku wypisz podsumowanie
        Console.WriteLine($"\n[Konsument {id}] zatrzymany. Statystyki:");
        foreach (var kv in consumedCount)
        {
            Console.WriteLine($"  → Producent {kv.Key} – {kv.Value} razy");
        }
    }
}

// Główna klasa programu
class Program
{
    static void Main()
    {
        int n = 3; // liczba producentów
        int m = 2; // liczba konsumentów

        Queue<DataItem> queue = new Queue<DataItem>(); // współdzielona kolejka
        object lockObject = new object(); // obiekt do synchronizacji

        List<ProducerThread> producers = new List<ProducerThread>();
        List<ConsumerThread> consumers = new List<ConsumerThread>();

        // Tworzenie i uruchamianie producentów
        for (int i = 0; i < n; i++)
        {
            var p = new ProducerThread(i, 1000, queue, lockObject);
            producers.Add(p);
            p.Thread.Start();
        }

        // Tworzenie i uruchamianie konsumentów
        for (int i = 0; i < m; i++)
        {
            var c = new ConsumerThread(i, 1200, queue, lockObject);
            consumers.Add(c);
            c.Thread.Start();
        }

        Console.WriteLine("Wciśnij 'q' aby zakończyć program...");

        // Pętla główna – czekamy aż użytkownik wciśnie q
        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                break;
            Thread.Sleep(100);
        }

        // Zatrzymanie wszystkich wątków
        producers.ForEach(p => p.Stop());
        consumers.ForEach(c => c.Stop());

        // Oczekiwanie aż każdy wątek zakończy działanie
        producers.ForEach(p => p.Thread.Join());
        consumers.ForEach(c => c.Thread.Join());

        Console.WriteLine("Program zakończony.");
    }
}
