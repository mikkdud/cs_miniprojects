using System;

namespace SystemBankowy
{
    class OsobaPrawna: PosiadaczRachunku
    {
        private string nazwa;
        private string siedziba;

        public string Nazwa {get {return this.nazwa;}}
        public string Siedziba {get {return this.nazwa;}}

        public override string ToString()
        {
            return $"Osoba prawna: {this.nazwa}, siedziba: {this.siedziba}";
        }

        public OsobaPrawna(string nazwa, string siedziba)
        {
            this.nazwa = nazwa;
            this.siedziba = siedziba;
        }
    }    
}