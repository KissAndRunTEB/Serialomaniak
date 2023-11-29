using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteka
{
    static public class Rozszerzenia
    {
        #region Rozszerzenia Stringa

        /// <summary>
        /// Wycina fragment tekstu
        /// </summary>
        /// <param name="co">Fragment z którego wycina</param>
        /// <param name="poczatek">Fragment od którego wycina nowy fragment tekstu</param>
        /// <param name="koniec">Opcjonalny fragment tekstu do którego wycina</param>
        /// <returns>Wycięty fragment tekstu</returns>
        static public string Pomiedzy(this string co, string poczatek, string koniec = null)
        {
            string wynik = "";

            if (koniec == null)
            {
                wynik = co.Substring(co.IndexOf(poczatek) + poczatek.Length, co.Count() - co.IndexOf(poczatek) - poczatek.Length);
            }
            else if (poczatek == "")
            {
                wynik = co.Substring(0, co.IndexOf(koniec));
            }
            else
            {
                wynik = co.Substring(co.IndexOf(poczatek) + poczatek.Length, co.IndexOf(koniec) - co.IndexOf(poczatek) - poczatek.Length);
            }

            return wynik;
        }

        /// <summary>
        /// Usuwa znaczniki html
        /// </summary>
        /// <param name="tekst">Tekst z którego zostaną usunięte znaczniki</param>
        /// <returns>Zwraca tekst bez znaczników html</returns>
        static public string Usun_znaczniki_html(this string tekst, bool czy_zawartosc_tez=false)
        {
            string wynik = tekst;

            bool jest_co_kasowac = true;

            while (jest_co_kasowac)
            {
                int index_od = wynik.IndexOf('<');
                int index_do = wynik.IndexOf('>');

                if (czy_zawartosc_tez)
                {
                    index_do = wynik.LastIndexOf('>');
                }

                if (index_od == -1 || index_do == -1)
                {
                    jest_co_kasowac = false;
                }
                else
                {
                    wynik = wynik.Remove(index_od, index_do - index_od + 1);
                }
            }
            return wynik;
        }

        /// <summary>
        /// Zamienia polskie znaki (ąęćśłżźóń) na (aecslzo)
        /// </summary>
        /// <param name="tekst">Tekst w którym zostaną zastąpione polskie znaki</param>
        /// <returns>Zwraca tekst bez polskich znaków ()</returns>
        static public string Bez_polskich_znakow(this string tekst)
        {
            string napis = tekst;
            napis = napis.Replace("ą", "a");
            napis = napis.Replace("ę", "e");
            napis = napis.Replace("ć", "c");
            napis = napis.Replace("ś", "s");
            napis = napis.Replace("ł", "l");
            napis = napis.Replace("ż", "z");
            napis = napis.Replace("ź", "z");
            napis = napis.Replace("ó", "o");
            napis = napis.Replace("ń", "n");


            napis = napis.Replace("Ą", "A");
            napis = napis.Replace("Ę", "E");
            napis = napis.Replace("Ć", "C");
            napis = napis.Replace("Ś", "Z");
            napis = napis.Replace("Ł", "L");
            napis = napis.Replace("Ż", "Z");
            napis = napis.Replace("Ź", "Z");
            napis = napis.Replace("Ó", "O");
            napis = napis.Replace("Ń", "N");
            return napis;
        }


        /// <summary>
        /// Usówa znaki specjalne (.,;:$&)
        /// </summary>
        /// <param name="tekst">Tekst w którym zostaną usunięte znaki specjalne</param>
        /// <returns>Zwraca tekst bez znaków specjalnych</returns>
        static public string Bez_znakow_specjalnych(this string tekst)
        {
            string napis = tekst;
            napis = napis.Replace(".", "");
            napis = napis.Replace(",", "");
            napis = napis.Replace(";", "");
            napis = napis.Replace(":", "");
            napis = napis.Replace("&", "");
            napis = napis.Replace("$", "");
            return napis;
        }

        /// <summary>
        /// Usówa z tekstu wskazany fragment (wielokrotnie)
        /// </summary>
        /// <param name="tekst">Tekst w którym zostanie wycięte kazde wystąpienie wskazanego fragmentu</param>
        /// <returns>Zwraca tekst wystąpień wskazanego fragmentu</returns>
        static public string Bez_fragmentu(this string tekst, string fragment)
        {
            string napis = tekst;
            napis = napis.Replace(fragment, "");
            return napis;
        }


        /// <summary>
        /// Konwertuje tekst reprezentujacy liczbe uzupełniony zerami na Int
        /// </summary>
        /// <param name="tekst">Tekst do konwersji</param>
        /// <returns>Liczba reprezentujaca wartość po konwersji</returns>
        static public int NaLiczbeGdyPoprzedonaZerami(this string tekst)
        {
            string napis = tekst;

            int wynik = 0;

            for (int i = 0; i < napis.Count(); i++)
            {
                int skladowa=int.Parse(napis[napis.Count() - 1 - i].ToString());
                wynik += skladowa * (int)Math.Pow(10, i);

            }

            return wynik;
        }

        #endregion

        #region Rozszerzenia Inta

        //Rozszerzenia do klasy Int
        /// <summary>
        /// Zamienia cyfre na tekst o ustalonej długosci
        /// </summary>
        /// <param name="numer">Numer do konwersji na tekst</param>
        /// <param name="dlugosc">Dlugosc wynikowego tekstu</param>
        /// <returns>Zwraca tekst o określonej długości reprezentujący liczbę</returns>
        static public string Na_tekst_o_dlugosci(this int numer, int dlugosc)
        {
            string napis = numer.ToString();

            while (dlugosc > napis.Length)
            {
                napis = "0"+napis;
            }

            return napis;
        }

        #endregion

        #region Rozszerzenia DateTime?

        //Rozszerzenia do klasy DateTime?
        /// <summary>
        /// Zamienia date na tekst
        /// </summary>
        /// <param name="data">Data do konwersji</param>
        /// <returns>Zwraca tekst lub pusty string</returns>
        static public string na_tekst(this DateTime? data)
        {
            string napis = "";

            if (data.HasValue)
            {
                napis = data.Value.ToShortDateString();
            }
            return napis;
        }

        //Rozszerzenia do klasy DateTime?
        /// <summary>
        /// Sprawdza czy daty są obok siebie w podanym zakresie dni
        /// </summary>
        /// <param name="data">Data do porownania</param>
        /// <param name="druga_data">Druga data do porownania</param>
        /// <param name="liczba_dni">Liczba dni określająca zakres</param>
        /// <returns>Zwraca true jesli daty nie rozni wiecej niz podana liczba dni, jesli roznia sie lub ktoras jest pusta zwraca false</returns>
        static public bool czy_data_w_ciagu_dni(this DateTime? data, DateTime? druga_data, int liczba_dni)
        {
            if (data.HasValue && druga_data.HasValue)
            {
                if (data.Value.CompareTo(druga_data.Value) == 0)
                {
                    return true;
                }
                else if (data.Value.CompareTo(druga_data.Value) > 0)
                {
                    if (druga_data.Value.AddDays(liczba_dni).CompareTo(data) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (data.Value.AddDays(liczba_dni).CompareTo(druga_data) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
