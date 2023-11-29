using Serialomaniak.Serialowa;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Serialomaniak
{
    public partial class App : Application
    {

        #region Interakcje wspólne

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (sender as Hyperlink);
            if (link != null)
            {
                Process.Start(link.NavigateUri.OriginalString);
            }
        }


        private void Grid_MouseEnter_2(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //ustawione na konkretna animacje stack panela w gridzie
            ((sender as Grid).Children[((sender as Grid).Children.Count) - 1] as StackPanel).animuj_przezroczystosc(0, 1, "0:0:1");
        }

        private void Grid_MouseLeave_2(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //ustawione na konkretna animacje stack panela w gridzie
            ((sender as Grid).Children[((sender as Grid).Children.Count) - 1] as StackPanel).animuj_przezroczystosc(1, 0, "0:0:1");
        }

        private void Image_MouseDown_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Serial klikniety = (((sender as Image).DataContext) as Serial);

            if (klikniety != null)
            {
                InformacjeOSerialu nowe = new InformacjeOSerialu(klikniety);
                nowe.Show();
            }
        }

        private void Image_MouseDown_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Serial klikniety = (((sender as Image).DataContext) as Serial);

            if (klikniety != null)
            {
                if (klikniety.NastepnyOdcinek != null)
                {
                    InformacjeOOdcinku nowe = new InformacjeOOdcinku(klikniety.NastepnyOdcinek);
                    nowe.ShowDialog();
                }
            }
        }

        private void Image_MouseDown_3(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Serial klikniety = (((sender as Image).DataContext) as Serial);

            if (klikniety != null)
            {
                klikniety.Ulubiony = !klikniety.Ulubiony;

                ListaSeriali.posortuj_kontrolke();
            }

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            if (((sender as CheckBox).DataContext as Odcinek).Czy_jest_ostnim_obejrznym_odcinkiem == true)
            {
                ((sender as CheckBox).DataContext as Odcinek).oznacz_jako_obejrzany();
            }
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            Odcinek odcinek = ((sender as CheckBox).DataContext as Odcinek);

            Serial s = ListaSeriali.SerialoNazwie(odcinek.NazwaSerialu);

            // s.poinformuj_o_zmianie_obejrzenia_jakiegos_odcinka();


            bool oznaczono = false;

            foreach (Sezon sezon in s.ListSezonow)
            {
                foreach (Odcinek o in sezon.ListaOdcinkow)
                {
                    if (odcinek != null)
                    {
                        if (o.Equals(odcinek))
                        {
                            o.Obejrzany = false;

                            oznaczono = true;
                        }
                        else
                        {
                            o.Obejrzany = (!oznaczono);
                        }
                    }
                }
            }
            s.poinformuj_o_zmianie_obejrzenia_jakiegos_odcinka();


        } 
        #endregion

    }
}
