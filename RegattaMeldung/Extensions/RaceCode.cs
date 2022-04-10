using RegattaMeldung.Data;
using RegattaMeldung.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Extensions
{
    public static class RaceCode
    {        
        public static string getRaceCode(string gender, Competition competition, Oldclass oldclass)
        {
            string rc = "";

            string c1, c2, c3, c5, c6 = "0";

            string bcname = competition.Boatclasses.Name;
            string ocname = oldclass.Name;
            int racelength = competition.Raceclasses.Length;

            switch (bcname)
            {
                case "K1":
                    c1 = "1";
                    break;
                case "K2":
                    c1 = "2";
                    break;
                case "K4":
                    c1 = "4";
                    break;
                case "C1":
                    c1 = "5";
                    break;
                case "C2":
                    c1 = "6";
                    break;
                case "S4":
                    c1 = "8";
                    break;
                case "S8":
                    c1 = "9";
                    break;
                default:
                    c1 = "0";
                    break;
            }

            switch (gender)
            {
                case "M":
                    c2 = "1";
                    break;
                case "W":
                    c2 = "2";
                    break;
                case "X":
                    c2 = "3";
                    break;
                default:
                    c2 = "0";
                    break;
            }

            switch (ocname)
            {
                case "Schüler D":
                    c3 = "19";
                    break;
                case "Schüler C/B10":
                    c3 = "01";
                    break;
                case "Schüler C7":
                    c3 = "16";
                    break;
                case "Schüler C8":
                    c3 = "17";
                    break;
                case "Schüler C9":
                    c3 = "18";
                    break;
                case "Schüler B10":
                    c3 = "02";
                    break;
                case "Schüler B11":
                    c3 = "03";
                    break;
                case "Schüler B12":
                    c3 = "04";
                    break;
                case "Schüler B":
                    c3 = "05";
                    break;
                case "Schüler A13":
                    c3 = "06";
                    break;
                case "Schüler A14":
                    c3 = "07";
                    break;
                case "Schüler A":
                    c3 = "08";
                    break;
                case "Jugend":
                    c3 = "09";
                    break;
                case "Junioren":
                    c3 = "10";
                    break;
                case "Leistungsklasse":
                    c3 = "11";
                    break;
                case "Senioren A":
                    c3 = "12";
                    break;
                case "Senioren B":
                    c3 = "13";
                    break;
                case "Senioren C":
                    c3 = "14";
                    break;
                case "Senioren D":
                    c3 = "15";
                    break;
                case "Senioren":
                    c3 = "20";
                    break;
                default:
                    c3 = "00";
                    break;
            }

            switch (racelength)
            {
                case 250:
                    c5 = "1";
                    break;
                case 100:
                    c5 = "0";
                    break;
                case 200:
                    c5 = "3";
                    break;
                case 2000:
                    c5 = "4";
                    break;
                case 4000:
                    c5 = "5";
                    break;
                default:
                    c5 = "0";
                    break;
            }

            c6 = "E";

            rc = c1 + c2 + c3 + c5 + c6;

            return rc;
        }
    }
}
