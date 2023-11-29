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

namespace Serialomaniak.Okna.Instalacja
{
    /// <summary>
    /// Interaction logic for InstalacjaZaloguj.xaml
    /// </summary>
    public partial class InstalacjaZaloguj : Window
    {
        public InstalacjaZaloguj()
        {
            InitializeComponent();
        }

        private void pole_login_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_login.Text == "wpisz numer profilu" || ((SolidColorBrush)pole_login.Foreground).Color == Colors.Red)
            {
                pole_login.Text = "";
                pole_login.Foreground = new SolidColorBrush(Colors.Black);
            }

            przycisk_zaloguj.Content = "Załóż konto";
        }

        private void pole_haslo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pole_haslo.Password == "hasło" || ((SolidColorBrush)pole_haslo.Foreground).Color == Colors.Red)
            {
                pole_haslo.Password = "";
                pole_haslo.Foreground = new SolidColorBrush(Colors.Black);
            }

            przycisk_zaloguj.Content = "Załóż konto";
        }

        private void przycisk_zaloguj_Click(object sender, RoutedEventArgs e)
        {
            //POLEPSZYC: Logowanie gdy już jest na serwerze konto
        }
    }
}
