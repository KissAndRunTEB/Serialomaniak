using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Biblioteka;

namespace Serialomaniak.Konta.Konwertery
{
    [ValueConversion(typeof(DateTime?), typeof(String))]
    public class KonwerterDaty : IValueConverter
    {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                DateTime? date = (DateTime?)value;
                return date.na_tekst();
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
