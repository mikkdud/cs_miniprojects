using System;


namespace SystemBankowy
{
    class Program {
        static void Main(String[] args){

            // tworzymy osoby
            OsobaFizyczna osoba1 = new OsobaFizyczna("Marek", "Kowalski", "Stefan", "2536", "436426");
            OsobaFizyczna osoba2 = new OsobaFizyczna("Anna", "Nowak", "Maria", "3654", "224745");
            OsobaPrawna firma = new OsobaPrawna("AGH", "Kraków");
            
            // wpłacamy coś na ich rachunki
            List<PosiadaczRachunku> posiadacze1 = new List<PosiadaczRachunku>{osoba1, firma};
            List<PosiadaczRachunku> posiadacze2 = new List<PosiadaczRachunku>{osoba2};

            RachunekBankowy rachunek1 = new RachunekBankowy("1345316", 1000, false, posiadacze1);
            RachunekBankowy rachunek2 = new RachunekBankowy("3254634", 2000, true, posiadacze2);

            // sprawdzenie aktualnego stanu kont
            Console.WriteLine("Stan początkowy rachunków:");
            Console.WriteLine(rachunek1.ToString());
            Console.WriteLine(rachunek2.ToString());
            Console.WriteLine("\n\n");

            // Poprawna transakcja: przelew między kontami
            Console.WriteLine("Poprawna transakcja (Przelew 500 zł):");
            RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 500, "Przelew za usługę");
            Console.WriteLine(rachunek1);
            Console.WriteLine(rachunek2);
            Console.WriteLine("\n\n");

            //  Próba przelewu większego niż dostępne środki (przy braku debetu)
            try
            {
                Console.WriteLine(" Test: Przelew 2000 zł z konta bez debetu:");
                RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, 2000m, "Duży przelew");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Błąd: {ex.Message}");
            }
            Console.WriteLine("\n\n");

            //  Próba stworzenia rachunku bez posiadacza (powinien rzucić wyjątek)
            try
            {
                Console.WriteLine(" Test: Tworzenie rachunku bez posiadacza:");
                RachunekBankowy rachunekError = new RachunekBankowy("999999", 500, true, new List<PosiadaczRachunku>());
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Błąd: {ex.Message}");
            }
            Console.WriteLine("\n\n");

            //  Próba wykonania transakcji bez podanych rachunków (powinien rzucić wyjątek)
            try
            {
                Console.WriteLine(" Test: Próba wykonania transakcji bez rachunku:");
                RachunekBankowy.DokonajTransakcji(null, null, 100, "Niepoprawna transakcja");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Błąd: {ex.Message}");
            }
            Console.WriteLine("\n\n");

            //  Próba wykonania transakcji na ujemną kwotę (powinien rzucić wyjątek)
            try
            {
                Console.WriteLine(" Test: Próba przelewu na ujemną kwotę:");
                RachunekBankowy.DokonajTransakcji(rachunek1, rachunek2, -100m, "Błędna transakcja");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Błąd: {ex.Message}");
            }
            Console.WriteLine("\n\n");
        }
    }
}