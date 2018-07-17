//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GaslandsTeamBuilderDataRepo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Weapon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Weapon()
        {
            this.BuildWeapons = new HashSet<BuildWeapon>();
        }
    
        public int Key { get; set; }
        public string Name { get; set; }
        public Nullable<int> CanCost { get; set; }
        public string FacingAllowed { get; set; }
        public Nullable<int> BuildSlotCost { get; set; }
        public string Range { get; set; }
        public Nullable<int> Ammo { get; set; }
        public string SpecialRules { get; set; }
        public string Attack { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildWeapon> BuildWeapons { get; set; }
    }
}
