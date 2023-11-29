using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa.Emisja
{
    [Serializable]
    public class EmisjaOdcinek : INotifyPropertyChanged
    {
        private string tytul_odcinka;

        public string Tytul_odcinka
        {
            get { return tytul_odcinka; }
            set { tytul_odcinka = value; }
        }

        private DateTime? data_emisji;

        public DateTime? Data_emisji
        {
            get { return data_emisji; }
            set { data_emisji = value; }
        }

        private string numer_sezonu;

        public string Numer_sezonu
        {
            get { return numer_sezonu; }
            set { numer_sezonu = value; }
        }
        private string numer_odcinka;

        public string Numer_odcinka
        {
            get { return numer_odcinka; }
            set { numer_odcinka = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string nazwaWlasciowosci)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWlasciowosci));
            }
        }
    }
}
