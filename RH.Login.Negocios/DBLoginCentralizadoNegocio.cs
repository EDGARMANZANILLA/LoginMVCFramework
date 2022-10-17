using RH.Login.Datos;
using RH.Login.Datos.DBNomina;
using RH.Login.Entidades;
using RH.Login.Entidades.DTO.UsuarioRootDTO;
using RH.Login.Entidades.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections;

namespace RH.Login.Negocios
{
    public class DBLoginCentralizadoNegocio
    {
        public static bool VerificarSiElUsuarioEsRoot(int idRegistroUsuarioAlpha)
        {
            bool esRoot = false;

            Transaccion transaccion = new Transaccion();
            var repositorio = new Repositorio<tbl_UsuariosWeb>(transaccion);
            tbl_UsuariosWeb registroObtenido = repositorio.Obtener(x => x.IdUsuarioAlpha_nomina_nom_cat_user == idRegistroUsuarioAlpha);

            if (registroObtenido != null) { esRoot = registroObtenido.EsRoot; }
            return esRoot;
        }

        public static List<MostrarAppsUsuarioDTO> ObtenerAplicacionesAsignadas(int idUsuarioLoginCentralizado) 
        {
            List<MostrarAppsUsuarioDTO> mostrarApps = new List<MostrarAppsUsuarioDTO>();

            Transaccion transaccion = new Transaccion();
            var repo = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            var repoAplicativos = new Repositorio<R_CatAplicativosWebs_CatServidores>(transaccion);
            
            List<int> listaAplicativosVigentes = repo.ObtenerPorFiltro(x => x.Id_UsuarioWeb == idUsuarioLoginCentralizado && x.Activo == true).Select(x => x.Id_R_CatAplicativosWebs_CatServidores).Distinct().ToList();

            foreach (int appEncontrado in listaAplicativosVigentes) 
            {
                R_CatAplicativosWebs_CatServidores relacionServidorEncontrado = repoAplicativos.Obtener(x => x.Id == appEncontrado);
                if(relacionServidorEncontrado != null) 
                {
                    MostrarAppsUsuarioDTO nuevaApp = new MostrarAppsUsuarioDTO();
                    nuevaApp.IdUser = idUsuarioLoginCentralizado;
                    nuevaApp.IdLlave = relacionServidorEncontrado.Id;
                    nuevaApp.NombreApp = relacionServidorEncontrado.Cat_AplicativosWebs.NombreApp;
                    mostrarApps.Add(nuevaApp);
                }
            }
            return mostrarApps;
        }

        public static bool RegistrarUsuarioAlphaWeb( string numEmpleado) 
        {
            bool bandera = false;
            try
            {
                int idTablaAlpha = Datos.DBNomina.InformacionUsuarios.ObtenerIdUsuarioAlphaPorNumeroEmpleado(numEmpleado);

                if (idTablaAlpha != 0) 
                {
                    Transaccion transaccion = new Transaccion();
                    var repositorio = new Repositorio<tbl_UsuariosWeb>(transaccion);

                    var usuariariosRegistrados = repositorio.ObtenerTodos();


                    if( !usuariariosRegistrados.Select(x => x.IdUsuarioAlpha_nomina_nom_cat_user).Contains(idTablaAlpha) ) 
                    {
                        tbl_UsuariosWeb nuevoUsuario = new tbl_UsuariosWeb();
                        nuevoUsuario.IdUsuarioAlpha_nomina_nom_cat_user = idTablaAlpha;
                        nuevoUsuario.UltimaSesion = null;
                        nuevoUsuario.EsRoot = false;
                        nuevoUsuario.Activo = true;
                        tbl_UsuariosWeb usuarioAgredadoExitosamente = repositorio.Agregar(nuevoUsuario);

                        bandera = usuarioAgredadoExitosamente != null ? true : false;
                    }
                   
                }

            }
            catch (Exception E)
            {
                
                bandera = false;   
            }

            return bandera;
        }






