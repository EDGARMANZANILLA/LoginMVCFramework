//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RH.Login.Entidades
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cat_Puertos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cat_Puertos()
        {
            this.R_CatAplicativosWebs_CatServidores = new HashSet<R_CatAplicativosWebs_CatServidores>();
        }
    
        public int Id { get; set; }
        public int Puerto { get; set; }
        public string EntradaFirewall { get; set; }
        public string SalidaFirewall { get; set; }
        public bool Activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_CatAplicativosWebs_CatServidores> R_CatAplicativosWebs_CatServidores { get; set; }
    }
}