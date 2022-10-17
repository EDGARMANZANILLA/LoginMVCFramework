using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO
{
    public class UsuarioInfDTO
    {
        public int IdUsuarioEnLoginCentralizado { get; set; }
        public int IdUsuarioEnAlpha { get; set; }

        public string NombreEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        
        public bool EsUsuarioRoot { get; set; }
        public bool EsUsuarioNuevo { get; set; }
        public string JWT { get; set; }
        public List<string> Roles { get; set; }

    }
}
