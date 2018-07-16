using System.Collections.Generic;
using System.Linq;

namespace GaslandsTeamBuilderCore.Entities
{
    public class Team
    {
        public Team()
        {
            TeamBuilds = new List<TeamBuild>();
        }

        public int Key { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Sponsor
        {
            get
            {
                switch (TeamBuilds.Count > 0 ? TeamBuilds.Select(b => b.Sponsor != null ? b.Sponsor.Name : "").Distinct().Count() : 0) {
                    case 0:
                        return "No Sponsor";
                    case 1:
                        return TeamBuilds.First().Sponsor != null ? TeamBuilds.First().Sponsor.Name : "No Sponsor";
                    default:
                        return "Unmatched";
                }
            }
        }
        public int TotalCost
        {
            get
            {
                return (TeamBuilds.Count > 0 ? TeamBuilds.Sum(b => b.FinalCanCost) : 0);
            }
        }

        public List<TeamBuild> TeamBuilds { get; set; }
    }
}
