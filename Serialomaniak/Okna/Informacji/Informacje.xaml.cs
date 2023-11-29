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
using Biblioteka;
using System.Deployment.Application;
using System.ComponentModel;

namespace Serialomaniak.Okna.Informacji
{
    public partial class Informacje : Window
    {
        long rozmiar_aktualizacji = 0;

        MainWindow okno;

        public Informacje(MainWindow okno=null)
        {
            InitializeComponent();
            this.okno = okno;
        }

        private void przycisk_ok_Click(object sender, System.Windows.RoutedEventArgs e)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WyslijSugestie okno = new WyslijSugestie();
            okno.ShowDialog();
        }

        private void tekst_na_komunikat_o_aktualizacji_Loaded(object sender, RoutedEventArgs e)
        {
            //zeby to testowac trzeba zrobic publishbo inaczej nie jest to NetworkDeployment

            //NIE PRZEJMOWAC SIE ŻE NAPIS SIE NIE ZMIENIA GDY TESTOWANY W VS

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                ad.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(zdarzenie_pelnego_sprawdzania);
                ad.CheckForUpdateProgressChanged += new DeploymentProgressChangedEventHandler(zdarzenie_zmiany_stanu_sprawdzania);

                ad.CheckForUpdateAsync();
            }
        }

        #region Sprawdzanie aktualizacji

        void zdarzenie_zmiany_stanu_sprawdzania(object sender, DeploymentProgressChangedEventArgs e)
        {
            tekst_na_komunikat_o_aktualizacji.Content = String.Format("Pobieranie: {0}. {1:D}K z {2:D}K pobrane.", napis_pobierania(e.State), e.BytesCompleted / 1024, e.BytesTotal / 1024);
        }
        string napis_pobierania(DeploymentProgressState stan)
        {
            if (stan == DeploymentProgressState.DownloadingApplicationFiles)
            {
                return "pliki aplikacji";
            }
            else if (stan == DeploymentProgressState.DownloadingApplicationInformation)
            {
                return "pliki manifestu aplikacji";
            }
            else
            {
                return "rozpakowywanie manifestu";
            }
        }
        void zdarzenie_pelnego_sprawdzania(object sender, CheckForUpdateCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Program.loguj("BŁĄD: Nie można było otrzymac nowej wersji aplikacji. Powód: \n" + e.Error.Message);
                MessageBox.Show("Błąd podczas pobierania aktualizacji.");
                return;
            }
            else if (e.Cancelled == true)
            {
                Program.loguj("Ta aktualizacja została anulowana.");
                MessageBox.Show("Ta aktualizacja została anulowana.");
            }

            if (e.UpdateAvailable)
            {
                rozmiar_aktualizacji = e.UpdateSizeBytes;
                przycisk_aktualizuj.IsEnabled = true;
                przycisk_aktualizuj.Visibility = Visibility.Visible;

                if (e.IsUpdateRequired)
                {
                    tekst_na_komunikat_o_aktualizacji.Content = "Aktualizacja ma status WAŻNEJ.";
                }
                else
                {
                    tekst_na_komunikat_o_aktualizacji.Content = "Aktualizacja ma status zalecanej.";
                }
            }
            else
            {
                tekst_na_komunikat_o_aktualizacji.Content = "Nie ma dostępnej aktualizacji.";
            }
        }
        #endregion

        private void RozpocznijAktualizacje()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += new AsyncCompletedEventHandler(zdarzenie_Skonczenia_aktualizacji);
            ad.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(zdarzenie_Zmiany_stanu_aktualizacji);
            ad.UpdateAsync();
        }

        void zdarzenie_Zmiany_stanu_aktualizacji(object sender, DeploymentProgressChangedEventArgs e)
        {
            String tekstPostepu = String.Format("{0:D}K z {1:D}K pobrane - {2:D}% całości", e.BytesCompleted / 1024, e.BytesTotal / 1024, e.ProgressPercentage);
            tekst_na_komunikat_o_aktualizacji.Content = tekstPostepu;
        }

        void zdarzenie_Skonczenia_aktualizacji(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Aktualizacja została anulowana.");
                return;
            }
            else if (e.Error != null)
            {
                Program.loguj("BŁĄD: Nie można była zainstalować najnowszej aktualizacji. Powód: \n" + e.Error.Message + "\n.");
                MessageBox.Show("BŁĄD: Nie można była zainstalować najnowszej aktualizacji. Powód: \n" + e.Error.Message + "\n.");
                return;
            }

            MessageBoxResult dr = MessageBox.Show("Aplikacja została zaaktualizowana. Uruchomić ją ponownie? (Jeśli nie uruchomisz aplikacji ponownie, zmiany z aktualizacji nie będą dostępne do momentu ponownej aktualizacji)", "Kończenie aktualizacji", MessageBoxButton.YesNo);
            if (MessageBoxResult.Yes == dr)
            {
                if (okno != null)
                {
                    okno.zamknij_program(true);
                }
            }
        }

        private void przycisk_odwiedz_Click(object sender, RoutedEventArgs e)
        {
            Internet.idz_do_strony("http://www.singit.pl/Aplikacje/Serialomaniak/Wita/");
        }

        private void przycisk_lista_zmian_Click(object sender, RoutedEventArgs e)
        {
            ListaZmian okno = new ListaZmian();
            okno.ShowDialog();
        }

        private void przycisk_aktualizuj_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RozpocznijAktualizacje();
        }



    }
}
