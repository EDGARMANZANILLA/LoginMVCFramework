using RH.Login.Datos.DBNomina;
using RH.Login.Entidades.DTO;
using RH.Login.Entidades.DTO.UsuarioRootDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RH.Login.Negocios
{
    public class UsuarioRootNegocio
    {

        public static List<UsuarioNoRegistradoDTO> ObtenerUsuariosNoRegistrados() 
        {
            return InformacionUsuarios.ObtenerUsuariosNoRegitradosEnSistemaWeb();
        }



        public static List<UsuarioNoRegistradoDTO> ObtenerUsuariosRegistradosEnLogin()
        {
            return InformacionUsuarios.ObtenerUsuariosRegitradosLoginCentralizado();
        }


        public static string ObtenerNombreUsuarioCuentaAlpha(int idCuentaLoginCentralizado)
        {
            return InformacionUsuarios.nombreUsuarioCuentaAlpha(idCuentaLoginCentralizado);
        }

        public static List<UniversoUsuariosAlphaDTO> ObtenerUniversoUsuariosAlpha() 
        {
            return InformacionUsuarios.ObtenerUniversoUsuariosAlpha();
        }


     

    }
}
