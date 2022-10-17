using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO
{
    public class AutenticacionDTO
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string DireccionMac { get; set; }
        public string HostNameCliente { get; set; }
    }
}
