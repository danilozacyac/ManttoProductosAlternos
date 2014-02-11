using System;
using System.ComponentModel;
using System.Linq;

namespace ManttoProductosAlternos.DTO
{
    class TemaNuevo
    {
        private int longitudTema;
        private String nuevoTema;

        public int LongitudTema
        {
            get
            {
                return this.longitudTema;
            }
            set
            {
                this.longitudTema = value;
                this.OnPropertyChanged("LongitudTema");
            }
        }

        public string NuevoTema
        {
            get
            {
                return this.nuevoTema;
            }
            set
            {
                this.nuevoTema = value;
                
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
    }
}
