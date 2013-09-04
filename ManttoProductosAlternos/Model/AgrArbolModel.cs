using System.Collections.Generic;
using System.Windows.Controls;
using ManttoProductosAlternos.DTO;

namespace ManttoProductosAlternos.Model
{
    public class GeneraArbol
    {
        
        public List<TreeViewItem> GeneraAgraria(int idPadre,int idProd)
        {
            List<TreeViewItem> temasSubT = new List<TreeViewItem>();
            List<Temas> temas = new TemasModel(idProd).GetTemas(idPadre);

            foreach (Temas tema in temas)
            {
                TreeViewItem padres = new TreeViewItem();
                padres.Tag = tema;
                padres.Header = tema.Tema;
                if(idProd == 1)
                    GetHijos(tema.Id, padres,idProd);
                temasSubT.Add(padres);
            }

            return temasSubT;
        }

        private TreeViewItem GetHijos(int idPadre, TreeViewItem nodoPadre,int idProd)
        {
            TreeViewItem temasSubT = new TreeViewItem();
            List<Temas> temas = new TemasModel(idProd).GetTemas(idPadre);

            foreach (Temas tema in temas)
            {
                TreeViewItem hijos = new TreeViewItem();
                hijos.Tag = tema;
                hijos.Header = tema.Tema;
                GetHijos(tema.Id, hijos,idProd);
                nodoPadre.Items.Add(hijos);
            }
            return temasSubT;
        }
    }
}
