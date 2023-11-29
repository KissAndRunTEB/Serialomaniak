using Biblioteka;
using Serialomaniak.Konta;
using Serialomaniak.Okna;
using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;
using System.Xml.Serialization;

namespace Serialomaniak
{
    public static class Program
    {
        public static string nazwa = "Serialomaniak";
        public static string wersja = "0.021";

        #region Instalacja

        public static bool nowy()
        {
            //jest albo nowa instalacja albo aktualizacja
            DirectoryInfo folder_programu = new DirectoryInfo(Adresy.program);
            if (folder_programu.Exists)
            {
                return false;
            }

            return true;
        }

        public static bool zainstaluj()
        {
            if (nowy())
            {
                tworz_foldery_programu();

                InstalacjaStart nowa = new InstalacjaStart();
                nowa.ShowDialog();

                bool czy_autostart = Ustawienia.Obecne.Autostart;
                Autostart.przelacz_autostart(czy_autostart, nazwa);

                Aktualizacja nowa_instalacja = new Aktualizacja("Witamy!", "Zainstalowano wersje " + Program.wersja, TypAktualizacji.nowa_instalacja);
                nowa_instalacja.Show();

                return true;
            }
            else
            {
                return false;
            }
        }

        public static void tworz_foldery_programu()
        {
            //tworzenie folderów
            DirectoryInfo folder_programu = new DirectoryInfo(Adresy.program);
            folder_programu.Create();
            DirectoryInfo folder_na_ustawienia = new DirectoryInfo(Adresy.ustawienia);
            folder_na_ustawienia.Create();
            DirectoryInfo folder_na_grafike = new DirectoryInfo(Adresy.grafika);
            folder_na_grafike.Create();
            DirectoryInfo folder_na_linki = new DirectoryInfo(Adresy.linki);
            folder_na_linki.Create();
            DirectoryInfo folder_na_seriale= new DirectoryInfo(Adresy.seriale);
            folder_na_seriale.Create();
            DirectoryInfo folder_na_przyjaciol = new DirectoryInfo(Adresy.przyjaciele);
            folder_na_przyjaciol.Create();
        }

        #endregion

        #region Aktualizacja

        public static void komunikat_o_aktualizacji_ukonczonej()
        {
            if ((App.Current.Resources["ustawienia"] as Ustawienia).Wersja != Program.wersja)
            {
                aktualizuj_do_najnowszej();
            }

            (App.Current.Resources["ustawienia"] as Ustawienia).Wersja = Program.wersja;
            Ustawienia.zapisz();
        }

        private static void aktualizuj_do_najnowszej()
        {
            Aktualizacja nowa = new Aktualizacja("Aktualizacja" + Program.wersja, "Aktualizacja przebiegła poprawnie", TypAktualizacji.aktualizacja);
            nowa.Show();

            Aktualizacja nowa_2 = new Aktualizacja("Ważna aktualizacja", "Pierwsze wydanie nowej edycji Serialomaniaka", TypAktualizacji.informacja);
            nowa_2.Show();
        }

        public static void aktualizuj_w_tle()
        {
            //Zmiany utrzymywane az wszyscy nie zaaktualizuja
            //do 1.02
            Directory.CreateDirectory(Adresy.seriale);


            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                ad.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(zdarzenie_pelnego_sprawdzania);

                ad.CheckForUpdateAsync();
            }
        }

        #region Sprawdzanie aktualizacji

