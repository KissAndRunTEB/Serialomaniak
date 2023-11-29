using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Biblioteka
{
    class Uwierzytelnianie
    {
        // POLEPSZYC: Zaszyfrowac hasła i loginy
        // POLEPSZYC: Usunąć DailyStars

        private static string login_serialomaniak = "singit";
        private static string haslo_serialomaniak = "7xFcFxKn";

        private static NetworkCredential serialomaniak = new NetworkCredential(login_serialomaniak, haslo_serialomaniak);

        //private static string login_dailystars = "dailysta";
        //private static string haslo_dailystars = "Xq2QGZtF";

        //private static NetworkCredential dailystars = new NetworkCredential(login_dailystars, haslo_dailystars);

        public static NetworkCredential pobierz(Enum serwer)
        {
            if ((Serwery)serwer == Serwery.Serialomaniak)
            {
                return serialomaniak;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }

    public enum Serwery
    {
        Serialomaniak,
    }
}
