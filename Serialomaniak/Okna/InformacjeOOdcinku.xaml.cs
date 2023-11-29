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
    public partial class InformacjeOOdcinku : Window
    {
        public InformacjeOOdcinku(Odcinek odcinek)
        {
            this.InitializeComponent();

            okno_informacji_o_odcinku.DataContext = odcinek;

            panel_na_informacje_serialowe.DataContext = ListaSeriali.SerialoNazwie(odcinek.NazwaSerialu);

            pole_lista_linkow.ItemsSource = odcinek.LinkiDoOdcinka;

            if(pole_lista_linkow.Items.Count!=0)
            {
                pole_lista_linkow.Visibility = Visibility.Visible;
            }
        }

        private void przycisk_obejrzyj_teraz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void pole_na_oznaczenie_Checked(object sender, RoutedEventArgs e)
        {
            (sender as CheckBox).Foreground = new SolidColorBrush(Colors.White);
            (sender as CheckBox).Content = " Obejrzany";

            if(((sender as CheckBox).DataContext as Odcinek).Czy_jest_ostnim_obejrznym_odcinkiem==true)
            {
            ((sender as CheckBox).DataContext as Odcinek).oznacz_jako_obejrzany();
            }
            //zbindowane do wlasciwosci Obejrzane
        }

        private void pole_na_oznaczenie_Loaded(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked.Value == true)
            {
                (sender as CheckBox).Foreground = new SolidColorBrush(Colors.White);
                (sender as CheckBox).Content = " Obejrzany";
            }
        }

        private void pole_na_oznaczenie_Unchecked(object sender, RoutedEventArgs e)
        {
            (sender as CheckBox).Foreground = Resources["Kolor_akcentu"] as SolidColorBrush;
            (sender as CheckBox).Content = " Oznacz jako obejrzany";

            ((sender as CheckBox).DataContext as Odcinek).oznacz_jako_nie_obejrzany();

        }

        private void pole_lista_linkow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pole_lista_linkow.SelectedItem != null)
            {
                pole_na_oznaczenie.IsChecked = true;
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

        private void okno_informacji_o_odcinku_Loaded(object sender, RoutedEventArgs e)
        {
            this.animuj_przezroczystosc(0, 1, "0:0:1");
        }
    }
}