namespace GaslandsTeamBuilderCore.Entities
{
    public class Perk
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public int CanCost { get; set; }
        public string Description { get; set; }

        public string ListDisplay
        {
            get
            {
                return Name + " (" + CanCost.ToString() + ")";
            }
        }
    }
}
