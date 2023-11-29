using Biblioteka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa.Dystrybutorzy
{
    class TvCalendar
    {
        static public string pobierz_zdjecie_serialu_duze(string nazwa_serialu)
        {
            if(nazwa_serialu=="Vampire Diaries")
            {
                nazwa_serialu = "The Vampire Diaries";
            }
            if (nazwa_serialu.Contains("Secret Life of the American"))
            {
                nazwa_serialu = "The Secret Life of the American Teenager";
            }
            if (nazwa_serialu.Contains("Two Broke Girls"))
            {
                nazwa_serialu = "2 Broke Girls";
            }

            string adres = Adresy.grafika+"/" + nazwa_serialu +"_dlugie"+ ".jpg";

            string przeksztalcona_nazwa=nazwa_serialu.Replace(" ","-");

            //http://www.pogdesign.co.uk/cat/imgs/sibig/The-Big-Bang-Theory.jpg
            string skad = "http://www.pogdesign.co.uk/cat/imgs/sibig/" + przeksztalcona_nazwa + ".jpg";

            Internet.zgraj_z_www(skad, adres);

            return adres;
        }
    }
}
