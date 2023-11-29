using Biblioteka;
using HtmlAgilityPack;
using Serialomaniak.Okna;
using Serialomaniak.Serialowa.Dystrybutorzy;
using Serialomaniak.Serialowa.Emisja;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Serialomaniak.Serialowa
{
    [Serializable]
    public class Serial : INotifyPropertyChanged
    {
        private string nazwa;
        private string zdjecie_male;
        private string zdjecie_duze;

        private bool ulubiony;

        private string tytul_odcinka;

        private StatusSerialu status;

        private List<Sezon> lista_sezonow = new List<Sezon>();

        private string stacja_tv;

        public string Stacja_tv
        {
            get { return stacja_tv; }
            set { stacja_tv = value;
            this.OnPropertyChanged("Stacja_tv");
            }
        }

        private string kraj;

        public string Kraj
        {
            get { return kraj; }
            set { kraj = value;
            this.OnPropertyChanged("Kraj");
            }
        }

        private int czas_trwania;

        public int Czas_trwania
        {
            get { return czas_trwania; }
            set
            {
                czas_trwania = value;
                this.OnPropertyChanged("Czas_trwania");
            }
        }

        //do serializacji
        public Serial()
        {
        }

        public Serial(string nazwa, string tytul_odcinka, string zdjecie_male, string zdjecie_duze, bool ulubiony, string stacja_tv, int czas, string kraj, StatusSerialu status = StatusSerialu.Nadawany)
        {
            this.nazwa = nazwa;
            this.zdjecie_male = zdjecie_male;
            this.zdjecie_duze = zdjecie_duze;

            this.ulubiony = ulubiony;

            this.tytul_odcinka = tytul_odcinka;

            this.status = status;

            this.stacja_tv = stacja_tv;
            this.kraj = kraj;
            this.czas_trwania = czas;
        }

        public void obejrzane_do(Odcinek obejrzany_odc)
        {
            bool oznaczono = false;

            foreach (Sezon s in lista_sezonow)
            {
                foreach (Odcinek o in s.ListaOdcinkow)
                {
                    if (obejrzany_odc != null)
                    {
                        if (o.Equals(obejrzany_odc))
                        {
                            o.Obejrzany = true;

                            oznaczono = true;
                        }
                        else
                        {
                            o.Obejrzany= (!oznaczono);
                        }
                    }
                }
            }

            poinformuj_o_zmianie_obejrzenia_jakiegos_odcinka();
        }

        public void poinformuj_o_zmianie_obejrzenia_jakiegos_odcinka()
        {
            this.OnPropertyChanged("TytulObejrzanego");
            this.OnPropertyChanged("ObejrzanyOdcinekTytul");
            this.OnPropertyChanged("ObejrzanyOdcinekNumerOdcinka");
            this.OnPropertyChanged("ObejrzanyOdcinekNumerSezonu");
            this.OnPropertyChanged("ObejrzanyOdcinekLiczbaOdcinkowWSezonie");
            this.OnPropertyChanged("PoprzedniOdcinek");
            this.OnPropertyChanged("NastepnyOdcinek");
            this.OnPropertyChanged("NastepnyOdcinekTytul");
            this.OnPropertyChanged("NastepnyOdcinekNumerOdcinka");
            this.OnPropertyChanged("NastepnyOdcinekNumerSezonu");

            this.OnPropertyChanged("ProcentObejrzanychWSezonie");
            this.OnPropertyChanged("ProcentObejrzanychWSezonieWPikselach");
            this.OnPropertyChanged("LiczbaDoOdcinkowDoObejrzenia");
            this.OnPropertyChanged("ProcentNieObejrzanychWSezonieWPikselach");
            this.OnPropertyChanged("Statystyka");
            this.OnPropertyChanged("ObejrzanyOdcinekNumerOdcinkaWWszystkichSezonach");

            this.OnPropertyChanged("Grupy");

            ListaSeriali.posortuj_kontrolke();
        }

        public void zaaktualizuj_zdjecia()
        {
            if (!File.Exists(this.zdjecie_male) || (new FileInfo(this.zdjecie_male).Length==0))
            {
                this.zdjecie_male = Sidereel.pobierz_zdjecie_serialu_male(this.nazwa);
            }
            if (!File.Exists(this.zdjecie_duze) || (new FileInfo(this.zdjecie_duze).Length == 0))
            {
                this.zdjecie_duze = TvCalendar.pobierz_zdjecie_serialu_duze(this.nazwa);
            }
        }

        //public void dodaj_liste_emitowanych_odcinkow()
        //{
        //    List<Sezon> lista_sezonow = DO_USUNIECIA_zastapienia_przez_wykorzystanie_wlasnych_list_stworz_liste_sezonow();

        //    if (lista_sezonow != null)
        //    {
        //        this.ListSezonow = new List<Sezon>();

        //        foreach (Sezon sezon in lista_sezonow)
        //        {
        //            this.dodaj_sezon(sezon);
        //        }
        //    }
        //}

        public void aktualizuj_liste_emisii_i_info_o_serialu()
        {
            if (Internet.czy_polaczenie())
            {
                string nazwa_pliku = this.Nazwa.Replace(":", "").Replace("?", "").Replace(",", "").Replace("!", "").Replace("'", "").Replace("/", "").Replace("\\", "");

                string adres_pliku = "ftp://ftp.singit.pl/www/Aplikacje/Serialomaniak/Seriale/" + nazwa_pliku + ".xml";
                string adres_www_pliku = "http://www.singit.pl/Aplikacje/Serialomaniak/Seriale/" + nazwa_pliku + ".xml";

                string adres_do_celowy = Adresy.seriale + "\\" + nazwa_pliku + ".xml";

                if (Internet.zgraj_z_serwera(Serwery.Serialomaniak, adres_pliku, adres_do_celowy, true, adres_www_pliku))
                {
                    //zapisywac na serwer tak jak tu czyli lista sezonow zamiast listy odcinkow

                    EmisjaSerial s = Program.Deserializuj_z_XML<EmisjaSerial>(this.Nazwa, Adresy.seriale);

                    this.Status = s.Status_serialu;

                    this.Czas_trwania = s.Czas_trwania;
                    this.Stacja_tv = s.Stacja_tv;
                    this.Kraj = s.Kraj;

                    //aktualizacja istniejacych wpisow
                    foreach (Sezon sezon in this.ListSezonow)
                    {
                        foreach (Odcinek odcinek in sezon.ListaOdcinkow)
                        {
                            //zdazaja sie poki co wpisy ktorych nei ma w bazie jeszcze
                            if (s.Lista_sezonow.Count <= sezon.NumerSezonu - 1 || s.Lista_sezonow[sezon.NumerSezonu - 1].ListaOdcinkow.Count <= odcinek.NumerOdcinka - 1)
                            {
                                
                            }
                            else//normalne sytuacje
                            {
                                odcinek.DataEmisji = s.Lista_sezonow[sezon.NumerSezonu - 1].ListaOdcinkow[odcinek.NumerOdcinka - 1].Data_emisji;
                                odcinek.Tytul_odcinka = s.Lista_sezonow[sezon.NumerSezonu - 1].ListaOdcinkow[odcinek.NumerOdcinka - 1].Tytul_odcinka;                                
                            }
                        }
                    }

                    foreach (EmisjaSezon e_sezon in s.Lista_sezonow)
                    {
                        foreach (EmisjaOdcinek e_odcinek in e_sezon.ListaOdcinkow)
                        {
                            if(this.ListSezonow.Count<e_sezon.NumerSezonu)
                            {
                                Sezon nowy = new Sezon();
                                nowy.NumerSezonu=e_sezon.NumerSezonu;
                                this.ListSezonow.Add(nowy);
                            }

                            if (this.ListSezonow[e_sezon.NumerSezonu - 1].ListaOdcinkow.Count < Int32.Parse(e_odcinek.Numer_odcinka))
                            {
                                Odcinek nowy_odcinek = new Odcinek(this.Nazwa, e_sezon.NumerSezonu, Int32.Parse(e_odcinek.Numer_odcinka), e_odcinek.Tytul_odcinka);
                                nowy_odcinek.DataEmisji = e_odcinek.Data_emisji;

                                this.ListSezonow[e_sezon.NumerSezonu - 1].ListaOdcinkow.Add(nowy_odcinek);
                            }
                        }
                    }

                }
            }

            
        }

        public List<string> generuj_liste_tytulow_odcinkow()
        {
            List<string> tytuly = new List<string>();

            foreach (Sezon sez in this.lista_sezonow)
            {
                foreach (Odcinek odc in sez.ListaOdcinkow)
                {
                    tytuly.Add(odc.Tytul_odcinka);
                }
            }

            return tytuly;
        }

        //public List<Sezon> DO_USUNIECIA_zastapienia_przez_wykorzystanie_wlasnych_list_stworz_liste_sezonow()
        //{
        //    List<Sezon> lista_sezonow = null;

        //    if (Internet.czy_polaczenie() && ListaSeriali.slownik_seriali_wszystkich.ContainsKey(this.Nazwa))
        //    {
        //        lista_sezonow = Stvplus.wyodrebnij_liste_sezonow(this, tytul_odcinka);
        //    }

        //    if (lista_sezonow != null && lista_sezonow.Count() == 0)
        //    {
        //        lista_sezonow = null;
        //    }

        //    if(lista_sezonow != null && lista_sezonow.Count() != 0)
        //    {
        //        //nadanie sezonom numerow
        //        for (int i = 0; i < lista_sezonow.Count(); i++)
        //        {
        //            lista_sezonow[i].NumerSezonu = i + 1;
        //        }
        //    }

        //    return lista_sezonow;
        //}

        public void uzupelni_liste_linkow()
        {
            foreach (Sezon s in this.ListSezonow)
            {
                foreach (Odcinek o in s.ListaOdcinkow)
                {
                    List<Link> linki_nowe = o.uzupelnij_liste_linkow();

                    foreach (Link nowy in linki_nowe)
                    {
                        //jezeli nowy to zawsze dodawaj
                        if (!o.LinkiDoOdcinka.Contains(nowy))
                        {
                            o.LinkiDoOdcinka.Add(nowy);
                        }
                        //jesli nie obejrzany sprawdz czy mozna poinfomowac zawsze
                        if ((!o.Obejrzany) && Profil.czy_informowac_o_odcinku(nowy.Wersja))
                        {
                            bool czy_informowac = false;
                            if (nowy.Wersja == WersjaOdcinka.Lektor && o.PoinformowanyOLektorze == false)
                            {
                                o.PoinformowanyOLektorze = true;
                                czy_informowac = true;
                            }
                            else if (nowy.Wersja == WersjaOdcinka.Napisy && o.PoinformowanyONapisach == false && o.PoinformowanyOLektorze == false)
                            {
                                o.PoinformowanyONapisach = true;
                                czy_informowac = true;
                            }
                            else if (nowy.Wersja == WersjaOdcinka.Oryginal && o.PoinformowanyOOryginale == false && o.PoinformowanyONapisach == false && o.PoinformowanyOLektorze == false)
                            {
                                o.PoinformowanyOOryginale = true;
                                czy_informowac = true;
                            }
                            if (czy_informowac)
                            {
                                App.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
                                      new Action(() =>
                                      {
                                          Powiadomienie nowe = new Powiadomienie(o);
                                          nowe.Show();
                                      })
                                     );
                            }
                        }
                    }
                }
            }
        }

        public Odcinek znajdz_ostatni_obejrzany_odcinek()
        {
            Odcinek ostatni_obejrzany = null;

            foreach (Sezon s in lista_sezonow)
            {
                if (s.znajdz_ostatni_obejrzany_odcinek() != null)
                {
                    ostatni_obejrzany = s.znajdz_ostatni_obejrzany_odcinek();
                }
            }

            return ostatni_obejrzany;
        }

        public void dodaj_sezon(Sezon sezon)
        {
            sezon.NumerSezonu = lista_sezonow.Count + 1;
            this.lista_sezonow.Add(sezon);

            //jak doda sie sezon zmienia sie liczba sezonow
            this.OnPropertyChanged("LiczbaSezonow");
        }

        private string komunikat_z_liczba_odcinkow_nie_obejrzanych()
        {
            int liczba_nie_obejrzanych = liczba_odcinkow_nie_obejrzanych();
            if (liczba_nie_obejrzanych > 4)
            {
                return liczba_odcinkow_nie_obejrzanych() + " odcinków";
            }
            else if (liczba_nie_obejrzanych > 1 && liczba_nie_obejrzanych <= 4)
            {
                return liczba_odcinkow_nie_obejrzanych() + " odcinki";
            }
            else if (liczba_nie_obejrzanych == 1)
            {
                return "Nowy odcinek";
            }
            else
            {
                return "Obejrzany";
            }
        }

        //POLEPSZYC zmienic system poruszania sie po liscie odcinkow na prostrzy wkyorzystujacy listy

        private Odcinek znajdz_nastepny_po_obejrzanym_odcinku()
        {
            Odcinek nastepny_po_obejrzanym = null;

            bool czy_znaleziony = false;

            foreach (Sezon s in lista_sezonow)
            {
                if (czy_znaleziony == false)
                {
                    Odcinek nastepny = s.znajdz_nastepny_po_obejrzanym_odcinku();

                    if (nastepny != null)
                    {
                        nastepny_po_obejrzanym = nastepny;
                        czy_znaleziony = true;
                    }
                }
            }

            return nastepny_po_obejrzanym;
        }

        private Odcinek znajdz_poprzedni_przed_obejrzanym_odcinku()
        {
            Odcinek poprzedni_przed_obejrzanym = null;

            bool czy_znaleziony = false;

            foreach (Sezon s in lista_sezonow)
            {
                if (czy_znaleziony == false)
                {
                    Odcinek poprzedni = s.znajdz_poprzedni_przed_obejrzanym_odcinku();
                    
                    if (poprzedni != null)
                    {
                        poprzedni_przed_obejrzanym = poprzedni;
                        czy_znaleziony = true;
                    }
                }
            }

            if (poprzedni_przed_obejrzanym == null)
            {
                poprzedni_przed_obejrzanym = this.lista_sezonow[0].ListaOdcinkow[0];
            }

            return poprzedni_przed_obejrzanym;
        }


        private int liczba_odcinkow_w_ogladanym_sezonie()
        {
            int numer_sezonu = znajdz_sezon_ostatniego_obejrzanego_odcinka();
            if (numer_sezonu != 0)
            {
                return lista_sezonow[numer_sezonu - 1].liczba_odcinkow();
            }
            else
            {
                return 0;
            }
        }

        private int znajdz_sezon_ostatniego_obejrzanego_odcinka()
        {
            int ostatni_obejrzany = 0;

            foreach (Sezon s in lista_sezonow)
            {
                if (s.znajdz_ostatni_obejrzany_odcinek() != null)
                {
                    ostatni_obejrzany = lista_sezonow.IndexOf(s) + 1;
                }
            }

            return ostatni_obejrzany;
        }

        private int liczba_odcinkow_nie_obejrzanych()
        {
            int liczba_odcinkow = LiczbaOdcinkowWyemitowanychSerialu - ObejrzanyOdcinekNumerOdcinkaWWszystkichSezonach;
            if (liczba_odcinkow < 0)
            {
                liczba_odcinkow = 0;
            }
            return liczba_odcinkow;
        }

        private int znajdz_sezon_nastepnego_odcinka()
        {
            int nastepny = 0;
            bool czy_znaleziony = false;

            foreach (Sezon s in lista_sezonow)
            {
                if (!czy_znaleziony)
                {
                    if (s.znajdz_nastepny_po_obejrzanym_odcinku() != null)
                    {
                        nastepny = lista_sezonow.IndexOf(s) + 1;
                        czy_znaleziony = true;
                    }
                }
            }

            return nastepny;
        }

        private string komunikat_z_grupa()
        {
            if (this.Ulubiony == true)
            {
                return "Ulubione";
            }
            int liczba_nie_obejrzanych = liczba_odcinkow_nie_obejrzanych();
            if (liczba_nie_obejrzanych >= 10)
            {
                return "Więcej niż 10 odcinków";
            }
            else if (liczba_nie_obejrzanych > 1 && liczba_nie_obejrzanych < 10)
            {
                return "Kilka odcinków";
            }
            else if (liczba_nie_obejrzanych == 1)
            {
                return "Nowy odcinek";
            }
            else if (liczba_nie_obejrzanych == 0 && this.Archiwum == true)
            {
                return "Archiwum";
            }
            //else if (liczba_nie_obejrzanych == 0 && this.NastepnyOdcinek!=null
            //    && !this.NastepnyOdcinek.DataEmisji.czy_data_w_ciagu_dni(DateTime.Now, 30))
            //{
            //        return "Obecnie nie emitowane";
            //}
            else
            {
                return "Obejrzany";
            }
        }


        #region Właściwości

        public string Nazwa
        {
            get
            {
                return nazwa;
            }
            set
            {
                nazwa = value;
                this.OnPropertyChanged("Nazwa");
            }
        }

        public string TytulObejrzanego
        {
            get
            {
                return tytul_odcinka;
            }
            set
            {
                tytul_odcinka = value;
                this.OnPropertyChanged("TytulObejrzanego");
            }
        }

        public string ZdjecieMale
        {
            get
            {
                return zdjecie_male;
            }
            set
            {
                zdjecie_male = value;
                this.OnPropertyChanged("ZdjecieMale");
            }
        }

        public string ZdjecieDuze
        {
            get
            {
                return zdjecie_duze;
            }
            set
            {
                zdjecie_duze = value;
                this.OnPropertyChanged("ZdjecieDuze");
            }
        }

        public bool Ulubiony
        {
            get
            {
                return ulubiony;
            }
            set
            {
                ulubiony = value;
                this.OnPropertyChanged("Ulubiony");
            }
        }

        public StatusSerialu Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.OnPropertyChanged("Status");
            }
        }

        public List<Sezon> ListSezonow
        {
            get
            {
                return lista_sezonow;
            }
            set
            {
                lista_sezonow = value;
                this.OnPropertyChanged("ListSezonow");
            }
        }

        public string ObejrzanyOdcinekTytul
        {
            get
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    return ostatni_obejrzany.Tytul_odcinka;
                }
                else
                {
                    return "Nie obejrzano jeszcze.";
                }
            }
            set
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    ostatni_obejrzany.Tytul_odcinka = value;
                    this.OnPropertyChanged("ObejrzanyOdcinekTytul");
                }
            }
        }
        public int ObejrzanyOdcinekNumerOdcinka
        {
            get
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    return ostatni_obejrzany.NumerOdcinka;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    ostatni_obejrzany.NumerOdcinka = value;
                    this.OnPropertyChanged("ObejrzanyOdcinekNumerOdcinka");
                }
            }
        }
        public int ObejrzanyOdcinekNumerSezonu
        {
            get
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    return ostatni_obejrzany.NumerSezonu;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    ostatni_obejrzany.NumerSezonu = value;
                    this.OnPropertyChanged("ObejrzanyOdcinekNumerSezonu");
                }
            }
        }
        public int ObejrzanyOdcinekLiczbaOdcinkowWSezonie
        {
            get
            {
                return liczba_odcinkow_w_ogladanym_sezonie();
            }
        }

        public Odcinek PoprzedniOdcinek
        {
            get
            {
                Odcinek poprzedni_przed_obejrzanym = znajdz_poprzedni_przed_obejrzanym_odcinku();                

                return poprzedni_przed_obejrzanym;
            }
        }

        public Odcinek NastepnyOdcinek
        {
            get
            {
                Odcinek nastepny_po_obejrzanym = znajdz_nastepny_po_obejrzanym_odcinku();
                return nastepny_po_obejrzanym;
            }
        }

        public string NastepnyOdcinekTytul
        {
            get
            {
                Odcinek nastepny_po_obejrzanym = znajdz_nastepny_po_obejrzanym_odcinku();
                if (nastepny_po_obejrzanym != null)
                {
                    return nastepny_po_obejrzanym.Tytul_odcinka;
                }
                else
                {
                    return "Niedostępny";
                }
            }
            set
            {
                Odcinek nastepny_po_obejrzanym = znajdz_nastepny_po_obejrzanym_odcinku();
                if (nastepny_po_obejrzanym != null)
                {
                    nastepny_po_obejrzanym.Tytul_odcinka = value;
                    this.OnPropertyChanged("NastepnyOdcinekTytul");
                }
            }
        }
        public int NastepnyOdcinekNumerOdcinka
        {
            get
            {
                Odcinek nastepny_po_obejrzanym = znajdz_nastepny_po_obejrzanym_odcinku();
                if (nastepny_po_obejrzanym != null)
                {
                    return nastepny_po_obejrzanym.NumerOdcinka;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                Odcinek nastepny_po_obejrzanym = znajdz_nastepny_po_obejrzanym_odcinku();
                if (nastepny_po_obejrzanym != null)
                {
                    nastepny_po_obejrzanym.NumerOdcinka = value;
                    this.OnPropertyChanged("NastepnyOdcinekNumerOdcinka");
                }
            }
        }

        public int NastepnyOdcinekNumerSezonu
        {
            get
            {
                return znajdz_sezon_nastepnego_odcinka();
            }
        }

        public int LiczbaSezonow
        {
            get
            {
                return lista_sezonow.Count();
            }
        }
        public int LiczbaWszystkichOdcinkowSerialu
        {
            get
            {
                int liczba_odcinkow = 0;
                foreach (Sezon s in lista_sezonow)
                {
                    liczba_odcinkow += s.LiczbaOdcinkow;
                }

                return liczba_odcinkow;
            }
            set
            {
                this.OnPropertyChanged("LiczbaWszystkichOdcinkowSerialu");
            }
        }

        public int LiczbaOdcinkowWyemitowanychSerialu
        {
            get
            {
                int liczba_odcinkow = 0;
                foreach (Sezon s in lista_sezonow)
                {
                    liczba_odcinkow += s.LiczbaOdcinkowWyemitowanych;
                }

                return liczba_odcinkow;
            }
            set
            {
                this.OnPropertyChanged("LiczbaOdcinkowWyemitowanychSerialu");
            }
        }

        public string NajnowszyOdcinekTytul
        {
            get
            {
                if (lista_sezonow.Count != 0)
                {
                    return lista_sezonow[lista_sezonow.Count - 1].znajdz_najnowszy().Tytul_odcinka;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                this.OnPropertyChanged("NajnowszyOdcinekTytul");
            }
        }

        public int ProcentObejrzanychWSezonie
        {
            get
            {
                int liczba_odcinkow = liczba_odcinkow_w_ogladanym_sezonie();
                if (liczba_odcinkow != 0)
                {
                    return (int)(ObejrzanyOdcinekNumerOdcinka * 100 / liczba_odcinkow);
                }
                else
                {
                    return 100;
                }
            }
        }
        public int ProcentObejrzanychWSezonieWPikselach
        {
            get
            {
                int dlugosc_paska = 698;
                int liczba_odcinkow = liczba_odcinkow_w_ogladanym_sezonie();
                if (liczba_odcinkow != 0)
                {
                    return (int)(ObejrzanyOdcinekNumerOdcinka * dlugosc_paska / liczba_odcinkow);
                }
                else
                {
                    return dlugosc_paska;
                }
            }
        }
        public int ProcentNieObejrzanychWSezonieWPikselach
        {
            get
            {
                int dlugosc_paska = 698;
                int liczba_odcinkow = liczba_odcinkow_w_ogladanym_sezonie();
                if (liczba_odcinkow != 0)
                {
                    return (dlugosc_paska - (int)(ObejrzanyOdcinekNumerOdcinka * dlugosc_paska / liczba_odcinkow) - 10);
                }
                else
                {
                    return 0;
                }
            }
        }


        public int LiczbaDoOdcinkowDoObejrzenia
        {
            get
            {
                return liczba_odcinkow_nie_obejrzanych();
            }
        }

        public string Statystyka
        {
            get
            {
                return komunikat_z_liczba_odcinkow_nie_obejrzanych();
            }
        }

        public int ObejrzanyOdcinekNumerOdcinkaWWszystkichSezonach
        {
            get
            {
                Odcinek ostatni_obejrzany = znajdz_ostatni_obejrzany_odcinek();
                if (ostatni_obejrzany != null)
                {
                    int suma_odcinkow_z_poprzednich_sezonow = 0;
                    for (int i = 0; i < ObejrzanyOdcinekNumerSezonu - 1; i++)
                    {
                        suma_odcinkow_z_poprzednich_sezonow += lista_sezonow[i].liczba_odcinkow();
                    }

                    return suma_odcinkow_z_poprzednich_sezonow + ObejrzanyOdcinekNumerOdcinka;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool Archiwum
        {
            get
            {
                if (this.Status == StatusSerialu.Anulowany||this.Status == StatusSerialu.Zakonczony)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this.OnPropertyChanged("Archiwum");
            }
        }

        public string Grupy
        {
            get
            {
                return komunikat_z_grupa();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string nazwaWlasciowosci)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWlasciowosci));
            }
        }

        #endregion

        //public enum StaryStatusSerialu
        //{
        //    Emitowany,
        //    Normalny,
        //    Zakończony
        //}
    }
}
