using RH.Login.Datos;
using RH.Login.Datos.DBNomina;
using RH.Login.Entidades.DTO.UsuarioRootDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RH.Login.Entidades;
using RH.Login.Entidades.DTO.ServiciosAPIDTO;

namespace RH.Login.Negocios
{
    public class ResetPasswordNegocio
    {
        public static string AdminReseteaContrasenia(int numEmpleado)
        {
            string nuevoPassword = "alpha";
            string numEmpleado5Digitos = empleado5Digitos(numEmpleado);
            if (numEmpleado5Digitos.Length != 5)
                return "El numero de empleado no concuerda con el criterio de busqueda, corrija e intente de nuevo";

            UsuarioDesdeNumEmpleadoDTO usuarioObtenido = InformacionUsuarios.ObtenerUsuarioDesdeNumEmpleado(numEmpleado5Digitos);
       
            if (usuarioObtenido.Id != 0)
            {
                nuevoPassword = nuevoPassword + new Random().Next(2022, 9999);

                //El usuario si existe asi que se debe cambiar su contraseña
                string blowFish = Validacion.ObtenerContraseniaBlowFish(nuevoPassword);
                string hashContrasenia = Validacion.ObtenerContraseniaHasheada(blowFish);
                int filaActualizada = InformacionUsuarios.CambiaPassword(usuarioObtenido.Id, hashContrasenia, true , "Admin_ResetWEB");

            }
            return nuevoPassword;
        }



        public static bool cambiarPassworsUsuarioNuevo(int IdUsuario, string nuevaContrasenia)
        {
            bool esUsuarioNuevo = Datos.DBNomina.InformacionUsuarios.EsUsuarioNuevo(IdUsuario);
            int filaActualizada = 0;
            if (esUsuarioNuevo)
            {
                //El usuario si existe asi que se debe cambiar su contraseña
                string blowFish = Validacion.ObtenerContraseniaBlowFish(nuevaContrasenia);
                string hashContrasenia = Validacion.ObtenerContraseniaHasheada(blowFish);
                filaActualizada = InformacionUsuarios.CambiaPassword(IdUsuario, hashContrasenia, false , "NewUserChangeWEB");
            }
            return filaActualizada == 1 ? true : false;
        }


        public static bool cambiarPassworsSolicitudAprobada(int idRegitroTablaUsers, string nuevaContrasenia , string nombreUsuario)
        {
            bool existeUsuario = InformacionUsuarios.ExisteUsuarioSolicitudAprobadaCambioPassword(idRegitroTablaUsers, nombreUsuario);
            int filaActualizada = 0;
            if (existeUsuario)
            {
                //El usuario si existe asi que se debe cambiar su contraseña
                string blowFish = Validacion.ObtenerContraseniaBlowFish(nuevaContrasenia);
                string hashContrasenia = Validacion.ObtenerContraseniaHasheada(blowFish);
                filaActualizada = InformacionUsuarios.CambiaPassword(idRegitroTablaUsers, hashContrasenia, false  , "WebChangeRequest");
            }
            return filaActualizada == 1 ? true : false;
        }


        //Como limite al dia solo puede solicitar 6 cambios de contraseña desde el login de la web
        public static bool puedeCrearSolicitud( int idUsuarioAlpha)
        {
            /*el limite de solicitudes por dia esta topado a 6 solicitudes por dia*/
            int LimiteDiario = 6;
            Transaccion transaccion = new Transaccion();
            var repositorioSolicitudes = new Repositorio<Entidades.tbl_SolicitudesCambioPassword>(transaccion);

            DateTime fechaActual = DateTime.Now;
            int solicitudesHechasMismoDia = repositorioSolicitudes.ObtenerPorFiltro(x => x.IdUsuarioALPHA == idUsuarioAlpha && x.FechaSolicitud.Year == fechaActual.Year && x.FechaSolicitud.Month == fechaActual.Month && x.FechaSolicitud.Day == fechaActual.Day).Count();

            return solicitudesHechasMismoDia < LimiteDiario;
        }


        public static (bool, int, int) timer(DateTime fechaSolitudCreada) 
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaExpiracion = fechaSolitudCreada.AddMinutes(10 /*5*/);

            int minutos = (int)fechaExpiracion.Subtract(fechaActual).Minutes;
            int segundos = (int)fechaExpiracion.Subtract(fechaActual).Seconds;

            //int tiempoEsperaSegundos = (int) fechaExpiracion.Subtract(fechaActual).TotalSeconds;

            minutos = minutos < 0 ? 0 : minutos;
            segundos = segundos < 0 ? 0 : segundos;

            //tiempoEsperaSegundos = tiempoEsperaSegundos < 0 ? 0 : tiempoEsperaSegundos;
            bool bandera = fechaActual < fechaExpiracion;
            return (bandera, minutos , segundos);
        }


