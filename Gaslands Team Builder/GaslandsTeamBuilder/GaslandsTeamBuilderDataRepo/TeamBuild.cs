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
    
    public partial class TeamBuild
    {
        public int TeamKey { get; set; }
        public int BuildKey { get; set; }
        public int Key { get; set; }
    
        public virtual Build Build { get; set; }
        public virtual Team Team { get; set; }
    }
}
