using System;
using System.Collections.Generic;
using System.Text;
using System.Data; // manejo de datos
using System.Data.OleDb;
using System.Xml;
using Npgsql;//Para controlar la conexion a base de datos con postgresql
using NpgsqlTypes;
using System.Windows.Forms;

namespace CONEXION
{
    public class AccesoDatosPostgre
    {
        private Boolean isError = false;        //Una bandera, para determinar si existe o no algun error
        private String errorDescripcion;        //Almacena la descripcion del error        

        public NpgsqlConnection conexion;       //Objeto de tipo conexion, para establecer comunicacion con la BD
        private NpgsqlTransaction transaccion;  //Objeto de tipo transaccion de base de datos, para iniciar, procesar y cerrar transacciones
        private bool hayTransaccion;            //Bandera que determina si hay una transaccion activa
        private string schema;                  //Almacena el esquema con el cual se trabaja en la base de datos, para devolverlo mediante un metodo get
        string tipoconexion = "2";

        static int instancias = 0;              //Contador de cuanta veces se ha instanciado la clase, para evitar que se instancie mas de una vez
        private static AccesoDatosPostgre instance;

        public static AccesoDatosPostgre Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccesoDatosPostgre();
                }
                return instance;
            }
        }

        public DataSet cargarIni()
        {
            DataSet dsetConf = new DataSet();
            try
            {
                string ArchivoXML = Application.StartupPath + "\\INI.xml";
                System.IO.FileStream fsReadXml = new System.IO.FileStream(ArchivoXML, System.IO.FileMode.Open);
                dsetConf.ReadXml(fsReadXml);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error cargando el archivo de configuración, " +
                                "detalle técnico: " + e.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            return dsetConf;
        }


        // Constructor
        private AccesoDatosPostgre()
        {
            limpiarEstado();

            if (instancias > 1)
                return;

            //DataSet parametros = this.cargarIni();
            //DataTable tabla = parametros.Tables[0];
            //DataRow fila = tabla.Rows[0];

            DataRow fila = this.cargarIni().Tables[0].Rows[0];

            conexion = new NpgsqlConnection("Encoding = UNICODE; Server=" + fila["Server"].ToString() +
                                            ";Port = " + fila["Port"].ToString() +
                                            ";User Id=" + fila["Usuario"].ToString() +
                                            ";Password=" + fila["Password"].ToString() +
                                            ";Database=" + fila["Database"].ToString() +
                                            ";CommandTimeout=3600;");
            instancias += 1;

            try
            {
                conexion.Open();
                this.schema = fila["Schema"].ToString();
            }
            catch (NpgsqlException error)
            {
                instancias = 0;
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

        }

        // Indica el estado de la persistencia
        public Boolean estado()
        {
            limpiarEstado();

            String mensaje = "";

            // estado dela conexión
            switch (conexion.State)
            {
                case System.Data.ConnectionState.Broken:
                    mensaje = "La conexión con la base de datos fue interrumpida.";
                    break;
                case System.Data.ConnectionState.Closed:
                    mensaje = "La conexión con la base de datos fue cerrada o no pudo ser establecida.";
                    break;
                case System.Data.ConnectionState.Connecting:
                    mensaje = "Conectandose.";
                    break;
                case System.Data.ConnectionState.Executing:
                    mensaje = "Ejecutando.";
                    break;
                case System.Data.ConnectionState.Fetching:
                    mensaje = "Extrayendo.";
                    break;
                case System.Data.ConnectionState.Open:
                    mensaje = "Abierta.";
                    break;
            }

            // cargar la propiedad con el estado de la conexion
            ErrorDescripcion = mensaje;

            if ((conexion.State == ConnectionState.Open) ||
                  (conexion.State == ConnectionState.Executing) ||
                  (conexion.State == ConnectionState.Fetching))
                return true;
            else
                return false;
        }

        // destructor
        ~AccesoDatosPostgre()
        {
        }

        public void conectar()
        {
            try
            {
                if (!(conexion.State == ConnectionState.Open))
                {
                    conexion.Open();
                    instancias = 1;
                }
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.ErrorDescripcion = error.Message;
            }
        }

        public void desconectar()
        {
            try
            {
                conexion.Close();
                instancias = 0;
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.ErrorDescripcion = error.Message;
            }
        }

        //Manipulacion de select
        public DataSet ejecutarConsultaSQL(String pSql)
        {
            limpiarEstado();

            NpgsqlDataAdapter oDataAdapter = new NpgsqlDataAdapter(pSql, conexion);
            DataSet oDataSet = new DataSet();

            // capturar la excepción
            try
            {
                oDataAdapter.Fill(oDataSet);
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

            return oDataSet;
        }

        public DataSet ejecutarConsultaSQL(String pSql, String pnTabla, Object[] myParamArray)
        {
            limpiarEstado();

            NpgsqlCommand cmd = new NpgsqlCommand(pSql, conexion);

            cmd.CommandType = CommandType.Text;

            for (int j = 0; j < myParamArray.Length; j++)
            {
                cmd.Parameters.Add((NpgsqlParameter)myParamArray[j]);
            }


            NpgsqlDataAdapter oDataAdapter = new NpgsqlDataAdapter(cmd);
            DataSet oDataSet = new DataSet();

            // capturar la excepción
            try
            {
                oDataAdapter.Fill(oDataSet, pnTabla);
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

            return oDataSet;
        }

        public DataSet ejecutarConsultaSQL(String pSql, String pnTabla)
        {
            limpiarEstado();


            NpgsqlDataAdapter oDataAdapter = new NpgsqlDataAdapter(pSql, conexion);
            DataSet oDataSet = new DataSet();

            // capturar la excepción
            try
            {
                oDataAdapter.Fill(oDataSet, pnTabla);
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

            return oDataSet;
        }

        // Método para manipular Insert, Update, Delete
        public void ejecutarSQL(String pSql)
        {
            limpiarEstado();

            // Definicion de Command
            NpgsqlCommand cmd = null;

            try
            {
                cmd = new NpgsqlCommand(pSql, conexion);

                if (this.hayTransaccion)
                {
                    cmd.Transaction = this.transaccion;
                }

                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

        }

        // Método para manipular Insert, Update, Delete con identidad
        public void ejecutarSQL(string pSql, Object[] myParamArray, ref string pNumero)
        {
            limpiarEstado();

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(pSql, conexion);
                cmd.CommandType = CommandType.Text;
                for (int j = 0; j < myParamArray.Length; j++)
                {
                    cmd.Parameters.Add((NpgsqlParameter)myParamArray[j]);
                }

                if (this.hayTransaccion)
                {
                    cmd.Transaction = this.transaccion;
                }

                //cmd.ExecuteNonQuery();
                try
                {
                    pNumero = "";
                    pNumero = cmd.ExecuteScalar().ToString();
                }
                catch { }
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }
        }

        //Método para manipular Insert, Update pero con parametros
        public void ejecutarSQL(string sql, Object[] myParamArray)
        {
            limpiarEstado();

            try
            {

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conexion);
                cmd.CommandType = CommandType.Text;
                for (int j = 0; j < myParamArray.Length; j++)
                {
                    cmd.Parameters.Add((NpgsqlParameter)myParamArray[j]);
                }

                if (this.hayTransaccion)
                {
                    cmd.Transaction = this.transaccion;
                }


                cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException error)
            {
                this.IsError = true;
                this.errorDescripcion = error.Message;
            }

        }

        //Metodos de transaccion
        public void iniciarTransaccion()
        {
            if (this.hayTransaccion == false)
            {
                this.transaccion = this.conexion.BeginTransaction();
                this.hayTransaccion = true;
            }
        }

        public void commitTransaccion()
        {
            if (this.hayTransaccion)
            {
                this.transaccion.Commit();
                this.hayTransaccion = false;
            }
        }

        public void rollbackTransaccion()
        {
            if (this.hayTransaccion)
            {
                this.transaccion.Rollback();
                this.hayTransaccion = false;
            }
        }

        // Metodos de Set & Get
        public Boolean IsError
        {
            set { isError = value; }
            get { return isError; }
        }

        public String ErrorDescripcion
        {
            set { errorDescripcion = value; }
            get { return errorDescripcion; }
        }

        public string Schema
        {
            get { return this.schema; }
        }

        public string TipoConexion
        {
            get { return this.tipoconexion; }
        }

        //Elimina el estado de error de la clase.
        public void limpiarEstado()
        {
            this.errorDescripcion = "";
            this.isError = false;
        }
    }//Finaliza la clase
}//Finaliza el namespace

