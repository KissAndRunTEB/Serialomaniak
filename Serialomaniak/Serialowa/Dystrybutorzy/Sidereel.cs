using Biblioteka;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa.Dystrybutorzy
{
    class Sidereel
    {
        static public string pobierz_zdjecie_serialu_male(string nazwa_serialu)
        {
            if (nazwa_serialu.Contains("Secret Life of the American"))
            {
                nazwa_serialu = "The Secret Life of the American Teenager";
            }
            if (nazwa_serialu.Contains("Two Broke Girls"))
            {
                nazwa_serialu = "2 Broke Girls";
            }

            string adres = Adresy.grafika + "/" + nazwa_serialu + "_male" + ".jpg";

            string skad = adres_grafiki_na_Sidereel("http://www.sidereel.com/" + nazwa_serialu.Replace(" ", "_"));

            Internet.zgraj_z_www(skad, adres);

            return adres;
        }

        static private string adres_grafiki_na_Sidereel(string adres_strony_z_serialem)
        {
            string adres_grafiki = "";

            if (Internet.czy_polaczenie())
            {
                string tresc = Internet.pobierz_strone(adres_strony_z_serialem);

                HtmlDocument dokument = Internet.stworz_dokument_HTML(tresc);

                if (dokument != null)
                {
                    if (dokument.DocumentNode != null)
                    {
                        //show-image
                        foreach (HtmlNode wpis in dokument.DocumentNode.SelectNodes("//div[@class='show-image']"))
                        {
                            //HtmlNode pod_wezel = wpis.LastChild.FirstChild;

                            HtmlNode wezel_obrazka = wpis.ChildNodes[1];
                            adres_grafiki = wezel_obrazka.GetAttributeValue("src", "");
                        }
                    }
                }
            }

            return adres_grafiki;
        }
    }
}
