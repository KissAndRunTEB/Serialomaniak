using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Serialomaniak.Konta
{
    public class Autostart
    {
        #region Autostart

        public static void przelacz_autostart(bool wlaczenie, string nazwa_programu)
        {
            if (wlaczenie)
            {
                wlacz_autostart(nazwa_programu);
            }
            else
            {
                if (czy_autostart_wlaczony(nazwa_programu))
                {
                    wylacz_autostart(nazwa_programu);
                }
            }
        }

        public static void wlacz_autostart(string nazwa_programu)
        {
            string nazwa_klucza = nazwa_programu;
            string lokacja_rejestru = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string lokacja_programu = Assembly.GetExecutingAssembly().Location;

            RegistryKey klucz = Registry.CurrentUser.CreateSubKey(lokacja_rejestru);
            klucz.SetValue(nazwa_klucza, lokacja_programu);
        }

        public static void wylacz_autostart(string nazwa_programu)
        {
            string nazwa_klucza = nazwa_programu;
            string lokacja_rejestru = @"Software\Microsoft\Windows\CurrentVersion\Run";

            RegistryKey klucz = Registry.CurrentUser.CreateSubKey(lokacja_rejestru);
            klucz.DeleteValue(nazwa_klucza);
        }

        public static bool czy_autostart_wlaczony(string nazwa_programu)
        {
            string nazwa_klucza = nazwa_programu;
            string lokacja_rejestru = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string lokacja_programu = Assembly.GetExecutingAssembly().Location;

            RegistryKey klucz = Registry.CurrentUser.OpenSubKey(lokacja_rejestru);

            if (klucz == null)
            {
                return false;
            }

            string wartosc = (string)klucz.GetValue(nazwa_klucza);
            if (wartosc == null)
            {
                return false;
            }

            return (wartosc == lokacja_programu);
        }
        #endregion
    }
}
