using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RH.Login.Entidades.DTO
{
    public class MostrarAppsUsuarioDTO
    {
        //Esta clase hace referencia a la tabla cat_aplicaticosWebs con un filtro que se aplica en el negocio para mostrar las apps segun el usuario que ingrese
        public int IdUser { get; set; }
        public int IdLlave { get; set; }
        
        public string NombreApp { get; set; }
    }
}
