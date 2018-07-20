using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Models
{
    public class TeamViewModel
    {
        public TeamViewModel() { TeamErrors = new List<string>(); }
        public TeamViewModel(Team _team, List<Build> buildList, List<string> errors = null)
        {
            team = _team;
            BuildList = new SelectList(buildList, "Key", "ListDisplay");
            TeamErrors = errors;
            if (TeamErrors == null)
            {
                TeamErrors = new List<string>();
            }
        }

        public Team team { get; set; }
        public List<string> TeamErrors { get; set; }
        public SelectList BuildList { get; set; }
    }
}