using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Biblioteka;

namespace Serialomaniak.Serialowa
{
    public class Sezon : INotifyPropertyChanged
    {
        private int numer_sezonu;

        private List<Odcinek> lista_odcinkow = new List<Odcinek>();

        public int liczba_odcinkow()
        {
            return lista_odcinkow.Count;
        }

        public int liczba_odcinkow_wyemitowanych()
        {
            int liczba = 0;
            DateTime wczoraj = DateTime.Now.AddDays(-1);
            
            foreach (Odcinek o in ListaOdcinkow)
            {
                if (o.DataEmisji.HasValue)
                {
                    if (o.DataEmisji.Value.CompareTo(wczoraj)<=0)
                    {
                        //POPRAWIC dodac uwzglednienie tego czy jest link
                        liczba++;
                    }
                }
            }
            return liczba;
        }

        public void dodaj_odcinek(Odcinek odcinek)
        {
            this.lista_odcinkow.Add(odcinek);
        }

        public void dodaj_odcinek_na_poczatku(Odcinek odcinek)
        {
            this.lista_odcinkow.Insert(0, odcinek);
        }

        public Odcinek znajdz_ostatni_obejrzany_odcinek()
        {
            Odcinek ostatni_obejrzany = null;
            foreach (Odcinek o in lista_odcinkow)
            {
                if (o.Obejrzany)
                {
                    ostatni_obejrzany = o;
                }
            }
            return ostatni_obejrzany;
        }

        public Odcinek znajdz_najnowszy()
        {
            Odcinek o = lista_odcinkow[lista_odcinkow.Count - 1];

            return o;
        }

        public Odcinek znajdz_nastepny_po_obejrzanym_odcinku()
        {
            bool czy_znaleziony = false;
            Odcinek nastepny_po_obejrzanym = null;

            foreach (Odcinek o in lista_odcinkow)
            {
                if (czy_znaleziony == false)
                {
                    if (!o.Obejrzany)
                    {
                        nastepny_po_obejrzanym = o;
                        czy_znaleziony = true;
                    }
                }
            }

            return nastepny_po_obejrzanym;
        }

        public Odcinek znajdz_poprzedni_przed_obejrzanym_odcinku()
        {
            bool czy_znaleziony = false;
            Odcinek nastepny_po_obejrzanym = null;
            Odcinek poprzedni_przed__obejrzanym = null;

            foreach (Odcinek o in lista_odcinkow)
            {
                if (czy_znaleziony == false)
                {
                    if (!o.Obejrzany)
                    {
                        nastepny_po_obejrzanym = o;
                        czy_znaleziony = true;
                    }
                }
            }

            int indeks = lista_odcinkow.IndexOf(nastepny_po_obejrzanym);

            indeks = indeks - 2;
 
            if (indeks >-1)
            {
                poprzedni_przed__obejrzanym = lista_odcinkow[indeks];
            }

            return poprzedni_przed__obejrzanym;
        }


        public Odcinek pierwszy_odcinek()
        {
            return lista_odcinkow[0];
        }

        #region Właściwości

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

        public List<Odcinek> ListaOdcinkow
        {
            get
            {
                return lista_odcinkow;
            }
            set
            {
                lista_odcinkow = value;
                this.OnPropertyChanged("ListaOdcinkow");
            }
        }

        public string NapisLiczbaOdcinkow
        {
            get
            {
                return "Liczba odcinków: " + liczba_odcinkow();
            }
            set
            {
                this.OnPropertyChanged("LiczbaOdcinkow");
            }
        }

        public int LiczbaOdcinkow
        {
            get
            {
                return liczba_odcinkow();
            }
            set
            {
                this.OnPropertyChanged("LiczbaOdcinkow");
            }
        }

        public int LiczbaOdcinkowWyemitowanych
        {
            get
            {
                return liczba_odcinkow_wyemitowanych();
            }
            set
            {
                this.OnPropertyChanged("LiczbaOdcinkowWyemitowanych");
            }
        }


        public string ZakresDat
        {
            get
            {
                string pierwszy = pierwszy_odcinek().DataEmisji.na_tekst();
                string najnowszy = znajdz_najnowszy().DataEmisji.na_tekst();
                if (pierwszy != "" && najnowszy != "")
                {
                    return "Emisja od: " + pierwszy_odcinek().DataEmisji.na_tekst() + ", do: " + znajdz_najnowszy().DataEmisji.na_tekst();
                }
                else if (pierwszy != "")
                {
                    return "Emisja od: " + pierwszy_odcinek().DataEmisji.na_tekst();
                }
                else if (najnowszy != "")
                {
                    return "Emisja do: " + znajdz_najnowszy().DataEmisji.na_tekst();
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this.OnPropertyChanged("ZakresDat");
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
