using Biblioteka;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serialomaniak.Serialowa.Dystrybutorzy
{
    class SKASUJStvplus
    {
        //doskasowania:
        //POLEPSZYC wyodrebnic metode wspolna pobierania danych o odcinku, aby naprawiac tylko jedna metode jak zmienia uklad strony stvplus

        //static public Dictionary<string, string> generuj_liste_seriali_w_Stvplus()
        //{
        //    Dictionary<string, string> lista = new Dictionary<string, string>();

        //    if (Internet.czy_polaczenie())
        //    {
        //        string tresc = Internet.pobierz_strone("http://stvplus.com/");

        //        HtmlDocument dokument = Internet.stworz_dokument_HTML(tresc);

        //        lista = wyodrebnij_tytuly_i_adresy_seriali(dokument);
        //    }

        //    return lista;
        //}

        //private static Dictionary<string, string> wyodrebnij_tytuly_i_adresy_seriali(HtmlDocument dokument)
        //{
        //    Dictionary<string, string> slownik_wszystkich_seriali = new Dictionary<string, string>();
        //    if (dokument != null)
        //    {
        //        if (dokument.DocumentNode != null)
        //        {
        //            foreach (HtmlNode wpis in dokument.DocumentNode.SelectNodes("//a[@class='show']"))
        //            {
        //                HtmlAttribute atr_url = wpis.Attributes["href"];

        //                string tytul = wpis.InnerHtml;
        //                tytul=tytul.Replace("&#39;","'");
        //                string link = atr_url.Value;
        //                slownik_wszystkich_seriali.Add(tytul, link);
        //            }
        //        }
        //    }
        //    return slownik_wszystkich_seriali;
        //}

        //static public List<string> generuj_liste_odcinkow_serialu_w_Stvplus(string nazwa_serialu)
        //{
        //    //jak beda inne zrodla seriali moze powodować problem bo będzie szukał nei swoich seriali
        //    Dictionary<string, string> slownik_seriali = ListaSeriali.slownik_seriali_wszystkich;

        //    List<string> lista_odcinkow = new List<string>();

        //    if (Internet.czy_polaczenie())
        //    {
        //        if (slownik_seriali.ContainsKey(nazwa_serialu))
        //        {

        //            string tresc = Internet.pobierz_strone(slownik_seriali[nazwa_serialu]);

        //            HtmlDocument dokument = Internet.stworz_dokument_HTML(tresc);

        //            lista_odcinkow = wyodrebnij_liste_odcinkow(dokument);
        //        }
        //    }
        //    return lista_odcinkow;
        //}

        //private static List<string> wyodrebnij_liste_odcinkow(HtmlDocument dokument)
        //{
        //    List<string> lista_odcinkow = new List<string>();
        //    if (dokument != null)
        //    {
        //        if (dokument.DocumentNode != null)
        //        {
        //            foreach (HtmlNode wpis in dokument.DocumentNode.SelectNodes("//div[@class='span6']"))
        //            {
        //                HtmlNode wezel_tytulu = wpis.ChildNodes[2];
        //                //S02E16 - And Just Plane Magic 
        //                List<string> wstępny_tytul = wezel_tytulu.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        //                wstępny_tytul.RemoveRange(0,2);

        //                string tytul="";
        //                foreach (string slowo in wstępny_tytul)
        //                {
        //                    tytul += slowo+" ";
        //                }
        //                tytul=tytul.Trim();

        //                tytul = tytul.Replace("&#39;", "'");

        //                lista_odcinkow.Insert(0, tytul);
        //            }

        //        }
        //    }
        //    return lista_odcinkow;
        //}

        //public static List<Sezon> wyodrebnij_liste_sezonow(Serial s, string tytul_odcinka)
        //{
        //    //jak beda inne zrodla seriali moze powodować problem bo będzie szukał nei swoich seriali
        //    Dictionary<string, string> slownik_seriali = ListaSeriali.slownik_seriali_wszystkich;

        //    List<Sezon> lista = new List<Sezon>();

        //    if (Internet.czy_polaczenie())
        //    {
        //        if (slownik_seriali.ContainsKey(s.Nazwa))
        //        {
        //            string tresc = Internet.pobierz_strone(slownik_seriali[s.Nazwa]);

        //            HtmlDocument dokument = Internet.stworz_dokument_HTML(tresc);

        //            lista = wyodrebnij_sezony(dokument, s.Nazwa, tytul_odcinka);
        //        }
        //    }

        //    return lista;
        //}

        //private static List<Sezon> wyodrebnij_sezony(HtmlDocument dokument, string nazwa_serialu, string tytul_odcinka)
        //{
        //    //POPRAWIC wyodrebnianie sezonow powinno odbywac sie z lsity na serwerze wczesniej wygenerowanej aby nie powodowalo bledow aplikacji spowodowanych zmiana szablonu html strony
        //    List<Sezon> lista = new List<Sezon>();
        //    int numer_sezonu_poprzedniego = 0;

        //    string identyfikator = wyodrebnij_identyfikator_odcinka(dokument, tytul_odcinka);

        //    int numer_sezonu_obejrzanego =Int32.Parse(identyfikator.Split(' ')[0]);
        //    int numer_odcinka_obejrzanego = Int32.Parse(identyfikator.Split(' ')[1]);

        //    if (dokument != null)
        //    {
        //        if (dokument.DocumentNode != null)
        //        {
        //            foreach (HtmlNode wpis in dokument.DocumentNode.SelectNodes("//div[@class='span6']"))
        //            {
        //                List<string> el = new List<string>();
        //                foreach (HtmlNode s in wpis.ChildNodes)
        //                {
        //                    string zawartosc = s.InnerHtml.Bez_fragmentu("\n").Bez_fragmentu("\t").Bez_fragmentu("&nbsp;").Bez_fragmentu("-").Trim();

        //                    //wywalanie wynikow ogladalnosci
        //                    zawartosc = zawartosc.Usun_znaczniki_html(true);

        //                    if (zawartosc != "")
        //                    {
        //                        el.Add(zawartosc);
        //                    }
        //                }

        //                string fragment_tytulu = el[1];
        //                string tytul = fragment_tytulu.Replace("&#39;", "'");

        //                tytul = tytul.Bez_fragmentu(tytul.Split(' ')[0]).Trim();



        //                //11th Oct 2012
        //                string tekst_daty = el[0];
        //                string[] fragmenty_daty = tekst_daty.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //                int numer_sezonu = 0;
        //                int numer_odcinka = 0;

        //                //S01E09
        //                try
        //                {
        //                    numer_sezonu = Int32.Parse(fragment_tytulu.Substring(1, 2));
        //                    numer_odcinka = Int32.Parse(fragment_tytulu.Substring(4, 2));
        //                }
        //                catch
        //                {
        //                    //POLEPSZYC dodac logowanie bledu 
        //                    numer_odcinka = 0;
        //                    numer_sezonu = 0;
        //                }

        //                int miesiac = 0;
        //                int dzien = 0;

        //                DateTime? data_emisji = null;
        //                if(!tekst_daty.Contains("TBA"))
        //                {
        //                    dzien = Int32.Parse(fragmenty_daty[0].Remove(fragmenty_daty[0].Count() - 2, 2));
        //                    miesiac = Angielski.z_skroconej_nazwy_na_numer_miesiaca(fragmenty_daty[1]);

        //                    data_emisji=new DateTime(Int32.Parse(fragmenty_daty[2]), miesiac, dzien);
        //                }



        //                if (numer_odcinka != 0 && numer_sezonu != 0)
        //                {
        //                    if (numer_sezonu_poprzedniego!=numer_sezonu)
        //                    {
        //                        lista.Insert(0,new Sezon());
        //                        numer_sezonu_poprzedniego = numer_sezonu;
        //                    }
        //                    bool czy_obejrzany =false;
        //                    if(numer_sezonu_obejrzanego>numer_sezonu)
        //                    {
        //                        czy_obejrzany = true;
        //                    }
        //                    else if (numer_sezonu_obejrzanego == numer_sezonu && numer_odcinka_obejrzanego>=numer_odcinka)
        //                    {
        //                        czy_obejrzany = true;
        //                    }
        //                    lista[0].dodaj_odcinek_na_poczatku(new Odcinek(nazwa_serialu, numer_sezonu, numer_odcinka, tytul, new List<Link>(), czy_obejrzany, data_emisji));
        //                }
        //                else
        //                {
        //                    //odcinek specjalny
        //                    //przy wczytaniu duzej ilosci seriali dodac tu wyrzucanie komunikatu aby sprawdzic ile jest odcinkow specjalnych i jakich
        //                }
        //            }
        //        }
        //    }

        //    return lista;
        //}


        //private static string wyodrebnij_identyfikator_odcinka(HtmlDocument dokument, string tytul_odcinka)
        //{
        //    List<Sezon> lista = new List<Sezon>();
        //    int numer_sezonu_poszukiwanego=0;
        //    int numer_odcinka_poszukiwanego=0;

        //    if (dokument != null)
        //    {
        //        if (dokument.DocumentNode != null)
        //        {
        //            foreach (HtmlNode wpis in dokument.DocumentNode.SelectNodes("//div[@class='span6']"))
        //            {
        //                List<string> el = new List<string>();
        //                foreach(HtmlNode s in wpis.ChildNodes)
        //                {
        //                    string zawartosc = s.InnerHtml.Bez_fragmentu("\n").Bez_fragmentu("\t").Bez_fragmentu("&nbsp;").Bez_fragmentu("-").Trim();

        //                    //wywalanie wynikow ogladalnosci
        //                    zawartosc = zawartosc.Usun_znaczniki_html(true);

        //                    if (zawartosc != "")
        //                    {
        //                        el.Add(zawartosc);
        //                    }
        //                }

        //                string tytul = el[0].Replace("&#39;", "'");

        //                tytul = tytul.Bez_fragmentu(tytul.Split(' ')[0]).Trim();



        //                //11th Oct 2012
        //                string tekst_daty = el[1];
        //                string[] fragmenty_daty = tekst_daty.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //                int numer_sezonu = 0;
        //                int numer_odcinka = 0;

        //                //S01E09
        //                try
        //                {
        //                numer_sezonu = Int32.Parse(el[0].Substring(1, 2));
        //                numer_odcinka = Int32.Parse(el[0].Substring(4, 2));
        //                }
        //                catch
        //                {
        //                    //POLEPSZYC dodac logowanie bledu 
        //                    numer_odcinka = 0;
        //                    numer_sezonu = 0;
        //                }


        //                if (tytul == tytul_odcinka)
        //                {
        //                    numer_odcinka_poszukiwanego = numer_odcinka;
        //                    numer_sezonu_poszukiwanego = numer_sezonu;
        //                }
        //            }
        //        }
        //    }

        //    return numer_sezonu_poszukiwanego+" "+numer_odcinka_poszukiwanego;
        //}
    }
}
