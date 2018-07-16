namespace GaslandsTeamBuilderCore.Entities
{
    public class Upgrade
    {
        public int UpgradeKey { get; set; }
        public string Name { get; set; }
        public int CanCost { get; set; }
        public int BuildSlotCost { get; set; }
        public int HullEffect { get; set; }
        public int HandlingEffect { get; set; }
        public int MaxGearEffect { get; set; }
        public int CrewEffect { get; set; }

        public string ListDisplay
        {
            get
            {
                return Name + " (" + CanCost.ToString() + ")";
            }
        }
    }
}
