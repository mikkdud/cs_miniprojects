﻿using System;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace lab
{
    class Program
    {
        static void Main(string[] args)
        {
            /// ZADANIE 2 ///
            
            int sum = 0;
            int counter = 0;
            while(true) {
                Console.WriteLine("give me some integers you want to add");
                string inp = Console.ReadLine() ?? "";
                if(!int.TryParse(inp, out int n)){
                    Console.WriteLine("it's not int!");
                    break;
                }
                if (n == 0) {
                    if (counter == 0) {
                        Console.WriteLine("0");
                    }
                    else {
                        StreamWriter sw = new StreamWriter("mean.txt", append:true);
                        sw.WriteLine((double)sum/(double)counter);
                        sw.Close();
                        break;
                    }
                }
                else {
                    sum += n;
                    counter ++;
                }
            }
        }
    }
}