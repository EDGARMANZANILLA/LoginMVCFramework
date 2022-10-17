using RH.Login.Entidades.DTO;
using RH.Login.Entidades.DTO.UsuarioRootDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace RH.Login.Datos.DBNomina
{
    public class InformacionUsuarios
    {
        static string cadenaDeConexion = ObtenerConexionesDB.obtenerCadenaConexionProductiva();

        static string cadenaDeConexion2 = ObtenerConexionesDB.obtenerCadenaConexionPrueba();


        public static UsuarioInfDTO ObtenerInformacionUsuario(string numEmpleado)
        {
            try
            {
                string query = "select nombre, DescripPto ,SUBSTRING(RU, 4,2) Ramo,  dPart  from nomina.dbo.vwResumenActivos where num = '"+numEmpleado+"' ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioInfDTO nuevoUsuarioInfDTO = new UsuarioInfDTO();
                        nuevoUsuarioInfDTO.NombreEmpleado = reader[0].ToString().Trim();
                        nuevoUsuarioInfDTO.Puesto = reader[1].ToString().Trim();
                        nuevoUsuarioInfDTO.Departamento = reader[2].ToString().Trim() +" - "+ reader[3].ToString().Trim();
                        return nuevoUsuarioInfDTO;
                    }
                }
            }
            catch (Exception E)
            {
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
            return new UsuarioInfDTO();
        }

        public static string ObtenerNumEmpleadoDesdeUsuario(string usuario)
        {
            try
            {
                string query = "select numEmpleado from nomina.dbo.nom_cat_users where username = '"+usuario+ "' and status = 1 ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return reader[0].ToString().Trim();
                    }
                }
            }
            catch (Exception E)
            {
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
            return "00000";
        }



        public static int ObtenerIdRegistroUsuarioAlpha(string usuario)
        {
            try
            {
                string query = "select id  from nomina.dbo.nom_cat_users where username = '"+usuario+"' and status = 1 ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception E)
            {
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
            return 00000;
        }


        public static int ObtenerIdUsuarioAlphaPorNumeroEmpleado(string numeroEmpleado)
        {
            try
            {
                string query = "select id  from nomina.dbo.nom_cat_users where numEmpleado = '"+numeroEmpleado+"' and status = 1 ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return reader.GetInt32(0);
                    }
                }
            }
            catch (Exception E)
            {
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
            return 00000;
        }


        public static bool EsUsuarioNuevo(int idRegitroTablaUsers) 
        {
            bool bandera = false;
            try
            {
                string query = "select usuarioNuevo from nomina.dbo.nom_cat_users where  status  = 1 and id = "+idRegitroTablaUsers+"";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                       bandera = Convert.ToBoolean(reader[0].ToString().Trim());
                    }
                }
            }
            catch (Exception E)
            {
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
            return bandera;
        }



        /****************************************************************************************************************************/
        /*********************************      Datos para UsuarioRoot      *********************************************************/
        /****************************************************************************************************************************/
        public static List<UsuarioNoRegistradoDTO> ObtenerUsuariosNoRegitradosEnSistemaWeb()
        {
            List<UsuarioNoRegistradoDTO> usuariosSinRegistro = new List<UsuarioNoRegistradoDTO>();
            try
            {
                //string query = "select id, numEmpleado , username from nomina.dbo.nom_cat_users where id not in (select IdUsuarioAlpha_nomina_nom_cat_user from LoginCentralizado.dbo.tbl_UsuariosWeb) and status = 1 ";
                string query = "select  users.numEmpleado , users.username ,  activo.nomb  from nomina.dbo.nom_cat_users users inner join nomina.dbo.vwResumenActivosN as activo on users.numEmpleado  = activo.num where users.id not in ( select IdUsuarioAlpha_nomina_nom_cat_user from LoginCentralizado.dbo.tbl_UsuariosWeb ) and users.status = 1 order by numEmpleado";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioNoRegistradoDTO nuevoUsuarioNoRegistrado = new UsuarioNoRegistradoDTO();
                        nuevoUsuarioNoRegistrado.Num = reader[0].ToString().Trim();  
                        nuevoUsuarioNoRegistrado.MostrarTexto = reader[1].ToString().Trim() + " || "+reader[2].ToString().Trim(); ;
                        usuariosSinRegistro.Add(nuevoUsuarioNoRegistrado);
                    }
                }
            }
            catch (Exception E)
            {
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
            return usuariosSinRegistro;
        }


        public static List<UsuarioNoRegistradoDTO> ObtenerUsuariosRegitradosLoginCentralizado()
        {
            List<UsuarioNoRegistradoDTO> usuariosSinRegistro = new List<UsuarioNoRegistradoDTO>();
            try
            {
                //string query = "select id, numEmpleado , username from nomina.dbo.nom_cat_users where id not in (select IdUsuarioAlpha_nomina_nom_cat_user from LoginCentralizado.dbo.tbl_UsuariosWeb) and status = 1 ";
                string query = "select  users.numEmpleado , users.username ,  activo.nomb  from nomina.dbo.nom_cat_users users inner join nomina.dbo.vwResumenActivosN as activo on users.numEmpleado  = activo.num where activo.num in ( select numEmpleado from nomina.dbo.nom_cat_users where id in (SELECT IdUsuarioAlpha_nomina_nom_cat_user FROM [LoginCentralizado].[dbo].[tbl_UsuariosWeb]) and status = 1 ) ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UsuarioNoRegistradoDTO nuevoUsuarioNoRegistrado = new UsuarioNoRegistradoDTO();
                        nuevoUsuarioNoRegistrado.Num = reader[0].ToString().Trim();
                        nuevoUsuarioNoRegistrado.MostrarTexto = reader[1].ToString().Trim() + " || " + reader[2].ToString().Trim(); ;
                        usuariosSinRegistro.Add(nuevoUsuarioNoRegistrado);
                    }
                }
            }
            catch (Exception E)
            {
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
            return usuariosSinRegistro;
        }

        public static string nombreUsuarioCuentaAlpha( int idCuentaLoginCentralizado) 
        {
            try
            {
                string query = "select username from nomina.dbo.nom_cat_users where id  = "+idCuentaLoginCentralizado+" and status  = 1";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        return reader[0].ToString().Trim();
                    }
                }
            }
            catch (Exception E)
            {
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
            return "00000";
        }


        public static List<UniversoUsuariosAlphaDTO> ObtenerUniversoUsuariosAlpha()
        {
            List<UniversoUsuariosAlphaDTO> usuariosSinRegistro = new List<UniversoUsuariosAlphaDTO>();
            try
            {
                //string query = "select id, numEmpleado , username from nomina.dbo.nom_cat_users where id not in (select IdUsuarioAlpha_nomina_nom_cat_user from LoginCentralizado.dbo.tbl_UsuariosWeb) and status = 1 ";
                string query = " select  users.numEmpleado , users.username , activo.nomb from nomina.dbo.nom_cat_users users inner join nomina.dbo.vwResumenActivosN as activo on users.numEmpleado = activo.num   where status = 1 ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        UniversoUsuariosAlphaDTO nuevoUsuarioNoRegistrado = new UniversoUsuariosAlphaDTO();
                        nuevoUsuarioNoRegistrado.Num = reader[0].ToString().Trim();
                        nuevoUsuarioNoRegistrado.MostrarTexto = reader[1].ToString().Trim() + " || " + reader[2].ToString().Trim(); ;
                        usuariosSinRegistro.Add(nuevoUsuarioNoRegistrado);
                    }
                }
            }
            catch (Exception E)
            {
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
            return usuariosSinRegistro;
        }




        /*****************************************************************************************************************************************/
        /*********************************     APLICAR RESET A CONTRASENIA EN ALPHA      *********************************************************/
        /*****************************************************************************************************************************************/
        public static UsuarioDesdeNumEmpleadoDTO ObtenerUsuarioDesdeNumEmpleado(string numEmpleado)
        {
            UsuarioDesdeNumEmpleadoDTO usuarioEncontrado = new UsuarioDesdeNumEmpleadoDTO();
            try
            {
                string query = "select id, username from nomina.dbo.nom_cat_users where numEmpleado = '"+numEmpleado+"' and status  = 1";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        usuarioEncontrado.Id = reader.GetInt32(0);
                        usuarioEncontrado.NombreUsuario = reader[1].ToString().Trim();
                        
                    }
                }
            }
            catch (Exception E)
            {
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
            return usuarioEncontrado;
        }

        public static int CambiaPassword(int idUsuario, string nuevaContraseniaaEncriptada, bool EsUsuarioNuevo , string tipoInsercion)
        {

            int usuarioNuevo = EsUsuarioNuevo ? 1 : 0;
            int filasAfectadas = 0;
            //string tipoInsercion = EsUsuarioNuevo ? "Admin_ReseteoWEB" :"Usuario_CambioWEB";
            string fechaActualizacion = DateTime.Now.ToShortDateString();
            string fechaFinalizacion = DateTime.Now.AddYears(1).ToShortDateString();
            try
            {
                string query = "update nomina.dbo.nom_cat_users set pass= '"+tipoInsercion+"' , updateDate = '"+fechaActualizacion+"' , fechaExpira = '"+fechaFinalizacion+"' ,  passSHA1 = '"+nuevaContraseniaaEncriptada+"', usuarioNuevo = "+usuarioNuevo+" where id = "+idUsuario+" and status = 1";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    filasAfectadas = command.ExecuteNonQuery();
                }
            }
            catch (Exception E)
            {
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
            return filasAfectadas;
        }

        

        public static string ObtenerCorreo(string numEmpleado )
        {
            string correo = "";
            try
            {
                string query = "SELECT nomina.dbo.fGetCorreo('"+numEmpleado+"')";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        correo = reader[0].ToString().Trim();
                    }
                }


            }
            catch (Exception E)
            {
                correo = "";
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
            return correo;
        }

        public static Boolean ExisteUsuarioParaReset(string numEmpleado ,  string nombreUsuario)
        {
            bool bandera = false; 
            try
            {
                string query = "select COUNT(*) from nomina.dbo.nom_cat_users where numEmpleado = '"+numEmpleado+"' and  username = '"+nombreUsuario+"' and status  = 1";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string bit =  reader[0].ToString().Trim();
                        bandera = bit == "0" ? false : true;
                    }
                }
            }
            catch (Exception E)
            {
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
            return bandera;
        }

        public static bool ExisteUsuarioSolicitudAprobadaCambioPassword(int idRegitroTablaUsers, string  nombreUsuario)
        {
            bool bandera = false;
            try
            {
                string query = "select username from nomina.dbo.nom_cat_users where  status  = 1 and id = " + idRegitroTablaUsers+ " and username = '"+nombreUsuario+"'  ";
                using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(cadenaDeConexion))
                {
                    connection.Open();
                    System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand(query, connection);
                    System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string nombreUsuarioEncontrado = reader[0].ToString().Trim();

                        if (nombreUsuarioEncontrado.Trim().ToUpper().Equals(nombreUsuario.Trim().ToUpper())) 
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                string a = E.Message;
            
            }
            return bandera;
        }


    }
}
