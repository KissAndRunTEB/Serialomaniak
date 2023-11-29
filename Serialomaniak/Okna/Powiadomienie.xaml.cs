using Serialomaniak.Serialowa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Serialomaniak.Okna
{
    public partial class Powiadomienie : Window
    {
        Storyboard ScenariuszZaladowania;

        //konieczne poniewaz po kliknieciu powiadomienia pojawia sie okno informacji o serialu
        Serial serial_informacje;

        public Powiadomienie(Odcinek o)
        {
            InitializeComponent();

            serial_informacje =ListaSeriali.SerialoNazwie(o.NazwaSerialu);

            #region Polozenie na ekranie 
            if ((App.Current.Resources["ustawienia"] as Ustawienia).PowiadomieniaWycentrowane)
            {
                powiadomienie.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            else
            {
                if (!(App.Current.Resources["ustawienia"] as Ustawienia).PoprawkaPolozenia)
                {
                    powiadomienie.Top = Screen.PrimaryScreen.WorkingArea.Height - powiadomienie.Height - (App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien * powiadomienie.Height;
                    powiadomienie.Left = Screen.PrimaryScreen.WorkingArea.Width - powiadomienie.Width;
                }
                else
                {
                    powiadomienie.Top = Screen.PrimaryScreen.WorkingArea.Height - powiadomienie.Height - (App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien * powiadomienie.Height - 100;
                    powiadomienie.Left = Screen.PrimaryScreen.WorkingArea.Width - powiadomienie.Width - 200;
                }
            }

            //gdy okno sie zaladuje wlacza animacje
            this.Loaded += new RoutedEventHandler(zaladowane_okno);

            //animacja konczy się zamknięciem okna
            utworz_animacje_okna();

            #endregion

            Ustawienia.dodano_powiadomienie();

            powiadomienie.DataContext = o;
            zdjecie.DataContext = serial_informacje;
        }

        #region Animacja

        private void utworz_animacje_okna()
        {
            DoubleAnimationUsingKeyFrames myDoubleAnimation = new DoubleAnimationUsingKeyFrames();
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(12));
            myDoubleAnimation.BeginTime = TimeSpan.FromSeconds((0));

            LinearDoubleKeyFrame d1 = new LinearDoubleKeyFrame();
            d1.KeyTime = TimeSpan.FromSeconds((8));
            d1.Value = 0.9;
            LinearDoubleKeyFrame d2 = new LinearDoubleKeyFrame();
            d2.KeyTime = TimeSpan.FromSeconds((12));
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
                powiadomienie.Top = powiadomienie.Top + ((App.Current.Resources["ustawienia"] as Ustawienia).LiczbaPowiadomien - 1) * (powiadomienie.Height + 5);
            }
        }

        private void scenariusz_sie_zakonczyl(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Interakcje

        private void powiadomienie_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Ustawienia.usunieto_powiadomienie();
        }

        private void powiadomienie_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //SerialInfo nowe = new SerialInfo(this.serial_informacje);
            //nowe.Show();

            Odcinek o = this.DataContext as Odcinek;
            InformacjeOOdcinku nowe = new InformacjeOOdcinku(o);
            nowe.ShowDialog();

        }

        #endregion
    }
}
