using System.Web;
using System.Web.Mvc;
using LoginMVCFramework.Filtros;

namespace LoginMVCFramework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            ////Carga el Filtro Agregado para el inicio de sesion 
            //filters.Add(new FiltroSeguridadSesion());
        }
    }
}