        public static tbl_SolicitudesCambioPassword ObtenerRegistroDelDia(int idUsuarioAlpha) 
        {
            Transaccion transaccion = new Transaccion();
            var repositorioSolicitudes = new Repositorio<Entidades.tbl_SolicitudesCambioPassword>(transaccion);

            DateTime fechaActual = DateTime.Now;
            tbl_SolicitudesCambioPassword ultimaSolicitud = repositorioSolicitudes.ObtenerPorFiltro(x => x.IdUsuarioALPHA == idUsuarioAlpha && x.FechaSolicitud.Year == fechaActual.Year && x.FechaSolicitud.Month == fechaActual.Month && x.FechaSolicitud.Day == fechaActual.Day && x.Activo == true).FirstOrDefault();


            return ultimaSolicitud;
        }

        public static async Task<string> solicitarCambioContraseniaCorreoElectronico(RH.Login.Entidades.DTO.InicioDTO.SolicitudCambioPasswordDTO nuevasolicitudDTO  /* int numEmpleado, string nombreUsuario*/)
        {
            string mensaje;
            string numEmpleado5Digitos = empleado5Digitos(nuevasolicitudDTO.numEmpleado);
            //bool existeUsuarioCambiaPass =  Datos.DBNomina.InformacionUsuarios.ExisteUsuarioParaReset(numEmpleado5Digitos, nombreUsuario);
            bool existeUsuarioCambiaPass = ExisteUsuarioParaReset( numEmpleado5Digitos, nuevasolicitudDTO.nombreUsuario);

            if (existeUsuarioCambiaPass)
            {
                Entidades.DTO.UsuarioInfDTO usuarioEncontrado = InformacionUsuarios.ObtenerInformacionUsuario(numEmpleado5Digitos);
                UsuarioDesdeNumEmpleadoDTO usuarioObtenido = InformacionUsuarios.ObtenerUsuarioDesdeNumEmpleado(numEmpleado5Digitos);


             
                /*puedoCrearNuevaSolicitud*/
                bool limiteDiarioAlcanzado = puedeCrearSolicitud(usuarioObtenido.Id);
                if (limiteDiarioAlcanzado) 
                {
                    string EmailDestino = Datos.DBNomina.InformacionUsuarios.ObtenerCorreo(numEmpleado5Digitos);
                    if (EmailDestino != null)
                    {
                        tbl_SolicitudesCambioPassword registroinicial = ObtenerRegistroDelDia(usuarioObtenido.Id);
                        if (registroinicial == null)
                        {
                            int folioPrimeraSolicitud = await CrearSolicitudCambioContrasenia(usuarioObtenido.Id /* IdUsuarioEnAlpha*/, nuevasolicitudDTO.hostNameCliente, nuevasolicitudDTO.direccionMac, nuevasolicitudDTO.ipCliente);

                            if (folioPrimeraSolicitud != 0)
                            {
                                EmpleadoInfoRecuperaPassBodyDTO nuevoBody = CrearInformacioRecuperacionPasswordDTO(usuarioEncontrado.NombreEmpleado, Convert.ToString(folioPrimeraSolicitud), usuarioObtenido.NombreUsuario, numEmpleado5Digitos);
                                string token = await ServiciosAPINegocio.ObtenerTokenRecuperacionContrasenia(nuevoBody);
                                return await EnviarCorreo(token, usuarioEncontrado.NombreEmpleado, EmailDestino);
                            }
                            else
                            {
                                mensaje = "ERROR || No se pudo generar la solicitud del cambio de contraseña";
                            }

                        }
                        else
                        {
                            // si es verdadero mostrar timer
                            var (bandera, minutosEspera, segundosEspera) = timer(registroinicial.FechaSolicitud);


                            if (!bandera)
                            {

                                cancelarUltimaSolicitudActivaUsuario(usuarioObtenido.Id);
                                int folioSolicitudCreada = await CrearSolicitudCambioContrasenia(usuarioObtenido.Id /* IdUsuarioEnAlpha*/, nuevasolicitudDTO.hostNameCliente, nuevasolicitudDTO.direccionMac, nuevasolicitudDTO.ipCliente);

                                if (folioSolicitudCreada != 0)
                                {
                                    EmpleadoInfoRecuperaPassBodyDTO nuevoBody = CrearInformacioRecuperacionPasswordDTO(usuarioEncontrado.NombreEmpleado, Convert.ToString(folioSolicitudCreada), usuarioObtenido.NombreUsuario, numEmpleado5Digitos);
                                    string token = await ServiciosAPINegocio.ObtenerTokenRecuperacionContrasenia(nuevoBody);
                                    return await EnviarCorreo(token, usuarioEncontrado.NombreEmpleado, EmailDestino);


                                }
                                else
                                {
                                    mensaje = "ERROR || No se pudo generar la solicitud del cambio de contraseña";
                                }


                            }
                            else
                            {
                                mensaje = "TIMEERROR || "+minutosEspera+":"+segundosEspera;
                            }

                        }

                    }
                    else 
                    {
                        mensaje = "ERROR || El correo no existe";
                    }
                  
                }
                else 
                {
                    return "ERROR || Alcanzó el limite diario de solicitudes. SOLICITELO A UN ADMINISTRADOR";
                }

            }
            else 
            {
                mensaje = "ERROR || El numero de empleado y/o nombre de cuenta no corresponden";
            }

            return  mensaje;
        }


