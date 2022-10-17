using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Negocios
{
    public class EnvioCorreosNegocio
    {
        private static string urlDomainTesting = "http://localhost:44312";

        private static string urlDomainDeploy = "http://172.19.3.171:85";

        public void EnviarCorreoWeb(string nombreEmpleado , string EmailDestino, string token){ SendEmail( nombreEmpleado ,EmailDestino, token); }
        private void SendEmail(string nombreEmpleado , string EmailDestino, string token)
        {
            //para configurar esta parte necesitas abrir la cuenta desde donde se va enviar el correo (la cuenta establecita aqui)
            //si modificas el emailOrigen  primero ve a https://myaccount.google.com/lesssecureapps y pon que si para que
            //le otorgues los permisos al correo a utilizar
            string urlDomain = System.Diagnostics.Debugger.IsAttached ? urlDomainTesting : urlDomainDeploy;

            string EmailOrigen = "safin.rh.2022.campeche@gmail.com";
            string Contraseña = "ebwrntfkcnvobmvb";
            string url = urlDomain + "/Inicio/ValidarSolicitudCambioPassword/?token="+token;
            MailMessage oMailMessage = new MailMessage(EmailOrigen, EmailDestino, "Recuperación de contraseña", ObtenerHTMLEnviar(nombreEmpleado,  url)) ;

            oMailMessage.IsBodyHtml = true;

            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = 587;
            oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, Contraseña);

            oSmtpClient.Send(oMailMessage);
            oSmtpClient.Dispose();
        }

        private  string ObtenerHTMLEnviar( string nombreEmpleado, string url) 
        {
            string HTML = "<div style='text-align:center'>" +
                                "<h2>Estimado '" + nombreEmpleado + "' </h2>" +
                                "<h3>Hemos notado que ha solicitado un cambio de contraseña desde el ecosistema ALPHABET </h3>" +
                                "<hr style='width:500px;'>" +
                                "<p> Haga click en el botón y recupere su cuenta </p>" +
                                "<a href ='" + url + "'> <button style = 'font-size:16px; font-weight:bold; letter-spacing:normal; line-height:1; padding:16px; background-color:blue; color: white; border-color: blue; border-radius: 8px;' > Recuperar cuenta </ button > </ a >" +
                          "</div>";
            return HTML;
        }


    }
}
