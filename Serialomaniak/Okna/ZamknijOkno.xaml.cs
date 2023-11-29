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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Serialomaniak
{
	public partial class ZamknijOkno : UserControl
	{
		public ZamknijOkno()
		{
			this.InitializeComponent();
		}

        #region Interakcja
        private void przycisk_zamknij_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Image).animuj_przezroczystosc(0.1, 1, "0:0:2");
        }

        private void przycisk_zamknij_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Image).animuj_przezroczystosc(0, 0.1, "0:0:2");
        } 
        #endregion
	}
}