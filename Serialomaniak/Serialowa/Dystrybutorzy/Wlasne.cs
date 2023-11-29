using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Biblioteka;
using System.Windows.Threading;
using System.Threading;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace Serialomaniak.Serialowa.Dystrybutorzy
{
    class Wlasne
    {
        //POPRAWIC dodac uzupelnianie listy odcinkow takze z serwera to co teraz jest w klasie Stvplus

        static public List<Link> uzupelnij_liste_linkow(Odcinek o)
        {
            string nazwa_pliku = o.NazwaSerialu + " " + o.NumerSezonu + " " + o.NumerOdcinka;
            nazwa_pliku = nazwa_pliku.ToLower();
            string adres_pliku = "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Linki/" + nazwa_pliku + ".xml";
            string adres_www_pliku = "http://www.singit.pl/Aplikacje/Serialomaniak/Linki/" + nazwa_pliku + ".xml";

            string adres_do_celowy = Adresy.linki + "\\" + nazwa_pliku + ".xml";

            if (Internet.zgraj_z_serwera(Serwery.Serialomaniak, adres_pliku, adres_do_celowy, true, adres_www_pliku))
            {
                return Program.Deserializuj_z_XML<List<Link>>(nazwa_pliku, Adresy.linki);
            }

            return new List<Link>();
        }

    }
}
