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

    public partial class ListaZmian : Window
    {
        public ListaZmian()
        {
            InitializeComponent();

            pole_lista_zmian.ItemsSource = Program.stworz_liste_zmian();
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


    }
}
