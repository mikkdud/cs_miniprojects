using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static volatile bool running = true;
    static int threadsStarted = 0;       // zlicza wątki, które faktycznie zaczęły pracę
    static int threadsFinished = 0;      // zlicza zakończone wątki
    static int n = 5;                    // liczba wątków
    static object lockObj = new object();

    static void Main()
    {
        List<Thread> threads = new List<Thread>();

        for (int i = 0; i < n; i++)
        {
            int localId = i; // potrzebne, bo i by się zmieniało w lambdzie
            Thread t = new Thread(() => WorkerThread(localId));
            threads.Add(t);
            t.Start();
        }

        // Czekamy aż wszystkie wątki wejdą do metody WorkerThread i zwiększą licznik
        while (threadsStarted < n)
        {
            Thread.Sleep(50); // nie męczymy CPU
        }

        Console.WriteLine("Wszystkie wątki rozpoczęły działanie.");

        // Zainicjuj zamykanie
        running = false;

        // Poczekaj aż wszystkie wątki się zakończą
        while (threadsFinished < n)
        {
            Thread.Sleep(50);
        }

        Console.WriteLine("Wszystkie wątki zakończyły działanie. Koniec programu.");
    }

    static void WorkerThread(int id)
    {
        // Wątek "wszedł" do działania – zaliczamy start
        Interlocked.Increment(ref threadsStarted);
        Console.WriteLine($"[Wątek {id}] start");

        // Pętla działania dopóki flaga `running` jest true
        while (running)
        {
            Thread.Sleep(100); // symulacja pracy
        }

        // Zgłaszamy zakończenie
        Console.WriteLine($"[Wątek {id}] koniec");
        Interlocked.Increment(ref threadsFinished);
    }
}
