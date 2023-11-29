using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Konta
{
    public class Zmiana : INotifyPropertyChanged
    {
        private string autor;
        public string Autor
        {
            get { return autor; }
            set
            {
                autor = value;
                this.OnPropertyChanged("Wnioskodawca");
            }
        }

        private string wersja;
        public string Wersja
        {
            get { return wersja; }
            set
            {
                wersja = value;
                this.OnPropertyChanged("Wersja");
            }
        }

        private string opis;
        public string Opis
        {
            get { return opis; }
            set
            {
                opis = value;
                this.OnPropertyChanged("Opis");
            }
        }

        public Zmiana(string tresc, string wnioskodawca, string numer_wersji)
        {
            this.opis = tresc;
            this.autor = wnioskodawca;
            this.wersja = numer_wersji;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nazwaWlasciowosci)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWlasciowosci));
            }
        }
    }
}
