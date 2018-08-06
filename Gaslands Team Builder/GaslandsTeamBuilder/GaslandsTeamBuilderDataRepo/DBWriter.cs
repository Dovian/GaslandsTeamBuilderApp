using System;
using System.Linq;

namespace GaslandsTeamBuilderDataRepo
{
    public class DBWriter : IDBWriter
    {
        GaslandsAppEntities _db = new GaslandsAppEntities();
        
        public int CreateUser(string username, string password)
        {
            if((_db.Users.SingleOrDefault(u => u.Username.ToLower() == username.ToLower())) != null)
            {
                return -1;
            }

            User newUser = new User()
            {
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return newUser.Key;
        }

        public int CreateBuild(int userId, Build build = null)
        {
            Build newBuild = new Build();

            if (build != null)
            {
                newBuild = build;
            }

            newBuild.User = userId;
            _db.Builds.Add(newBuild);
            _db.SaveChanges();

            return newBuild.Key;
        }

        public int CreateTeam(int userId)
        {
            Team team = new Team();
            team.User = userId;
            _db.Teams.Add(team);
            _db.SaveChanges();

            return team.Key;
        }

        public int DeleteBuild(int buildKey, int userId)
        {
            if (_db.Builds.SingleOrDefault(b => b.Key == buildKey && b.User == userId) != null)
            {
                try
                {
                    _db.TeamBuilds.RemoveRange(_db.TeamBuilds.Where(tb => tb.BuildKey == buildKey));
                    _db.BuildUpgrades.RemoveRange(_db.BuildUpgrades.Where(bu => bu.BuildKey == buildKey));
                    _db.BuildWeapons.RemoveRange(_db.BuildWeapons.Where(bw => bw.BuildKey == buildKey));
                    _db.Builds.Single(b => b.Key == buildKey).Perks.Clear();
                    _db.Builds.Remove(_db.Builds.Single(b => b.Key == buildKey));
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }

                return 1;
            }

            return 0;
        }

        public int DeleteTeam(int teamKey, int userId)
        {
            if (_db.Teams.SingleOrDefault(t => t.Key == teamKey && t.User == userId) != null)
            {
                try
                {
                    _db.TeamBuilds.RemoveRange(_db.TeamBuilds.Where(tb => tb.TeamKey == teamKey));
                    _db.Teams.Remove(_db.Teams.Single(t => t.Key == teamKey));
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return 0;
                }

                return 1;
            }

            return 0;
        }

        public Build AddUpgradeToBuild(int buildKey, int upgradeKey, int userId)
        {
            var dbBuild = _db.Builds.Include("BuildUpgrades").SingleOrDefault(b => b.Key == buildKey && b.User == userId);
            if (dbBuild != null)
            {
                var newUpgrade = _db.Upgrades.SingleOrDefault(w => w.Key == upgradeKey);
                dbBuild.BuildUpgrades.Add(new BuildUpgrade()
                {
                    Build = dbBuild,
                    BuildKey = dbBuild.Key,
                    Upgrade = newUpgrade,
                    UpgradeKey = newUpgrade.Key,
                });

                _db.SaveChanges();
            }

            return dbBuild;
        }

        public Build RemoveUpgradeFromBuild(int buildKey, int buildUpgradeKey, int userId)
        {
            var build = _db.Builds.SingleOrDefault(b => b.Key == buildKey && b.User == userId);

            if (build != null)
            {
                _db.BuildUpgrades.Remove(_db.BuildUpgrades.Single(bu => bu.Key == buildUpgradeKey));
                _db.SaveChanges();
            }
            
            return build;
        }

        public Build AddWeaponToBuild(int buildKey, int weaponKey, string facing, int userId)
        {
            var dbBuild = _db.Builds.Include("BuildWeapons").SingleOrDefault(b => b.Key == buildKey && b.User == userId);
            if (dbBuild != null)
            {
                var newWeapon = _db.Weapons.SingleOrDefault(w => w.Key == weaponKey);
                dbBuild.BuildWeapons.Add(new BuildWeapon()
                {
                    Build = dbBuild,
                    BuildKey = dbBuild.Key,
                    Weapon = newWeapon,
                    WeaponKey = newWeapon.Key,
                    Facing = facing
                });

                _db.SaveChanges();
            }

            return dbBuild;
        }

        public Build RemoveWeaponFromBuild(int buildKey, int buildWeaponKey, int userId)
        {
            var build = _db.Builds.SingleOrDefault(b => b.Key == buildKey && b.User == userId);

            if (build != null)
            {
                _db.BuildWeapons.Remove(_db.BuildWeapons.Single(bw => bw.Key == buildWeaponKey));
                _db.SaveChanges();
            }

            return build;
        }

        public Build UpdateBuild(Build updatedBuild, int userId, bool updatePerks = false)
        {
            var dbBuild = _db.Builds.Include("Perks").SingleOrDefault(b => b.Key == updatedBuild.Key && b.User == userId);
            if (dbBuild != null)
            {
                var oldSponsor = dbBuild.Sponsor;
                var newSponsor = updatedBuild.Sponsor;

                dbBuild.Driver = updatedBuild.Driver;
                dbBuild.Name = updatedBuild.Name;
                dbBuild.Notes = updatedBuild.Notes;
                dbBuild.Sponsor = updatedBuild.Sponsor == 0 ? null : updatedBuild.Sponsor;
                dbBuild.Vehicle = updatedBuild.Vehicle == 0 ? null : updatedBuild.Vehicle;

                //Perks
                if (updatePerks)
                {
                    var perkKeyList = updatedBuild.Perks.Select(p => p.Key).ToList();
                    var newPerks = _db.Perks
                        .Where(p => perkKeyList.Contains(p.Key))
                        .ToList();
                    dbBuild.Perks.Clear();
                    foreach (Perk newPerk in newPerks)
                        dbBuild.Perks.Add(newPerk);
                }
                //Update Sponsor Perks
                if(oldSponsor != newSponsor)
                {
                    var moddedPerkKeyList = dbBuild.Perks.Where(p => p.Sponsor != oldSponsor).ToList();
                    dbBuild.Perks.Clear();

                    moddedPerkKeyList.AddRange(_db.Perks.Where(p => p.Sponsor == newSponsor));

                    foreach(Perk newPerk in moddedPerkKeyList.ToList())
                    {
                        dbBuild.Perks.Add(newPerk);
                    }
                }

                _db.SaveChanges();

                return dbBuild;
            }
            else
            {
                return null;
            }
        }

        public Team AddBuildToTeam(int teamkey, int buildKey, int userId)
        {
            var dbTeam = _db.Teams.Include("TeamBuilds").SingleOrDefault(t => t.Key == teamkey && t.User == userId);
            if (dbTeam != null)
            {
                var newBuild = _db.Builds.SingleOrDefault(b => b.Key == buildKey && b.User == userId);
                dbTeam.TeamBuilds.Add(new TeamBuild()
                {
                    Team = dbTeam,
                    TeamKey = dbTeam.Key,
                    Build = newBuild,
                    BuildKey = newBuild.Key
                });

                _db.SaveChanges();
            }

            return dbTeam;
        }

        public Team RemoveBuildFromTeam(int teamKey, int teamBuildkey, int userId)
        {
            var team = _db.Teams.SingleOrDefault(t => t.Key == teamKey && t.User == userId);

            if (team != null)
            {
                _db.TeamBuilds.Remove(_db.TeamBuilds.Single(tb => tb.Key == teamBuildkey));
                _db.SaveChanges();
            }

            return team;
        }

        public Team UpdateTeam(Team team, int userId)
        {
            var dbTeam = _db.Teams.SingleOrDefault(t => t.Key == team.Key && t.User == userId);
            if (dbTeam != null)
            {
                var updatedTeam = team;

                dbTeam.Name = updatedTeam.Name;
                dbTeam.Notes = updatedTeam.Notes;
                _db.SaveChanges();

                return dbTeam;
            }
            else
            {
                return null;
            }
        }
    }
}