        public static List<AplicativosWebsDTO> ObtenerAplicativosWebActivos()
        {
            Transaccion transaccion = new Transaccion();
            var repositorio = new Repositorio<R_CatAplicativosWebs_CatServidores>(transaccion);
            List<R_CatAplicativosWebs_CatServidores> listaAplicativosRegistrados =  repositorio.ObtenerPorFiltro(x => x.Activo == true).ToList();

            if (listaAplicativosRegistrados != null)
            {
                List<AplicativosWebsDTO> aplicativosWebs = new List<AplicativosWebsDTO>();
                foreach (R_CatAplicativosWebs_CatServidores nuevoAplicativo in listaAplicativosRegistrados)
                {
                    AplicativosWebsDTO nuevaAPP = new AplicativosWebsDTO();
                    nuevaAPP.Id = nuevoAplicativo.Id;
                    nuevaAPP.NombreAplicativo = nuevoAplicativo.Cat_AplicativosWebs.NombreApp + " || " + nuevoAplicativo.Cat_Servidores.NombreServidor + " || " + nuevoAplicativo.Cat_Servidores.Direccion + " ||  " + nuevoAplicativo.Cat_Puertos.Puerto;
                    aplicativosWebs.Add( nuevaAPP );
                }
                return aplicativosWebs;
            }
            return new List<AplicativosWebsDTO>();
        }



        public static List<RolesWebDTO> ObtenerRolesActivos()
        {
            Transaccion transaccion = new Transaccion();
            var repositorio = new Repositorio<Cat_Roles>(transaccion);
            List<Cat_Roles> rolesRegistrados = repositorio.ObtenerPorFiltro(x => x.Activo == true ).ToList();

            if (rolesRegistrados != null)
            {
                List<RolesWebDTO> aplicativosWebs = new List<RolesWebDTO>();
                foreach (Cat_Roles rol in rolesRegistrados)
                {
                    RolesWebDTO nuevoRol = new RolesWebDTO();
                    nuevoRol.Id = rol.Id;
                    nuevoRol.DescripcionRol = rol.NombreRol + " || " + rol.Descripcion;
                    aplicativosWebs.Add(nuevoRol);
                }
                return aplicativosWebs;
            }
            return new List<RolesWebDTO>();
        }




        /***********************************************************************************************************************************************/
        /***********************************************************************************************************************************************/
        /***********************************    Metodo para vincular usuarios con la aplicativos y roles    ********************************************/
        /***********************************************************************************************************************************************/
        /***********************************************************************************************************************************************/
        /// <summary>
        /// Obtiene el id del numero de empleado que esta registrado en el loggin centralizado, sino existe devuelve 0
        /// </summary>
        /// <returns>Retorna el id del registo del empleado o el numero "0" en casa que el usuario no se encuentre registrado</returns>
        public static int ObtieneIdNumEmpleadoRegistrado( string numEmpleado) 
        {
                int idTablaAlpha = Datos.DBNomina.InformacionUsuarios.ObtenerIdUsuarioAlphaPorNumeroEmpleado(numEmpleado);

            return idTablaAlpha;
        }

        public static int ObtenerIdUsuarioEnLoginCentralizado(int idRegistroAlpha) 
        {
            Transaccion transaccion = new Transaccion();
            var repoUsuario = new Repositorio<tbl_UsuariosWeb>(transaccion);

            if (idRegistroAlpha == 0)
                return 0;
            
            return repoUsuario.Obtener(x => x.IdUsuarioAlpha_nomina_nom_cat_user == idRegistroAlpha ).Id;
        }




        /// <summary>
        /// Verifica que el rol con el que se quiere vincular un usuario a una app no este duplicado ni que cuente con un Rol de alto rango
        /// </summary>
        /// <returns> Retorna una bandera y solo si el resultado es false deberia seguir de lo contrario si la bandera es True es porque el rol con el que se quiere vincular al usuario con la aplicacion ya existe y no es necesario duplicarlo</returns>
        public static bool ExisteRolUsuarioEnAplicativo(VincularUsuarioAppRolDTO rolAppUsuario) 
        {
            bool bandera = true;
            Transaccion transaccion = new Transaccion();
            var repo = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            var repoRoles = new Repositorio<Cat_Roles>(transaccion);

            List<Cat_Roles> catRoles = repoRoles.ObtenerTodos().ToList();
            int root = catRoles.Where(x => x.NombreRol.Trim() == "Administrador(Root)").Select(x => x.Id).ToList().FirstOrDefault();
            int userCRUD = catRoles.Where(x => x.NombreRol.Trim() == "Usuario(CRUD)").Select(x => x.Id).ToList().FirstOrDefault();

            
            List<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores> rolesAsigandosEnApp = repo.ObtenerPorFiltro(x => x.Id_UsuarioWeb == rolAppUsuario.IdRegitroLoginCentralizado && x.Id_R_CatAplicativosWebs_CatServidores == rolAppUsuario.IdAplicativo).ToList();

            //Sino contiene un rango de alto nivel puede adquirir mas roles siempre y cuando no se repitan para la misma aplicacion 
            if (!rolesAsigandosEnApp.Select(x => x.Id_Rol).Contains(root))
            {
                if (!rolesAsigandosEnApp.Select(x => x.Id_Rol).Contains(userCRUD))
                {
                    //Sino contiene el IdRol que se desea vincular al usuario en la aplicacion podra agregarlo
                    if (!rolesAsigandosEnApp.Select(x => x.Id_Rol).Contains(rolAppUsuario.IdRol))
                    {

                        bandera = false;
                    }
                }

            }

            // si contiene algun Rol de Rango alto no necesita adquirir mas roles ya que podra hacer de todo en la app
            return bandera;
        }






