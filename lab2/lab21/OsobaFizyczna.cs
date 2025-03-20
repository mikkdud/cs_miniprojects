using System;

namespace SystemBankowy
{
    public class OsobaFizyczna : PosiadaczRachunku
    {
        private string imie;
        private string nazwisko;
        private string drugieImie;
        private string PESEL;
        private string numerPaszportu;

        public string Imie { get {return imie; } set {imie = value; } }
        public string Nazwisko { get { return nazwisko; } set { nazwisko = value; } }
        public string DrugieImie { get { return drugieImie; } set { drugieImie = value; } }
        public string Pesel { get { return PESEL; } set { PESEL = value; } }
        public string NumerPaszportu { get { return numerPaszportu; } set { numerPaszportu = value; } }

        // Konstruktor
        public OsobaFizyczna(string imie, string nazwisko, string drugieImie, string pesel, string numerPaszportu)
        {
            if (string.IsNullOrWhiteSpace(pesel) && string.IsNullOrWhiteSpace(numerPaszportu))
            {
                throw new ArgumentException("PESEL i numer paszportu nie mogą być jednocześnie puste!");
            }

            this.imie = imie;
            this.nazwisko = nazwisko;
            this.drugieImie = drugieImie;
            this.PESEL = pesel;
            this.numerPaszportu = numerPaszportu;
        }

        public override string ToString()
        {
            return $"Osoba fizyczna: {Imie} {Nazwisko}";
        }
    }    
}