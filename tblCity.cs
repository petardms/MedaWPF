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
    
    public partial class tblCity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tblCity()
        {
            this.tblInstitutionCroatias = new HashSet<tblInstitutionCroatia>();
            this.tblInstitutionSlovenias = new HashSet<tblInstitutionSlovenia>();
            this.tblInstitutionSerbias = new HashSet<tblInstitutionSerbia>();
        }
    
        public int ctyID { get; set; }
        public string ctyName { get; set; }
        public Nullable<int> dstID { get; set; }
    
        public virtual tblDistrict tblDistrict { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInstitutionCroatia> tblInstitutionCroatias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInstitutionSlovenia> tblInstitutionSlovenias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tblInstitutionSerbia> tblInstitutionSerbias { get; set; }
    }
}