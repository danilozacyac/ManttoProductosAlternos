using System;

namespace ManttoProductosAlternos.DTO
{
    public class Usuarios
    {
        private int llave;

        public int Llave
        {
            get { return llave; }
            set { llave = value; }
        }

        private String nombre;

        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        private String usuario;

        public String Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        private String contrasena;

        public String Contrasena
        {
            get { return contrasena; }
            set { contrasena = value; }
        }

        private String autorizados;

        public String Autorizados
        {
            get { return autorizados; }
            set { autorizados = value; }
        }

        private int grupo;

        public int Grupo
        {
            get { return grupo; }
            set { grupo = value; }
        }
    }
}
