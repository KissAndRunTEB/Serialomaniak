using Serialomaniak.Okna.Instalacja;
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

namespace Serialomaniak.Okna
{
    public partial class InstalacjaStart : Window
    {
        public InstalacjaStart()
        {
            InitializeComponent();
        }

        private void przycisk_nowe_konto_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();

            InstalacjaNowy nowa = new InstalacjaNowy();
            nowa.ShowDialog();

            this.Close();
        }

        private void przycisk_mam_juz_konto_Click(object sender, RoutedEventArgs e)
        {
            InstalacjaZaloguj nowa = new InstalacjaZaloguj();
            nowa.Show();

            this.Close();
        }
    }
}
