using Biblioteka;
using Serialomaniak.Serialowa.Dystrybutorzy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Xml;

namespace Serialomaniak.Serialowa
{
    [Serializable]
    public class ListaSeriali : ObservableCollection<Serial>, INotifyPropertyChanged
    {
        public static List<string> lista_seriali_wszystkich = new List<string>();

        //public static Dictionary<string, string> slownik_seriali_wszystkich = new Dictionary<string, string>();
        //public static List<string> lista_odcinkow_serialu_obecnego = new List<string>();

        static public void dodaj_serial(Serial s)
        {
                (App.Current.Resources["lista_seriali"] as ListaSeriali).Add(s);
        }

        public static void usun_serial(Serial s)
        {
            (App.Current.Resources["lista_seriali"] as ListaSeriali).Remove(s);
        }

        public static void posortuj_kontrolke()
        {
            ListCollectionView do_sortowania = CollectionViewSource.GetDefaultView((App.Current.Resources["lista_seriali"] as ListaSeriali)) as ListCollectionView;
            do_sortowania.GroupDescriptions.Clear();
            do_sortowania.GroupDescriptions.Add(new PropertyGroupDescription("Grupy"));
            do_sortowania.CustomSort = new Sortowanie();

        }

        public static void zapisz()
        {
            XmlWriterSettings sledzone = new XmlWriterSettings();
            sledzone.Indent = true; //oddziela nowa linia

            XmlWriter plik_ustawien = XmlWriter.Create(Adresy.ustawienia + "\\ListaSeriali.xml", sledzone);

            plik_ustawien.WriteStartDocument();
            plik_ustawien.WriteComment("Lista śledzonych przez użytkownika seriali");

            plik_ustawien.WriteStartElement("lista_seriali");

            foreach (Serial serial in (App.Current.Resources["lista_seriali"] as ListaSeriali))
            {
                plik_ustawien.WriteStartElement("serial");
                plik_ustawien.WriteAttributeString("nazwa", serial.Nazwa);
                plik_ustawien.WriteAttributeString("tytul_obejrzanego", serial.TytulObejrzanego);
                plik_ustawien.WriteAttributeString("zdjecie_male", serial.ZdjecieMale);
                plik_ustawien.WriteAttributeString("zdjecie_duze", serial.ZdjecieDuze);

                plik_ustawien.WriteAttributeString("ulubiony", serial.Ulubiony.ToString());

                plik_ustawien.WriteAttributeString("stacja_tv", serial.Stacja_tv.ToString());
                plik_ustawien.WriteAttributeString("czas", serial.Czas_trwania.ToString());
                plik_ustawien.WriteAttributeString("kraj", serial.Kraj.ToString());

                plik_ustawien.WriteAttributeString("status", ((int)serial.Status).ToString());

                foreach (Sezon sezon in serial.ListSezonow)
                {
                    plik_ustawien.WriteStartElement("sezon");

                    foreach (Odcinek odc in sezon.ListaOdcinkow)
                    {
                        plik_ustawien.WriteStartElement("odcinek");

                        plik_ustawien.WriteAttributeString("tytul", odc.Tytul_odcinka);
                        plik_ustawien.WriteAttributeString("numer_odcinka", odc.NumerOdcinka.ToString());

                        plik_ustawien.WriteAttributeString("obejrzany", odc.Obejrzany.ToString());

                        plik_ustawien.WriteAttributeString("info_o_lektor", odc.PoinformowanyOLektorze.ToString());
                        plik_ustawien.WriteAttributeString("info_o_napisach", odc.PoinformowanyONapisach.ToString());
                        plik_ustawien.WriteAttributeString("info_o_oryginale", odc.PoinformowanyOOryginale.ToString());

                        if (odc.DataEmisji == null)
                        {
                            plik_ustawien.WriteAttributeString("data_emisji", "");
                        }
                        else
                        {

                            plik_ustawien.WriteAttributeString("data_emisji", odc.DataEmisji.Value.ToString("yyyy-MM-dd HH:mm"));
                        }

                            plik_ustawien.WriteStartElement("linki");
                            foreach (Link link in odc.LinkiDoOdcinka)
                            {
                                plik_ustawien.WriteStartElement("link");

                                plik_ustawien.WriteAttributeString("wersja", link.Wersja.ToString());
                                plik_ustawien.WriteAttributeString("dystrybutor", link.Dystrybutor.ToString());
                                plik_ustawien.WriteAttributeString("adres", link.Adres.ToString());

                                plik_ustawien.WriteEndElement();
                            }
                            plik_ustawien.WriteEndElement();

                        //POLEPSZYC: dodac zapis pozostalych pól

                        plik_ustawien.WriteEndElement();
                    }

                    plik_ustawien.WriteEndElement();
                }

                plik_ustawien.WriteEndElement();
            }

            plik_ustawien.WriteEndElement();

            plik_ustawien.Flush();
            plik_ustawien.Close();

        }

