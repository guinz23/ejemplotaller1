using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENTIADES;
using DAO;

namespace BO
{
    public class alumnoBO

    { 
        private alumnoDAO AlumnoDAO;
       public bool validacion;
        public alumnoBO()
        {
            validacion = false;
            AlumnoDAO = new alumnoDAO();
        }
        public void verificarAlumno(alumno alumno)
        {

            try
            {
                if (alumno.Nombre == String.Empty)
                {
                    throw new Exception("Ingrese un nombre ");
                }
                if (alumno.Apellido == String.Empty)
                {
                    throw new Exception("Ingrese un Apellido");
                }
                if (alumno.Direccion == String.Empty)
                {
                    throw new Exception("Ingrese una Direccion");
                }
                if (alumno.Correo == String.Empty)
                {
                    throw new Exception("Ingrese un correo");
                }
                if (alumno.Telefono <= 0)
                {
                    throw new Exception("ingrese un numero de telefono");
                }
                if (alumno.Telefono.Equals(""))
                {
                    throw new Exception("ingrese solamente numeros");
                }
                string validar = alumno.Correo;
                for (int i = 0; i < validar.Length; i++)
                {

                    char a = validar[i];
                    if (a ==  '@')
                    {
                        
                       
                        validacion = true;

                    }
                    
                }
                if (validacion == false) {

                    throw new Exception("El correo necesita @");
                }
                AlumnoDAO.insertarAlumno(alumno);
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }
    }
}
