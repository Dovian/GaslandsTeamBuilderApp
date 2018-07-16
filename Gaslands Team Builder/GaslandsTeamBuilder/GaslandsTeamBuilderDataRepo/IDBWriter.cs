namespace GaslandsTeamBuilderDataRepo
{
    public interface IDBWriter
    {
        int CreateUser(string username, string password);
        int CreateTeam(int userId);
        int CreateBuild(int userId, Build build = null);
        Team UpdateTeam(Team team, int userId);
        Build UpdateBuild(Build build, int userId, bool updatePerks = false);
        Build AddUpgradeToBuild(int buildKey, int upgradeKey, int userId);
        Build RemoveUpgradeFromBuild(int buildKey, int buildUpgradeKey, int userId);
        Build AddWeaponToBuild(int buildKey, int weaponKey, string facing, int userId);
        Build RemoveWeaponFromBuild(int buildKey, int buildWeaponKey, int userId);
        Team AddBuildToTeam(int teamKey, int buildKey, int userId);
        Team RemoveBuildFromTeam(int teamKey, int teamBuildKey, int userId);
        int DeleteTeam(int teamKey, int userId);
        int DeleteBuild(int buildKey, int userId);
    }
}