        /// <summary>
        /// Inserta el vinculo del id de un usuario registrado previamente con la aplicacion web que usara y su rol que tendra
        /// </summary>
        /// <param name="vincularnuevoUsuario"></param>
        /// <returns>Devuelve una boleano true para saber que la insersion fue exitosa y false para saber que no se pudo insertar </returns>
        public static bool VincularUsuarioAppRol(VincularUsuarioAppRolDTO vincularnuevoUsuario) 
        {
            /*
             * 1 => 1 el usuario se a vinculado con exito a la appicacion seleccionada
             * 0 => 0 No se pudo vincular el usuario con el aplicativo web 
             */
            Transaccion transaccion = new Transaccion();
            var repoVinculoWebUsuarioRol = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            

            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores nuevoViculo = new R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores();
            nuevoViculo.Id_UsuarioWeb = vincularnuevoUsuario.IdRegitroLoginCentralizado;
            nuevoViculo.Id_Rol = vincularnuevoUsuario.IdRol;
            nuevoViculo.Id_R_CatAplicativosWebs_CatServidores = vincularnuevoUsuario.IdAplicativo;
            nuevoViculo.Activo = true;
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores vinculoAgregado = repoVinculoWebUsuarioRol.Agregar(nuevoViculo);

            bool bandera = false;
            if (vinculoAgregado != null) 
            {
                bandera = true;
            }
            return bandera;
        }




        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        /***********************************    Metodos para obtener los vinculos que existe de un usuario con aplicativos y roles    *******************************************/
        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        /// <summary>
        /// obtene los viculos con los roles dentro de los aplicativos donde esta dado de alta un usuario 
        /// </summary>
        /// <param name="idUsuarioLoginCentralizado"></param>
        public static List<RolesAppUsuariosVinculadosDTO> ObtenerVinculosAppRol(int idUsuarioLoginCentralizado, int idRegistroAlpha) 
        {
            Transaccion transaccion = new Transaccion();
            var repoVinvulos = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            var vinculosObtenidos = repoVinvulos.ObtenerPorFiltro(x => x.Id_UsuarioWeb == idUsuarioLoginCentralizado);

            string nombreCuentaAlpha = UsuarioRootNegocio.ObtenerNombreUsuarioCuentaAlpha(idRegistroAlpha);
            string numEmpleado = InformacionUsuarios.ObtenerNumEmpleadoDesdeUsuario(nombreCuentaAlpha); 

            List<RolesAppUsuariosVinculadosDTO> listaRolesxAplicativos = new List<RolesAppUsuariosVinculadosDTO>();
            int iterador = 0;
            foreach (var nuevoVinculo in vinculosObtenidos) 
            {
                RolesAppUsuariosVinculadosDTO nuevoRol = new RolesAppUsuariosVinculadosDTO();

                nuevoRol.IdReal = nuevoVinculo.Id ;
                nuevoRol.Id = ++iterador ;
                nuevoRol.NumEmpleado = numEmpleado ;
                nuevoRol.Usuario = nombreCuentaAlpha;
                nuevoRol.NombreApp = nuevoVinculo.R_CatAplicativosWebs_CatServidores.Cat_AplicativosWebs.NombreApp;
                nuevoRol.Direccion = nuevoVinculo.R_CatAplicativosWebs_CatServidores.Cat_Servidores.Direccion +":"+nuevoVinculo.R_CatAplicativosWebs_CatServidores.Cat_Puertos.Puerto;
                nuevoRol.Rol = nuevoVinculo.Cat_Roles.NombreRol;
                nuevoRol.Activo =nuevoVinculo.Activo;
                listaRolesxAplicativos.Add(nuevoRol);
            }
            return listaRolesxAplicativos;
        }



