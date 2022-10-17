using LoginMVCFramework.Filtros;
using RH.Login.Negocios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginMVCFramework.Models.ModelosDatosEntrada.UsuariosRoot;
using RH.Login.Entidades.DTO.UsuarioRootDTO;

namespace LoginMVCFramework.Controllers
{
    [FiltroSeguridadSesion]
    public class UsuarioRootController : Controller
    {
        // GET: UsuarioRoot
        public ActionResult RegistrarUsuario()
        {
            return PartialView(UsuarioRootNegocio.ObtenerUsuariosNoRegistrados());
        }


        public ActionResult VincularUsuario()
        {
            ViewBag.AplicativosWeb = DBLoginCentralizadoNegocio.ObtenerAplicativosWebActivos();

            ViewBag.Roles = DBLoginCentralizadoNegocio.ObtenerRolesActivos();

            return PartialView(UsuarioRootNegocio.ObtenerUsuariosRegistradosEnLogin());
        }


        public ActionResult VerVinculosPorEmpleado() 
        {
            return PartialView(UsuarioRootNegocio.ObtenerUsuariosRegistradosEnLogin());
        }

        public ActionResult ResetearContraseniaUsuario() 
        {
            return PartialView(UsuarioRootNegocio.ObtenerUniversoUsuariosAlpha());
        }





        [HttpPost]
        // Registrar Usuario
        public ActionResult RegistrarUsuarioEnAlphaWeb(string numEmpleado ) 
        {
          return Json(DBLoginCentralizadoNegocio.RegistrarUsuarioAlphaWeb(numEmpleado), JsonRequestBehavior.AllowGet);
        }


        // Metodo de la vista VincularUsuario
        public ActionResult VincularUsuarioConAplicativoWEB(VinculoUsuarioAppRol nuevoVinculo)
        {
            bool bandera = false;
            int codigoResultado = 503;
            string mensaje = "";

            if (ModelState.IsValid)
            {

                int idRegistroAlpha = DBLoginCentralizadoNegocio.ObtieneIdNumEmpleadoRegistrado(nuevoVinculo.NumEmpleado);
                int idEmpleadoRegistrado = DBLoginCentralizadoNegocio.ObtenerIdUsuarioEnLoginCentralizado(idRegistroAlpha);

                if (idRegistroAlpha == 0 || idEmpleadoRegistrado == 0 )
                {
                    codigoResultado = 503;
                    mensaje = "El numero de empleado no se encuentra registrado en alpha o dado de alta para usar aplicativos web's";
                }
                else 
                {
                    VincularUsuarioAppRolDTO nuevoVinculoDTO = new VincularUsuarioAppRolDTO();
                    nuevoVinculoDTO.IdRegitroLoginCentralizado = idEmpleadoRegistrado;
                    nuevoVinculoDTO.IdAplicativo = nuevoVinculo.IdAppWeb;
                    nuevoVinculoDTO.IdRol = nuevoVinculo.IdRol;


                    bool existeDuplicadoORolAlto =  DBLoginCentralizadoNegocio.ExisteRolUsuarioEnAplicativo(nuevoVinculoDTO);
                    if (!existeDuplicadoORolAlto)
                    {

                        bool registroExitoso = DBLoginCentralizadoNegocio.VincularUsuarioAppRol(nuevoVinculoDTO);

                        if (registroExitoso)
                        {
                            codigoResultado = 200;
                            mensaje = "Se a vinculado con exito al empleado : " + nuevoVinculo.NumEmpleado;
                        }
                        else
                        {
                            codigoResultado = 503;
                            mensaje = "No se pudo vincular al empleado : " + nuevoVinculo.NumEmpleado;
                        }
                    }
                    else 
                    {
                        codigoResultado = 503;
                        mensaje = "No se puede vincular el usuario al aplicativo. El rol seleccionado es innecesario o carece de sentido";
                    }



                }

                return Json(new
                {
                    RespuestaServidor = new HttpStatusCodeResult(codigoResultado, mensaje),
                });
            }
            else 
            {
                return Json(new
                {
                    RespuestaServidor = new HttpStatusCodeResult(codigoResultado, "Datos no validos correctamente, intente de nuevo"),
                });
            }

        }





        //Metodos de la vista VerVinculosPorEmpleado
        public ActionResult ObtenerVinculosUsuario(string numEmpleado)
        {
            List<RolesAppUsuariosVinculadosDTO> listaRoles = new List<RolesAppUsuariosVinculadosDTO>();
            string mensaje = "";
            int codigoResultado = 200;

            int idRegistroAlpha = DBLoginCentralizadoNegocio.ObtieneIdNumEmpleadoRegistrado(numEmpleado);
            int idEmpleadoRegistradoLoginCentralizado = DBLoginCentralizadoNegocio.ObtenerIdUsuarioEnLoginCentralizado(idRegistroAlpha);

            if (idRegistroAlpha == 0 || idEmpleadoRegistradoLoginCentralizado == 0)
            {
                codigoResultado = 503;
                mensaje = "El numero de empleado no se encuentra registrado en alpha o dado de alta para usar aplicativos web's";
            }
            else
            {
                listaRoles = DBLoginCentralizadoNegocio.ObtenerVinculosAppRol(idEmpleadoRegistradoLoginCentralizado, idRegistroAlpha);
            }


            return Json(new
            {
                RespuestaServidor = new HttpStatusCodeResult(codigoResultado, mensaje),
                Roles = listaRoles
            });
        }


        public ActionResult ActivarRol(int idRelacionesRolAplicaciones)
        {
            return Json(DBLoginCentralizadoNegocio.ActivarRol( idRelacionesRolAplicaciones), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DesactivarRol(int idRelacionesRolAplicaciones) 
        {
            return Json(DBLoginCentralizadoNegocio.DesactivarRol(idRelacionesRolAplicaciones), JsonRequestBehavior.AllowGet);
        }




        //Meto de la vista ResetearContraseniaUsuario() 
        public ActionResult ResetearPassword(int numEmpleado) 
        {
            string resultadoProceso = ResetPasswordNegocio.AdminReseteaContrasenia(numEmpleado);
            int codigo = resultadoProceso.Contains("alpha") ? 200: 403;


            return Json(new { RespuestaServidor = new HttpStatusCodeResult(codigo, resultadoProceso) });
        }
    }




}