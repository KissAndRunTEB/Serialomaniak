using Serialomaniak.Serialowa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak
{
    public class Profil : INotifyPropertyChanged
    {
        private string nick;
        private string adres_mail;
        private string haslo;
        private int numer;
        private string avatar;

        private WersjaOdcinka wersja_odcinka_do_informowania;

        public static void zaloz_nowy(string nick, string mail, string haslo, int numer, string avatar = "", WersjaOdcinka wersja = WersjaOdcinka.Oryginal)
        {
            (App.Current.Resources["profil"] as Profil).Nick = nick;
            (App.Current.Resources["profil"] as Profil).AdresMail = mail;
            (App.Current.Resources["profil"] as Profil).Haslo = haslo;
            (App.Current.Resources["profil"] as Profil).Numer = numer;
            (App.Current.Resources["profil"] as Profil).Avatar = avatar;

            (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania = wersja;
        }

        public static void zapisz()
        {
            Program.Serializuj_do_XML<Profil>((App.Current.Resources["profil"] as Profil), "Profil");
        }

        public static void wczytaj()
        {
            App.Current.Resources["profil"] = Program.Deserializuj_z_XML<Profil>("Profil");
        }

        public static bool czy_informowac_o_odcinku(WersjaOdcinka wersja)
        {
            bool czy_informowac = false;
            WersjaOdcinka wersja_profilu = (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania;

            if (wersja_profilu == wersja)
            {
                czy_informowac = true;
            }

            if (wersja_profilu == WersjaOdcinka.Oryginal)
            {
                czy_informowac = true;
            }

            if (wersja_profilu == WersjaOdcinka.Napisy && wersja == WersjaOdcinka.Lektor)
            {
                czy_informowac = true;
            }

            return czy_informowac;
        }

        #region Właściwości

        public string Nick
        {
            get
            {
                return nick;
            }
            set
            {
                nick = value;
                this.OnPropertyChanged("Nick");
            }
        }

        public string AdresMail
        {
            get
            {
                return adres_mail;
            }
            set
            {
                adres_mail = value;
                this.OnPropertyChanged("AdresMail");
            }
        }

        public string Haslo
        {
            get
            {
                return haslo;
            }
            set
            {
                haslo = value;
                this.OnPropertyChanged("Haslo");
            }
        }
        public int Numer
        {
            get
            {
                return numer;
            }
            set
            {
                numer = value;
                this.OnPropertyChanged("Numer");
            }
        }
        public string Avatar
        {
            get
            {
                return avatar;
            }
            set
            {
                avatar = value;
                this.OnPropertyChanged("Avatar");
            }
        }

        public WersjaOdcinka WersjaOdcinkaDoInformowania
        {
            get
            {
                return wersja_odcinka_do_informowania;
            }
            set
            {
                wersja_odcinka_do_informowania = value;
                this.OnPropertyChanged("WersjaOdcinkaDoInformowania");
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
