using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Konta
{
    public class KanalySeriali : INotifyPropertyChanged
    {
        private bool wszystkie;

        private bool iiTV;

        public static void zapisz()
        {
            Program.Serializuj_do_XML<KanalySeriali>((App.Current.Resources["kanaly_seriali"] as KanalySeriali), "KanalySeriali");
        }

        #region Właściwości

        public bool Wszystkie
        {
            get
            {
                return wszystkie;
            }
            set
            {
                wszystkie = value;
                this.OnPropertyChanged("Wszystkie");
            }
        }

        public bool IiTV
        {
            get
            {
                return iiTV;
            }
            set
            {
                iiTV = value;
                this.OnPropertyChanged("IiTV");
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
