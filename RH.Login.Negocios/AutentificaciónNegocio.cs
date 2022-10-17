using RH.Login.Datos;
using RH.Login.Datos.DBNomina;
using RH.Login.Entidades.DTO;
using RH.Login.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Negocios
{
    public class AutentificaciónNegocio
    {
        public static int AutenticarUsuario( AutenticacionDTO autenticarDatos, int IdUsuarioEnAlpha  ) 
        {
            /*  status - Cadena  
             *      0  - La Autenticacion se cumplio con exito en todos los criterios
             *      1  - La autenticacion del usuario o contraseña es incorrecta 
             *      2  - La autenticacion de la mac y el host no se encuentan registrados 
             *      3 - El usuario no esta autorizado para usuar ALPHABET 
             */

            //Obtener el encriptado del password
            string contraseniaEncriptada = RH.Login.Datos.Validacion.ObtenerContraseniaBlowFish(autenticarDatos.Password.Trim());

            //Autenticar al usuario
            bool usuarioAutenticado = RH.Login.Datos.Validacion.AutenticarCredenciales( autenticarDatos.User , contraseniaEncriptada);

            //Autenticar Maquina del cliente ( host de donde se conecta el cliente )
            //bool maquinaAutenticada = Validacion.AutenticarMac( autenticarDatos.DireccionMac , autenticarDatos.HostNameCliente );
            bool maquinaAutenticada = EsComputadoraAutorizada( autenticarDatos.DireccionMac , autenticarDatos.HostNameCliente );

            Transaccion transaccion = new Transaccion();
            var repoTbl_UsuarioWeb = new Repositorio<tbl_UsuariosWeb>(transaccion);
            var usuariosRegistrados = repoTbl_UsuarioWeb.ObtenerTodos();
            bool usuarioAutorizadoALPHABET =  usuariosRegistrados.Select(x => x.IdUsuarioAlpha_nomina_nom_cat_user).Contains(IdUsuarioEnAlpha);


                if (!usuarioAutorizadoALPHABET)
                {
                    return 3;  /*3 - El usuario no esta autorizado para usuar ALPHABET */
                }
                if (!usuarioAutenticado)
                {
                    return 1;  /*1 - La autenticacion del usuario o contraseña es incorrecta*/
                }
                else if (!maquinaAutenticada)
                {
                    return 2; /*2 - La autenticacion de la mac y el host no se encuentan registrados */
                }
                else
                {
                    return 0; /*0 - La Autenticacion se cumplio con exito en todos los criterios */
                }
            


        }

        public static bool EsComputadoraAutorizada( string direccionMac , string hostNameCliente) 
        {
            return Validacion.AutenticarMac(direccionMac , hostNameCliente);
        }


        public static UsuarioInfDTO ObtenerInfoUsuario( string numEmpleado)
        {
          return  Datos.DBNomina.InformacionUsuarios.ObtenerInformacionUsuario(numEmpleado);
        }

        public static string ObtenerNumEmpleadoDesdeUsuario(string usuario)
        {
            return Datos.DBNomina.InformacionUsuarios.ObtenerNumEmpleadoDesdeUsuario(usuario);
        }

        public static int ObtenerIdRegistroEnAlphaDesdeUsuario(string usuario)
        {
            return InformacionUsuarios.ObtenerIdRegistroUsuarioAlpha(usuario);
        }


        public static bool EsUsuarioNuevo(int id)
        {
            return InformacionUsuarios.EsUsuarioNuevo(id);
        }



    }
}