        public static void wczytaj()
        {
            (App.Current.Resources["lista_seriali"] as ListaSeriali).Clear();

            FileInfo plik_z_lista = new FileInfo(Adresy.ustawienia + "\\ListaSeriali.xml");
            if (plik_z_lista.Exists)
            {
                XmlDocument dokument_xml = new XmlDocument(); dokument_xml.Load(Adresy.ustawienia + "\\ListaSeriali.xml");
                XmlNodeList lista_w_seriali = dokument_xml.GetElementsByTagName("serial");


                foreach (XmlNode serial_z_pliku in lista_w_seriali)
                {
                    XmlNodeList lista_w_sezonow = serial_z_pliku.ChildNodes;

                    string nazwa = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "nazwa");
                    string tytul_obejrzanego = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "tytul_obejrzanego");
                    string zdjecie_male = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "zdjecie_male");
                    string zdjecie_duze = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "zdjecie_duze");
                    bool ulubiony = (bool)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "ulubiony","Boolean");

                    string stacja_tv = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "stacja_tv");
                    int czas = (int)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "czas", "Int32");
                    string kraj = (string)wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "kraj");

                    StatusSerialu status = (StatusSerialu)(wartosc_atrybutu_jesli_istnieje(serial_z_pliku, "status", "StatusSerialu"));

                    Serial serial = new Serial(nazwa, tytul_obejrzanego, zdjecie_male, zdjecie_duze, ulubiony, stacja_tv, czas,kraj,status);

                    int numer_sezonu = 0;
                    foreach (XmlNode w_sezon in lista_w_sezonow)
                    {
                        numer_sezonu++;

                        XmlNodeList lista_w_odcinkow = w_sezon.ChildNodes;
                        Sezon sezon = new Sezon();

                        foreach (XmlNode w_odcinek in lista_w_odcinkow)
                        {
                            string tytul = w_odcinek.Attributes["tytul"].InnerText;
                            int numer = Int32.Parse(w_odcinek.Attributes["numer_odcinka"].InnerText);

                            bool obejrzany = Boolean.Parse(w_odcinek.Attributes["obejrzany"].InnerText);

                            bool info_o_lektor = Boolean.Parse(w_odcinek.Attributes["info_o_lektor"].InnerText);
                            bool info_o_napisach = Boolean.Parse(w_odcinek.Attributes["info_o_napisach"].InnerText);
                            bool info_o_oryginale = Boolean.Parse(w_odcinek.Attributes["info_o_oryginale"].InnerText);

                            string data_tekst = w_odcinek.Attributes["data_emisji"].InnerText;

                            DateTime? data;
                            if (data_tekst == "")
                            {
                                data = null;
                            }
                            else
                            {
                                data = DateTime.ParseExact(data_tekst, "yyyy-MM-dd HH:mm", null);
                            }

                            XmlNodeList lista_linkow= w_odcinek.FirstChild.ChildNodes;
                            List<Link> linki = new List<Link>();
                            foreach (XmlNode w_link in lista_linkow)
                            {
                                WersjaOdcinka wersja = Link.OkreslWersjeOdcinka(w_link.Attributes["wersja"].InnerText);
                                Dystrybutor dystrybutor = Link.OkreslDystrybutora(w_link.Attributes["dystrybutor"].InnerText);
                                string adres = w_link.Attributes["adres"].InnerText;

                                linki.Add(new Link(wersja, dystrybutor, adres));
                            }                            

                            Odcinek odcinek = new Odcinek(nazwa, numer_sezonu, numer, tytul, linki, obejrzany, data, info_o_lektor, info_o_napisach, info_o_oryginale);

                            sezon.dodaj_odcinek(odcinek);
                        }
                        serial.dodaj_sezon(sezon);
                    }

                    //serial = ustaw_status(serial);

                    ListaSeriali.dodaj_serial(serial);
                }
            }
        }

        private static object wartosc_atrybutu_jesli_istnieje(XmlNode serial_z_pliku, string atrybut, string typ="")
        {
            if (serial_z_pliku.Attributes[atrybut] != null)
            {
                string wartosc_atrybutu=serial_z_pliku.Attributes[atrybut].InnerText;
                if (typ == "")
                {
                    return wartosc_atrybutu;
                }
                else if (typ=="Int32")
                {
                    int wynik;
                    if(Int32.TryParse(wartosc_atrybutu, out wynik))
                    {
                        return  wynik;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else if (typ == "Boolean")
                {
                    bool wynik;
                    if (Boolean.TryParse(wartosc_atrybutu, out wynik))
                    {
                        return wynik;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (typ == "StatusSerialu")
                {
                    return (StatusSerialu)Enum.Parse(typeof(StatusSerialu), wartosc_atrybutu);                   
                }
            }
            else
            {
                if (typ == "Int32")
                {
                        return 0;
                }
                if (typ == "Boolean")
                {
                    return false;
                }
                if (typ == "StatusSerialu")
                {
                    return StatusSerialu.Nadawany;
                }
                return "";
            }

            return "";
        }


        public static void generuj_liste_seriali_wszystkich()
        {
            List<string> lista = Internet.wylistuj_folder_serwera(Serwery.Serialomaniak, "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Seriale",true);

            for (int i = 0; i < lista.Count; i++ )
            {
                lista[i] = lista[i].Remove(lista[i].Length - 4);
            }

            lista_seriali_wszystkich = lista;
        }

        public static List<string> generuj_liste_odcinkow_serialu(string nazwa_serialu)
        {
            Serial sztuczny = new Serial(nazwa_serialu, "", "", "", false, "", 0, "");

            sztuczny.aktualizuj_liste_emisii_i_info_o_serialu();

            return sztuczny.generuj_liste_tytulow_odcinkow();
        }

        //public static Serial ustaw_status(Serial do_ustawienia)
        //{
        //    if (SpisSeriali.Zakonczonych.Contains(do_ustawienia.Nazwa))
        //    {
        //        do_ustawienia.Status = Serial.StaryStatusSerialu.Zakończony;
        //    }

        //    if (SpisSeriali.Emitowanych.Contains(do_ustawienia.Nazwa))
        //    {
        //        do_ustawienia.Status = Serial.StaryStatusSerialu.Emitowany;
        //    }

        //    return do_ustawienia;
        //}


        #region Właściwości

        public static string OdcinkowDoObejrzenia
        {
            get
            {
                int liczba_odcinkow = 0;
                foreach (Serial s in (App.Current.Resources["lista_seriali"] as ListaSeriali))
                {
                    string tytul = s.Nazwa;
                    liczba_odcinkow += s.LiczbaDoOdcinkowDoObejrzenia;
                }
                if (liczba_odcinkow < 0)
                {
                    liczba_odcinkow = 0;
                }
                return liczba_odcinkow.ToString();
            }
        }

        public static int LiczbaSeriali
        {
            get
            {
                return (App.Current.Resources["lista_seriali"] as ListaSeriali).Count;
            }
        }

        public static Serial SerialoIndeksie(int indeks)
        {
            return (App.Current.Resources["lista_seriali"] as ListaSeriali)[indeks];
        }

        public static Serial SerialoNazwie(string nazwa)
        {
            return (App.Current.Resources["lista_seriali"] as ListaSeriali).FirstOrDefault(a => a.Nazwa == nazwa);
        }

        new public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string nazwaWlasciowosci)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWlasciowosci));
            }
        }

        #endregion
    }
}
