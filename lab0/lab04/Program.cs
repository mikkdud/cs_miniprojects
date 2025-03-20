﻿using System;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace lab
{
    class Program
    {
        static void Main(string[] args)
        {
            /// ZAD 4 ///
            string[] tones = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "B", "H"};
            Console.WriteLine("Write down a tune");
            string? your_tone = Console.ReadLine();
            /* szukamy który to jest dźwięk*/
            int counter = 0;
            int tone_num = 0;

            while(true) {
                if (your_tone == tones[counter]) {
                    tone_num = counter;
                    break;
                }
                else if (counter < 13) {
                    counter ++;
                }
                else {
                    Console.WriteLine("Wrong tone!");
                    break;
                }
            }
            int[] shifts = {2, 2, 1, 2, 2, 2, 1};
            for (int i = 0; i < 7; i++) {
                tone_num += shifts[i];
                Console.WriteLine(tones[tone_num%12]);
            }
        }
    }
}