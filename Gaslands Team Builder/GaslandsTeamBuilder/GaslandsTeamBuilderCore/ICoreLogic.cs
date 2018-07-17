using GaslandsTeamBuilderCore.Entities;
using System.Collections.Generic;

namespace GaslandsTeamBuilderCore
{
    public interface ICoreLogic
    {
        int CreateUser(string username, string password);
        int Login(string username, string password);
        int CopyBuild(Build build, int userId);
        int CreateBuild(int userId);
        int CreateTeam(int userId);
        Build UpdateBuild(Build build, int userId);
        Team UpdateTeam(Team team, int userId);
        Build GetBuild(int key, int userId);
        Team GetTeam(int key, int userId);
        int RemoveBuild(int key, int userId);
        int RemoveTeam(int key, int userId);
        Build AddPerk(int buildKey, int perkKey, int userId);
        Build AddUpgrade(int buildKey, int upgradeKey, int userId);
        Build AddWeapon(int buildKey, int weaponKey, string facing, int userId);
        Build RemovePerk(int buildKey, int perkKey, int userId);
        Build RemoveUpgrade(int buildKey, int buildUpgradeKey, int userId);
        Build RemoveWeapon(int buildKey, int buildWeaponKey, int userId);
        Team AddBuildToTeam(int teamKey, int buildKey, int userId);
        Team RemoveBuildFromTeam(int teamKey, int teamBuildKey, int userId);
        DropDownContainer GetDropDowns(Build build);
        List<SpecialRule> GetSpecialRules(Build build);
        List<Build> GetAllBuilds(int userId);
        List<Team> GetAllTeams(int userId);
        List<string> ValidateBuild(Build build);
        List<string> ValidateTeam(Team team);
    }
}
