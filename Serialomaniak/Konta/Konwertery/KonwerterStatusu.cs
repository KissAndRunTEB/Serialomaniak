using Serialomaniak.Serialowa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Serialomaniak.Konta.Konwertery
{    
    [ValueConversion(typeof(StatusSerialu), typeof(String))]
    class KonwerterStatusu : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StatusSerialu status = (StatusSerialu)value;

            if (targetType == typeof(string))
            {
                if (status == StatusSerialu.Nadawany)
                {
                    return "OBECNIE NADAWANY";
                }
                else if (status == StatusSerialu.Nowy)
                {
                    return "NOWY W TYM SEZONIE";
                }
                else if (status == StatusSerialu.Powroci)
                {
                    return "SERIAL POWRÓCI, ZOSTAŁ ZAMÓWIONY NOWY SEZON!";
                }
                else if (status == StatusSerialu.FinalowySezon)
                {
                    return "EMITOWANY JEST FINAŁOWY SEZON SERIALU";
                }
                else if (status == StatusSerialu.DecydujeSie)
                {
                    return "OBECNIE NADAWANY";
                }
                else if (status == StatusSerialu.Anulowany)
                {
                    return "SERIAL ZOSTAŁ ANULOWANY, NIE BĘDZIE JUŻ KONTYNUUOWANY";
                }
                else if (status == StatusSerialu.Zakonczony)
                {
                    return "SERIAL NIE JEST JUŻ EMITOWANY";
                }
            }
            else if (targetType == typeof(Brush))
            {
                if (status == StatusSerialu.Nadawany)//zielony
                {
                    return new SolidColorBrush(Color.FromArgb(255, 108, 255, 0));
                }
                else if (status == StatusSerialu.Nowy)//niebieski
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                else if (status == StatusSerialu.Powroci)//rozowy
                {
                    return new SolidColorBrush(Color.FromArgb(255, 255, 0, 80));
                }
                else if (status == StatusSerialu.FinalowySezon)//fiolet
                {
                    return new SolidColorBrush(Colors.Violet);
                }
                else if (status == StatusSerialu.DecydujeSie)//pomaranczowy
                {
                    return new SolidColorBrush(Colors.Orange);
                }
                else if (status == StatusSerialu.Anulowany)//czerwony
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else if (status == StatusSerialu.Zakonczony)//czarny
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string strValue = value as string;
            //DateTime resultDateTime;
            //if (DateTime.TryParse(strValue, out resultDateTime))
            //{
            //    return resultDateTime;
            //}
            //return DependencyProperty.UnsetValue;

            //NAPRAWIC dodac kownersje wsteczna

            return new DateTime();
        }
    }
}
