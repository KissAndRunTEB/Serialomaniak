using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Biblioteka
{
    public class Inteligencja
    {
        public static List<string> najtrafniejsze_wynik(string poszukiwany_tekst, int liczba_wynikow, List<string> lista)
        {
            List<string> lista_wynikow = new List<string>();

            List<int> wyniki = new List<int>();

            foreach (string s in lista)
            {
                wyniki.Add(jak_podobne(s, poszukiwany_tekst));
            }

            while (liczba_wynikow != 0)
            {
                int indeks_najlepszego=0;
                int wynik_najlepszego=-1;

                for(int i =0; i<wyniki.Count; i++)
                {
                    if(wyniki[i]>wynik_najlepszego)
                    {
                        wynik_najlepszego=wyniki[i];
                        indeks_najlepszego=i;
                    }
                }

                //wynik dodanego ustawiamy na ujemny
                wyniki[indeks_najlepszego] = -1*liczba_wynikow;

                lista_wynikow.Add(lista[indeks_najlepszego]);
                liczba_wynikow--;
            }

            return lista_wynikow;
        }

        public static int jak_podobne(string pierwszy, string drugi)
        {
            int wynik = (int)(100*oblicz_podobienstwo(pierwszy, drugi));

                return wynik;
        }

        private static float oblicz_podobienstwo(string pierwszy, string drugi)
        {
            float odleglosc = oblicz_odleglosc(pierwszy, drugi);
            float maksymalna_dlugosc = pierwszy.Length;
            if (maksymalna_dlugosc < drugi.Length)
                maksymalna_dlugosc = drugi.Length;
            if (maksymalna_dlugosc == 0.0F)
            {
                return 1.0F;
            }
            else
            {
                return 1.0F - odleglosc / maksymalna_dlugosc;
            }
        }

        private static int oblicz_odleglosc(string pierwszy, string drugi)
        {
            int n = pierwszy.Length;
            int m = drugi.Length;
            int[,] odleglosci = new int[n + 1, m + 1];
            int koszt = 0;
            if (n == 0) return m;
            if (m == 0) return n;
            for (int i = 0; i <= n; odleglosci[i, 0] = i++) ;
            for (int j = 0; j <= m; odleglosci[0, j] = j++) ;

            //znajdz najmniejsza odleglosc
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    koszt = (drugi.Substring(j - 1, 1) ==
                        pierwszy.Substring(i - 1, 1) ? 0 : 1);
                    odleglosci[i, j] = minimum_z_trzech(
                        odleglosci[i - 1, j] + 1,
                    odleglosci[i, j - 1] + 1,
                    odleglosci[i - 1, j - 1] + koszt);
                }
            }
            return odleglosci[n, m];
        }

        public static int minimum_z_trzech(int a, int b, int c)
        {
            return Math.Min(Math.Min(a, b), c);
        }
    }
}
