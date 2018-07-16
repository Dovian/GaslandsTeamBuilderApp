using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsTeamBuilderDataRepo;
using GaslandsTeamBuilderCore;
using GaslandsTeamBuilderCore.Entities;
using System.Web;

namespace GaslandsTeamBuilderCore
{
    public class CoreLogic : ICoreLogic
    {
        IDBReader _dBReader;
        IDBWriter _dBWriter;
        Converter _converter;

        public CoreLogic(IDBReader dBReader, IDBWriter dBWriter)
        {
            _converter = new Converter();
            _dBReader = dBReader;
            _dBWriter = dBWriter;
        }

        public int CreateUser(string username, string password)
        {
            return _dBWriter.CreateUser(username, password);
        }

        public string Login(string username, string password)
        {
            return _dBReader.Login(username, password);
        }

        public int CopyBuild(Entities.Build build, int userId)
        {
            return _dBWriter.CreateBuild(userId, _converter.ConvertToDBBuild(build));
        }

        public Entities.Build GetBuild(int key, int userId)
        {
            return _converter.ConvertToCoreBuild(_dBReader.GetBuild(key, userId));
        }

        public List<Entities.Build> GetAllBuilds(int userId)
        {
            return _dBReader.GetAllBuilds(userId)
                .Select(b => _converter.ConvertToCoreBuild(b)).ToList();
        }

        public Entities.Team GetTeam(int key, int userId)
        {
            return _converter.ConvertToCoreTeam(_dBReader.GetTeam(key, userId));
        }

        public List<Entities.Team> GetAllTeams(int userId)
        {
            return _dBReader.GetAllTeams(userId)
                .Select(t => _converter.ConvertToCoreTeam(t)).ToList();
        }

        public int RemoveBuild(int buildKey, int userId)
        {
            return _dBWriter.DeleteBuild(buildKey, userId);
        }

        public int RemoveTeam(int teamKey, int userId)
        {
            return _dBWriter.DeleteTeam(teamKey, userId);
        }

        public DropDownContainer GetDropDowns(Entities.Build build)
        {
            var container = new DropDownContainer();

            container.PerkDropDown = build.Sponsor != null ? _converter.ConvertToCorePerks(_dBReader.GetPerks(build.Sponsor.Name)) : new List<Entities.Perk>();
            container.SponsorDropDown = _converter.ConvertToCoreSponsors(_dBReader.GetSponsors());

            container.UpgradeDropDown = _converter.ConvertToCoreUpgrades(_dBReader.GetUpgrades());
            if (build.Sponsor == null || build.Sponsor.Name != "Warden Cadeila" || build.FinalWeightClass != "Middleweight")
                container.UpgradeDropDown = container.UpgradeDropDown.Where(u => !u.Name.Contains("Prison")).ToList();

            container.VehicleDropDown = _converter.ConvertToCoreVehicles(_dBReader.GetVehicles());
            if (build.Sponsor == null || build.Sponsor.Name != "Rutherford")
                container.VehicleDropDown = container.VehicleDropDown.Where(v => string.IsNullOrEmpty(v.SpecialRules) || !v.SpecialRules.Contains("Military")).ToList();

            container.WeaponDropDown = _converter.ConvertToCoreWeapons(_dBReader.GetWeapons());
            if (build.Sponsor == null || build.Sponsor.Name != "Mishkin")
                container.WeaponDropDown = container.WeaponDropDown.Where(w => string.IsNullOrEmpty(w.SpecialRules) || !w.SpecialRules.Contains("Electrical")).ToList();

            return container;
        }

