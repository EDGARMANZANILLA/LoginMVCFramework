using LoginMVCFramework.Models;
using RH.Login.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LoginMVCFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            AutoMapper.Mapper.Initialize(cfg => {
                cfg.CreateMap<LoginMVCFramework.Models.AutenticacionModel , AutenticacionDTO>();
                cfg.CreateMap<UsuarioInfDTO , InformacionUsuarioModel>();
                cfg.CreateMap<LoginMVCFramework.Models.ModelosDatosEntrada.SolicitudCambioPassword.SolicitarCambioPasswordModel, RH.Login.Entidades.DTO.InicioDTO.SolicitudCambioPasswordDTO>();
            });

        }
    }
}
