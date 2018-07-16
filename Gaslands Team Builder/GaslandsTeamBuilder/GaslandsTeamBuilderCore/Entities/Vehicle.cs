namespace GaslandsTeamBuilderCore.Entities
{
    public class Vehicle
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public string WeightClass { get; set; }
        public int Hull { get; set; }
        public int Handling { get; set; }
        public int MaxGear { get; set; }
        public int Crew { get; set; }
        public int BuildSlots { get; set; }
        public int CanCost { get; set; }
        public string SpecialRules { get; set; }

        public string ListDisplay
        {
            get
            {
                return Name + " (" + CanCost.ToString() + ")";
            }
        }
    }
}
