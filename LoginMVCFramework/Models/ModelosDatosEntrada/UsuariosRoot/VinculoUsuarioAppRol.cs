using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginMVCFramework.Models.ModelosDatosEntrada.UsuariosRoot
{
    public class VinculoUsuarioAppRol
    {
        [Required]
        public string NumEmpleado { get; set; }
        [Required]
        public int IdAppWeb { get; set; }
        [Required]
        public int IdRol { get; set; }

    }
}