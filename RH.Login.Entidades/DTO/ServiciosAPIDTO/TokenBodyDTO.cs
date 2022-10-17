using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO.ServiciosAPIDTO
{
    public class TokenBodyDTO
    {
        public string NumEmpleado { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
        
    }
}
