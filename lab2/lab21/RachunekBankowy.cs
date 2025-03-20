using System;

namespace SystemBankowy
{
    class RachunekBankowy
    {
        private string numer; /** numer rachunku*/
        private decimal stanRachunku;
        private bool czyDozwolonyDebet;
        private List<PosiadaczRachunku> _PosiadaczeRachunku;
        private List<Transakcja> _Transakcje;

        public string Numer{ get {return numer;} set { numer = value;}}
        public decimal StanRachunku{ get {return stanRachunku;} set { stanRachunku = value;}}
        public bool CzyDozwolonyDebet
        { 
            get {return czyDozwolonyDebet;} 
            set { czyDozwolonyDebet = value;}
        }
        public List<PosiadaczRachunku> PosiadaczeRachunku {get { return _PosiadaczeRachunku; }}
        public RachunekBankowy(string numer, decimal stanRachunku, bool czyDozwolonyDebet, List<PosiadaczRachunku> posiadaczeRachunku)
        {
            if (posiadaczeRachunku == null || posiadaczeRachunku.Count < 1)
            {
                throw new Exception("lista posiadaczy rachunku nie może być pusta!");
            }
            this.numer = numer;
            this.stanRachunku = stanRachunku;
            this.czyDozwolonyDebet = czyDozwolonyDebet;
            this._PosiadaczeRachunku = new List<PosiadaczRachunku>(posiadaczeRachunku); // Tworzymy kopię listy
            this._Transakcje = new List<Transakcja>(); // Inicjalizacja listy transakcji
        }
        public static void DokonajTransakcji(RachunekBankowy? rachunekZrodlowy, RachunekBankowy? rachunekDocelowy, decimal kwota, string opis)
        {
            if (kwota < 0)
            {
                throw new Exception("Kwota nie może być mniejsza od zera");
            }
            if (rachunekDocelowy == null && rachunekZrodlowy == null)
            {
                throw new Exception("Nie podano rachunku");
            }
            if (rachunekZrodlowy != null && rachunekZrodlowy.CzyDozwolonyDebet == false && kwota > rachunekZrodlowy.StanRachunku)
            {
                throw new Exception("Nie zezwolono na debet");
            }
            if (rachunekZrodlowy == null && rachunekDocelowy != null)  // Wpłata gotówkowa
            {   
                rachunekDocelowy.StanRachunku += kwota;
                Transakcja transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
                rachunekDocelowy._Transakcje.Add(transakcja);
            }
            if (rachunekZrodlowy != null && rachunekDocelowy == null)  // Wypłata gotówkowa
            {
                rachunekZrodlowy.StanRachunku -= kwota;
                Transakcja transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
                rachunekZrodlowy._Transakcje.Add(transakcja);
            }
            if (rachunekDocelowy != null && rachunekZrodlowy != null)
            {
                rachunekZrodlowy.StanRachunku -= kwota;
                rachunekDocelowy.StanRachunku += kwota;
                Transakcja transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
                rachunekDocelowy._Transakcje.Add(transakcja);
                rachunekZrodlowy._Transakcje.Add(transakcja);
            }
        }

        public override string ToString()
        {
            string posiadacze = string.Join(", ", _PosiadaczeRachunku.Select(p => p.ToString()));
            string transakcje = _Transakcje.Count > 0 
                ? string.Join("\n", _Transakcje.Select(t => t.ToString())) 
                : "Brak transakcji";

            return $"Rachunek: {Numer}\n" +
                $"Stan rachunku: {StanRachunku:C}\n" +
                $"Czy dozwolony debet: {(CzyDozwolonyDebet ? "Tak" : "Nie")}\n" +
                $"Posiadacze rachunku: {posiadacze}\n" +
                $"Historia transakcji:\n{transakcje}\n";
        }

    }    
}