using GaslandsTeamBuilderCore;
using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Models
{
    public class BuildViewModel
    {
        public BuildViewModel() { }
        public BuildViewModel(Build _build, DropDownContainer _dropdowns)
        {
            build = _build;
            BuildErrors = new List<string>();
            PerkDropDown = new SelectList(_dropdowns.PerkDropDown, "Key", "ListDisplay");
            SponsorDropDown = new SelectList(_dropdowns.SponsorDropDown, "Key", "Name");
            UpgradeDropDown = new SelectList(_dropdowns.UpgradeDropDown, "UpgradeKey", "ListDisplay");
            VehicleDropDown = new SelectList(_dropdowns.VehicleDropDown, "Key", "ListDisplay");
            WeaponDropDown =_dropdowns.WeaponDropDown;
        }

        public Build build { get; set; }
        public SelectList UpgradeDropDown { get; set; }
        public SelectList SponsorDropDown { get; set; }
        public SelectList PerkDropDown { get; set; }
        public SelectList VehicleDropDown { get; set; }
        public List<Weapon> WeaponDropDown { get; set; }
        public List<string> BuildErrors { get; set; }
        public List<SpecialRule> SpecialRules { get; set; }
    }
}