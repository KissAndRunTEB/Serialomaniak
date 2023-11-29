using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa
{
    public class Link : INotifyPropertyChanged, IEquatable<Link>
    {
        WersjaOdcinka wersja;
        Dystrybutor dystrybutor;
        string adres;

        public Link(WersjaOdcinka wersja, Dystrybutor dystr, string adres)
        {
            this.wersja = wersja;
            this.dystrybutor = dystr;
            this.adres = adres;
        }

        //dodeserializacji
        public Link()
        {
        }

        //do porownywania obiektów za pomocą contains()
        public bool Equals(Link other)
        {
            if (this.Adres == other.Adres && this.Dystrybutor == other.Dystrybutor
                && this.Wersja == other.Wersja)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Dystrybutor OkreslDystrybutora(string adres)
        {
            adres = adres.ToLower();
            if (adres.Contains("odsiebie"))
            {
                return Dystrybutor.Odsiebie;
            }
            else if (adres.Contains("wgrane"))
            {
                return Dystrybutor.Wgrane;
            }
            else if (adres.Contains("maxvideo"))
            {
                return Dystrybutor.Maxvideo;
            }
            else if (adres.Contains("anyfiles"))
            {
                return Dystrybutor.Anyfiles;
            }
            else if (adres.Contains("cda"))
            {
                return Dystrybutor.Cda;
            }
            else if (adres.Contains("sprocked"))
            {
                return Dystrybutor.Sprocked;
            }
            else if (adres.Contains("putlocker"))
            {
                return Dystrybutor.Putlocker;
            }
            else if (adres.Contains("gorillavid"))
            {
                return Dystrybutor.Gorillavid;
            }
            else if (adres.Contains("novaMov"))
            {
                return Dystrybutor.NovaMov;
            }
            else if (adres.Contains("dwn"))
            {
                return Dystrybutor.Dwn;
            }
            else if (adres.Contains("hd3d"))
            {
                return Dystrybutor.Hd3d;
            }
            else
            {
                return Dystrybutor.Inny;
            }
        }

        public static WersjaOdcinka OkreslWersjeOdcinka(string tekst)
        {
            if (tekst == WersjaOdcinka.Lektor.ToString())
            {
                return WersjaOdcinka.Lektor;
            }
            else if(tekst == WersjaOdcinka.Napisy.ToString())
            {
                return WersjaOdcinka.Napisy;
            }
            else if (tekst == WersjaOdcinka.Oryginal.ToString())
            {
                return WersjaOdcinka.Oryginal;
            }
            else
            {
                //POLEPSZYC zglasza blad i loguje go
                return WersjaOdcinka.Oryginal;
            }
        }

        #region Właściwości
        public WersjaOdcinka Wersja
        {
            get { return wersja; }
            set
            {
                wersja = value;
                this.OnPropertyChanged("Wersja");
            }
        }

        public Dystrybutor Dystrybutor
        {
            get { return dystrybutor; }
            set
            {
                dystrybutor = value;
                this.OnPropertyChanged("Dystrybutor");

            }
        }

        public string Adres
        {
            get { return adres; }
            set
            {
                adres = value;
                this.OnPropertyChanged("Adres");

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


    public enum Dystrybutor
    {
        Odsiebie,
        Wgrane,
        Maxvideo,
        Anyfiles,
        Cda,
        Sprocked,
        Putlocker,
        Gorillavid,
        NovaMov,
        Dwn,
        Hd3d,
        Inny
    }

    public enum WersjaOdcinka
    {
        Lektor,
        Napisy,
        Oryginal
    }

    public enum StatusSerialu
    {
        Nadawany = 1,
        Nowy = 2,
        Powroci = 3,
        FinalowySezon = 4,
        DecydujeSie = 5,
        Anulowany = 6,
        Zakonczony = 7,
    }
}