        private static void zdarzenie_pelnego_sprawdzania(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.UpdateAvailable)
            {
                if (e.IsUpdateRequired)
                {
                    //rozpoczyna aktualizacje tylko jak jest wazna
                    RozpocznijAktualizacje();
                }
                else
                {
                    Aktualizacja nowa_2 = new Aktualizacja("Aktualizacja opcjonalna", "Wykryto drobną aktualizacje, w wolnej chwilii zainstaluj ją.", TypAktualizacji.informacja);
                    nowa_2.Show();
                }
            }

            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.CheckForUpdateCompleted -= zdarzenie_pelnego_sprawdzania;
        }

        private static void RozpocznijAktualizacje()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += zdarzenie_Koniec_aktualizacji;
            ad.UpdateAsync();
        }

        static void zdarzenie_Koniec_aktualizacji(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Aktualizacja nowa_2 = new Aktualizacja("Ważna aktualizacja", "Zaaktualizowano Serialomaniaka, nowe funkcje będą dostępne po restarcie aplikacji.", TypAktualizacji.informacja);
            nowa_2.Show();

            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted -= zdarzenie_Koniec_aktualizacji;            
        }

        #endregion

        #endregion


        #region System Operacyjny
        public static WersjaSystemu system()
        {
            //http://msdn.microsoft.com/en-us/library/ms724832(VS.85).aspx
            //http://www.nirmaltv.com/2009/08/17/windows-os-version-numbers/

            WersjaSystemu obecna = WersjaSystemu.StarszyOdXp;

            int wersja = Environment.OSVersion.Version.Major;
            int pod_wersja = Environment.OSVersion.Version.Minor;

            if (wersja == 6 & pod_wersja > 1)
            {
                obecna = WersjaSystemu.NowszyOdWin7;
            }
            else if (wersja == 6 & pod_wersja == 1)
            {
                obecna = WersjaSystemu.Win7;
            }
            else if (wersja == 6 & pod_wersja == 0)
            {
                obecna = WersjaSystemu.Vista;
            }
            else if (wersja == 5)
            {
                obecna = WersjaSystemu.XP;
            }

            return obecna;
        }

        public enum WersjaSystemu
        {
            StarszyOdXp,
            XP,
            Vista,
            Win7,
            NowszyOdWin7
        }
        #endregion

        #region Serializacja
        public static void Serializuj_do_XML<T>(T objekt, string nazwa)
        {
            StreamWriter zapisywanie = new StreamWriter(Adresy.ustawienia + "//" + nazwa + ".xml");

            XmlRootAttribute korzen = new XmlRootAttribute();
            korzen.ElementName = nazwa;
            korzen.IsNullable = true;

            XmlSerializer serializator = new XmlSerializer(typeof(T), korzen);
            serializator.Serialize(zapisywanie, objekt);

            zapisywanie.Close();
        }

        public static T Deserializuj_z_XML<T>(string nazwa, string skad = "")
        {
            if (skad == "")
            {
                skad = Adresy.ustawienia;
            }
            StreamReader odczytywanie = new StreamReader(skad + "//" + nazwa + ".xml");

            XmlRootAttribute korzen = new XmlRootAttribute();
            korzen.ElementName = nazwa;
            korzen.IsNullable = true;

            T wynik;
            XmlSerializer deserilizowanie = new XmlSerializer(typeof(T), korzen);
            wynik = (T)deserilizowanie.Deserialize(odczytywanie);

            odczytywanie.Dispose();
            odczytywanie.Close();

            return wynik;
        }
        #endregion

        #region Rozszerzenia (Animacja)
        //Rozszerzenia do klasy UIElement
        /// <summary>
        /// Rozpoczyna animacje
        /// </summary>
        /// <param name="od_wartosci">Wartość do której startuje zmiana</param>
        /// <param name="do_wartosci">Wartość do której następuje zmiana</param>
        /// <param name="czas">Czas animacji w formacie h:m:s</param>
        /// <returns>Nic nie zwraca</returns>
        public static void animuj_przezroczystosc(this UIElement element, double od_wartosci, double do_wartosci, string czas)
        {
            element.Opacity = od_wartosci;

            DoubleAnimation animacja = new DoubleAnimation();
            animacja.To = do_wartosci;
            animacja.Duration = new Duration(TimeSpan.Parse(czas));

            element.BeginAnimation(UIElement.OpacityProperty, animacja);
        }
        #endregion

        #region Logowanie do pliku

        public static void loguj(string tekst)
        {
            string nazwa_pliku = Adresy.ustawienia + "//" + "Log" + ".txt";

            StreamWriter pisanie = File.AppendText(nazwa_pliku);
            pisanie.Write(DateTime.Now.ToString("HH:mm:ss"));
            pisanie.Write((char)9);
            pisanie.WriteLine(tekst);
            pisanie.WriteLine("");

            pisanie.Flush();
            pisanie.Close();
        }

        public static void loguj_start()
        {
            string nazwa_pliku = Adresy.ustawienia + "//" + "Log" + ".txt";

            //FileInfo plik = new FileInfo(nazwa_pliku);
            //if (File.Exists(nazwa_pliku))
            //{
            //    File.Delete(nazwa_pliku);
            //}

            StreamWriter pisanie = File.AppendText(nazwa_pliku);

            pisanie.WriteLine("----------------------------------------------------");

            pisanie.Write("Uruchomienie Serialomaniaka ");
            pisanie.Write(wersja);
            pisanie.Write((char)9);
            pisanie.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
            pisanie.WriteLine("");

            pisanie.Flush();
            pisanie.Close();
        } 
        #endregion

        //POPRAWIC zapisywanie (dodac zapisywanie listy seriali)
        public static void zapisz_na_serwerze()
        {
            string nazwa_pliku = Adresy.ustawienia + "\\" + "Log" + ".txt";

            if (Internet.czy_polaczenie())
            {
                Internet.wgraj_na_serwer(Serwery.Serialomaniak, nazwa_pliku, "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Ludzie/" + (App.Current.Resources["profil"] as Profil).AdresMail + "\\" + "Log.txt");
            }
        }

        public static List<Zmiana> stworz_liste_zmian()
        {
            //przy katualziacjach wymaganych pamietaj zmienic NUMER WERSJI WYMAGANEJ w propertis

            List<Zmiana> lista = new List<Zmiana>();

            lista.Add(new Zmiana("Lista seriali zawiera teraz 710 seriali!", "", "0.02"));
            lista.Add(new Zmiana("Zmiany wyglądu interfejsu aplikacji", "", "0.02"));
            lista.Add(new Zmiana("Wyszukiwarka seriali, przy dodawaniu ich do listy", "", "0.02"));            
            lista.Add(new Zmiana("Uproszczenie okna dodawania nowego serialu do listy", "", "0.02"));
            lista.Add(new Zmiana("Poprawiono zbieranie informacji o błędach", "", "0.02"));
            lista.Add(new Zmiana("Usunięto kilka błędów powodujących utrate stabilności", "", "0.02"));

            lista.Add(new Zmiana("Usprawnienie komunikat aktualizacji", "Asia", "0.01"));

            lista.Add(new Zmiana("Oznaczanie i odznaczanie odcinków jako obejrzanych", "Asia", "0.01"));
            lista.Add(new Zmiana("Licznik odcinków do obejrzenia (w ikonie paska zadań)", "Asia", "0.01"));
            lista.Add(new Zmiana("Usprawnienie pojawiania się okna (z ikony paska zadań)", "Asia", "0.01"));

            lista.Add(new Zmiana("Drobne błędy ortograficzne", "Artur", "0.01"));
            lista.Add(new Zmiana("Naprawa dodawania seriali zawierających apostrof", "Artur", "0.01"));

            lista.Add(new Zmiana("Usprawnienie procesu aktualizacji", "", "0.01"));
            lista.Add(new Zmiana("Dodanie informacji o postępie procesu aktualizacji", "", "0.01"));
            lista.Add(new Zmiana("Okno Lista Zmian", "", "0.01"));

            return lista;
        }
    }
}
