using AutoMapper;
using LoginMVCFramework.Filtros;
using LoginMVCFramework.Models;
using RH.Login.Entidades.DTO;
using RH.Login.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginMVCFramework.Controllers
{
    [FiltroSeguridadSesion]
    public class SesionUsuarioController : Controller
    {

        public ActionResult InicioApp()
        {
            LoginMVCFramework.Models.InformacionUsuarioModel informacionUsuario = Mapper.Map<InformacionUsuarioModel>((UsuarioInfDTO)Session["User"]);
            informacionUsuario.EsUsuarioRoot = RH.Login.Negocios.DBLoginCentralizadoNegocio.VerificarSiElUsuarioEsRoot(informacionUsuario.IdUsuarioEnAlpha);
            informacionUsuario.EsUsuarioNuevo = AutentificaciónNegocio.EsUsuarioNuevo(informacionUsuario.IdUsuarioEnAlpha);

            ViewBag.idCuentaUsuarioCambiaPass = informacionUsuario.EsUsuarioNuevo == true ? informacionUsuario.IdUsuarioEnAlpha : 0;

            ViewBag.Aplicaciones = DBLoginCentralizadoNegocio.ObtenerAplicacionesAsignadas(informacionUsuario.IdUsuarioEnLoginCentralizado);

            return View(informacionUsuario);
        }


        [HttpGet]
        //[HttpPost]
        public HttpResponseBase CrearUrl(int idCatAppServidor, int idTblUsuario) 
        {
            LoginMVCFramework.Models.InformacionUsuarioModel informacionUsuario = Mapper.Map<InformacionUsuarioModel>((UsuarioInfDTO)Session["User"]);
            bool estaAutorizado = DBLoginCentralizadoNegocio.ValidarAccesoAppUsuario( idTblUsuario, idCatAppServidor);


            string rutaRedireccionar = "";
            string direccionservervidor = "";
            if (!estaAutorizado)
            {
                direccionservervidor = System.Diagnostics.Debugger.IsAttached ? "localhost:44312" : "172.19.3.171:85";/* ruta que tendra dentro del servidor*/
                rutaRedireccionar = $"http://{direccionservervidor}/Errores/ErrorEnVistaSolicitada";
            }
            else 
            {
                //local hace referencia al aplicativo de foliacion con el cual se estan realizando pruebas y cuando se obtiene la url es de rutas de los servidores de pruebas
                direccionservervidor = System.Diagnostics.Debugger.IsAttached ? "localhost:52797" : DBLoginCentralizadoNegocio.ObtenerUrlApp(idTblUsuario, idCatAppServidor);
                rutaRedireccionar = $"http://{direccionservervidor}/Validador/ValidaToken?token={informacionUsuario.JWT.Trim()}";
            }



            //Correcto
            if (HttpContext.Response.IsClientConnected)
            {
                HttpContext.Response.StatusCode = 200;
                //HttpContext.Response.AppendHeader("Toooooken", informacionUsuario.JWT);
                HttpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");
                HttpContext.Response.Redirect(rutaRedireccionar, true);
                HttpContext.Response.End();
            }


            return HttpContext.Response;
        }


        [HttpPost]
        public ActionResult CambiarPasswordUsuarioNuevo(string nuevaContrasenia , int idCuentaUsuarioCambiaPass)
        {
            bool esCorrectoCambioPass = false;
            int codigoResultado = 403;

            if (!string.IsNullOrEmpty(nuevaContrasenia) || idCuentaUsuarioCambiaPass != 0) 
            {
                esCorrectoCambioPass = ResetPasswordNegocio.cambiarPassworsUsuarioNuevo(idCuentaUsuarioCambiaPass, nuevaContrasenia);

                 codigoResultado = esCorrectoCambioPass ? 200 : 403;
                
            }

            string mensaje = esCorrectoCambioPass ? "Se cambio su contraseña correctamente" : "Ocurrio un error al intentar cambiar su contraseña";

            return Json(new
            {
                RespuestaServidor = new HttpStatusCodeResult(codigoResultado, mensaje)

            });

        }



        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["User"] = null;

            return RedirectToAction("Login", "Inicio");
        }

    }
}
