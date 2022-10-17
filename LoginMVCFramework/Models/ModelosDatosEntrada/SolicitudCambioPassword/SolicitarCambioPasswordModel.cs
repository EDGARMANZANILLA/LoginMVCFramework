using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace LoginMVCFramework.Models.ModelosDatosEntrada.SolicitudCambioPassword
{
    public class SolicitarCambioPasswordModel
    {
        [Required]
        public string nombreUsuario { get; set; }
        [Required]
        public int numEmpleado  { get; set; }
        [Required]
        public  string direccionMac { get; set; }
        [Required]
        public  string hostNameCliente { get; set; }
        [Required]
        public string ipCliente { get; set; }
    }
}