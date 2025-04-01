using System;
using System.IO;
using System.Threading;

class Program
{
    private static FileSystemWatcher? watcher;
    private static volatile bool running = true;

    static void Main()
    {
        Console.Write("Podaj ścieżkę folderu do monitorowania: ");
        string folderPath = Console.ReadLine() ?? "";

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Podany folder nie istnieje.");
            return;
        }

        // Wątek do monitorowania folderu
        Thread monitoringThread = new Thread(() => StartMonitoring(folderPath));
        monitoringThread.Start();

        Console.WriteLine("Monitorowanie rozpoczęte. Wciśnij 'q', aby zakończyć.");

        // Główny wątek: nasłuchiwanie na 'q'
        while (true)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                running = false;
                break;
            }
            Thread.Sleep(100);
        }

        // Czekamy na zakończenie monitora
        monitoringThread.Join();
        Console.WriteLine("Zakończono monitorowanie.");
    }

    private static void StartMonitoring(string path)
    {
        watcher = new FileSystemWatcher(path);
        watcher.IncludeSubdirectories = false;
        watcher.EnableRaisingEvents = true;

        // Obsługa zdarzeń
        watcher.Created += (sender, e) =>
        {
            Console.WriteLine($"dodano plik {e.Name}");
        };

        watcher.Deleted += (sender, e) =>
        {
            Console.WriteLine($"usunięto plik {e.Name}");
        };

        // Działamy dopóki flaga `running` jest true
        while (running)
        {
            Thread.Sleep(200); // zmniejszenie użycia CPU
        }

        watcher.EnableRaisingEvents = false;
        watcher.Dispose();
    }
}