        public static EmpleadoInfoRecuperaPassBodyDTO CrearInformacioRecuperacionPasswordDTO( string nombreEmpleado , string folioSolicitudCreada , string nombreUsuario , string numEmpleado5Digitos) 
        {
            EmpleadoInfoRecuperaPassBodyDTO nuevoBody = new EmpleadoInfoRecuperaPassBodyDTO();
            nuevoBody.NombreEmpleado =nombreEmpleado;
            nuevoBody.FolioSolicitud =folioSolicitudCreada;
            nuevoBody.NombreUsuario = nombreUsuario;
            nuevoBody.NumeroEmpleado = numEmpleado5Digitos;
            return nuevoBody;
        }

        public static async Task<string> EnviarCorreo(string token , string NombreEmpleado , string EmailDestino) 
        {
            
            if (!string.IsNullOrEmpty(token))
            {
                EnvioCorreosNegocio nuevoEnvio = new EnvioCorreosNegocio();
                nuevoEnvio.EnviarCorreoWeb(NombreEmpleado, EmailDestino, token);
                return "Instrucciones enviadas, revise su correo electronico : " + EmailDestino;
            }
            else
            {
                return "ERROR || La firma de validacion fallo, Contacte a un desarrollador";
            }
        }

       
        public static bool ExisteUsuarioParaReset(string numEmpleado5Digitos , string nombreUsuario) 
        {
          return   Datos.DBNomina.InformacionUsuarios.ExisteUsuarioParaReset(numEmpleado5Digitos, nombreUsuario);
        }


        public static string empleado5Digitos (int number)
        {
            return String.Format("{0:D4}", number);
        }


        public static async Task<int> CrearSolicitudCambioContrasenia(int idUsuarioAlpha, string PC , string MAC ,  string ip) 
        {
            Transaccion transaccion = new Transaccion();
            var repositorioSolicitudes = new Repositorio<Entidades.tbl_SolicitudesCambioPassword>(transaccion);

            Entidades.tbl_SolicitudesCambioPassword nuevaSolicitud = new Entidades.tbl_SolicitudesCambioPassword();
            nuevaSolicitud.IdUsuarioALPHA = idUsuarioAlpha;
            nuevaSolicitud.SolicitudDesdePC = PC;
            nuevaSolicitud.SolicitudDesdeMAC = MAC;
            nuevaSolicitud.SolicitudDesdeIP = ip;
            nuevaSolicitud.FechaSolicitud = DateTime.Now;
            nuevaSolicitud.SeUsoFolio = false;
            nuevaSolicitud.Activo = true;

            var resultadoInsercion = await repositorioSolicitudes.AgregarAsincronamente(nuevaSolicitud);
            int folioSolicitudCreada = resultadoInsercion.Id;

            return folioSolicitudCreada > 0 ? folioSolicitudCreada : 0;            
        }


        public static void cancelarUltimaSolicitudActivaUsuario(int idUsuarioAlpha) 
        {
            Transaccion transaccion = new Transaccion();
            var repositorioSolicitudes = new Repositorio<Entidades.tbl_SolicitudesCambioPassword>(transaccion);

            var ultimaSolicitudActiva = repositorioSolicitudes.ObtenerPorFiltro(x => x.IdUsuarioALPHA == idUsuarioAlpha && x.Activo == true).OrderByDescending(x => x.Id).FirstOrDefault();

            if (ultimaSolicitudActiva != null ) 
            {
                ultimaSolicitudActiva.Activo = false;
                repositorioSolicitudes.Modificar(ultimaSolicitudActiva);
            }
        }



        public static bool VerificaFolioEstaActivoSinExpirar(int folioSolicitud) 
        {
            Transaccion transaccion = new Transaccion();
            var repoSolicitudesCambioPass = new Repositorio<tbl_SolicitudesCambioPassword>(transaccion);
            bool bandera = false;
            tbl_SolicitudesCambioPassword solicitudEncontrada = repoSolicitudesCambioPass.Obtener(x => x.Id == folioSolicitud);
            if (solicitudEncontrada.Activo) 
            {
                DateTime horaExpiracion = solicitudEncontrada.FechaSolicitud.AddMinutes(20);
                bandera = DateTime.Now <= horaExpiracion;
            }
            return bandera;
        }


        public static void InactivaFolioUsado(int folioSolicitud) 
        {
            Transaccion transaccion = new Transaccion();
            var repoSolicitudesCambioPass = new Repositorio<tbl_SolicitudesCambioPassword>(transaccion);
            tbl_SolicitudesCambioPassword inactivarSolicitud = repoSolicitudesCambioPass.Obtener(x => x.Id == folioSolicitud);
            inactivarSolicitud.SeUsoFolio = true;
            inactivarSolicitud.Activo = false;
            repoSolicitudesCambioPass.Modificar(inactivarSolicitud);
        }
    }
}
