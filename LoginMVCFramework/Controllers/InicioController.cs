using AutoMapper;
using LoginMVCFramework.Models;
using RH.Login.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginMVCFramework.Filtros;
using System.Threading.Tasks;
using System.Web.Security;
using RH.Login.Negocios;
using System.IO;
using LoginMVCFramework.Controllers;

namespace LoginMVCFramework.Controllers
{

    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Login()
        {
            return View();
        }

        //Metodo que se ejecuta cuando el usuario da click desde su correo electronico para validar que tenga el token correctamente, donde se validan datos para recuperar su contraseña
        public ActionResult ValidarSolicitudCambioPassword(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                ServiciosAPINegocio service = new ServiciosAPINegocio();

                if (service.esTokenValido(token))
                {
                    if (service.GetTokenExpirationTime(token))
                    {

                        var datosDesencriptados = service.ObtenerTokenDesencriptado(token);

                        if (ResetPasswordNegocio.VerificaFolioEstaActivoSinExpirar(Convert.ToInt32( datosDesencriptados.FolioSolicitud ))) 
                        {
                            bool existeCuenta = ResetPasswordNegocio.ExisteUsuarioParaReset(datosDesencriptados.NumeroEmpleado, datosDesencriptados.NombreUsuario);

                            if (existeCuenta)
                            {
                                int idCuentaUsuarioCambiaPass = AutentificaciónNegocio.ObtenerIdRegistroEnAlphaDesdeUsuario(datosDesencriptados.NombreUsuario);
                                if (idCuentaUsuarioCambiaPass != 0)
                                {
                                    ViewBag.NombreCompleto = datosDesencriptados.NombreEmpleado;
                                    ViewBag.NombreUsuario = datosDesencriptados.NombreUsuario;
                                    ViewBag.folioSolicitud = datosDesencriptados.FolioSolicitud;
                                    ViewBag.idCuentaUsuarioCambiaPass = idCuentaUsuarioCambiaPass;

                                    return View();
                                }
                            }
                        }
                    }
                }

            }
           return RedirectToAction("ErrorEnVistaSolicitada", "Errores");
        }


        [HttpPost]
        public async Task<ActionResult> InicioDeSesion(AutenticacionModel Autenticacion  /* string User , string Password , string DireccionMac  ,  string HostNameCliente */ )
        {
            //int codigoResultado = 403; //403 - Prohibido 
            int codigoResultado = 401; //401-  No autorizado 
            string mensajeCliente = "";
            string JWT = "";
            if (ModelState.IsValid)
            {
                /*  status - Cadena  
                *      0  - La Autenticacion se cumplio con exito en todos los criterios
                *      1  - La autenticacion del usuario o contraseña es incorrecta 
                *      2  - La autenticacion de la mac y el host no se encuentan registrados 
                *      3 - El usuario no esta autorizado para usuar ALPHABET 
                */

              
                AutenticacionDTO autenticarDatos = Mapper.Map<AutenticacionDTO>(Autenticacion);
                int IdUsuarioEnAlpha = RH.Login.Negocios.AutentificaciónNegocio.ObtenerIdRegistroEnAlphaDesdeUsuario(autenticarDatos.User);
                int estatusAutenticacion = RH.Login.Negocios.AutentificaciónNegocio.AutenticarUsuario(autenticarDatos,   IdUsuarioEnAlpha);


                UsuarioInfDTO infoUsuarioRecuperada = new UsuarioInfDTO();
                switch (estatusAutenticacion)
                {
                    case 0:
                        string numEmpleado = RH.Login.Negocios.AutentificaciónNegocio.ObtenerNumEmpleadoDesdeUsuario(autenticarDatos.User);
                        infoUsuarioRecuperada = RH.Login.Negocios.AutentificaciónNegocio.ObtenerInfoUsuario(numEmpleado);

                        infoUsuarioRecuperada.IdUsuarioEnAlpha = IdUsuarioEnAlpha;
                        infoUsuarioRecuperada.IdUsuarioEnLoginCentralizado = RH.Login.Negocios.DBLoginCentralizadoNegocio.ObtenerIdUsuarioEnLoginCentralizado(infoUsuarioRecuperada.IdUsuarioEnAlpha);
                        infoUsuarioRecuperada.EsUsuarioRoot = false;
                        infoUsuarioRecuperada.Roles = DBLoginCentralizadoNegocio.ObtenerRolesUsuarioWeb(infoUsuarioRecuperada.IdUsuarioEnLoginCentralizado);
                        infoUsuarioRecuperada.JWT = infoUsuarioRecuperada.Roles != null ? await RH.Login.Negocios.ServiciosAPINegocio.ObtenerToken(numEmpleado, Autenticacion.User, infoUsuarioRecuperada.Roles) : null;
                        Session["User"] = infoUsuarioRecuperada;
                        codigoResultado = 200;
                        mensajeCliente = "La Autenticacion se cumplio con exito";
                        break;
                    case 1:
                        mensajeCliente = "El Usuario y/o la contraseña es incorrecta";
                        break;
                    case 2:
                        mensajeCliente = "Este equipo no se encuentra autorizado para usar los aplicativos webs";
                        break;
                    case 3:
                        mensajeCliente = "El usuario no esta autorizado para usar ALPHABET , Contacte con un administrador del sistema";
                        break;
                }

                if (infoUsuarioRecuperada.JWT != null)
                {
                    FormsAuthentication.SetAuthCookie(infoUsuarioRecuperada.JWT, false);

                    return Json(new
                    {
                        RespuestaServidor = new HttpStatusCodeResult(codigoResultado, mensajeCliente)

                    });

                }
                else
                {
                    return Json(new
                    {
                        RespuestaServidor = new HttpStatusCodeResult(401, mensajeCliente == "" ? "Ocurrio un error firmando la autorizacion contacte con el desarrollador" : mensajeCliente)
                    });
                }


            }
            else
            {
                return Json(new
                {
                    RespuestaServidor = new HttpStatusCodeResult(codigoResultado, mensajeCliente)
                });
            }

        }


        public FilePathResult DescargarValidadorCliente()
        {
            string path = Path.Combine(Server.MapPath("~/"), "Dowloads/WizardLogin.rar");

            return File(path, "application/octet-stream", "Validador_v" + DateTime.Now.Month + ".0.0.rar");
        }





        //Metodo que se incovo al solicitar la recuperacion de la contraseña via web 
        public async Task<ActionResult> SolicitarCambioPassword(LoginMVCFramework.Models.ModelosDatosEntrada.SolicitudCambioPassword.SolicitarCambioPasswordModel nuevaSolicitud )
        {
           
            string mensajeObtenido = "El equipo desde donde se intenta solicitar el cambio de contraseña no esta autorizado para esta acción";
            int codigoResultadoServer = 403;
            if (ModelState.IsValid)
            {
                RH.Login.Entidades.DTO.InicioDTO.SolicitudCambioPasswordDTO nuevasolicitudDTO = Mapper.Map<RH.Login.Entidades.DTO.InicioDTO.SolicitudCambioPasswordDTO>(nuevaSolicitud);
                bool esPcAutorizada = AutentificaciónNegocio.EsComputadoraAutorizada(nuevasolicitudDTO.direccionMac, nuevasolicitudDTO.hostNameCliente);

                if (esPcAutorizada)
                {

                    mensajeObtenido = await ResetPasswordNegocio.solicitarCambioContraseniaCorreoElectronico(  nuevasolicitudDTO );

                    if (mensajeObtenido.Contains("TIMEERROR"))
                    {
                        //var e = mensajeObtenido.Split('|').Where(x => x != "");
                        mensajeObtenido = mensajeObtenido.Split('|').Where(x => x != "").Skip(1).FirstOrDefault().Trim();
                        /*No autoritazado para hacer otra solicitud*/
                        codigoResultadoServer = 401;
                    }
                    else 
                    {
                        codigoResultadoServer = mensajeObtenido.ToString().Contains("ERROR ") ? 403 : 200;
                    }

                }
            }
             

            return Json(new
            {
                RespuestaServidor = new HttpStatusCodeResult(codigoResultadoServer, mensajeObtenido.ToString())
            });

        }


        public ActionResult SolicitudConfirmadaCambiaPassword(LoginMVCFramework.Models.ModelosDatosEntrada.SolicitudCambioPassword.SolicitudAprobadaCambioPasswordModel solicitudAprobada ) 
        {
            string mensajeObtenido = "El equipo desde donde se intenta solicitar el cambio de contraseña no esta autorizado para esta acción";
            int codigoResultadoServer = 403;
            bool esCorrectoCambioPass = false;
            if (ModelState.IsValid) 
            {

                bool esPcAutorizada = AutentificaciónNegocio.EsComputadoraAutorizada(solicitudAprobada.direccionMac , solicitudAprobada.hostNameCliente);

                if (esPcAutorizada)
                {
                    if (ResetPasswordNegocio.VerificaFolioEstaActivoSinExpirar(solicitudAprobada.folioSolicitud))
                    {
                        esCorrectoCambioPass = ResetPasswordNegocio.cambiarPassworsSolicitudAprobada(solicitudAprobada.idCuentaUsuarioCambiaPass, solicitudAprobada.sustituirPassword, solicitudAprobada.nombreUsuario);
                        codigoResultadoServer = esCorrectoCambioPass ? 200 : 403;
                        mensajeObtenido = esCorrectoCambioPass ? "Se cambio su contraseña correctamente" : "Ocurrio un error al intentar cambiar su contraseña";
                        if (esCorrectoCambioPass)
                            ResetPasswordNegocio.InactivaFolioUsado(solicitudAprobada.folioSolicitud);
                    }
                    else 
                    {
                        mensajeObtenido = "El folio "+solicitudAprobada.folioSolicitud+" de la solicitud ya no se encuentra disponible o expiro, haga una solicitud de nuevo";
                    }

                }

            }

            return Json(new
            {
                RespuestaServidor = new HttpStatusCodeResult(codigoResultadoServer, mensajeObtenido.ToString())
            });
        }




    }
}
