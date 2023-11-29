using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa.Emisja
{
    public class EmisjaSezon : INotifyPropertyChanged
    {
        private int numer_sezonu;

        private List<EmisjaOdcinek> lista_odcinkow = new List<EmisjaOdcinek>();

        public int liczba_odcinkow()
        {
            return lista_odcinkow.Count;
        }

        public void dodaj_odcinek(EmisjaOdcinek odcinek)
        {
            this.lista_odcinkow.Add(odcinek);
        }

        public void dodaj_odcinek_na_poczatku(EmisjaOdcinek odcinek)
        {
            this.lista_odcinkow.Insert(0, odcinek);
        }

        public EmisjaOdcinek znajdz_najnowszy()
        {
            EmisjaOdcinek o = lista_odcinkow[lista_odcinkow.Count - 1];

            return o;
        }



        public EmisjaOdcinek pierwszy_odcinek()
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

        public List<EmisjaOdcinek> ListaOdcinkow
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
