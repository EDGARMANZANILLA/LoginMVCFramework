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
    
    public partial class R_CatAplicativosWebs_CatServidores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public R_CatAplicativosWebs_CatServidores()
        {
            this.R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores = new HashSet<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores>();
        }
    
        public int Id { get; set; }
        public int Id_Cat_AplicativoWeb { get; set; }
        public int Id_Cat_Servidor { get; set; }
        public int Id_Cat_Puerto { get; set; }
        public bool Activo { get; set; }
    
        public virtual Cat_AplicativosWebs Cat_AplicativosWebs { get; set; }
        public virtual Cat_Puertos Cat_Puertos { get; set; }
        public virtual Cat_Servidores Cat_Servidores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores> R_UsuariosWebs_CatRol_RCatAplicativosWebsServidores { get; set; }
    }
}
