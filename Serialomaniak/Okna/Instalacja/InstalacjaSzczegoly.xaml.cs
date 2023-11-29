using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Biblioteka;
using Serialomaniak.Konta;
using System.Threading;
using Serialomaniak.Serialowa;
using System.Windows.Threading;

namespace Serialomaniak.Okna.Instalacja
{    
    public partial class InstalacjaSzczegoly : Window
    {
        int numer_profilu;

        Thread watek_lista_seriali;

        public InstalacjaSzczegoly(int numer_profilu)
        {
            InitializeComponent();

            this.numer_profilu = numer_profilu;

            //ukrywam okno aby ktos nie kliknal
            panel_zawartosci_okna.Opacity = 0;
            panel_zawartosci_okna.IsEnabled = false;

            wczytaj_liste_seriali_wszystkich();
        }

        private void wczytaj_liste_seriali_wszystkich()
        {
            watek_lista_seriali = new Thread(new ThreadStart(watek_lista_seriali_wszystkich));
            watek_lista_seriali.Start();
        }

        private void watek_lista_seriali_wszystkich()
        {
            ListaSeriali.generuj_liste_seriali_wszystkich();
            Dispatcher.Invoke(DispatcherPriority.Normal,
                  new Action(() =>
                  {
                      panel_zawartosci_okna.IsEnabled = true;
                      tekst_trwa_laczenie.Visibility = Visibility.Collapsed;
                      panel_zawartosci_okna.animuj_przezroczystosc(0, 1, "0:0:2");
                  })
                 );
        }

        private void przycisk_zaloz_konto_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.Resources["ustawienia"] as Ustawienia).Autostart = (bool)pole_czy_autostart.IsChecked;

            //POPRAWIC automatem oznacza jako informuj o wszystkich
            (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania = Serialowa.WersjaOdcinka.Oryginal;
            
            //int index = lista_wersji_poziomu_powiadomien.SelectedIndex;
            //switch (index)
            //{
            //    case 0:
            //        (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania = Serialowa.WersjaOdcinka.Oryginal;
            //        break;
            //    case 1:
            //        (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania = Serialowa.WersjaOdcinka.Napisy;
            //        break;
            //    case 2:                    
            //        (App.Current.Resources["profil"] as Profil).WersjaOdcinkaDoInformowania = Serialowa.WersjaOdcinka.Lektor;
            //        break;
            //}

            Ustawienia.zapisz();
            Profil.zapisz();
            KanalySeriali.zapisz();

            this.Close();

            //POLEPSZYC: okno z umowa
            //POLEPSZYC: okno z procesem tworzenia konta (pasek postepu) plus wgrywaneie plików z ustawieniami na serwer
        }

        private void przycisk_dodaj_Click_1(object sender, RoutedEventArgs e)
        {
            DodajSerial dodawanie = new DodajSerial();
            dodawanie.ShowDialog();
        }

        private void przycisk_wczytaj_Click(object sender, RoutedEventArgs e)
        {
            WczytanieZPlikuSeriali nowe = new WczytanieZPlikuSeriali();
            nowe.ShowDialog();
        }

    }
}
