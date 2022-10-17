using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Datos
{
    public class Validacion
    {
        public static string ObtenerContraseniaBlowFish(string contrasenia)
        {

            string respuestaEncryptada = "";
            try
            {
                string query = "select nomina.dbo.fEncryptBlowfish('"+contrasenia+"')";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(ObtenerConexionesDB.obtenerCadenaConexionProductiva()))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        respuestaEncryptada = reader[0].ToString().Trim();
                    }
                }


            }
            catch (Exception E)
            {
                respuestaEncryptada = "";
                string a = E.Message;
                //var transaccion = new Transaccion();
                //var repositorio = new Repositorio<LOG_EXCEPCIONES>(transaccion);
                //LOG_EXCEPCIONES NuevaExcepcion = new LOG_EXCEPCIONES();

                //NuevaExcepcion.Clase = "FoliarConsultasDBSinEntity";
                //NuevaExcepcion.Metodo = "ObtenerDatosIdNomina";
                //NuevaExcepcion.Usuario = null;
                //NuevaExcepcion.Excepcion = E.Message;
                //NuevaExcepcion.Comentario = "No se han podido leer los datos correctamente o el archivo no existe para crear la revicion del pdf de cada nomina";
                //NuevaExcepcion.Fecha = DateTime.Now;

                //repositorio.Agregar(NuevaExcepcion);

                ////agregar un dato si hay un error para que el usuario se entere que hubo un error y avise al administrador del sistema
                //DatosReporteRevisionNominaDTO NuevoErrorDatoReporte = new DatosReporteRevisionNominaDTO();

                //NuevoErrorDatoReporte.Id = 1;
                //NuevoErrorDatoReporte.Partida = "";
                //NuevoErrorDatoReporte.Nombre = "Verifique que la nomina que desea";
                //NuevoErrorDatoReporte.Deleg = "";
                //NuevoErrorDatoReporte.Num_Che = "foliar";
                //NuevoErrorDatoReporte.Liquido = "exista";
                //NuevoErrorDatoReporte.CuentaBancaria = "";

                //ListaDatosReporteFoliacionPorNomina.Add(NuevoErrorDatoReporte);

                //return ListaDatosReporteFoliacionPorNomina;
            }
            return respuestaEncryptada;
        }


        public static string ObtenerContraseniaHasheada(string contraseniaBlowFish)
        {
            string respuestaEncryptada = "";
            try
            {
                string query = "select nomina.dbo.fGetHash('"+contraseniaBlowFish+"')";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(ObtenerConexionesDB.obtenerCadenaConexionProductiva()))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        respuestaEncryptada = reader[0].ToString().Trim();
                    }
                }


            }
            catch (Exception E)
            {
                respuestaEncryptada = "";
                string a = E.Message;
                //var transaccion = new Transaccion();
                //var repositorio = new Repositorio<LOG_EXCEPCIONES>(transaccion);
                //LOG_EXCEPCIONES NuevaExcepcion = new LOG_EXCEPCIONES();

                //NuevaExcepcion.Clase = "FoliarConsultasDBSinEntity";
                //NuevaExcepcion.Metodo = "ObtenerDatosIdNomina";
                //NuevaExcepcion.Usuario = null;
                //NuevaExcepcion.Excepcion = E.Message;
                //NuevaExcepcion.Comentario = "No se han podido leer los datos correctamente o el archivo no existe para crear la revicion del pdf de cada nomina";
                //NuevaExcepcion.Fecha = DateTime.Now;

                //repositorio.Agregar(NuevaExcepcion);

                ////agregar un dato si hay un error para que el usuario se entere que hubo un error y avise al administrador del sistema
                //DatosReporteRevisionNominaDTO NuevoErrorDatoReporte = new DatosReporteRevisionNominaDTO();

                //NuevoErrorDatoReporte.Id = 1;
                //NuevoErrorDatoReporte.Partida = "";
                //NuevoErrorDatoReporte.Nombre = "Verifique que la nomina que desea";
                //NuevoErrorDatoReporte.Deleg = "";
                //NuevoErrorDatoReporte.Num_Che = "foliar";
                //NuevoErrorDatoReporte.Liquido = "exista";
                //NuevoErrorDatoReporte.CuentaBancaria = "";

                //ListaDatosReporteFoliacionPorNomina.Add(NuevoErrorDatoReporte);

                //return ListaDatosReporteFoliacionPorNomina;
            }
            return respuestaEncryptada;
        }


        public static bool AutenticarCredenciales(string Usuario , string contraseniaBlowfish)
        {
            int a = 0;
            bool banderaDeAutenticacion = false;
            try
            {
                string query = "EXEC nom_proc_ValidUser '"+Usuario+"','"+contraseniaBlowfish+"','SD'";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(ObtenerConexionesDB.obtenerCadenaConexionProductiva()))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        banderaDeAutenticacion = Convert.ToBoolean(  reader.GetInt32(0));
                    }
                }
            }
            catch (Exception E)
            {
                banderaDeAutenticacion = false;
                string w = E.Message;
                //var transaccion = new Transaccion();
                //var repositorio = new Repositorio<LOG_EXCEPCIONES>(transaccion);
                //LOG_EXCEPCIONES NuevaExcepcion = new LOG_EXCEPCIONES();

                //NuevaExcepcion.Clase = "FoliarConsultasDBSinEntity";
                //NuevaExcepcion.Metodo = "ObtenerDatosIdNomina";
                //NuevaExcepcion.Usuario = null;
                //NuevaExcepcion.Excepcion = E.Message;
                //NuevaExcepcion.Comentario = "No se han podido leer los datos correctamente o el archivo no existe para crear la revicion del pdf de cada nomina";
                //NuevaExcepcion.Fecha = DateTime.Now;

                //repositorio.Agregar(NuevaExcepcion);

                ////agregar un dato si hay un error para que el usuario se entere que hubo un error y avise al administrador del sistema
                //DatosReporteRevisionNominaDTO NuevoErrorDatoReporte = new DatosReporteRevisionNominaDTO();

                //NuevoErrorDatoReporte.Id = 1;
                //NuevoErrorDatoReporte.Partida = "";
                //NuevoErrorDatoReporte.Nombre = "Verifique que la nomina que desea";
                //NuevoErrorDatoReporte.Deleg = "";
                //NuevoErrorDatoReporte.Num_Che = "foliar";
                //NuevoErrorDatoReporte.Liquido = "exista";
                //NuevoErrorDatoReporte.CuentaBancaria = "";

                //ListaDatosReporteFoliacionPorNomina.Add(NuevoErrorDatoReporte);

                //return ListaDatosReporteFoliacionPorNomina;
            }
            
            return banderaDeAutenticacion ; 
        }



        public static bool AutenticarMac(string Mac , string Host)
        {

            bool banderaDeAutenticacion = false;
            try
            {
                string query = "SELECT COUNT(*) FROM nomina.dbo.nom_cat_MacAddress where Mac = '" + Mac+"' and NombrePC = '"+Host+"' and Status = 1";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(ObtenerConexionesDB.obtenerCadenaConexionProductiva()))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        banderaDeAutenticacion = Convert.ToBoolean(reader.GetInt32(0));
                    }
                }
            }
            catch (Exception E)
            {
                banderaDeAutenticacion = false;
                string a = E.Message;
                //var transaccion = new Transaccion();
                //var repositorio = new Repositorio<LOG_EXCEPCIONES>(transaccion);
                //LOG_EXCEPCIONES NuevaExcepcion = new LOG_EXCEPCIONES();

                //NuevaExcepcion.Clase = "FoliarConsultasDBSinEntity";
                //NuevaExcepcion.Metodo = "ObtenerDatosIdNomina";
                //NuevaExcepcion.Usuario = null;
                //NuevaExcepcion.Excepcion = E.Message;
                //NuevaExcepcion.Comentario = "No se han podido leer los datos correctamente o el archivo no existe para crear la revicion del pdf de cada nomina";
                //NuevaExcepcion.Fecha = DateTime.Now;

                //repositorio.Agregar(NuevaExcepcion);

                ////agregar un dato si hay un error para que el usuario se entere que hubo un error y avise al administrador del sistema
                //DatosReporteRevisionNominaDTO NuevoErrorDatoReporte = new DatosReporteRevisionNominaDTO();

                //NuevoErrorDatoReporte.Id = 1;
                //NuevoErrorDatoReporte.Partida = "";
                //NuevoErrorDatoReporte.Nombre = "Verifique que la nomina que desea";
                //NuevoErrorDatoReporte.Deleg = "";
                //NuevoErrorDatoReporte.Num_Che = "foliar";
                //NuevoErrorDatoReporte.Liquido = "exista";
                //NuevoErrorDatoReporte.CuentaBancaria = "";

                //ListaDatosReporteFoliacionPorNomina.Add(NuevoErrorDatoReporte);

                //return ListaDatosReporteFoliacionPorNomina;
            }
            return banderaDeAutenticacion;
        }



    }
}
