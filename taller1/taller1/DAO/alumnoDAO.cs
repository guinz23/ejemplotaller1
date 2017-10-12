using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIADES;
using CONEXION;
using NpgsqlTypes;
using System.Data;

namespace DAO
{
    public class alumnoDAO
    {
        private List<alumno> Alumnos;
        private AccesoDatosPostgre conexion;
        public alumnoDAO() {
            conexion = AccesoDatosPostgre.Instance;
            Alumnos = new List<alumno>();
        }

        public List<alumno> Alumnos1 { get => Alumnos; set => Alumnos = value; }

        public void insertarAlumno(alumno alumno)
        {
            try
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

                if (conexion.IsError)
                {
                    Exception e = new Exception(this.conexion.ErrorDescripcion);

                    throw e;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }
        public List<alumno> obtnerAlumnos()
        {

            try
            {
               
                DataSet dsetEmpleados;
                string sql = "select * from alumnos";
                dsetEmpleados = this.conexion.ejecutarConsultaSQL(sql);
                foreach (DataRow posicion in dsetEmpleados.Tables[0].Rows)
                {
                    alumno pAlumnos = new alumno();
                    pAlumnos.Id = Int32.Parse(posicion["id_alumno"].ToString());
                    pAlumnos.Nombre = posicion["nombre"].ToString();
                    pAlumnos.Apellido = posicion["apellido"].ToString();
                    pAlumnos.Direccion = posicion["direccion"].ToString();
                    pAlumnos.Correo = posicion["correo"].ToString();
                    pAlumnos.Telefono =Int32.Parse( posicion["telefono"].ToString());

                    Alumnos1.Add(pAlumnos);


                }
                if (conexion.IsError)
                {
                    Exception e = new Exception(this.conexion.ErrorDescripcion);

                    throw e;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Alumnos1;
        }
    }
}
