using System.Collections.Generic;
using System.Linq;

namespace GaslandsTeamBuilderCore.Entities
{
    public class Build
    {
        public Build()
        {
            Weapons = new List<BuildWeapon>();
            Upgrades = new List<BuildUpgrade>();
            Perks = new List<Perk>();
        }

        public int Key { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
        public string Notes { get; set; }
        public int FinalCanCost
        {
            get
            {
                return ((Vehicle != null ? Vehicle.CanCost : 0)
                    + (Weapons.Count > 0 ? Weapons.Where(w => w.Facing != "360").Select(w => w.CanCost).Sum()
                    + (Weapons.Where(w => w.Facing == "360").Select(w => w.CanCost).Sum() * 3) : 0)
                    + (Upgrades.Count > 0 ? Upgrades.Select(u => u.CanCost).Sum() : 0)
                    + (Perks.Count > 0 ? Perks.Select(u => u.CanCost).Sum() : 0))
                    - DiscountCheck();
            }
        }
        public int FinalHull
        {
            get
            {
                return ((Vehicle != null ? Vehicle.Hull : 0)
                    + (Upgrades.Count > 0 ? Upgrades.Select(u => u.HullEffect).Sum() : 0));
            }
        }
        public int FinalHandling
        {
            get
            {
                return ((Vehicle != null ? Vehicle.Handling : 0)
                    + (Upgrades.Count > 0 ? Upgrades.Select(u => u.HandlingEffect).Sum() : 0));
            }
        }
        public int FinalMaxGear
        {
            get
            {
                return ((Vehicle != null ? Vehicle.MaxGear : 0)
                    + (Upgrades.Count > 0 ? Upgrades.Select(u => u.MaxGearEffect).Sum() : 0));
            }
        }
        public int FinalCrew
        {
            get
            {
                return ((Vehicle != null ? Vehicle.Crew : 0)
                    + (Upgrades.Count > 0 ? Upgrades.Select(u => u.CrewEffect).Sum() : 0));
            }
        }
        public int FinalBuildSlots
        {
            get
            {
                return ((Vehicle != null ? Vehicle.BuildSlots : 0)
                    - (Weapons.Count > 0 ? Weapons.Select(w => w.BuildSlotCost).Sum() : 0)
                    - (Upgrades.Count > 0 ? Upgrades.Select(u => u.BuildSlotCost).Sum() : 0));
            }
        }
        public string FinalWeightClass
        {
            get
            {
                return (Vehicle != null ? Vehicle.WeightClass : "");
            }
        }

        public string ListDisplay
        {
            get
            {
                return Name + " - " + (Sponsor != null ? Sponsor.Name : "") + " (" + FinalCanCost + ")";
            }
        }

        public Sponsor Sponsor { get; set; }
        public Vehicle Vehicle { get; set; }
        public List<BuildWeapon> Weapons { get; set; }
        public List<BuildUpgrade> Upgrades { get; set; }
        public List<Perk> Perks { get; set; }

        private int DiscountCheck()
        {
            int totalDiscount = 0;

            //Idris Nitro Discount
            if(Sponsor != null && Sponsor.Name == "Idris")
            {
                totalDiscount += (int)(Upgrades.Where(u => u.Name == "Nitro Booster").Sum(u => u.CanCost) * 0.5);
            }

            return totalDiscount;
        }
    }
}
