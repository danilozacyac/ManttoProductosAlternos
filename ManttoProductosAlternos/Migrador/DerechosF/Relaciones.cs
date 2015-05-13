using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManttoProductosAlternos.Migrador.DerechosF
{
    public class Relaciones
    {
        private int idClasifDisco;
        private int ius;

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

        public int Ius
        {
            get
            {
                return this.ius;
            }
            set
            {
                this.ius = value;
            }
        }
    }
}
