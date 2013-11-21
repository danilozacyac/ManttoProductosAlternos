using System.Collections.Generic;
using System.Windows.Controls;
using ManttoProductosAlternos.DTO;
using System.Collections.ObjectModel;

namespace ManttoProductosAlternos.Model
{
    public class GeneraArbol
    {
        
        public List<TreeViewItem> GeneraAgraria(int idPadre,int idProd)
        {
            List<TreeViewItem> temasSubT = new List<TreeViewItem>();
            ObservableCollection<Temas> temas = new TemasModel(idProd).GetTemas(idPadre);

            foreach (Temas tema in temas)
            {
                TreeViewItem padres = new TreeViewItem();
                padres.Tag = tema;
                padres.Header = tema.Tema;
                if(idProd == 1)
                    GetHijos(tema.IdTema, padres,idProd);
                temasSubT.Add(padres);
            }

            return temasSubT;
        }

        private TreeViewItem GetHijos(int idPadre, TreeViewItem nodoPadre,int idProd)
        {
            TreeViewItem temasSubT = new TreeViewItem();
            ObservableCollection<Temas> temas = new TemasModel(idProd).GetTemas(idPadre);

            foreach (Temas tema in temas)
            {
                TreeViewItem hijos = new TreeViewItem();
                hijos.Tag = tema;
                hijos.Header = tema.Tema;
                GetHijos(tema.IdTema, hijos,idProd);
                nodoPadre.Items.Add(hijos);
            }
            return temasSubT;
        }
    }
}
