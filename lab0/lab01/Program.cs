using System;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace lab
{
    class Program
    {
        static void Main(string[] args)
        {
            /// ZADANIE 1 ///
            
            List<string> words = new List<string>();

            while (true) {
                string x = Console.ReadLine() ?? "";
                if (int.TryParse(x, out int reps)){
                    // Console.WriteLine($"To jest liczba całkowita: {reps}");
                    for (int i = 0; i < reps; i++) {
                        Console.WriteLine(string.Join(", ", words));
                    }
                    break;
                }
                else {
                    words.Add(x);
                }
            }
        }
    }
}