﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GaslandsAppEntities : DbContext
    {
        public GaslandsAppEntities()
            : base("name=GaslandsAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Build> Builds { get; set; }
        public virtual DbSet<BuildWeapon> BuildWeapons { get; set; }
        public virtual DbSet<PerkClass> PerkClasses { get; set; }
        public virtual DbSet<Perk> Perks { get; set; }
        public virtual DbSet<Sponsor> Sponsors { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Upgrade> Upgrades { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Weapon> Weapons { get; set; }
        public virtual DbSet<SpecialRule> SpecialRules { get; set; }
        public virtual DbSet<BuildUpgrade> BuildUpgrades { get; set; }
        public virtual DbSet<TeamBuild> TeamBuilds { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}