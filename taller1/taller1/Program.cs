using CONEXION;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taller1
{
    static class Program
    {
        public static AccesoDatosPostgre cnx;
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            cnx = AccesoDatosPostgre.Instance;
            if (cnx.IsError) {
                MessageBox.Show("Error conectando ala base de datos:" + cnx.ErrorDescripcion,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            else {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
           
        }
    }
}
