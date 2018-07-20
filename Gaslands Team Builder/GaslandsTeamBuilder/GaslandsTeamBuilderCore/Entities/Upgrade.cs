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
        public string SpecialText { get; set; }

        public string EffectDisplay
        {
            get
            {
                string effect = "";
                if (HullEffect != 0) { effect += (" Hull: " + (HullEffect > 0 ? "+" + HullEffect.ToString() : HullEffect.ToString())); }
                if (HandlingEffect != 0) { effect += (" Handling: " + (HandlingEffect > 0 ? "+" + HandlingEffect.ToString() : HandlingEffect.ToString())); }
                if (MaxGearEffect != 0) { effect += (" Max Gear: " + (MaxGearEffect > 0 ? "+" + MaxGearEffect.ToString() : MaxGearEffect.ToString())); }
                if (CrewEffect != 0) { effect += (" Crew: " + (CrewEffect > 0 ? "+" + CrewEffect.ToString() : CrewEffect.ToString())); }
                if (!string.IsNullOrEmpty(SpecialText)) { effect += " " + SpecialText; }

                return effect;
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