        public List<Entities.SpecialRule> GetSpecialRules(Entities.Build build)
        {
            var rules = new List<Entities.SpecialRule>();

            if (build.Weapons != null)
            {
                var weaponRuleNames = build.Weapons.SelectMany(w => w.SpecialRules.Split(','));
                foreach (string rule in weaponRuleNames)
                {
                    var result = _dBReader.GetSpecialRule(rule);
                    if (result.Item1 != "" && !rules.Select(r => r.Name).Contains(result.Item1))
                    {
                        rules.Add(new Entities.SpecialRule()
                        {
                            Category = "Weapon",
                            Description = HttpUtility.HtmlDecode(result.Item2),
                            Name = result.Item1
                        });
                    }
                }
            }

            if (build.Vehicle != null && build.Vehicle.SpecialRules != null)
            {
                var vehicleRuleNames = build.Vehicle.SpecialRules.Split(',');
                foreach (string rule in vehicleRuleNames)
                {
                    var result = _dBReader.GetSpecialRule(rule);
                    if (result.Item1 != "" && !rules.Select(r => r.Name).Contains(result.Item1))
                    {
                        rules.Add(new Entities.SpecialRule()
                        {
                            Category = "Vehicle",
                            Description = HttpUtility.HtmlDecode(result.Item2),
                            Name = result.Item1
                        });
                    }
                }
            }

            return rules;
        }

        public int CreateBuild(int userId)
        {
            return _dBWriter.CreateBuild(userId);
        }

        public Entities.Build UpdateBuild(Entities.Build build, int userId)
        {
            return _converter.ConvertToCoreBuild(_dBWriter.UpdateBuild(_converter.ConvertToDBBuild(build), userId));
        }

        public int CreateTeam(int userId)
        {
            return _dBWriter.CreateTeam(userId);
        }

        public Entities.Team UpdateTeam(Entities.Team team, int userId)
        {
            return _converter.ConvertToCoreTeam(_dBWriter.UpdateTeam(_converter.ConvertToDBTeam(team), userId));
        }

        public List<string> ValidateBuild(Entities.Build build)
        {
            throw new NotImplementedException();
        }

        public List<string> ValidateTeam(Entities.Team team)
        {
            throw new NotImplementedException();
        }

        public Entities.Build AddPerk(int buildKey, int perkKey, int userId)
        {
            var build = _dBReader.GetBuild(buildKey, userId);

            if (build.Perks.Where(p => p.Key == perkKey).Count() == 0)
            {
                build.Perks.Add(new GaslandsTeamBuilderDataRepo.Perk()
                {
                    Key = perkKey,
                });
                build = _dBWriter.UpdateBuild(build, userId, true);
            }

            return _converter.ConvertToCoreBuild(build);

        }

        public Entities.Build AddUpgrade(int buildKey, int upgradeKey, int userId)
        {
            var build = _dBWriter.AddUpgradeToBuild(buildKey, upgradeKey, userId);

            return _converter.ConvertToCoreBuild(build);
        }

        public Entities.Build AddWeapon(int buildKey, int weaponKey, string facing, int userId)
        {
            var build = _dBWriter.AddWeaponToBuild(buildKey, weaponKey, facing, userId);

            return _converter.ConvertToCoreBuild(build);
        }

        public Entities.Build RemovePerk(int buildKey, int perkKey, int userId)
        {
            var build = _dBReader.GetBuild(buildKey, userId);

            if (build.Perks.Where(p => p.Key == perkKey).Count() > 0)
            {
                build.Perks.Remove(build.Perks.Single(p => p.Key == perkKey));
                build = _dBWriter.UpdateBuild(build, userId, true);
            }

            return _converter.ConvertToCoreBuild(build);
        }

        public Entities.Build RemoveUpgrade(int buildKey, int buildUpgradeKey, int userId)
        {
            var build = _dBWriter.RemoveUpgradeFromBuild(buildKey, buildUpgradeKey, userId);

            return _converter.ConvertToCoreBuild(build);
        }

        public Entities.Build RemoveWeapon(int buildKey, int buildWeaponKey, int userId)
        {
            var build = _dBWriter.RemoveWeaponFromBuild(buildKey, buildWeaponKey, userId);

            return _converter.ConvertToCoreBuild(build);
        }

        public Entities.Team AddBuildToTeam(int teamKey, int buildKey, int userId)
        {
            var team = _dBWriter.AddBuildToTeam(teamKey, buildKey, userId);

            return _converter.ConvertToCoreTeam(team);
        }

        public Entities.Team RemoveBuildFromTeam(int teamKey, int teamBuildKey, int userId)
        {
            var team = _dBWriter.RemoveBuildFromTeam(teamKey, teamBuildKey, userId);

            return _converter.ConvertToCoreTeam(team);
        }
    }
}
