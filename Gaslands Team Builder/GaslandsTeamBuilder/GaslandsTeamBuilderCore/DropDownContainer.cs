using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;

namespace GaslandsTeamBuilderCore
{
    public class DropDownContainer
    {
        public List<Upgrade> UpgradeDropDown { get; set; }
        public List<Sponsor> SponsorDropDown { get; set; }
        public List<Perk> PerkDropDown { get; set; }
        public List<Vehicle> VehicleDropDown { get; set; }
        public List<Weapon> WeaponDropDown { get; set; }
    }
}
