using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO.InicioDTO
{
    public class SolicitudCambioPasswordDTO
    {
        public string nombreUsuario { get; set; }
        
        public int numEmpleado { get; set; }
        
        public string direccionMac { get; set; }
        
        public string hostNameCliente { get; set; }
        
        public string ipCliente { get; set; }
    }
}
