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
using System.IO;
using System.Threading;
using Serialomaniak.Serialowa;
using Serialomaniak.Serialowa.Dystrybutorzy;
using System.Windows.Threading;

namespace Serialomaniak.Okna.Instalacja
{
    public partial class WczytanieZPlikuSeriali : Window
    {
        List<string> lista = new List<string>();

        public WczytanieZPlikuSeriali()
        {
            InitializeComponent();
        }

        private void przycisk_zaloz_konto_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void przycisk_wczytaj_Click(object sender, RoutedEventArgs e)
        {
            List<string> linijki = new List<string>();
            Microsoft.Win32.OpenFileDialog otworz = new Microsoft.Win32.OpenFileDialog();
            otworz.Filter = "Pliki z listą seriali Serialomaniaka (srl)|*.srl";

            StreamReader myStream = null;
            Nullable<bool> result = otworz.ShowDialog();
            if (result == true)
            {
                try
                {
                    if ((myStream = new StreamReader(otworz.FileName)) != null)
                    {
                        using (myStream)
                        {
                            string linijka_tekstu = "";
                            while ((linijka_tekstu = myStream.ReadLine()) != null)
                            {
                                linijki.Add(linijka_tekstu);
                            }
                        }
                    }
                }
                catch
                {
                    Program.loguj("Jakiś błąd przy wczytywaniu listy seriali z pliku");
                }
            }

            wczytaj_seriale(linijki);
        }

        private void wczytaj_seriale(List<string> z_pliku)
        {
            z_pliku.RemoveAt(0);//usowanie wpisu o wersji Serialomaniaka

            lista = z_pliku;
            pasek_postepu.Maximum = lista.Count;

            pasek_postepu.Visibility = Visibility.Visible;
            komunikat_na_blad.Visibility = Visibility.Visible;

            Thread watek = new Thread(new ThreadStart(watek_dodawania));
            watek.Start();
        }

        private void watek_dodawania()
        {
            ListaSeriali.generuj_liste_seriali_wszystkich();


            foreach (string s in lista)
            {
                string[] elementy = s.Split(new string[] {"$ser$"}, StringSplitOptions.RemoveEmptyEntries);
                string nazwa_serialu = elementy[0];
                //elementy[1] -nazwa odcinka
                //elementy[2] -numer sezonu
                //elementy[3] -numer odcinka
                int numer_odcinka_w_wszystkich_sezonach = Int32.Parse(elementy[4]);
                bool czy_ulubiony = bool.Parse(elementy[5]);

                string nazwa_odcinka="";//zostanie wczytana


                //if (nazwa_serialu.ToLower().Contains("Awkward.".ToLower()))
                //{
                //    nazwa_serialu = "Awkward";
                //}
                //if (nazwa_serialu.ToLower().Contains("The Secret Life".ToLower()))
                //{
                //    nazwa_serialu = "The Secret Life of the American ";
                //}

                if (!ListaSeriali.lista_seriali_wszystkich.Contains(nazwa_serialu))
                {
                    zglos_blad("Nie udało się dodać: " + nazwa_serialu);
                }
                else if (ListaSeriali.SerialoNazwie(nazwa_serialu) != null)//sprawdzam czy juz nie jest na liście
                {
                    zglos_blad("Serial: " + nazwa_serialu + " był już na liście");
                }
                else
                {
                    List<string> lista_odcinkow = ListaSeriali.generuj_liste_odcinkow_serialu(nazwa_serialu);
                    bool wystapil_blad = false;

                    if (lista_odcinkow.Count >= numer_odcinka_w_wszystkich_sezonach && numer_odcinka_w_wszystkich_sezonach != 0)
                    {
                        nazwa_odcinka = lista_odcinkow[numer_odcinka_w_wszystkich_sezonach - 1];
                    }
                    else
                    {
                        wystapil_blad = true;
                    }
                    //foreach (string odc in lista_odcinkow)
                    //{
                    //    if (odc.ToLower().Contains(nazwa_odcinka.ToLower()))
                    //    {
                    //        nazwa_odcinka = odc;
                    //        wystapil_blad = false;
                    //    }
                    //}
                    //if (wystapil_blad)
                    //{
                    //    nazwa_odcinka = "";
                    //}

                    if (ListaSeriali.SerialoNazwie(nazwa_serialu) == null)
                    {
                        string zdjecie_male = Sidereel.pobierz_zdjecie_serialu_male(nazwa_serialu);


                        string zdjecie_duze = TvCalendar.pobierz_zdjecie_serialu_duze(nazwa_serialu);


                        ///cat/imgs/sibig/The-Big-Bang-Theory.jpg
                        /////http://www.pogdesign.co.uk/cat/The-Big-Bang-Theory-summary

                        Serial nowy = new Serial(nazwa_serialu, nazwa_odcinka, zdjecie_male, zdjecie_duze, czy_ulubiony, "",0,"");

                        nowy.aktualizuj_liste_emisii_i_info_o_serialu();


                        Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(() =>
                                {
                                    ListaSeriali.dodaj_serial(nowy);
                                })
                               );
                    }

                    if (!wystapil_blad)
                    {
                        zglos_blad("Dodano: " + nazwa_serialu + ", oznaczono jako obejrzany: " + nazwa_odcinka);
                    }
                    else
                    {
                        zglos_blad("Dodano: " + nazwa_serialu + " ale nie znaleziono konkretnego odcinka");
                    }
                }


                Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() =>
                {
                    pasek_postepu.Value = pasek_postepu.Value + 1;
                })
                );
            }

            ListaSeriali.zapisz();

                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        przycisk_kontynuuj.IsEnabled = true;

                        pasek_postepu.Visibility = Visibility.Hidden;

                        komunikat_na_blad.Visibility = Visibility.Hidden;
                    })
                    );
        }

        private void zglos_blad(string tekst)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
        new Action(() =>
        {
            komunikat_na_blad.Content = tekst;
        })
       );
        }

        private void ZamknijOkno_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
