using System;

namespace UtilsAlternos
{
    public class MiscFunciones
    {

        public static string CambiaLtr123(string cLineaPr, string cCarOld, string cCarNvo)
        {
            string cLinea = cLineaPr;
            string cLin;
            string cLineaTmp;
            int nPos, nAct;
            int nLengthLinea;
            int nLengthCarOld;


            //  C# IndexOf | VB6 InStr

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
        }  // fin QuitaCarCad

        public static string ConvMay(string cCadena)
        {
            string sCadena = cCadena;

            sCadena = CambiaLtr123(sCadena, "á", "A");
            sCadena = CambiaLtr123(sCadena, "é", "E");
            sCadena = CambiaLtr123(sCadena, "í", "I");
            sCadena = CambiaLtr123(sCadena, "ó", "O");
            sCadena = CambiaLtr123(sCadena, "ú", "U");
            sCadena = CambiaLtr123(sCadena, "ñ", "Ñ");
            sCadena = CambiaLtr123(sCadena, "ü", "U");
            sCadena = CambiaLtr123(sCadena, "Ü", "U");
            sCadena = CambiaLtr123(sCadena, "Á", "A");
            sCadena = CambiaLtr123(sCadena, "É", "E");
            sCadena = CambiaLtr123(sCadena, "Í", "I");
            sCadena = CambiaLtr123(sCadena, "Ó", "O");
            sCadena = CambiaLtr123(sCadena, "Ú", "U");

            sCadena.ToUpper();
            return sCadena;
        }


        public static String GetTemasStr(String cCadena)
        {
            String texto = ""; 
            cCadena = MiscFunciones.ConvMay(MiscFunciones.QuitaCarCad(cCadena)).ToUpper();

            foreach (String palabra in cCadena.Split(' '))
            {
                int x = 0;
                bool result = Int32.TryParse(palabra, out x);

                if (!result)
                {
                    String numeric = "";
                    String complement = "";
                    foreach (char letra in palabra.ToCharArray())
                    {
                        if (Char.IsDigit(letra))
                        {
                            numeric += letra;
                        }
                        else
                            complement += letra;

                    }
                    numeric = SetCeros(numeric) + complement;

                    texto += numeric;
                }
                else
                {
                    texto += SetCeros(palabra);
                }

            }

            return cCadena;
        }

        private static String SetCeros(String cCadena)
        {
            switch (cCadena.Length)
            {
                case 1: return cCadena = "000" + cCadena;
                    
                case 2: return cCadena = "00" + cCadena;
                    
                case 3: return cCadena = "0" + cCadena;
                    
                default: return cCadena;
            }

        }

        public static String TituloVentanas(int idProducto)
        {
            switch (idProducto)
            {
                case 1: return "Jurisprudencia en Materia Agraria";
                case 2: return "Suspensión del Acto Reclamado";
                case 3: return "Improcedencia del Juicio de Amparo";
                case 4: return "Facultades Exclusivas de la SCJN";
                default : return "";
            }
        }


    }
}
