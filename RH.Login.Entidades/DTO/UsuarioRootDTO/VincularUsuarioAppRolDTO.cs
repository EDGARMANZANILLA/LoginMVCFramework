using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO.UsuarioRootDTO
{
    public class VincularUsuarioAppRolDTO
    {
        public int IdRegitroLoginCentralizado { get; set; }
        public int IdAplicativo { get; set; }
        public int IdRol  { get; set; }
    }
}
