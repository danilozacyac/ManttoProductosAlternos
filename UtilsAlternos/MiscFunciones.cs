using System;

namespace UtilsAlternos
{
    public class MiscFunciones
    {


        /// <summary>
        /// Convierte a mayúsculas las vocales cuando tienen acento o diéresis en el caso de la U, también la Ñ
        /// </summary>
        /// <param name="cCadena"></param>
        /// <returns></returns>
        public static string ConvMay(string cCadena)
        {
            string sCadena = cCadena;

            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "á", "A");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "é", "E");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "í", "I");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "ó", "O");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "ú", "U");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "ñ", "Ñ");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "ü", "U");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "Ü", "U");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "Á", "A");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "É", "E");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "Í", "I");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "Ó", "O");
            sCadena = FlowDocumentHighlight.CambiaLtr123(sCadena, "Ú", "U");

            sCadena.ToUpper();
            return sCadena;
        }


        public static String GetTemasStr(String cCadena)
        {
            String texto = ""; 
            cCadena = MiscFunciones.ConvMay(FlowDocumentHighlight.QuitaCarCad(cCadena)).ToUpper();

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

        /// <summary>
        /// Devuelve el título de la ventana principal de acuerdo al usuario que iongreso
        /// </summary>
        /// <param name="idProducto"></param>
        /// <returns></returns>
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
