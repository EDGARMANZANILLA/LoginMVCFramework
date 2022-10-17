using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using RH.Login.Entidades.DTO.ServiciosAPIDTO;
using System.Text.Json;

namespace RH.Login.Datos.ServiciosAPI
{
    public class RepositorioJWT 
    {

        private readonly HttpClient client = new HttpClient();

        public async Task<string> Login(TokenBodyDTO datosBody, string path)
        {
            string token = null;
       
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(path, datosBody);
                response.EnsureSuccessStatusCode();

                token = await response.Content.ReadAsAsync<string>();
              //token = await response.Content.ReadAsStringAsync();
                
                response.Dispose();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return token;
        }




        public async Task<string> ObtenerTokenRecuperacionContrasenia(EmpleadoInfoRecuperaPassBodyDTO enviarBody, string path)
        {
            string token = null;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(path, enviarBody);
                if (response.IsSuccessStatusCode)
                {
                    token = await response.Content.ReadAsAsync<string>();
                }

                response.EnsureSuccessStatusCode();
                response.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return token;
        }



 

    }
}
