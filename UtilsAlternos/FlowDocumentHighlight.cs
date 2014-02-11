using System;
using System.Linq;


namespace UtilsAlternos
{
    public class FlowDocumentHighlight
    {

        public static string Normaliza(string item)
        {
            String resultado = item.ToLower();
            resultado = resultado.Trim(Constants.EmpiezaCon.ToCharArray());
            resultado = resultado.Trim(Constants.TerminaCon.ToCharArray());
            resultado = resultado.Replace('ñ', 'n');
            resultado = resultado.Replace('á', 'a');
            resultado = resultado.Replace('é', 'e');
            resultado = resultado.Replace('í', 'i');
            resultado = resultado.Replace('ó', 'o');
            resultado = resultado.Replace('ú', 'u');
            resultado = resultado.Replace('ä', 'a');
            resultado = resultado.Replace('ë', 'e');
            resultado = resultado.Replace('ï', 'i');
            resultado = resultado.Replace('ö', 'o');
            resultado = resultado.Replace('ü', 'u');
            resultado = resultado.Replace("*", "");
            return resultado.ToUpper();
        }

        public static string NormalizaSinAsterisco(string item)
        {
            String resultado = item.ToLower();
            resultado = resultado.Trim(Constants.EmpiezaCon.ToCharArray());
            resultado = resultado.Trim(Constants.TerminaCon.ToCharArray());
            //resultado = resultado.Replace('ñ', 'n');
            resultado = resultado.Replace('á', 'a');
            resultado = resultado.Replace('é', 'e');
            resultado = resultado.Replace('í', 'i');
            resultado = resultado.Replace('ó', 'o');
            resultado = resultado.Replace('ú', 'u');
            resultado = resultado.Replace('ä', 'a');
            resultado = resultado.Replace('ë', 'e');
            resultado = resultado.Replace('ï', 'i');
            resultado = resultado.Replace('ö', 'o');
            resultado = resultado.Replace('ü', 'u');
            return resultado.ToUpper();
        }

        public static string QuitaCarOrden(string cCadena)
        {
            string cChr = "";
            string sCadena = cCadena;

            sCadena = CambiaLtr123(sCadena, "+", "");
            sCadena = CambiaLtr123(sCadena, "=", "");
            sCadena = CambiaLtr123(sCadena, "*", "");
            sCadena = CambiaLtr123(sCadena, "&", "");
            sCadena = CambiaLtr123(sCadena, "^", "");
            sCadena = CambiaLtr123(sCadena, "$", "");

            sCadena = CambiaLtr123(sCadena, "#", "");
            sCadena = CambiaLtr123(sCadena, "@", "");
            sCadena = CambiaLtr123(sCadena, "!", "");
            sCadena = CambiaLtr123(sCadena, "¡", "");
            sCadena = CambiaLtr123(sCadena, "?", "");
            sCadena = CambiaLtr123(sCadena, "¿", "");
            sCadena = CambiaLtr123(sCadena, "<", "");
            sCadena = CambiaLtr123(sCadena, ">", "");
            sCadena = CambiaLtr123(sCadena, "~", "");

            sCadena = CambiaLtr123(sCadena, "|", "");
            sCadena = CambiaLtr123(sCadena, "°", "");
            sCadena = CambiaLtr123(sCadena, "ª", "");
            sCadena = CambiaLtr123(sCadena, "º", "");

            sCadena = CambiaLtr123(sCadena, ".", "");
            sCadena = CambiaLtr123(sCadena, ",", "");
            sCadena = CambiaLtr123(sCadena, ":", "");
            sCadena = CambiaLtr123(sCadena, ";", "");
            sCadena = CambiaLtr123(sCadena, "%", "");

            sCadena = CambiaLtr123(sCadena, "(", "");
            sCadena = CambiaLtr123(sCadena, ")", "");
            sCadena = CambiaLtr123(sCadena, "[", "");
            sCadena = CambiaLtr123(sCadena, "]", "");
            sCadena = CambiaLtr123(sCadena, "{", "");
            sCadena = CambiaLtr123(sCadena, "}", "");
            sCadena = CambiaLtr123(sCadena, "`", "");
            sCadena = CambiaLtr123(sCadena, "-", "");
            sCadena = CambiaLtr123(sCadena, "_", "");
            sCadena = CambiaLtr123(sCadena, "/", "");



            cChr = Convert.ToChar(92).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, "");
            sCadena = CambiaLtr123(sCadena, "'", "");

            cChr = Convert.ToChar(34).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, "");

