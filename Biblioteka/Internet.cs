using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;

namespace Biblioteka
{
    public class Internet
    {
        public static bool czy_polaczenie()
        {
            bool internet = true;
            string adres_www = "www.google.com";

            try
            {
                System.Net.IPHostEntry wejscie = System.Net.Dns.GetHostEntry(adres_www);
            }
            catch (WebException)
            {
                internet = false;
            }
            catch (SocketException)
            {
                internet = false;
            }

            return internet;
        }

        public static void idz_do_strony(string adres)
        {
            if (!adres.Contains("www."))
            {
                adres = "www." + adres;
            }
            if (!adres.Contains("http://"))
            {
                adres = "http://" + adres;
            }

            Process.Start(adres);
        }

        public static HtmlDocument stworz_dokument_HTML(string tresc)
        {
            HtmlDocument dokument = new HtmlDocument();

            if (tresc != null & tresc != "")
            {
                dokument.OptionFixNestedTags = true;

                dokument.LoadHtml(tresc);

                if (dokument.ParseErrors != null && dokument.ParseErrors.Count() > 0)
                {
                    //dokument = null;
                }
            }
            else
            {
                dokument = null;
            }

            return dokument;
        }

        public static string pobierz_strone(string adres_www)
        {
            string tresc= null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(adres_www);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            tresc = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                tresc = null;
            }

            return tresc;
        }

