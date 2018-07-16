using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;

namespace GaslandsTeamBuilder.Models
{
    public class GarageViewModel
    {
        public List<Team> Teams { get; set; }
        public List<Build> Builds { get; set; }
    }
}