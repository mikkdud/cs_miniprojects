using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

class Program
{
    private static ConcurrentQueue<string> foundFiles = new ConcurrentQueue<string>();
    private static volatile bool searchingDone = false;

    static void Main()
    {
        Console.Write("Podaj ścieżkę katalogu startowego: ");
        string folderPath = Console.ReadLine() ?? "";

        Console.Write("Podaj fragment nazwy pliku do wyszukania: ");
        string pattern = Console.ReadLine() ?? "";

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Podany katalog nie istnieje.");
            return;
        }

        // Tworzymy i uruchamiamy wątek wyszukujący
        Thread searchThread = new Thread(() => SearchFiles(folderPath, pattern));
        searchThread.Start();

        Console.WriteLine("Wyszukiwanie rozpoczęte...\n");

        // Główny wątek co chwilę sprawdza, czy coś jest w kolejce
        while (!searchingDone || !foundFiles.IsEmpty)
        {
            while (foundFiles.TryDequeue(out string? file))
            {
                Console.WriteLine($"Znaleziono: {file}");
            }

            Thread.Sleep(100);
        }

        Console.WriteLine("\nWyszukiwanie zakończone.");
    }

    // Wątek wyszukujący pliki pasujące do wzorca
    static void SearchFiles(string startDir, string pattern)
    {
        try
        {
            foreach (var file in Directory.EnumerateFiles(startDir, "*", SearchOption.AllDirectories))
            {
                if (file.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    foundFiles.Enqueue(file);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas przeszukiwania: {ex.Message}");
        }
        finally
        {
            searchingDone = true;
        }
    }
}