        public static bool ActivarRol(int idRelacionesRolAplicaciones) 
        {
            Transaccion transaccion = new Transaccion();
            var repoVinculos = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores vinculos = repoVinculos.Obtener(x => x.Id == idRelacionesRolAplicaciones);
            vinculos.Activo = true;
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores rolActivado = repoVinculos.Modificar(vinculos);
            
           return  rolActivado != null ? true : false;
        }
        
        
        public static bool DesactivarRol(int idRelacionesRolAplicaciones) 
        {
            Transaccion transaccion = new Transaccion();
            var repoVinculos = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores vinculos = repoVinculos.Obtener(x => x.Id == idRelacionesRolAplicaciones);
            vinculos.Activo = false;
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores rolActivado = repoVinculos.Modificar(vinculos);

            return rolActivado != null ? true : false;
        }




        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        /***********************************            Valida si el usuario tiene permiso de redirigirse a una app         *****************************************************/
        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        public static bool ValidarAccesoAppUsuario(int idLoginCentralizado, int idcatRelacionAppWeb)
        {
            bool estaAutoridoUsuarioConApp = false;
            Transaccion transaccion = new Transaccion();
            var repoVinculosServidorUsuario = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores aplicativoAsignadoEmpleado = repoVinculosServidorUsuario.ObtenerPorFiltro(x => x.Id_UsuarioWeb == idLoginCentralizado && x.Id_R_CatAplicativosWebs_CatServidores == idcatRelacionAppWeb && x.Activo == true).FirstOrDefault();


            if (aplicativoAsignadoEmpleado != null)
            {
                estaAutoridoUsuarioConApp = true;
            }

            return estaAutoridoUsuarioConApp;
        }



        public static string ObtenerUrlApp(int idLoginCentralizado, int idcatRelacionAppWeb) 
        {
            string url = "";
            Transaccion transaccion = new Transaccion();
            var repoVinculosServidorUsuario = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
            R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores aplicativoAsignadoEmpleado = repoVinculosServidorUsuario.ObtenerPorFiltro(x => x.Id_UsuarioWeb == idLoginCentralizado && x.Id_R_CatAplicativosWebs_CatServidores == idcatRelacionAppWeb && x.Activo == true).FirstOrDefault();


            if (aplicativoAsignadoEmpleado != null)
            {
                var repoAplicativoWeb = new Repositorio<R_CatAplicativosWebs_CatServidores>(transaccion);
                R_CatAplicativosWebs_CatServidores appEnServidorEncontrado = repoAplicativoWeb.Obtener(x => x.Id == aplicativoAsignadoEmpleado.Id_R_CatAplicativosWebs_CatServidores);


                url = appEnServidorEncontrado.Cat_Servidores.Direccion + ":" + appEnServidorEncontrado.Cat_Puertos.Puerto;
                if (appEnServidorEncontrado.Cat_Puertos.Puerto == 80) 
                {
                    url = url + "/Login.aspx";
                }
            }
            return url;
        }







        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        /***********************************            Valida y devuelve roles no duplicadore          *****************************************************/
        /************************************************************************************************************************************************************************/
        /************************************************************************************************************************************************************************/
        public static List<string> ObtenerRolesUsuarioWeb(int IdUsuarioEnLoginCentralizado ) 
        {
            List<string> roles = new List<string>();
           
            if (IdUsuarioEnLoginCentralizado > 0) 
            {
                Transaccion transaccion = new Transaccion();
                var repoRelacionUsuarioServerRol = new Repositorio<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>(transaccion);
                List<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores> rolesAsignadosUsuario = repoRelacionUsuarioServerRol.ObtenerPorFiltro(x => x.Id_UsuarioWeb == IdUsuarioEnLoginCentralizado && x.Activo == true).ToList();

                if (rolesAsignadosUsuario.Count > 0)
                {
                    foreach (var rol in rolesAsignadosUsuario)
                    {
                        roles.Add(rol.Cat_Roles.NombreRol.Trim());
                    }
                }
                else 
                {
                    roles.Add("No contiene Roles");
                }



            }
            return roles;
        }

    }
}
