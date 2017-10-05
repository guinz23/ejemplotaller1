using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIADES;
using CONEXION;
using NpgsqlTypes;

namespace DAO
{
    public class alumnoDAO
    {
        private AccesoDatosPostgre conexion;
        public alumnoDAO() {
            conexion = AccesoDatosPostgre.Instance;
        }

        public void insertarAlumno(alumno alumno)
        {
            string sql = "INSERT INTO " + this.conexion.Schema + "alumnos(nombre, apellido, direccion, correo, telefono) " +
                             "values(@nombre,@apellido,@direccion,@correo,@telefono)";
            Parametro prm = new Parametro();
            prm.agregarParametro("@nombre", NpgsqlDbType.Varchar, alumno.Nombre);
            prm.agregarParametro("@apellido", NpgsqlDbType.Varchar, alumno.Apellido);
            prm.agregarParametro("@direccion", NpgsqlDbType.Varchar, alumno.Direccion);
            prm.agregarParametro("@correo", NpgsqlDbType.Varchar, alumno.Correo);
            prm.agregarParametro("@telefono", NpgsqlDbType.Integer, alumno.Telefono);
            this.conexion.ejecutarSQL(sql, prm.obtenerParametros());
        }
    }
}
