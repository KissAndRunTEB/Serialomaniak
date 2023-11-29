using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa
{
    public class Sortowanie : IComparer
    {
        public int Compare(object x, object y)
        {
            Serial pierwszy = x as Serial;
            Serial drugi = y as Serial;

            int porownanie = pierwszy.LiczbaDoOdcinkowDoObejrzenia.CompareTo(drugi.LiczbaDoOdcinkowDoObejrzenia);

            if (porownanie == 0)
            {
                porownanie = (-1) * pierwszy.Nazwa.CompareTo(drugi.Nazwa);
            }

            //uwzglednienie ulubionego
            if (pierwszy.Ulubiony && drugi.Ulubiony)
            {
                return (-1) * porownanie;
            }
            else if (pierwszy.Ulubiony)
            {
                porownanie = 1;
            }
            else if (drugi.Ulubiony)
            {
                porownanie = -1;
            }


            return (-1) * porownanie;
        }
    }
}
