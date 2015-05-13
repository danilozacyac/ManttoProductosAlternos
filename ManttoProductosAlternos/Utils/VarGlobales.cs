using System;
using ManttoProductosAlternos.Dto;

namespace ManttoProductosAlternos.Utils
{
    public class VarGlobales
    {
        //public static Temas temaNuevo = null;
        //public static int idSiguiente = 0;

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
                default: return "";
            }
        }

        


        /// <summary>
        /// Mensajes que se envían en  diferentes escenarios de la aplicación para 
        /// confirmar la realización de algunas acciones
        /// </summary>

        public const String MensajeMoverTema = "¿Está seguro de que quiere mover el tema \n";
        public const String MensajeEliminar = "¿Esta seguro que desea eliminar: \n";
        public const String TituloGeneral = "Mantenimiento de temas";
    }
}
