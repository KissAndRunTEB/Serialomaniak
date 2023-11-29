using Serialomaniak.Serialowa.Dystrybutorzy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa
{
    public class Odcinek : INotifyPropertyChanged, IEquatable<Odcinek>
    {
        private string nazwa_serialu;

        private int numer_sezonu;
        private int numer_odcinka;

        private string tytul_odcinka;
        private bool obejrzany;

        private bool poinformowany_o_lektorze;
        private bool poinformowany_o_napisach;
        private bool poinformowany_o_oryginale;

        List<Link> linki_do_odcinka;

        private DateTime? data_emisji;

        public Odcinek(string nazwa_serialu, int numer_sezonu, int numer_odcinka, string tytul,
            List<Link> linki_do_odcinka = null, bool obejrzany = false, DateTime? data_emisji = null,
            bool poinformowany_o_lektorze = false, bool poinformowany_o_napisach = false, bool poinformowany_o_oryginale = false)
        {
            //jesli zmiana listy wlasciwosci to i aktualizowac funkcje Equals
            this.nazwa_serialu = nazwa_serialu;
            this.numer_sezonu = numer_sezonu;
            this.numer_odcinka = numer_odcinka;

            this.tytul_odcinka = tytul;
            this.obejrzany = obejrzany;

            this.poinformowany_o_lektorze = poinformowany_o_lektorze;
            this.poinformowany_o_napisach = poinformowany_o_napisach;
            this.poinformowany_o_oryginale = poinformowany_o_oryginale;


            this.linki_do_odcinka = linki_do_odcinka;

            if (this.linki_do_odcinka==null)
            {
                this.linki_do_odcinka = new List<Link>();
            }

            this.data_emisji = data_emisji;

            //jako że usa jest przesuniete o cztery godziny aby porownywanai czasu dobrze działały dodaj cztery godziny
            if (this.data_emisji.HasValue)
            {
                this.data_emisji.Value.AddHours(20); // o 20.00 startuja seriale w usa
                this.data_emisji.Value.AddHours(4); // u nas jest to 24.00
                this.data_emisji.Value.AddHours(12); // czas na pojawienie sie pierwszych polskich linkow
                //POPRAWIC 12 godzin skasowac (zamienic na 6 godziny), bo linki takze usa sie pojawiaja na liscie
            }
        }


        public bool Equals(Odcinek inny)
        {
            bool rowne = false;

            if (
                this.nazwa_serialu == inny.nazwa_serialu &&
            this.numer_sezonu == inny.numer_sezonu &&
            this.numer_odcinka == inny.numer_odcinka &&

            this.tytul_odcinka == inny.tytul_odcinka &&
            this.obejrzany == inny.obejrzany &&

            this.poinformowany_o_lektorze == inny.poinformowany_o_lektorze &&
            this.poinformowany_o_napisach == inny.poinformowany_o_napisach &&
            this.poinformowany_o_oryginale == inny.poinformowany_o_oryginale &&

            this.linki_do_odcinka == inny.linki_do_odcinka &&
            this.data_emisji == inny.data_emisji

                )
            {
                rowne = true;
            }

            return rowne;
        }

        public bool zasadniczo_rowne(Odcinek inny)
        {
            bool rowne = false;

            if (
                this.nazwa_serialu == inny.nazwa_serialu &&
            this.numer_sezonu == inny.numer_sezonu &&
            this.numer_odcinka == inny.numer_odcinka
                )
            {
                rowne = true;
            }

            return rowne;
        }

        public void oznacz_jako_obejrzany()
        {
           Serial s = ListaSeriali.SerialoNazwie(this.NazwaSerialu);

           s.obejrzane_do(this);
        }

        public void oznacz_jako_nie_obejrzany()
        {
            Serial s = ListaSeriali.SerialoNazwie(this.NazwaSerialu);

            s.obejrzane_do(s.PoprzedniOdcinek);
        }

        public List<Link> uzupelnij_liste_linkow()
        {
            List<Link> linki_nowe = Wlasne.uzupelnij_liste_linkow(this);
            return linki_nowe;
        }


        # region Właściwości

        public string NazwaSerialu
        {
            get
            {
                return nazwa_serialu;
            }
            set
            {
                nazwa_serialu = value;
                this.OnPropertyChanged("NazwaSerialu");
            }
        }
        public int NumerSezonu
        {
            get
            {
                return numer_sezonu;
            }
            set
            {
                numer_sezonu = value;
                this.OnPropertyChanged("NumerSezonu");
            }
        }
        public int NumerOdcinka
        {
            get
            {
                return numer_odcinka;
            }
            set
            {
                numer_odcinka = value;
                this.OnPropertyChanged("NumerOdcinka");
            }
        }
        public string Tytul_odcinka
        {
            get
            {
                return tytul_odcinka;
            }
            set
            {
                tytul_odcinka = value;
                this.OnPropertyChanged("Tytul");
            }
        }

        public bool Obejrzany
        {
            get
            {
                return obejrzany;
            }
            set
            {
                obejrzany = value;

                this.OnPropertyChanged("Obejrzany");
            }
        }

        public bool Czy_jest_ostnim_obejrznym_odcinkiem
        {
            get
            {
                Serial s = ListaSeriali.SerialoNazwie(this.NazwaSerialu);

                if (s.znajdz_ostatni_obejrzany_odcinek().Equals(this))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool PoinformowanyOLektorze
        {
            get
            {
                return poinformowany_o_lektorze;
            }
            set
            {
                poinformowany_o_lektorze = value;
                this.OnPropertyChanged("PoinformowanyOLektorze");
            }
        }

        public bool PoinformowanyONapisach
        {
            get
            {
                return poinformowany_o_napisach;
            }
            set
            {
                poinformowany_o_napisach = value;
                this.OnPropertyChanged("PoinformowanyONapisach");
            }
        }

        public bool PoinformowanyOOryginale
        {
            get
            {
                return poinformowany_o_oryginale;
            }
            set
            {
                poinformowany_o_oryginale = value;
                this.OnPropertyChanged("PoinformowanyOOryginale");
            }
        }


        public List<Link> LinkiDoOdcinka
        {
            get
            {
                return linki_do_odcinka;
            }
            set
            {
                linki_do_odcinka = value;
                this.OnPropertyChanged("LinkiDoOdcinka");
            }
        }

        public DateTime? DataEmisji
        {
            get
            {
                return data_emisji;
            }
            set
            {
                data_emisji = value;
                this.OnPropertyChanged("DataEmisji");
            }
        }

        public WersjaOdcinka NajlepszaWersja
        {
            get
            {
                WersjaOdcinka najlepsza = WersjaOdcinka.Oryginal;

                foreach (Link l in this.LinkiDoOdcinka)
                {
                    if (l.Wersja == WersjaOdcinka.Lektor)
                    {
                        najlepsza = WersjaOdcinka.Lektor;
                        break;
                    }
                    else if (l.Wersja == WersjaOdcinka.Napisy)
                    {
                        najlepsza = WersjaOdcinka.Napisy;
                    }
                }
                return najlepsza;
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