            cChr = Convert.ToChar(13).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, "");

            cChr = Convert.ToChar(10).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, "");

            char comienza = Convert.ToChar(sCadena.Substring(0, 1));
            if (Char.IsDigit(comienza))
            {
                sCadena = sCadena.Replace("0", "");
                sCadena = sCadena.Replace("1", "");
                sCadena = sCadena.Replace("2", "");
                sCadena = sCadena.Replace("3", "");
                sCadena = sCadena.Replace("4", "");
                sCadena = sCadena.Replace("5", "");
                sCadena = sCadena.Replace("6", "");
                sCadena = sCadena.Replace("7", "");
                sCadena = sCadena.Replace("8", "");
                sCadena = sCadena.Replace("9", "");

            }


            return sCadena;
        }  // fin QuitaCarCad

        public static string QuitaCarCad(string cCadena)
        {
            string cChr = "";
            string sCadena = cCadena;

            sCadena = CambiaLtr123(sCadena, "+", " ");
            sCadena = CambiaLtr123(sCadena, "=", " ");
            sCadena = CambiaLtr123(sCadena, "*", " ");
            sCadena = CambiaLtr123(sCadena, "&", " ");
            sCadena = CambiaLtr123(sCadena, "^", " ");
            sCadena = CambiaLtr123(sCadena, "$", " ");

            sCadena = CambiaLtr123(sCadena, "#", " ");
            sCadena = CambiaLtr123(sCadena, "@", " ");
            sCadena = CambiaLtr123(sCadena, "!", " ");
            sCadena = CambiaLtr123(sCadena, "¡", " ");
            sCadena = CambiaLtr123(sCadena, "?", " ");
            sCadena = CambiaLtr123(sCadena, "¿", " ");
            sCadena = CambiaLtr123(sCadena, "<", " ");
            sCadena = CambiaLtr123(sCadena, ">", " ");
            sCadena = CambiaLtr123(sCadena, "~", " ");

            sCadena = CambiaLtr123(sCadena, "|", " ");
            sCadena = CambiaLtr123(sCadena, "°", " ");
            sCadena = CambiaLtr123(sCadena, "ª", " ");
            sCadena = CambiaLtr123(sCadena, "º", " ");

            sCadena = CambiaLtr123(sCadena, ".", " ");
            sCadena = CambiaLtr123(sCadena, ",", " ");
            sCadena = CambiaLtr123(sCadena, ":", " ");
            sCadena = CambiaLtr123(sCadena, ";", " ");
            sCadena = CambiaLtr123(sCadena, "%", " ");

            sCadena = CambiaLtr123(sCadena, "(", " ");
            sCadena = CambiaLtr123(sCadena, ")", " ");
            sCadena = CambiaLtr123(sCadena, "[", " ");
            sCadena = CambiaLtr123(sCadena, "]", " ");
            sCadena = CambiaLtr123(sCadena, "{", " ");
            sCadena = CambiaLtr123(sCadena, "}", " ");
            sCadena = CambiaLtr123(sCadena, "`", " ");
            sCadena = CambiaLtr123(sCadena, "-", " ");
            sCadena = CambiaLtr123(sCadena, "_", " ");
            sCadena = CambiaLtr123(sCadena, "/", " ");


            cChr = Convert.ToChar(92).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, " ");
            sCadena = CambiaLtr123(sCadena, "'", " ");

            cChr = Convert.ToChar(34).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, " ");

            cChr = Convert.ToChar(13).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, " ");

            cChr = Convert.ToChar(10).ToString();
            sCadena = CambiaLtr123(sCadena, cChr, " ");


            return sCadena;
        }

        public static string CambiaLtr123(string cLineaPr, string cCarOld, string cCarNvo)
        {
            string cLinea = cLineaPr;
            string cLin;
            string cLineaTmp;
            int nPos, nAct;
            int nLengthLinea;
            int nLengthCarOld;

            nLengthCarOld = cCarOld.Length;
            nAct = 1;
            cLin = "";
            nPos = cLinea.IndexOf(cCarOld, nAct);

            while (nPos > 0)
            {
                cLin = cLin + cLinea.Substring(0, nPos) + cCarNvo;
                nLengthLinea = cLinea.Length - (nPos + nLengthCarOld);
                cLineaTmp = cLinea.Substring(nPos + nLengthCarOld, nLengthLinea);
                cLinea = cLineaTmp;

                if (cLinea != "")
                {
                    nPos = cLinea.IndexOf(cCarOld, nAct);
                }
                else
                {
                    nPos = 0;
                }

            }
            cLin = cLin + cLinea;

            return cLin;
        }  // fin CambiaLtr123



    }
}
