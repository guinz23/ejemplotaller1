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
                if (AlumnoBO.validacion.Equals(true))
                {
                    lblmesajecorreo.Text = "correo con formato correcto";
                }
                Alumno.Nombre = txtnombre.Text;
                Alumno.Apellido = txtApellido.Text;
                Alumno.Direccion = txtDireccion.Text;
                Alumno.Correo = txtCorreo.Text;
                Alumno.Telefono = Int32.Parse(txtTelefono.Text);
                AlumnoBO.verificarAlumno(Alumno);
                MessageBox.Show("Usuario agregado correctamente");

            }
            catch (Exception exception)
            {
                string error = exception.Message;
                MessageBox.Show(error);
            }
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            cargar();
        }
        public void cargar(){
            try
            {
              
                List<alumno> Alumnos = AlumnoBO.CargarAlumnos();
                this.dataAlumnos.DataSource = Alumno.ToString();
              }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

      }
      
    }
}
