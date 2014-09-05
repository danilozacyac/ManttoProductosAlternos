using System;
using System.Linq;

namespace UtilsAlternos
{
    public class Constants
    {

        public const int WinHeightAgregaTema = 180;
        public const int WinHeightAgregaTemaLargo = 452;


        /// <summary>
        /// Mensajes que se envían en  diferentes escenarios de la aplicación para 
        /// confirmar la realización de algunas acciones
        /// </summary>

        public const String MensajeMoverTema = "¿Está seguro de que quiere mover el tema \n";
        public const String MensajeEliminar = "¿Esta seguro que desea eliminar: \n";
        public const String TituloGeneral = "Mantenimiento de temas";
        
        ///// <summary>
        /////     Los separadores para juntar campos en busqueda por palabras
        ///// </summary>
        ///// <remarks>
        /////     
        ///// </remarks>
        //public const String SeparadorFrases = " &&& ";

        /// <summary>
        /// Los comodines permiidos en la búsqueda.
        /// </summary>
        public static String[] Comodines = { "*", "?" };
        public static String EmpiezaCon = "|";
        public static String TerminaCon = "'";
        /// <summary>
        /// Las palabras que no se incluyen en busquedas o que no deben ser pintadas en los
        /// resultados de las mismas.
        /// </summary>
        public static String[] Stopers = new String[]
        {
            "el", "la", "las", "le", "lo", "los", "no", ".",
            "pero", "puede", "se", "sus", "y", "o", "n", "a", "al", "aquel", "aun", "cada", "como", "con", "cual",
            "de", "debe", "deben", "del", "el", "en", "este", "esta", "la", "las", "le", "lo", "los",
            "para", "pero", "por", "puede", "que", "se", "sin", "sus", "un", "una"
        };
        //public const String CadenaVacia = "";
        ///// <summary>
        ///// Los separadores comunes de las palabras.
        ///// </summary>
        //public static String[] Separadores = new String[] { " ", ",", ".", "\n", "\"", ";", ":", "'", "´", "‘", ")", "(" };
        
        //public static String[] NOPermitidosCorreo = new String[]
        //{
        //    "+", "=", "'", "&", "^", "$", "#",
        //    "!", "¡", "¿", "?", "<", ">", "~", "¬", "|", "°", ",", ";", ";", "%", "\n",
        //    "(", ")", "[", "]", "{", "}", "´", "¨", "`", "¥", "€", "\""
        //};
        ///// <summary>
        ///// Caracteres no permitidos en una búsqueda por palabra.
        ///// </summary>
        //public static String[] NOPermitidos = new String[]
        //{
        //    "+", "=", "'", "&", "^", "$", "#", "@", "-", "\\",
        //    "!", "¡", "¿", "?", "<", ">", "~", "¬", "|", "°", ";", ";", "%", "\n",
        //    "(", ")", "[", "]", "{", "}", "´", "¨", "_", "`", "¥", "€"
        //};
    }
}