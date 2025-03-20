using System;

namespace SystemBankowy
{
    class Transakcja
    {
        private RachunekBankowy? rachunekZrodlowy;
        private RachunekBankowy? rachunekDocelowy;
        private decimal kwota;
        private string opis;

        public Transakcja(RachunekBankowy? rachunekZrodlowy, RachunekBankowy? rachunekDocelowy, decimal kwota, string opis)
        {
            if (rachunekDocelowy == null && rachunekZrodlowy == null)
            {
                throw new Exception("nie uzupełniono obu rachunkow bankowych");
            }
            this.rachunekZrodlowy = rachunekZrodlowy;
            this.rachunekDocelowy = rachunekDocelowy;
            this.kwota = kwota;
            this.opis = opis;
        }
        public override string ToString()
        {
            string rachunekZrodlowyNumer = rachunekZrodlowy?.Numer ?? "Gotówka";
            string rachunekDocelowyNumer = rachunekDocelowy?.Numer ?? "Gotówka";

            return $"Od: {rachunekZrodlowyNumer} → Do: {rachunekDocelowyNumer}, Kwota: {kwota:C}, Opis: {opis}";
        }

    }
}