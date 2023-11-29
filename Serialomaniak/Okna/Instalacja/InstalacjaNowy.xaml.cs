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

namespace Serialomaniak.Okna.Instalacja
{
    public partial class InstalacjaNowy : Window
    {
        public InstalacjaNowy()
        {
            InitializeComponent();
        }

        private void pole_nazwa_profilu_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_nazwa_profilu.Text == "wpisz nazwe profilu" || ((SolidColorBrush)pole_nazwa_profilu.Foreground).Color == Colors.Red)
            {
            pole_nazwa_profilu.Text = "";
            pole_nazwa_profilu.Foreground = new SolidColorBrush(Colors.Black);
            }

            przycisk_zaloz_konto.Content = "Załóż konto";
        }

        private void pole_adres_e_mail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_adres_e_mail.Text == "wpisz adres e-mail" || ((SolidColorBrush)pole_adres_e_mail.Foreground).Color == Colors.Red)
            {
                pole_adres_e_mail.Text = "";
                pole_adres_e_mail.Foreground = new SolidColorBrush(Colors.Black);
            }

            przycisk_zaloz_konto.Content = "Załóż konto";
        }

        private void pole_haslo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_haslo.Password == "hasło" || ((SolidColorBrush)pole_haslo.Foreground).Color == Colors.Red)
            {
                pole_haslo.Password = "";
                pole_haslo.Foreground = new SolidColorBrush(Colors.Black);
            }

            przycisk_zaloz_konto.Content = "Załóż konto";
        }

        private void przycisk_zaloz_konto_Click(object sender, RoutedEventArgs e)
        {
            if (pole_nazwa_profilu.Text.Length < 5 || ((SolidColorBrush)pole_nazwa_profilu.Foreground).Color == Colors.Red)
            {
                pole_nazwa_profilu.Text = "nazwa musi mieć conajmniej 5 znaków";
                pole_nazwa_profilu.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (pole_adres_e_mail.Text.Length < 5 || (!pole_adres_e_mail.Text.Contains("@")) || ((SolidColorBrush)pole_adres_e_mail.Foreground).Color == Colors.Red)
            {
                pole_adres_e_mail.Text = "popraw adres e-mail";
                pole_adres_e_mail.Foreground = new SolidColorBrush(Colors.Red);
            }

            if (pole_haslo.Password.Length < 7 || ((SolidColorBrush)pole_haslo.Foreground).Color == Colors.Red)
            {
                pole_haslo.Foreground = new SolidColorBrush(Colors.Red);
            }

            przycisk_zaloz_konto.IsEnabled = false;
            przycisk_zaloz_konto.Content = "Trwa sprawdzanie dostępności...";

            //POLEPSZYC: w nowym watku aby nie blokowalo

            if ((((SolidColorBrush)pole_nazwa_profilu.Foreground).Color != Colors.Red) && (((SolidColorBrush)pole_adres_e_mail.Foreground).Color != Colors.Red) && (((SolidColorBrush)pole_haslo.Foreground).Color != Colors.Red))
            {
                if (pole_nazwa_profilu.Text.Length > 4 && pole_adres_e_mail.Text.Length > 4 && pole_haslo.Password.Length>6 )
                {
                    if (pole_adres_e_mail.Text.Contains("@"))
                    {
                        if (czy_adres_email_jest_wolny(pole_adres_e_mail.Text))
                        {
                            if (zaloz_konto(pole_nazwa_profilu.Text, pole_adres_e_mail.Text, pole_haslo.Password) == 0)
                            {
                                przycisk_zaloz_konto.Content="Błąd połączenia z serwerem, spróbuj później";
                            }
                        }
                        else
                        {
                            pole_adres_e_mail.Text = "Podany adres e-mail jest już zajęty";
                            pole_adres_e_mail.Foreground = new SolidColorBrush(Colors.Red);
                        }
                    }
                }
            }

            przycisk_zaloz_konto.Content = "Załóż konto";
            przycisk_zaloz_konto.IsEnabled = true;
        }

        private bool czy_adres_email_jest_wolny(string adres)
        {
            bool czy_wolny = true;

            // NAPRAWIC: Zamienic sprawdzanie poprzez odczyt pliku na sprawdzanei poprzez wylistowanie nową funkcją czy_istnieje
            if (Internet.pobierz_strone("http://www.singit.pl/Aplikacje/Serialomaniak/Ludzie/"+adres)!=null)
            {
                czy_wolny = false;   
            }

            return czy_wolny;
        }

        private int zaloz_konto(string nazwa, string adres_email, string haslo)
        {
            // POLEPSZYC: Zaszyfruj haslo

            int numer = 0;

            numer = zarejestruj_uzytkownika_na_serwerze(nazwa, adres_email, haslo);

            if (numer != 0)
            {
                Profil.zaloz_nowy(nazwa, adres_email, haslo, numer);
                Ustawienia.zaloz_nowe(Program.wersja);

                if (Program.system() == Program.WersjaSystemu.Vista)
                {
                    (App.Current.Resources["ustawienia"] as Ustawienia).PoprawkaPolozenia = true;
                }

                Profil.zapisz();
                Ustawienia.zapisz();

                po_zalogowaniu(numer);
            }

            return numer;
        }

        private int zarejestruj_uzytkownika_na_serwerze(string nazwa, string adres, string haslo)
        {
            int numer = 0;

            if (Internet.czy_polaczenie())
            {
                numer = dodaj_wpis_o_nowym_koncie(adres);
            }

            return numer;
        }

        private int dodaj_wpis_o_nowym_koncie(string adres)
        {
            int numer = 0;

            #region Stworz unikalny folder
            string folder = "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Ludzie/" + adres;
            Internet.stworz_folder_na_serwerze(Serwery.Serialomaniak, folder);
            #endregion

            numer=Internet.wylistuj_folder_serwera(Serwery.Serialomaniak, "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Ludzie",true).Count;
            
            numer+=1231230;
            return numer;
        }

        private void po_zalogowaniu(int numer_profilu)
        {
            this.Hide();
            InstalacjaSzczegoly nowa = new InstalacjaSzczegoly(numer_profilu);
            nowa.ShowDialog();
            this.Close();
        }
    }
}
