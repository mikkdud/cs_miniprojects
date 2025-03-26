using System;

namespace lab
{
    class Program
    {
        static void Main(string[] args)
        {

            ////////////////////////////////////
            /// WCZYTANIE KLAS Z PLIKOW
            ////////////////////////////////////

            var wczytywaczRegionow = new Wczytywacz<Region>();
            List<Region> regiony = wczytywaczRegionow.WczytajListe("import-northwind-dataset/regions.csv", x => new Region(x[0], x[1]));

            var wczytywaczTerytoriow = new Wczytywacz<Territory>();
            List<Territory> terytoria = wczytywaczTerytoriow.WczytajListe("import-northwind-dataset/territories.csv", x => new Territory(x[0], x[1], x[2]));

            var wczytywaczPowiazan = new Wczytywacz<EmployeeTerritory>();
            List<EmployeeTerritory> powiazania = wczytywaczPowiazan.WczytajListe(
                "import-northwind-dataset/employee_territories.csv", x => new EmployeeTerritory(x[0], x[1])
            );

            var wczytywaczPracownikow = new Wczytywacz<Employee>();
            List<Employee> pracownicy = wczytywaczPracownikow.WczytajListe("import-northwind-dataset/employees.csv", x => new Employee(x[0], x[1], x[2]));


            ////////////////////////////////////
            /// wybierz nazwiska wszystkich pracowników
            ////////////////////////////////////
            ///
            var nazwiskaPracownikow = pracownicy.Select(p => p.LastName).ToList();

            Console.WriteLine("\n\n\n\n\n ########## 2 ###########");
            foreach (var nazwisko in nazwiskaPracownikow)
            {
                Console.WriteLine(nazwisko);
            }


            ////////////////////////////////////
            /// wypisz nazwiska pracowników oraz dla każdego z nich nazwę regionu i terytorium gdzie pracuje
            ////////////////////////////////////
            
            var wynik1 = from pracownik in pracownicy
            join powiazanie in powiazania on pracownik.EmployeeID equals powiazanie.EmployeeID
            join terytorium in terytoria on powiazanie.TerritoryID equals terytorium.TerritoryID
            join region in regiony on terytorium.RegionID equals region.RegionID
            select new
            {
                Nazwisko = pracownik.LastName,
                Terytorium = terytorium.TerritoryDescription,
                Region = region.RegionDescription
            };

            Console.WriteLine("\n\n\n\n\n ########## 3 ###########");
            foreach (var rekord in wynik1)
            {
                Console.WriteLine($"{rekord.Nazwisko} - {rekord.Terytorium} - {rekord.Region}");
            }


            ////////////////////////////////////
            /// wypisz nazwy regionów oraz nazwiska pracowników, którzy pracują w tych regionach, 
            // pracownicy mają być zagregowani po regionach, 
            // rezultatem ma być lista regionów z podlistą pracowników (odpowiednik groupjoin)
            ////////////////////////////////////
            
            var wynik2 = from region in regiony
                        join terytorium in terytoria on region.RegionID equals terytorium.RegionID
                        join powiazanie in powiazania on terytorium.TerritoryID equals powiazanie.TerritoryID
                        join pracownik in pracownicy on powiazanie.EmployeeID equals pracownik.EmployeeID
                        group pracownik by region.RegionDescription into regionGroup
                        select new
                        {
                            Region = regionGroup.Key, // nazwa regionu
                            Pracownicy = regionGroup.Select(p => p.LastName).Distinct().ToList() // lista nazwisk pracowników w danym regionie
                        };

            Console.WriteLine("\n\n\n\n\n ########## 4 ###########");
            foreach (var rekord in wynik2)
            {
                Console.WriteLine($"Region: {rekord.Region}");
                foreach (var nazwisko in rekord.Pracownicy)
                {
                    Console.WriteLine($"  - {nazwisko}");
                }
            }


            ////////////////////////////////////
            /// wypisz nazwy regionów oraz liczbę pracowników w tych regionach
            ////////////////////////////////////
            
            var wynik3 = from region in regiony
            join terytorium in terytoria on region.RegionID equals terytorium.RegionID
            join powiazanie in powiazania on terytorium.TerritoryID equals powiazanie.TerritoryID
            join pracownik in pracownicy on powiazanie.EmployeeID equals pracownik.EmployeeID
            group pracownik by region.RegionDescription into regionGroup
            select new
            {
                Region = regionGroup.Key, // nazwa regionu
                LiczbaPracownikow = regionGroup.Count() // liczba pracowników w regionie
            };

            Console.WriteLine("\n\n\n\n\n ########## 5 ###########");
            foreach (var rekord in wynik3)
            {
                Console.WriteLine($"Region: {rekord.Region}, Liczba pracowników: {rekord.LiczbaPracownikow}");
            }


        }
    }
}
