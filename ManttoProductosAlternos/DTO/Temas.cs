
namespace ManttoProductosAlternos.DTO
{
    public class Temas
    {
        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string tema;

        public string Tema
        {
            get { return tema; }
            set { tema = value; }
        }

        private int orden;

        public int Orden
        {
            get { return orden; }
            set { orden = value; }
        }

        private string temaStr;

        public string TemaStr
        {
            get { return temaStr; }
            set { temaStr = value; }
        }

        private char lInicial;

        public char LInicial
        {
            get { return lInicial; }
            set { lInicial = value; }
        }


        private int nivel;

        public int Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }

        private int padre;

        public int Padre
        {
            get { return padre; }
            set { padre = value; }
        }

        private int idProd;

        public int IdProd
        {
            get { return idProd; }
            set { idProd = value; }
        }
    }
}

