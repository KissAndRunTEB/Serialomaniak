using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak
{
    public class Ustawienia : INotifyPropertyChanged
    {
        private string wersja;
        private bool autostart;
        private bool powiadomienia_wycentrowane;
        private bool poprawka_polozenia;

        private double gora;
        private double lewy;

        private static int liczba_powiadomien = 0;

        public const int co_ile_pobierac_linki = 10 *60 *1000;// 10 minut


        public static void zaloz_nowe(string wersja, bool autostart = true, bool powiad_wycentrowane = false, bool poprawka_polozenia = false, double gora =-1, double lewy =-1)
        {
            (App.Current.Resources["ustawienia"] as Ustawienia).Wersja = wersja;
            (App.Current.Resources["ustawienia"] as Ustawienia).Autostart = autostart;
            (App.Current.Resources["ustawienia"] as Ustawienia).PowiadomieniaWycentrowane = powiad_wycentrowane;
            (App.Current.Resources["ustawienia"] as Ustawienia).PoprawkaPolozenia = poprawka_polozenia;

            (App.Current.Resources["ustawienia"] as Ustawienia).Gora = gora;
            (App.Current.Resources["ustawienia"] as Ustawienia).Lewy = lewy;
        }

        public static void zapisz()
        {
            Program.Serializuj_do_XML<Ustawienia>((App.Current.Resources["ustawienia"] as Ustawienia), "Ustawienia");
        }

        public static void wczytaj()
        {
            App.Current.Resources["ustawienia"] = Program.Deserializuj_z_XML<Ustawienia>("Ustawienia");
        }

        public static void dodano_powiadomienie()
        {
            liczba_powiadomien++;
        }
        public static void usunieto_powiadomienie()
        {
            liczba_powiadomien--;
        }

        #region Właściwości

        public static Ustawienia Obecne
        {
            get
            {
                return (App.Current.Resources["ustawienia"] as Ustawienia);
            }
        }

        public string Wersja
        {
            get
            {
                return wersja;
            }
            set
            {
                wersja = value;
                this.OnPropertyChanged("Wersja");
            }
        }
        public bool Autostart
        {
            get
            {
                return autostart;
            }
            set
            {
                autostart = value;
                Konta.Autostart.przelacz_autostart(autostart, Program.nazwa);
                this.OnPropertyChanged("Autostart");
            }
        }

        public bool PowiadomieniaWycentrowane
        {
            get
            {
                return powiadomienia_wycentrowane;
            }
            set
            {
                powiadomienia_wycentrowane = value;
                this.OnPropertyChanged("PowiadomieniaWycentrowane");
            }
        }
        public bool PoprawkaPolozenia
        {
            get
            {
                return poprawka_polozenia;
            }
            set
            {
                poprawka_polozenia = value;
                this.OnPropertyChanged("PoprawkaPolozenia");
            }
        }

        public double Gora
        {
            get
            {
                return gora;
            }
            set
            {
                gora = value;
                this.OnPropertyChanged("Gora");
            }
        }

        public double Lewy
        {
            get
            {
                return lewy;
            }
            set
            {
                lewy = value;
                this.OnPropertyChanged("Lewy");
            }
        }

        public int LiczbaPowiadomien
        {
            get
            {
                return liczba_powiadomien;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nazwaWlasciowosci)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWlasciowosci));
            }
        }

        #endregion
    }
}
