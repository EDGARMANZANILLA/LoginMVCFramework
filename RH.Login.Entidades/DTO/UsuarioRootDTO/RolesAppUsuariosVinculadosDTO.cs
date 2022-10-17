using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO.UsuarioRootDTO
{
    public class RolesAppUsuariosVinculadosDTO
    {
        public int Id { get; set; }
        public int IdReal { get; set; }
        public string NumEmpleado { get; set; }
        public string Usuario { get; set; }
        public string NombreApp { get; set; }
        public string Rol { get; set; }
        public string Direccion { get; set; }
        public bool Activo  { get; set; }

    }
}
