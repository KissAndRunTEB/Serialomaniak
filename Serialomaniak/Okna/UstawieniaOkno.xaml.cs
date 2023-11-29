using Serialomaniak.Okna.Instalacja;
using Serialomaniak.Serialowa;
using Serialomaniak.Serialowa.Dystrybutorzy;
using System;
using System.Collections.Generic;
using System.IO;
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

    public partial class UstawieniaOkno : Window
    {
        public UstawieniaOkno()
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

        private void przycisk_zapisz_Click(object sender, RoutedEventArgs e)
        {            
            Microsoft.Win32.SaveFileDialog okno_dialogowe = new Microsoft.Win32.SaveFileDialog();
            okno_dialogowe.FileName = "lista seriali";
            okno_dialogowe.DefaultExt = ".serialomaniak";
            okno_dialogowe.Filter = "Pliki Serialomaniaka (.srl)|*.srl";

            Nullable<bool> result = okno_dialogowe.ShowDialog();

            
            if (result == true)
            {
                string adres_zapisu = okno_dialogowe.FileName;

                FileInfo plik = new FileInfo(adres_zapisu);
                if (File.Exists(adres_zapisu))
                {
                    File.Delete(adres_zapisu);
                }

                StreamWriter pisanie = File.CreateText(adres_zapisu);
                pisanie.WriteLine("Lista seriali Serialomaniaka " + Program.wersja);

                for (int i = 0; i < ListaSeriali.LiczbaSeriali; i++)
                {
                    Serial s = ListaSeriali.SerialoIndeksie(i);
                    pisanie.WriteLine(s.Nazwa + "$ser$"
                        + s.ObejrzanyOdcinekTytul + "$ser$"
                        + s.ObejrzanyOdcinekNumerSezonu + "$ser$"
                        + s.ObejrzanyOdcinekNumerOdcinka + "$ser$"
                        + s.ObejrzanyOdcinekNumerOdcinkaWWszystkichSezonach + "$ser$"
                        + s.Ulubiony
                        );
                }

                pisanie.Flush();
                pisanie.Close();
            }
        }

        private void przycisk_wczytaj_Click(object sender, RoutedEventArgs e)
        {
            WczytanieZPlikuSeriali nowe = new WczytanieZPlikuSeriali();
            nowe.ShowDialog();
        }

    }
}
