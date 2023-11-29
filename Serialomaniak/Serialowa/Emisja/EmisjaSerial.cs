using Serialomaniak.Serialowa.Emisja;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa
{
    [Serializable]
    public class EmisjaSerial : INotifyPropertyChanged
    {
        private string stacja_tv;

        public string Stacja_tv
        {
            get { return stacja_tv; }
            set { stacja_tv = value; }
        }

        private string kraj;

        public string Kraj
        {
            get { return kraj; }
            set { kraj = value; }
        }

        private int czas_trwania;

        public int Czas_trwania
        {
            get { return czas_trwania; }
            set { czas_trwania = value; }
        }

        private string nazwa_serialu;

        public string Nazwa_serialu
        {
            get { return nazwa_serialu; }
            set { nazwa_serialu = value; }
        }
        private StatusSerialu status_serialu;

        public StatusSerialu Status_serialu
        {
            get { return status_serialu; }
            set { status_serialu = value; }
        }

        private List<EmisjaSezon> lista_sezonow;

        public List<EmisjaSezon> Lista_sezonow
        {
            get { return lista_sezonow; }
            set { lista_sezonow = value; }
        }

        public EmisjaSerial()
        {
            lista_sezonow = new List<EmisjaSezon>();
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
