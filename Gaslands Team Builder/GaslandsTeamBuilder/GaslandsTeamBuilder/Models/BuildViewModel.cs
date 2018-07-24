using GaslandsTeamBuilderCore;
using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace GaslandsTeamBuilder.Models
{
    public class BuildViewModel
    {
        public BuildViewModel() { }
        public BuildViewModel(Build _build, DropDownContainer _dropdowns, List<SpecialRule> _specialRules = null)
        {
            build = _build;
            BuildErrors = new List<string>();
            if (_dropdowns != null)
            {
                PerkDropDown = new SelectList(_dropdowns.PerkDropDown, "Key", "ListDisplay");
                SponsorDropDown = new SelectList(_dropdowns.SponsorDropDown, "Key", "Name");
                UpgradeDropDown = new SelectList(_dropdowns.UpgradeDropDown, "UpgradeKey", "ListDisplay");
                VehicleDropDown = new SelectList(_dropdowns.VehicleDropDown, "Key", "ListDisplay");
                WeaponDropDown = _dropdowns.WeaponDropDown;
            }
            SpecialRules = _specialRules;

            if (SpecialRules == null)
            {
                SpecialRules = new List<SpecialRule>();
            }
        }

        public Build build { get; set; }
        public SelectList UpgradeDropDown { get; set; }
        public SelectList SponsorDropDown { get; set; }
        public SelectList PerkDropDown { get; set; }
        public SelectList VehicleDropDown { get; set; }
        public List<Weapon> WeaponDropDown { get; set; }
        public List<string> BuildErrors { get; set; }
        public List<SpecialRule> SpecialRules { get; set; }

        public void PrepForPrint()
        {
            if(build.Perks != null)
            {
                string[] perksToRemove = new string[]
                {
                    "Might Is Right",
                    "Military Hardware",
                    "Elegance",
                    "Thumpermonkey",
                    "N20 Addict",
                    "Kiss My Asphalt",
                    "Prison Cars"
                };
                build.Perks.RemoveAll(p => perksToRemove.Contains(p.Name));
            }
            if(SpecialRules.Count > 0)
            {
                string[] rulesToRemove = new string[]
                {
                    "Electrical",
                    "Military",
                    "Bombs Away"
                };
                SpecialRules.RemoveAll(sr => rulesToRemove.Contains(sr.Name));
            }
        }
    }
}