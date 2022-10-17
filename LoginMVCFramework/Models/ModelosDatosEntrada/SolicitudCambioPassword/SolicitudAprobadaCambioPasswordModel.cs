using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginMVCFramework.Models.ModelosDatosEntrada.SolicitudCambioPassword
{
    public class SolicitudAprobadaCambioPasswordModel
    {
        [Required]
        public int idCuentaUsuarioCambiaPass { get; set; }
        [Required]
        public int folioSolicitud { get; set; }
        [Required]
        public  string nombreUsuario { get; set; }
        [Required]
        public string sustituirPassword { get; set; }
        [Required]
        public string direccionMac { get; set; }
        [Required]
        public string hostNameCliente { get; set; }
    }
}