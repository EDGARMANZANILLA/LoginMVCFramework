using Microsoft.IdentityModel.Tokens;
using RH.Login.Datos.ServiciosAPI;
using RH.Login.Entidades.DTO.ServiciosAPIDTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Negocios
{
    public class ServiciosAPINegocio
    {
        private static string url = "http://172.19.3.171:82/api/login";

        public static async Task<string> ObtenerToken(string numEmpleado, string Username , List<string> roles)
        {
            TokenBodyDTO tokenBodyDTO = new TokenBodyDTO();
            tokenBodyDTO.NumEmpleado = numEmpleado;
            tokenBodyDTO.Username =  Username;
            tokenBodyDTO.Roles =  roles;

            RepositorioJWT jwt = new RepositorioJWT();
            return await jwt.Login(tokenBodyDTO, url);
        }


        public static async Task<string> ObtenerTokenRecuperacionContrasenia(EmpleadoInfoRecuperaPassBodyDTO enviarBody )
        {
            RepositorioJWT jwt = new RepositorioJWT();
            return await jwt.ObtenerTokenRecuperacionContrasenia(enviarBody, "http://172.19.3.171:82/api/recovery");
        }


        public  bool GetTokenExpirationTime(string token)
        {
            bool bandera = false;
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var e = handler.CanReadToken(token);
                var jwtSecurityToken = handler.ReadJwtToken(token);
                string tokenNbf = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("nbf")).Value;
                string tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
                long noExpirado = long.Parse(tokenNbf);
                long expirado = long.Parse(tokenExp);


                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                DateTime hrsInicioUTC = origin.AddSeconds(noExpirado);
                DateTime hrsInicioLocal = hrsInicioUTC.ToLocalTime();



                DateTime hrsExpiracionUTC = origin.AddSeconds(expirado);
                DateTime hrsExpiracionLocal = hrsExpiracionUTC.ToLocalTime();

                
                if (DateTime.Now < hrsExpiracionLocal && DateTime.Now > hrsInicioLocal)
                {
                    bandera = true;
                }
            }
            catch (Exception E) 
            {
                string mensaje = E.Message;
            }
        


            return bandera;
        }

        public bool esTokenValido(string token) 
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.CanReadToken(token);
        }

        public RecuperacionPasswordTokenDTO ObtenerTokenDesencriptado(string token) 
        {
            RecuperacionPasswordTokenDTO nuevaRecuperacion = new RecuperacionPasswordTokenDTO();
            nuevaRecuperacion.NumeroEmpleado = ObtenerClaimToken(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"); ;
            nuevaRecuperacion.NombreUsuario = ObtenerClaimToken(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name");
            nuevaRecuperacion.NombreEmpleado = ObtenerClaimToken(token, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname");
            nuevaRecuperacion.FolioSolicitud = ObtenerClaimToken(token, "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor");

            return nuevaRecuperacion; 
        }




        /// <summary>
        /// Obtiene los datos contenidos en el token, buscandolos por el nombre del Claim
        /// </summary>
        /// <param name="token">Token de autenticación</param>
        /// <param name="nombre">Nombre del Claim del cuál se quiere obtener su valor</param>
        /// <returns>El valor obtenido de los Claims de tipo String, Si contiene la palabra "ERROR" algo malo sucedio </returns>
        private static string ObtenerClaimToken(string token, string nombre)
        {
            string claimObtenido = "";
            try
            {
                var stream = token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;

                return tokenS.Claims.First(claim => claim.Type == nombre).Value;
            }
            catch (Exception E)
            {
                claimObtenido = "ERROR " + E.Message;
            }

            return claimObtenido;
        }



    }
}
