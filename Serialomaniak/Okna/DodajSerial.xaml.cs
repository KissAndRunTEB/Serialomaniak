using Biblioteka;
using Serialomaniak.Serialowa;
using Serialomaniak.Serialowa.Dystrybutorzy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Serialomaniak.Okna
{

    public partial class DodajSerial : Window
    {
        public DodajSerial()
        {
            InitializeComponent();
        }

        private void pole_lista_seriali_Loaded(object sender, RoutedEventArgs e)
        {
            pole_lista_seriali.ItemsSource = ListaSeriali.lista_seriali_wszystkich;

        }
        private void pole_lista_seriali_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Thread watek_listy_odcinkow = new Thread(new ThreadStart(stworz_liste_odcinkow));
            watek_listy_odcinkow.Start();

            pole_lista_odcinkow.IsEnabled = false;
            pole_lista_odcinkow.Items.Add("Trwa aktualizowanie listy odcinków serialu ...");
            pole_lista_odcinkow.SelectedIndex = 0;

            przycisk_dodaj_do_sledzonych.IsEnabled = true;
        }

        private void stworz_liste_odcinkow()
        {
            string tytul_serialu = "";
            Dispatcher.Invoke(DispatcherPriority.Normal,
              new Action(() =>
              {
                  tytul_serialu = pole_lista_seriali.SelectedItem as string;
              })
             );

           // List<string> lista = ListaSeriali.generuj_liste_odcinkow_serialu(tytul_serialu);
            if (tytul_serialu!=null)
            {
            Serial sztuczny = new Serial(tytul_serialu, "", "", "", false,"",0,"");
            sztuczny.aktualizuj_liste_emisii_i_info_o_serialu();

            List<Sezon> lista_sezonow = sztuczny.ListSezonow;


            Dispatcher.Invoke(DispatcherPriority.Normal,
              new Action(() =>
              {
                  pole_lista_odcinkow.Items.Clear();
                  for(int i=0; i< lista_sezonow.Count; i++)
                  {
                      pole_lista_odcinkow.Items.Add("Sezon "+(i+1));
                      foreach(Odcinek o in lista_sezonow[i].ListaOdcinkow)
                      {
                          if (o.DataEmisji.HasValue)
                          {
                              if (o.DataEmisji.Value.CompareTo(DateTime.Now) <= 0)
                              {
                                  pole_lista_odcinkow.Items.Add("["+(lista_sezonow[i].ListaOdcinkow.IndexOf(o)+1)+"] " + o.Tytul_odcinka);
                              }
                          }
                      }
                  }

                  pole_lista_odcinkow.IsEnabled = true;
              })
             );
            }
        }

        private void przycisk_dodaj_do_sledzonych_Click(object sender, RoutedEventArgs e)
        {
            przycisk_dodaj_do_sledzonych.IsEnabled = false;
            pasek_postepu.Value = 0;
            panel_dodawania_serialu.Visibility = Visibility.Visible;

            Thread watek_dodaj_serial = new Thread(new ThreadStart(dodaj_serial));
            watek_dodaj_serial.Start();
        }

        private void dodaj_serial()
        {
            string nazwa_serialu = "";
            string nazwa_odcinka = "";
            int numer_odcinka=0;
            int numer_sezonu = 0;

            Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(() =>
                    {
                        nazwa_serialu = pole_lista_seriali.SelectedItem.ToString();

                        if (pole_lista_odcinkow.SelectedItem != null)
                        {
                            numer_odcinka = Int32.Parse(pole_lista_odcinkow.SelectedItem.ToString().Split(' ')[0].Remove(2).Remove(0,1));
                            nazwa_odcinka =pole_lista_odcinkow.SelectedItem.ToString().Replace(pole_lista_odcinkow.SelectedItem.ToString().Split(' ')[0], "").Trim();

                            //zliczam slowa sezon poki nie trafi sie ten odcinek
                            bool ten_odcinek=false;
                            int licznik = 0;
                            while (!ten_odcinek)
                            {
                                if (pole_lista_odcinkow.Items[licznik] == pole_lista_odcinkow.SelectedItem)
                                {
                                    ten_odcinek = true;
                                }
                                else
                                {
                                    if (pole_lista_odcinkow.Items[licznik].ToString().Contains("Sezon"))
                                    {
                                        numer_sezonu++;
                                    }

                                    licznik++;                                    
                                }
                            }
                        }
                    })
                   );

            if (ListaSeriali.SerialoNazwie(nazwa_serialu)==null && (!nazwa_odcinka.Contains("Sezon")))
            {
                string zdjecie_male = Sidereel.pobierz_zdjecie_serialu_male(nazwa_serialu);
                zrob_krok();

                string zdjecie_duze = TvCalendar.pobierz_zdjecie_serialu_duze(nazwa_serialu);
                zrob_krok();

                ///cat/imgs/sibig/The-Big-Bang-Theory.jpg
                /////http://www.pogdesign.co.uk/cat/The-Big-Bang-Theory-summary

                Serial nowy = new Serial(nazwa_serialu, nazwa_odcinka, zdjecie_male, zdjecie_duze, false, "",0,"");                

                nowy.aktualizuj_liste_emisii_i_info_o_serialu();

                Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            //znajdz odcinek
                            Odcinek odcinek_obejrzany = nowy.ListSezonow[numer_sezonu - 1].ListaOdcinkow[numer_odcinka - 1];

                            //oznacz znaleziony jako obejrzany
                            nowy.obejrzane_do(odcinek_obejrzany);
                        })
                       );
                
                nowy.uzupelni_liste_linkow();

                zrob_krok();



                Dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() =>
                        {
                            ListaSeriali.dodaj_serial(nowy);
                        })
                       );
            }

            //ponizej dzieje sie nawet jak serial byl juz na liscie
            Dispatcher.Invoke(DispatcherPriority.Normal,
                                    new Action(() =>
                    {
                        panel_dodawania_serialu.Visibility = Visibility.Hidden;
                        przycisk_dodaj_do_sledzonych.IsEnabled = true;                        
                    })
                   );

            zrob_krok();
        }

        private void zrob_krok()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() =>
                {
                    pasek_postepu.Value++;
                })
               );
        }

        private void przycisk_zamknij_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StackPanel_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        	base.OnMouseLeftButtonDown(e);

            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void ZamknijOkno_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void pole_wyszukaj_TextChanged(object sender, TextChangedEventArgs e)
        {
            przefiltruj_liste(pole_wyszukaj.Text);
        }

        private void pole_wyszukaj_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_wyszukaj.Text.Contains("Wpisz fragment tytułu serialu ..."))
            {
                pole_wyszukaj.Text = "";
            }
            else
            {
                przefiltruj_liste(pole_wyszukaj.Text);
            }
        }

        private void przefiltruj_liste(string szukany)
        {
            List<string> z_filtrowane = Inteligencja.najtrafniejsze_wynik(szukany, 10, ListaSeriali.lista_seriali_wszystkich);


            
            pole_lista_seriali.ItemsSource = z_filtrowane;
        }
    }
}
