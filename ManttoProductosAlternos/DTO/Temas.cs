using System.ComponentModel;
using System.Collections.ObjectModel;
using ManttoProductosAlternos.Model;

namespace ManttoProductosAlternos.DTO
{
    public class Temas : INotifyPropertyChanged
    {
        static readonly Temas dummyChild = new Temas();
        Temas parent;
        private readonly Temas parentItem;

        bool isExpanded;
        bool isSelected;


        private bool isChecked;

        

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
            }
        }

        private int idTema;

        public int IdTema
        {
            get
            {
                return idTema;
            }
            set
            {
                idTema = value;
            }
        }

        private string tema;

        public string Tema
        {
            get
            {
                return tema;
            }
            set
            {
                tema = value;
                this.OnPropertyChanged("Tema");
                longitudTema = 250 - tema.Length;
            }
        }

        private int orden;

        public int Orden
        {
            get
            {
                return orden;
            }
            set
            {
                orden = value;
            }
        }

        private string temaStr;

        public string TemaStr
        {
            get
            {
                return temaStr;
            }
            set
            {
                temaStr = value;
            }
        }

        private char lInicial;

        public char LInicial
        {
            get
            {
                return lInicial;
            }
            set
            {
                lInicial = value;
            }
        }

        private int nivel;

        public int Nivel
        {
            get
            {
                return nivel;
            }
            set
            {
                nivel = value;
            }
        }

        private int longitudTema;

        public int LongitudTema
        {
            get
            {
                if (this.longitudTema == 0)
                    longitudTema = tema.Length;

                return this.longitudTema;
            }
            set
            {
                this.longitudTema = value;
                this.OnPropertyChanged("LongitudTema");
            }
        }

        private int padre;

        public int Padre
        {
            get
            {
                return padre;
            }
            set
            {
                padre = value;
            }
        }

        /// <summary>
        /// Permite diferenciar a que "tematico" es al que quiero acceder
        /// </summary>
        private int idProducto;

        public int IdProducto
        {
            get
            {
                return idProducto;
            }
            set
            {
                idProducto = value;
            }
        }

        private ObservableCollection<Temas> subTemas;

        public ObservableCollection<Temas> SubTemas
        {
            get
            {
                if (this.subTemas == null)
                {
                    this.subTemas = new ObservableCollection<Temas>();
                }
                return this.subTemas;
            }
            set
            {
                this.subTemas = value;
            }
        }

        #region Constructores

        public Temas()
        {
        }

        public Temas(Temas parent)
        {
            this.parentItem = parent;
        }

        public Temas(Temas parent, bool lazyLoadChildren)
        {
            this.parent = parent;

            subTemas = new ObservableCollection<Temas>();

            if (lazyLoadChildren)
                subTemas.Add(dummyChild);
        }

        #endregion

        #region HasLoadedChildren

        /// <summary>
        /// Returns true if this object's Children have not yet been populated.
        /// </summary>
        public bool HasDummyChild
        {
            get
            {
                return this.SubTemas.Count == 1 && this.SubTemas[0] == dummyChild;
            }
        }

        #endregion // HasLoadedChildren

        #region Parent

        public Temas Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
            }
        }

        #endregion

        #region IsExpanded

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
                // Expand all the way up to the root.
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;
                // Lazy load the child items, if necessary.
                if (this.HasDummyChild)
                {
                    this.SubTemas.Remove(dummyChild);
                    this.LoadChildren();
                }
            }
        }

        #endregion // IsExpanded

        #region LoadChildren

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
            //SubTemas = this.GetTemas(this, this.Materia);
            foreach (Temas item in new TemasModel(this.idProducto).GetTemas(this.Padre))
                SubTemas.Add(item);
        }

        #endregion // LoadChildren

        #region AddRemove Children

        public virtual void AddSubtema(Temas child)
        {
            child.Parent = this;
            SubTemas.Add(child);
        }

        public virtual void RemoveSubTema(Temas child)
        {
            SubTemas.Remove(child);
        }

        #endregion
        #region IsSelected

        /// <summary>
        /// Gets/sets whether the TreeViewItem 
        /// associated with this object is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion // IsSelected




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