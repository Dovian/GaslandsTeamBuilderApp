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
    
    public partial class Build
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Build()
        {
            this.BuildWeapons = new HashSet<BuildWeapon>();
            this.Perks = new HashSet<Perk>();
            this.BuildUpgrades = new HashSet<BuildUpgrade>();
            this.TeamBuilds = new HashSet<TeamBuild>();
        }
    
        public int Key { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
        public string Notes { get; set; }
        public Nullable<int> Sponsor { get; set; }
        public Nullable<int> Vehicle { get; set; }
        public int User { get; set; }
    
        public virtual Sponsor Sponsor1 { get; set; }
        public virtual Vehicle Vehicle1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildWeapon> BuildWeapons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Perk> Perks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildUpgrade> BuildUpgrades { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeamBuild> TeamBuilds { get; set; }
        public virtual User User1 { get; set; }
    }
}