        public static string pobierz_strone_POST(string URI, string parametry)
        {
            string odpowiedz_serwera;

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                odpowiedz_serwera = wc.UploadString(URI, parametry);

            }
            return odpowiedz_serwera;
        }

        #region Podstawowe operacje na serwerze

        public static void wgraj_na_serwer(Serwery do_kad, string adres_pliku, string adres_docelowy_pliku)
        {
            WebClient client = new WebClient();
            client.Credentials = Uwierzytelnianie.pobierz(do_kad);

            Uri adres_skad = new Uri(adres_docelowy_pliku);

            client.UploadFile(adres_skad, adres_pliku);
        }

        public static bool zgraj_z_serwera(Serwery skad, string adres_pliku, string adres_docelowy_pliku, bool nadpisac = false, string adres_www="")
        {
            bool czy_ok = true;

            WebClient client = new WebClient();
            client.Credentials = Uwierzytelnianie.pobierz(skad);

            Uri adres_skad = new Uri(adres_pliku);

            //nadpisywanie
            if (nadpisac)
            {
                if (File.Exists(adres_docelowy_pliku))
                {
                    File.Delete(adres_docelowy_pliku);
                }
            }
            if (!File.Exists(adres_docelowy_pliku))
            {
                //sposob sprawdzania czy jest plik na serweze
                //if (Internet.pobierz_strone(adres_pliku) != null)
                //{
                if (czy_plik_istnieje_na_serwerze(adres_www))
                {
                    try
                    {
                        client.DownloadFile(adres_skad, adres_docelowy_pliku);
                    }
                    catch (WebException)
                    {
                        czy_ok = false;
                    }
                }
                else
                {
                    czy_ok = false;
                }
                //}
                //else
                //{
                //    czy_ok = false;
                //}
            }
            return czy_ok;
        }

        //private static bool czy_plik_istnieje_na_serwerze(string url)
        //{
        //    WebRequest request = WebRequest.Create(new Uri(url));
        //    request.Method = "HEAD";

        //    WebResponse response = request.GetResponse();

        //    if (response.ContentLength > 0)
        //    {
        //        response.Close();

        //        return true;
        //    }
        //    else
        //    {
        //        response.Close();

        //        return false;
        //    }
        //}

        public static bool czy_plik_istnieje_na_serwerze(string url)
        {
            if (url != "")
            {
                try
                {
                    //Creating the HttpWebRequest
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //Setting the Request method HEAD, you can also use GET too.
                    request.Method = "HEAD";
                    //przyspieszacz
                    request.Timeout = 1000;
                    //Getting the Web Response.
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //Returns TRUE if the Status code == 200
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        response.Close();
                        return true;
                    }
                    else
                    {
                        response.Close();
                        return false;
                    }
                }
                catch
                {
                    //Any exception will returns false.
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static void zgraj_z_www(string skad, string do_kad)
        {
            if(skad!="" && do_kad!="")
            {
            WebClient pobieranie = new WebClient();
            pobieranie.DownloadFileAsync(new Uri(skad), do_kad);
            }
        }

        public static DateTime sprawdz_date_pliku_na_serwerze(Serwery gdzie, string adres_pliku)
        {
            DateTime data = DateTime.Now;

            try
            {

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(adres_pliku);
                request.Credentials = Uwierzytelnianie.pobierz(gdzie);

                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                data = response.LastModified;

            }
            catch
            {
                data = new DateTime(2012, 1, 1);
            }

            return data;
        }

        public static bool stworz_folder_na_serwerze(Serwery gdzie, string adres)
        {
            WebRequest request = WebRequest.Create(adres);
            request.Credentials = Uwierzytelnianie.pobierz(gdzie);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                WebResponse response = request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void skasuj_folder_z_zawartoscia_na_serwerze(Serwery skad, string adres)
        {
            List<string> lista = wylistuj_folder_serwera(skad, adres, true);

            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].Split('/').Count<string>() > 1)
                {
                    lista[i] = lista[i].Split('/')[1];
                }
            }

            foreach (string s in lista)
            {
                if (!skasuj_plik(skad, adres + "/" + s))
                {
                    skasuj_folder_z_zawartoscia_na_serwerze(skad, adres + "/" + s);
                    skasuj_folder(skad, adres + "/" + s);
                }
                skasuj_folder(skad, adres + "/" + s);
            }

            skasuj_folder(skad, adres);
        }

        public static bool skasuj_plik(Serwery skad, string adres)
        {
            WebRequest request = WebRequest.Create(adres);
            request.Credentials = Uwierzytelnianie.pobierz(skad);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            try
            {
                WebResponse response = request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool skasuj_folder(Serwery skad, string adres)
        {
            WebRequest request = WebRequest.Create(adres); ;
            request.Credentials = Uwierzytelnianie.pobierz(skad);

            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            try
            {
                WebResponse response = request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<string> wylistuj_folder_serwera(Serwery skad, string adres_folder, bool czy_pliki_tez = false)
        {
            List<string> lista = new List<string>();

            if (czy_polaczenie())
            {
                string adres = adres_folder;

                FtpWebRequest rzadanie;

                rzadanie = (FtpWebRequest)WebRequest.Create(new Uri(adres));
                rzadanie.Method = WebRequestMethods.Ftp.ListDirectory;
                rzadanie.Credentials = Uwierzytelnianie.pobierz(skad);
                rzadanie.KeepAlive = false;
                rzadanie.UseBinary = true;
                using ((FtpWebResponse)rzadanie.GetResponse())
                {
                    Stream ciag = rzadanie.GetResponse().GetResponseStream();

                    using (StreamReader czytnik = new StreamReader(ciag))
                    {
                        string folder;
                        while ((folder = czytnik.ReadLine()) != null)
                        {
                            if (czy_pliki_tez)
                            {
                                lista.Add(folder);
                            }
                            else
                            {
                                if (!folder.Contains("."))
                                {
                                    lista.Add(folder);
                                }
                            }
                        }
                    }
                }
            }

            if (czy_pliki_tez)
            {
                if (lista.Count > 2 && (lista[0] == "." || lista[0] == ".."))
                {
                    lista.RemoveAt(0);
                    lista.RemoveAt(0);
                }
            }

            return lista;
        }

        #endregion

        #region Maile

        public static void wyślij_maila_na_ds(string tytul, string wiadomosc, string od_kogo, string od_mail)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(od_mail, od_kogo);
            message.To.Add(new MailAddress("dailystars.redakcja@gmail.com"));
            message.Subject = tytul;
            message.Body = wiadomosc;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("aplikacja.serialomaniak@gmail.com", "ser.5glee1");

            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.Send(message);

        }

        #endregion
    }
}
