using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteka
{
    public class Angielski
    {
        static public int z_skroconej_nazwy_na_numer_miesiaca(string nazwa)
        {
            int numer = 1;

            switch (nazwa)
            {
                case "Jan":
                    numer = 1;
                    break;
                case "Feb":
                    numer = 2;
                    break;
                case "Mar":
                    numer = 3;
                    break;
                case "Apr":
                    numer = 4;
                    break;
                case "May":
                    numer = 5;
                    break;
                case "Jun":
                    numer = 6;
                    break;
                case "Jul":
                    numer = 7;
                    break;
                case "Aug":
                    numer = 8;
                    break;
                case "Sep":
                    numer = 9;
                    break;
                case "Oct":
                    numer = 10;
                    break;
                case "Nov":
                    numer = 11;
                    break;
                case "Dec":
                    numer = 12;
                    break;
                default:
                    break;
            }

            return numer;
        }
    }
}
