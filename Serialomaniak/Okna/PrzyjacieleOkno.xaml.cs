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

    public partial class PrzyjacieleOkno : Window
    {
        public PrzyjacieleOkno()
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string dane = pole_na_dane_przyjaciela.Text;
            pole_na_dane_przyjaciela.Text = "";
        }

    }
}
