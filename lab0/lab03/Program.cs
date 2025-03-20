﻿using System;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace lab
{
    class Program
    {
        static void Main(string[] args)
        {
            /// ZAD 3 ///
            
            List<double> list_of_nums = new List<double>();
            StreamReader sr = new StreamReader("mean.txt");
            while (!sr.EndOfStream)
            {
                string t = sr.ReadLine() ?? "";
                double.TryParse(t, out double num);
                // Console.WriteLine(num);
                list_of_nums.Add(num);
            }
            sr.Close();
            Console.WriteLine(list_of_nums.Max());
        }
    }
}