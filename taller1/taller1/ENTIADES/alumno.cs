using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTIADES
{
    public class alumno
    {
        private int id;
        private string nombre;
        private string apellido;
        private string direccion;
        private string correo;
        private int telefono;

        public alumno() {

        }
        public alumno(int id, string nombre, string apellido, string direccion, string correo, int telefono)
        {
            this.id = id;
            this.nombre = nombre;
            this.apellido = apellido;
            this.direccion = direccion;
            this.correo = correo;
            this.telefono = telefono;
        }

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Correo { get => correo; set => correo = value; }
        public int Telefono { get => telefono; set => telefono = value; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
   
}
