using System.Collections.Generic;

namespace GaslandsTeamBuilderCore.Entities
{
    public class Weapon
    {
        public int WeaponKey { get; set; }
        public string Name { get; set; }
        public int CanCost { get; set; }
        public int BuildSlotCost { get; set; }
        public string Range { get; set; }
        public int? Ammo { get; set; }
        public string SpecialRules { get; set; }
        public List<string> FacingAllowed { get; set; }
        public string Attack { get; set; }

        public string SpecialRulesDisplay
        {
            get
            {
                return SpecialRules != null ? (SpecialRules.Contains(",") ? SpecialRules.Replace(",", ", ") : SpecialRules) : "";
            }
        }

        public string ListDisplay
        {
            get
            {
                return Name + " (" + CanCost.ToString() + ", " + BuildSlotCost.ToString() + ")";
            }
        }
    }
}
