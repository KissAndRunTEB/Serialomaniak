using Biblioteka;
using Serialomaniak.Konta;
using Serialomaniak.Okna;
using Serialomaniak.Okna.Informacji;
using Serialomaniak.Serialowa;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Serialomaniak
{
    public partial class MainWindow : Window
    {
        Thread watek_sprawdz_nowe_odcinki;
        Thread watek_lista_seriali;
        Thread watek_lista_odcinkow_seriali;

        System.Windows.Forms.NotifyIcon ikona_w_przyborniku;

        //sprawdz Task List na dole w Error List

        //POLEPSZYC z danych o ogladalnosci zrobic dwa wykresy (ogladalnosc/ogladalnosc 18-49)

        //POLEPSZYC dodac rozpoznanie blednej instalacji aplikacji i blednego zamkniecia aplikacji lub usuniecie przy unistalacji takze plikow z folderu Roaming/Serialomaniak
        //moze wylapywac w try catch blad podczas Program.zainstaluj?
        
        public MainWindow()
        {
            bool nowy = Program.zainstaluj();
            if (nowy)
            {
                Program.loguj("Zainstalowano poprawnie");
            }

            Program.loguj_start();

            Program.aktualizuj_w_tle();
            Program.loguj("Zaaktualizowano program2222222222222");

            Profil.wczytaj();
            Program.loguj("Wczytano Profil");
            Ustawienia.wczytaj();
            Program.loguj("Wczytano Ustawienia");
            ListaSeriali.wczytaj();
            Program.loguj("Wczytano Liste seriali");

            Program.komunikat_o_aktualizacji_ukonczonej();

            //Program.aktualizuj();
            //Program.aktualizuj_w_tle();
            //Program.loguj("Zaaktualizowano program");


            InitializeComponent();
            Program.loguj("Udana inicjalizacja");

            #region Wyglad okna
            if (nowy)
            {
                if (!(App.Current.Resources["ustawienia"] as Ustawienia).PoprawkaPolozenia)
                {
                    this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.Height;
                    this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                }
                else
                {
                    this.Top = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.Height - 100;
                    this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - this.Width - 200;
                }
            }
            else
            {
                this.Top = Ustawienia.Obecne.Gora;
                this.Left = Ustawienia.Obecne.Lewy;

                this.WindowState = WindowState.Minimized;                
                this.Hide();
            }

            zamienienie_alt_f4_na_zaprogramowane_wylaczenie();

            stworz_ikone_w_przyborniku_systemowym();

            #endregion


            wczytaj_liste_seriali_wszystkich();

            ListaSeriali.posortuj_kontrolke();

            Program.loguj("Udane uruchomienie");

            Program.zapisz_na_serwerze();           


            sprawdzaj_nowe_linki();

            zaaktualizuj_liste_emisji_odcinkow();
        }

        #region Watki
        private void zaaktualizuj_liste_emisji_odcinkow()
        {
            watek_lista_odcinkow_seriali = new Thread(new ThreadStart(watek_zaaktualizuj_liste_emisji_odcinkow));
            watek_lista_odcinkow_seriali.Start();
        }

        public void watek_zaaktualizuj_liste_emisji_odcinkow()
        {
            //odczekanie aby zaladowala sie lista seriali
            Thread.Sleep(Convert.ToInt32(30 * 1000));

            while (true)
            {
                Program.loguj("Rozpoczęcie aktualizowania listy odcinków ...");

                if (Internet.czy_polaczenie())
                {
                    for (int i = 0; i < ListaSeriali.LiczbaSeriali; i++)
                    {
                        Serial s = ListaSeriali.SerialoIndeksie(i);

                        s.aktualizuj_liste_emisii_i_info_o_serialu();
                    }
                }
                ListaSeriali.zapisz();

                Program.loguj("... koniec aktualizowania listy odcinków");

                Thread.Sleep(Convert.ToInt32(60 * 60 * 4 * 1000));
            }
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
                      przycisk_dodaj.ToolTip = "Dodaj serial";
                      przycisk_dodaj.animuj_przezroczystosc(0, 1, "0:0:2");
                  })
                 );
        }

        private void sprawdzaj_nowe_linki()
        {
            watek_sprawdz_nowe_odcinki = new Thread(new ThreadStart(watek_nowe_odcinki));
            watek_sprawdz_nowe_odcinki.Start();
        }

        public void watek_nowe_odcinki()
        {
            //odczekanie aby zaladowala sie lsita seriali
            Thread.Sleep(Convert.ToInt32(15 * 1000));

            while (true)
            {
                if (Internet.czy_polaczenie())
                {
                    for (int i = 0; i < ListaSeriali.LiczbaSeriali; i++)
                    {
                        Serial s = ListaSeriali.SerialoIndeksie(i);

                        s.uzupelni_liste_linkow();

                        s.zaaktualizuj_zdjecia();
                    }
                }
                Thread.Sleep(Convert.ToInt32(Ustawienia.co_ile_pobierac_linki));
                ListaSeriali.zapisz();
            }
        } 
        #endregion

        #region Menu kontekstowe listy seriali i ikona w przyborniku

        private void stworz_ikone_w_przyborniku_systemowym()
        {
            ikona_w_przyborniku = new System.Windows.Forms.NotifyIcon();
            ikona_w_przyborniku.Icon = Properties.Resources.ikona_programu;
            ikona_w_przyborniku.Visible = true;

            ikona_w_przyborniku.MouseClick += delegate(object sender, System.Windows.Forms.MouseEventArgs e)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    if (this.WindowState != WindowState.Normal)
                    {
                        this.Topmost = true;
                        this.Show();
                        this.WindowState = WindowState.Normal;
                        //Ustawienia.Obecne.Gora;
                        //this.Top = Ustawienia.Obecne.Gora;
                        //this.Left = Ustawienia.Obecne.Lewy;

                        this.Topmost = false;
                    }
                    else
                    {
                        this.WindowState = WindowState.Minimized;
                        this.Hide();
                    }
                }
            };

            ikona_w_przyborniku.MouseMove += delegate(object sender, System.Windows.Forms.MouseEventArgs args)
            {
                ikona_w_przyborniku.Text = "Oglądasz: " + ListaSeriali.LiczbaSeriali + " seriali, do obejrzenia: " + ListaSeriali.OdcinkowDoObejrzenia + " odcinków.";
            };

            System.Windows.Forms.ContextMenuStrip menu = new System.Windows.Forms.ContextMenuStrip();
            ikona_w_przyborniku.ContextMenuStrip = menu;

            menu.Items.Add("Dodaj serial");
            menu.Items.Add("-");

            menu.Items.Add("Zamknij");

            menu.ItemClicked += delegate(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs args)
            {
                if (args.ClickedItem.Text == "Dodaj serial")
                {
                    DodajSerial nowy = new DodajSerial();
                    nowy.ShowDialog();
                }
                else if (args.ClickedItem.Text == "Zamknij")
                {
                    zamknij_program();
                }
            };
        }

        //PILNE Klikniecie linku lub odchaczenia powoduje oznaczenie serialu jako obejrzanego

        private void lista_seriali_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.ContextMenu menu = new System.Windows.Controls.ContextMenu();
            lista_seriali.ContextMenu = menu;

            MenuItem elem_obejrzyj = new MenuItem();
            elem_obejrzyj.Header = "Obejrzyj odcinek";
            elem_obejrzyj.Click += new RoutedEventHandler(elem_obejrzyj_Click);
            menu.Items.Add(elem_obejrzyj);

            menu.Items.Add(new Separator());

            MenuItem elem_bez_ogladania = new MenuItem();
            elem_bez_ogladania.Header = "Oznacz bez oglądania";
            elem_bez_ogladania.Click += new RoutedEventHandler(elem_bez_ogladania_Click);
            elem_bez_ogladania.Loaded += elem_bez_ogladania_Loaded;

            menu.Items.Add(elem_bez_ogladania);

            MenuItem elem_cofnij_ogladanie = new MenuItem();
            elem_cofnij_ogladanie.Header = "Cofnij oznaczenie";
            elem_cofnij_ogladanie.Click += new RoutedEventHandler(elem_cofnij_ogladanie_Click);
            menu.Items.Add(elem_cofnij_ogladanie);

            menu.Items.Add(new Separator());

            MenuItem elem_usun = new MenuItem();
            elem_usun.Header = "Usuń serial";
            elem_usun.Click += new RoutedEventHandler(elem_usun_Click);
            menu.Items.Add(elem_usun);
        }

        void elem_bez_ogladania_Loaded(object sender, RoutedEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                Serial s = (lista_seriali.SelectedItem as Serial);
                if (s.NastepnyOdcinek == null)
                {
                    (sender as MenuItem).IsEnabled = false;
                }
            }
        }

        void elem_bez_ogladania_Click(object sender, RoutedEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                Serial s = (lista_seriali.SelectedItem as Serial);
                s.NastepnyOdcinek.oznacz_jako_obejrzany();
            }
        }

        void elem_usun_Click(object sender, RoutedEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                Serial s = (lista_seriali.SelectedItem as Serial);

                ListaSeriali.usun_serial(s);
            }
        }

        void elem_obejrzyj_Click(object sender, RoutedEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                if ((lista_seriali.SelectedItem as Serial).NastepnyOdcinek != null)
                {
                    InformacjeOOdcinku nowe = new InformacjeOOdcinku((lista_seriali.SelectedItem as Serial).NastepnyOdcinek);
                    nowe.ShowDialog();
                }
            }
        }

        void elem_cofnij_ogladanie_Click(object sender, RoutedEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                Serial s = (lista_seriali.SelectedItem as Serial);
                s.PoprzedniOdcinek.oznacz_jako_nie_obejrzany();
            }
        }

        #endregion


        public void zamknij_program(bool zrestartowac=false)
        {
            Program.loguj("Zamykanie programu ...");


            if (watek_lista_seriali != null)
            {
                watek_lista_seriali.Abort();
            }
            if (watek_sprawdz_nowe_odcinki != null)
            {
                watek_sprawdz_nowe_odcinki.Abort();
            }
            if (watek_lista_odcinkow_seriali != null)
            {
                watek_lista_odcinkow_seriali.Abort();
            }

            Profil.zapisz();
            Ustawienia.zapisz();
            KanalySeriali.zapisz();

            ListaSeriali.zapisz();

            ikona_w_przyborniku.Visible = false;
            ikona_w_przyborniku.Icon = null;
            ikona_w_przyborniku.Dispose();

            this.Visibility = Visibility.Hidden;

            Program.loguj("... program został zamknięty.");

            if(zrestartowac)
            {
                System.Windows.Forms.Application.Restart();
            }

            System.Windows.Application.Current.Shutdown();
        }

        #region Interakcje
        private void lista_seriali_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((lista_seriali.SelectedItem as Serial) != null)
            {
                InformacjeOSerialu nowe = new InformacjeOSerialu(lista_seriali.SelectedItem as Serial);
                nowe.Show();
            }
        }

        private void przycisk_zamknij_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            //nadpisane aby dodac automatyczne chowanie po minimalizacji
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }

        private void przycisk_informacje_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Informacje nowe = new Informacje(this);
            nowe.ShowDialog();
        }


        private void przycisk_dodaj_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if ((sender as Image).Opacity == 1)
            {
                DodajSerial nowy = new DodajSerial();
                nowy.ShowDialog();
            }
        }

        private void przycisk_ustawienia_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UstawieniaOkno nowe = new UstawieniaOkno();
            nowe.ShowDialog();
        }

        private void Grid_MouseLeftButtonDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window 
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();

                Ustawienia.Obecne.Gora = this.Top;
                Ustawienia.Obecne.Lewy = this.Left;
            }
        }

        private void przycisk_zamknij_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Image).animuj_przezroczystosc(0.1, 1, "0:0:2");
        }

        private void przycisk_zamknij_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Image).animuj_przezroczystosc(1, 0.1, "0:0:2");
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.animuj_przezroczystosc(0, 1, "0:0:1");
        }

        private void zamienienie_alt_f4_na_zaprogramowane_wylaczenie()
        {
            this.KeyDown += MainWindow_KeyDown;
        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.System && e.SystemKey == Key.F4)
            {
                e.Handled = true;
                this.WindowState = WindowState.Minimized;
                this.Hide();
            }
        } 
        #endregion
    }
}

