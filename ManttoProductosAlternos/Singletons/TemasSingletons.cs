using System;
using System.Collections.ObjectModel;
using System.Linq;
using ManttoProductosAlternos.Dto;
using ManttoProductosAlternos.Model;

namespace ManttoProductosAlternos.Singletons
{
    public class TemasSingletons
    {
        
        private static ObservableCollection<Temas> agraria;
        private static ObservableCollection<Temas> suspension;
        private static ObservableCollection<Temas> improcedencia;
        private static ObservableCollection<Temas> facultades;
        private static ObservableCollection<Temas> electoral;

        private TemasSingletons()
        {
        }

        public static ObservableCollection<Temas> Temas(int idMateria)
        {
            switch (idMateria)
            {
                case 1:
                    if (agraria == null)
                        agraria = new TemasModel(idMateria).GetTemas(null);

                    return agraria;

                case 2:
                    if (suspension == null)
                        suspension = new TemasModel(idMateria).GetTemas(null);

                    return suspension;

                case 3:
                    if (improcedencia == null)
                        improcedencia = new TemasModel(idMateria).GetTemas(null);

                    return improcedencia;

                case 4:
                    if (facultades == null)
                        facultades = new TemasModel(idMateria).GetTemas(null);

                    return facultades;

                case 15:
                    if (electoral == null)
                        electoral = new TemasModel(idMateria).GetTemas(null);

                    return electoral;

               

                default: return null;
            }


        }


    }
}
