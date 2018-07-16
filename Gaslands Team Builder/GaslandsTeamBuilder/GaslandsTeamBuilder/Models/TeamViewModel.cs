using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GaslandsTeamBuilder.Models
{
    public class TeamViewModel
    {
        public TeamViewModel() { }
        public TeamViewModel(Team _team, List<Build> buildList, List<string> errors)
        {
            team = _team;
            BuildList = new SelectList(buildList, "Key", "ListDisplay");
            TeamErrors = errors;
        }

        public Team team { get; set; }
        public List<string> TeamErrors { get; set; }
        public SelectList BuildList { get; set; }
    }
}