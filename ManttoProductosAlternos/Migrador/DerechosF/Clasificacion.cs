using System;
using System.Linq;

namespace ManttoProductosAlternos.Migrador.DerechosF
{
    public class Clasificacion
    {
        private int idClasifDisco;
        private string descripcion;
        private int idClasifScjn;
        private int idClasifTcc;
        private int idClasifPc;
        private int totalTesisRelacionadas;

        public int IdClasifDisco
        {
            get
            {
                return this.idClasifDisco;
            }
            set
            {
                this.idClasifDisco = value;
            }
        }

        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
            }
        }

        public int IdClasifScjn
        {
            get
            {
                return this.idClasifScjn;
            }
            set
            {
                this.idClasifScjn = value;
            }
        }

        public int IdClasifTcc
        {
            get
            {
                return this.idClasifTcc;
            }
            set
            {
                this.idClasifTcc = value;
            }
        }

        public int IdClasifPc
        {
            get
            {
                return this.idClasifPc;
            }
            set
            {
                this.idClasifPc = value;
            }
        }

        public int TotalTesisRelacionadas
        {
            get
            {
                return this.totalTesisRelacionadas;
            }
            set
            {
                this.totalTesisRelacionadas = value;
            }
        }

        
    }
}
