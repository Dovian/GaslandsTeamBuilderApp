using System;
using System.Collections.Generic;

namespace GaslandsTeamBuilderDataRepo
{
    public interface IDBReader
    {
        int Login(string username, string password);
        Build GetBuild(int key, int userId);
        Team GetTeam(int key, int userId);
        List<Build> GetAllBuilds(int userId);
        List<Team> GetAllTeams(int userId);
        List<Vehicle> GetVehicles();
        List<Weapon> GetWeapons();
        Weapon GetWeapon(int key);
        List<Perk> GetPerks(string sponsor);
        Perk GetPerk(int key);
        List<Upgrade> GetUpgrades();
        Upgrade GetUpgrade(int key);
        Tuple<string, string> GetSpecialRule(string name);
        List<Sponsor> GetSponsors();
    }
}
