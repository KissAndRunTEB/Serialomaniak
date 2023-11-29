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
    public partial class WyslijSugestie : Window
    {
        public WyslijSugestie()
        {
            InitializeComponent();
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

        private void pole_na_wiadomosc_GotFocus(object sender, RoutedEventArgs e)
        {
            pole_na_wiadomosc.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            przycisk_wyslij.IsEnabled = false;

            string wiadomosc = pole_na_wiadomosc.Text;
            string nick = (App.Current.Resources["profil"] as Profil).Nick;
            string mail = (App.Current.Resources["profil"] as Profil).AdresMail;

            if (Internet.czy_polaczenie())
            {
                Internet.wyślij_maila_na_ds("Serialomaniak, " + nick, wiadomosc, nick, mail);

                pole_na_wiadomosc.Text = "Wysłane, dziękujemy w następnej wersji postaram się wziąść Twoje sugestie pod uwagę.";

                przycisk_wyslij.IsEnabled = true;
            }

            przycisk_wyslij.Content = "Dziękujemy!";
            przycisk_wyslij.IsEnabled = false;
        }     


    }
}
