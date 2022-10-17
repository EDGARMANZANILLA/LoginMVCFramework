using AutoMapper;
using LoginMVCFramework.Controllers;
using LoginMVCFramework.Models;
using RH.Login.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace LoginMVCFramework.Filtros
{
    public class FiltroSeguridadSesion : AuthorizeAttribute,  IAuthenticationFilter 
    {

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            // _token = Convert.ToString(filterContext.HttpContext.Session["Token"]);
            UsuarioInfDTO sessioDeUser = (UsuarioInfDTO) filterContext.HttpContext.Session["User"];

            if (sessioDeUser == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            else
            {
                //filterContext.Result = new HttpStatusCodeResult(200);
            }
        }




        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null ||  filterContext.Result is HttpUnauthorizedResult)
            {
                // Redireccionando el usuario a la vista de Logueo del controlador Usuario
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                         { "controller", "Inicio" },
                         { "action", "Login" }
                });
            }
        }

    }
}




//public class AuthenticationTokenFilter : AuthorizeAttribute, IAuthenticationFilter
//{
//    private string _token;
//    private string _usuario;

//    public void OnAuthentication(AuthenticationContext filterContext)
//    {
//        _token = Convert.ToString(filterContext.HttpContext.Session["Token"]);
//        if (string.IsNullOrEmpty(_token))
//        {
//            filterContext.Result = new HttpUnauthorizedResult();
//            VariablesGlobales.TOKEN = "";
//            VariablesGlobales.USERNAME = "";
//        }
//        else
//        {
//            VariablesGlobales.TOKEN = _token;
//            _usuario = Generico.ObtenerClaimToken(_token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
//            VariablesGlobales.USERNAME = _usuario;
//        }
//    }

//    public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
//    {
//        if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
//        {
//            // Redireccionando el usuario a la vista de Logueo del controlador Usuario
//            filterContext.Result = new RedirectToRouteResult(
//            new RouteValueDictionary
//            {
//                     { "controller", "Usuario" },
//                     { "action", "Login" }
//            });
//        }
//    }


//    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//    {

//        filterContext.HttpContext.Response.StatusCode = 401;
//        filterContext.HttpContext.Response.End();

//        base.HandleUnauthorizedRequest(filterContext);
//    }
//}
//}
