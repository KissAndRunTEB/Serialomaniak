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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;
using Serialomaniak.Okna.Informacji;

namespace Serialomaniak.Okna
{

    public partial class Aktualizacja : Window
    {
        //POLEPSZYC: zastapic System.Windows.Forms Screen czyms stabilniejszym (dodatkowo wymagalo doddania referencji + system drawnig)

        string informacja;
        string szczegoly;
        TypAktualizacji typ;

        Storyboard ScenariuszZaladowania;


        public Aktualizacja(string informacja, string szczegoly, TypAktualizacji typ)
        {
            InitializeComponent();

            this.informacja = informacja;
            this.szczegoly = szczegoly;
            this.typ = typ;

            pole_na_naglowek.Content = this.informacja;
            pole_na_szczegoly.Text = this.szczegoly;


            #region Zmiana obrazka

            if (typ == TypAktualizacji.aktualizacja)
            {
                this.zdjecie.Source = new BitmapImage(new Uri("/Serialomaniak;component/Okna/Grafika/aktualizacja.png", UriKind.Relative));
            }
            else if (typ == TypAktualizacji.nowa_funkcja)
            {
                this.zdjecie.Source = new BitmapImage(new Uri("/Serialomaniak;component/Okna/Grafika/ikona_info.png", UriKind.Relative));
            }
            else if (typ == TypAktualizacji.nowa_instalacja)
            {
                this.zdjecie.Source = new BitmapImage(new Uri("/Serialomaniak;component/Okna/Grafika/instalacja.png", UriKind.Relative));
            }
            else if (typ == TypAktualizacji.informacja)
            {
                this.zdjecie.Source = new BitmapImage(new Uri("/Serialomaniak;component/Okna/Grafika/aktualizacja.png", UriKind.Relative));
            }
            else
            {
                this.zdjecie.Source = new BitmapImage(new Uri("/Serialomaniak;component/Okna/Grafika/aktualizacja.png", UriKind.Relative));
            }

            #endregion

            if ((App.Current.Resources["ustawienia"] as Ustawienia).PowiadomieniaWycentrowane)
            {
                aktualizacja.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                if (!(App.Current.Resources["ustawienia"] as Ustawienia).PoprawkaPolozenia)
                {
                    aktualizacja.Top = Screen.PrimaryScreen.WorkingArea.Height - aktualizacja.Height - (App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien * aktualizacja.Height;
                    aktualizacja.Left = Screen.PrimaryScreen.WorkingArea.Width - aktualizacja.Width;
                }
                else
                {
                    aktualizacja.Top = Screen.PrimaryScreen.WorkingArea.Height - aktualizacja.Height - (App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien * aktualizacja.Height - 100;
                    aktualizacja.Left = Screen.PrimaryScreen.WorkingArea.Width - aktualizacja.Width - 200;
                }
            }

            //gdy okno sie zaladuje wlacza animacje
            this.Loaded += new RoutedEventHandler(zaladowane_okno);

            //animacja konczy się zamknięciem okna
            utworz_animacje_okna();

            Ustawienia.dodano_powiadomienie();
        }



        #region Animacja

        private void utworz_animacje_okna()
        {
            DoubleAnimationUsingKeyFrames myDoubleAnimation = new DoubleAnimationUsingKeyFrames();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(8));
            myDoubleAnimation.BeginTime = TimeSpan.FromSeconds((0));

            LinearDoubleKeyFrame d1 = new LinearDoubleKeyFrame();
            d1.KeyTime = TimeSpan.FromSeconds((6));
            d1.Value = 0.9;
            LinearDoubleKeyFrame d2 = new LinearDoubleKeyFrame();
            d2.KeyTime = TimeSpan.FromSeconds((8));
            d2.Value = 0;


            myDoubleAnimation.KeyFrames.Add(d1);
            myDoubleAnimation.KeyFrames.Add(d2);

            ScenariuszZaladowania = new Storyboard();
            ScenariuszZaladowania.Children.Add(myDoubleAnimation);
            Storyboard.SetTargetName(myDoubleAnimation, this.Name);
            Storyboard.SetTargetProperty(myDoubleAnimation, new PropertyPath(Window.OpacityProperty));

            ScenariuszZaladowania.Completed += new EventHandler(scenariusz_sie_zakonczyl);
        }

        #endregion

        #region Obsługa zdarzen

        private void zaladowane_okno(object sender, RoutedEventArgs e)
        {
            ScenariuszZaladowania.Begin(this);

            if ((App.Current.Resources["ustawienia"] as Ustawienia).PowiadomieniaWycentrowane)
            {
                aktualizacja.Top = aktualizacja.Top + ((App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien - 1) * (aktualizacja.Height + 5);
            }
        }

        private void scenariusz_sie_zakonczyl(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Interakcje

        private void aktualizacja_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                Ustawienia.usunieto_powiadomienie();
        }

        #endregion

        private void aktualizacja_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Informacje nowe = new Informacje();
            nowe.Show();
        }

    }
    public enum TypAktualizacji
    {
        aktualizacja,
        nowa_funkcja,
        nowa_instalacja,
        informacja
    }

}
