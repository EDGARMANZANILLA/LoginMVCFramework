using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginMVCFramework.Models
{
    public class InformacionUsuarioModel
    {
        public int IdUsuarioEnLoginCentralizado { get; set; }
        public int IdUsuarioEnAlpha { get; set; }
        public string NombreEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public bool EsUsuarioRoot { get; set; }
        public bool EsUsuarioNuevo { get; set; }
        public string JWT { get; set; }
    }
}