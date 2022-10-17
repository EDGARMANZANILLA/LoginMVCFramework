using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO.ServiciosAPIDTO
{
    public class RecuperacionPasswordTokenDTO
    {
        public string NombreEmpleado { get; set; }
        public string NombreUsuario { get; set; }
        public string NumeroEmpleado { get; set; }
        public string FolioSolicitud { get; set; }
        
    }
}
