using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ENTIADES;
using BO;

namespace taller1
{
    public partial class Form1 : Form
    {
        private alumno Alumno;
        private alumnoBO AlumnoBO;
        public Form1()
        {
            InitializeComponent();
            Alumno = new alumno();
            AlumnoBO = new alumnoBO();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Alumno.Nombre = txtnombre.Text;
                Alumno.Apellido = txtApellido.Text;
                Alumno.Direccion = txtDireccion.Text;
                Alumno.Correo = txtCorreo.Text;
                if (AlumnoBO.validacion.Equals(true))
                {
                  lblmesajecorreo.Text="correo con formato correcto";
                }
                Alumno.Telefono = Int32.Parse(txtTelefono.Text);
                AlumnoBO.verificarAlumno(Alumno);
            }
            catch (Exception exception)
            {
                string error = exception.Message;
                MessageBox.Show(error);
            }
           

        }
    }
}
