//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfMedaSlovenija
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblInstitutionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblInstitutionType()
        {
            this.tblInstitutionSubtypes = new HashSet<tblInstitutionSubtype>();
        }
    
        public int itpID { get; set; }
        public string itpName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInstitutionSubtype> tblInstitutionSubtypes { get; set; }
    }
}
