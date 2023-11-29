using Serialomaniak.Serialowa;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Serialomaniak
{
    public partial class InformacjeOSerialu : Window
    {
        public InformacjeOSerialu(Serial serial)
        {
            this.InitializeComponent();

            okno_informacji_o_serialu.DataContext = serial;
        }

        private void przycisk_obejrzyj_teraz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pole_na_liste_sezonow_Loaded(object sender, RoutedEventArgs e)
        {
            pole_na_liste_sezonow.SelectedIndex = pole_na_liste_sezonow.Items.Count - 1;
        }

        private void pole_na_liste_sezonow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sezon sezon = pole_na_liste_sezonow.SelectedItem as Sezon;
            if ((sezon) != null)
            {
                pole_na_lista_odcinkow.DataContext = sezon;
                pole_na_lista_odcinkow.Visibility = Visibility.Visible;
            }
            else
            {
                pole_na_lista_odcinkow.Visibility = Visibility.Hidden;
            }
        }

        private void pole_na_lista_odcinkow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Odcinek odcinek = pole_na_lista_odcinkow.SelectedItem as Odcinek;
            if (odcinek != null)
            {
                InformacjeOOdcinku nowe = new InformacjeOOdcinku(odcinek);
                nowe.ShowDialog();
            }
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

        private void okno_informacji_o_serialu_Loaded_1(object sender, RoutedEventArgs e)
        {
            okno_informacji_o_serialu.animuj_przezroczystosc(0, 1, "0:0:1");
        }
    }
}