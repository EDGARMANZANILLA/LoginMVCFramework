using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoginMVCFramework.Models
{
    public class AutenticacionModel
    {
        [Required]
        public string User { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string DireccionMac { get; set; }

        [Required]
        public string HostNameCliente { get; set; }
    }
}