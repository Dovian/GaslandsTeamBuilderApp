using System;
using System.Collections.Generic;
using System.Linq;
using GaslandsTeamBuilderDataRepo;
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

        public int Login(string username, string password)
        {
            return _dBReader.Login(username, password);
        }

        public int CopyBuild(int buildKey, int userId)
        {
            return _dBWriter.CopyBuild(buildKey, userId);
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
            var hasSponsor = build.Sponsor != null;
            var hasVehicle = build.Vehicle != null;

            container.PerkDropDown = build.Sponsor != null ? _converter.ConvertToCorePerks(_dBReader.GetPerks(build.Sponsor.Name)) : new List<Entities.Perk>();
            container.SponsorDropDown = _converter.ConvertToCoreSponsors(_dBReader.GetSponsors());

            container.UpgradeDropDown = _converter.ConvertToCoreUpgrades(_dBReader.GetUpgrades());
            container.VehicleDropDown = _converter.ConvertToCoreVehicles(_dBReader.GetVehicles());
            container.WeaponDropDown = _converter.ConvertToCoreWeapons(_dBReader.GetWeapons());

            if (!hasSponsor || build.Sponsor.Name != "Warden Cadeila" || build.FinalWeightClass != "Middleweight")
                container.UpgradeDropDown = container.UpgradeDropDown.Where(u => !u.Name.Contains("Prison")).ToList();

            if (!hasSponsor || build.Sponsor.Name != "Rutherford")
                container.VehicleDropDown = container.VehicleDropDown.Where(v => string.IsNullOrEmpty(v.SpecialRules) || !v.SpecialRules.Contains("Military")).ToList();

            if (!hasSponsor || build.Sponsor.Name != "Mishkin")
                container.WeaponDropDown = container.WeaponDropDown.Where(w => string.IsNullOrEmpty(w.SpecialRules) || !w.SpecialRules.Contains("Electrical")).ToList();

            if (hasSponsor)
            {
                if (build.Sponsor.Name == "Rutherford")
                    container.VehicleDropDown = container.VehicleDropDown.Where(v => v.WeightClass != "Lightweight").ToList();

                if (build.Sponsor.Name == "Miyazaki")
                {
                    var limit = new string[] { "Truck", "Bus", "War Rig" };
                    container.VehicleDropDown = container.VehicleDropDown.Where(v => !limit.Contains(v.Name)).ToList();
                }

                if (build.Sponsor.Name == "Idris")
                    container.VehicleDropDown = container.VehicleDropDown.Where(v => v.Name != "Gyrocopter").ToList();

                if (hasVehicle)
                {
                    if (container.PerkDropDown.Where(p => p.Name == "Stunt Driver" || p.Name == "Skiing").Count() > 0 && !new string[] { "Bike", "Buggy", "Car", "Performance Car" }.Contains(build.Vehicle.Name))
                        container.PerkDropDown = container.PerkDropDown.Where(p => p.Name != "Stunt Driver" && p.Name != "Skiing").ToList();

                    if (container.PerkDropDown.Where(p => p.Name == "Experimental Nuclear Engine").Count() > 0 && build.Vehicle.WeightClass == "Lightweight")
                        container.PerkDropDown = container.PerkDropDown.Where(p => p.Name != "Experimental Nuclear Engine").ToList();
                }
            }

            if (hasVehicle)
            {
                if (new string[] { "Tank", "Helicopter", "Gyrocopter" }.Contains(build.Vehicle.Name))
                    container.UpgradeDropDown = container.UpgradeDropDown.Where(u => u.Name != "Tank Tracks").ToList();

                if (build.Vehicle.Name == "Bike")
                    container.WeaponDropDown = container.WeaponDropDown.Where(w => w.Name != "Exploding Ram").ToList();

            }

            return container;
        }

        public List<string> ValidateBuild(Entities.Build build, int userId)
        {
            List<string> errors = new List<string>();
            var dbBuild = _dBReader.GetBuild(build.Key, userId);
            var buildSponsor = dbBuild.Sponsor.HasValue ? dbBuild.Sponsor1.Name : null;
            var hasVehicle = dbBuild.Vehicle.HasValue;
            var hasWeapon = dbBuild.BuildWeapons.Count > 0;
            var hasUpgrades = dbBuild.BuildUpgrades.Count > 0;

            //Check Perks against Sponsor
            if (buildSponsor != null)
            {
                foreach (Perk perk in dbBuild.Perks)
                {
                    if (perk.PerkClass != null && !dbBuild.Sponsor1.PerkClasses.Contains(perk.PerkClass))
                    {
                        errors.Add(perk.Name + ": " + dbBuild.Sponsor1.Name + " does not support this perk.");
                    }
                }
            }

            if (hasVehicle)
            {
                if (buildSponsor == "Rutherford" && dbBuild.Vehicle1.WeightClass == "Lightweight")
                {
                    errors.Add("Rutherford cannot purchase lightweight vehicle: " + dbBuild.Vehicle1.Name);
                }
                else if (buildSponsor == "Miyazaki")
                {
                    var limit = new string[] { "Truck", "Bus", "War Rig" };
                    if (limit.Contains(dbBuild.Vehicle1.Name))
                    {
                        errors.Add("Miyazaki cannot purchase: " + dbBuild.Vehicle1.Name);
                    }
                }
                else if (buildSponsor == "Idris" && dbBuild.Vehicle1.Name == "Gyrocopter")
                {
                    errors.Add("Idris cannot purchase a Gyrocopter");
                }

                if (buildSponsor != "Rutherford" && !string.IsNullOrEmpty(dbBuild.Vehicle1.SpecialRules) && dbBuild.Vehicle1.SpecialRules.Contains("Military"))
                {
                    errors.Add("Only Rutherford may purchase Military vehicle: " + dbBuild.Vehicle1.Name);
                }
                if(dbBuild.Perks.SingleOrDefault(p => p.Name == "Skiing") != null)
                {
                    var limit = new string[] { "Bike", "Buggy", "Car", "Performance Car" };
                    if (!limit.Contains(dbBuild.Vehicle1.Name))
                    {
                        errors.Add("Skiing cannot be taken on a " + dbBuild.Vehicle1.Name);
                    }
                }
                if (dbBuild.Perks.SingleOrDefault(p => p.Name == "Stunt Driver") != null)
                {
                    var limit = new string[] { "Bike", "Buggy", "Car", "Performance Car" };
                    if (!limit.Contains(dbBuild.Vehicle1.Name))
                    {
                        errors.Add("Stunt Driver cannot be taken on a " + dbBuild.Vehicle1.Name);
                    }
                }
                if (dbBuild.Perks.SingleOrDefault(p => p.Name == "Experimental Nuclear Engine") != null && dbBuild.Vehicle1.WeightClass == "Lightweight")
                {
                    errors.Add("Experimental Nuclear Engine cannot be taken on a Lightweight vehicle.");
                }
            }
            if (hasWeapon)
            {
                if(dbBuild.BuildWeapons.Select(bw => bw.Weapon).Where(w => !string.IsNullOrEmpty(w.SpecialRules) && w.SpecialRules.Contains("Specialist")).Count() > 1)
                {
                    errors.Add("A vehicle may only ever purchase one weapon with the 'Specialist' special rule.");
                }
                if (dbBuild.BuildWeapons.Select(bw => bw.Weapon).Where(w => !string.IsNullOrEmpty(w.SpecialRules) && w.SpecialRules.Contains("Electrical")).Count() > 0
                    && buildSponsor != "Mishkin")
                {
                    errors.Add("Only Mishkin may purchase weapons with the \"Electrical\" Special Rule");
                }
                if (dbBuild.BuildWeapons.Where(bw => bw.Weapon.Name == "Ram").Count() > 1)
                {
                    int[] facing = new int[] { 0, 0, 0 };
                    foreach (BuildWeapon ram in dbBuild.BuildWeapons.Where(bw => bw.Weapon.Name == "Ram"))
                    {
                        switch (ram.Facing)
                        {
                            case "Front":
                                facing[0]++;
                                break;
                            case "Side":
                                facing[1]++;
                                break;
                            case "Rear":
                                facing[2]++;
                                break;
                            default:
                                break;
                        }
                        if (facing[0] > 1 || facing[1] > 1 || facing[2] > 1)
                            errors.Add("A vehicle may only purchase a \"Ram\" once per facing.");
                    }
                }
                if (dbBuild.BuildWeapons.Where(bw => bw.Weapon.Name == "Exploding Ram").Count() > 0)
                {
                    if (dbBuild.BuildWeapons.Where(bw => bw.Weapon.Name == "Exploding Ram").Count() > 1)
                        errors.Add("A vehicle may only purchase one \"Exploding Ram\"");

                    if (hasVehicle && dbBuild.Vehicle1.Name == "Bike")
                        errors.Add("Bikes may not purchase an \"Exploding Ram\"");
                }
            }

            if (hasUpgrades)
            {
                if (hasVehicle)
                {
                    if (dbBuild.BuildUpgrades.Select(bu => bu.Upgrade).Where(u => u.Name == "Prison Car").Count() > 0
                        && (buildSponsor != "Warden Cadeila" || dbBuild.Vehicle1.WeightClass != "Middleweight"))
                    {
                        errors.Add("Only Warden Cadeila may purchase the \"Prison Car\" Upgrade and only on Middleweight vehicles");
                    }
                    if (dbBuild.BuildUpgrades.Select(bu => bu.Upgrade).Where(u => u.Name == "Extra Crewmember").Count() > dbBuild.Vehicle1.Crew)
                    {
                        errors.Add("You may not have more than double a vehicle's crew with the \"Extra Crewmember\" upgrade");
                    }
                    if (dbBuild.BuildUpgrades.Select(bu => bu.Upgrade).Where(u => u.Name == "Tank Tracks").Count() > 0)
                    {
                        var limit = new string[] { "Tank", "Helicopter", "Gyrocopter" };
                        if (limit.Contains(dbBuild.Vehicle1.Name))
                        {
                            errors.Add("You may not purchase \"Tank Tracks\" on a " + dbBuild.Vehicle1.Name);
                        }
                    }
                }
                if (dbBuild.BuildUpgrades.Select(bu => bu.Upgrade).Where(u => u.Name == "Tank Tracks").Count() > 1)
                {
                    errors.Add("You may not have more than one \"Tank Tracks\" upgrade");
                }
            }
            //'Duh' checks
            if(hasVehicle)
            {
                int buildSlots = 0;

                if (hasWeapon)
                    buildSlots += (int)dbBuild.BuildWeapons.Sum(bw => bw.Weapon.BuildSlotCost);

                if (hasUpgrades)
                    buildSlots += (int)dbBuild.BuildUpgrades.Sum(bu => bu.Upgrade.BuildSlotCost);

                if (dbBuild.Vehicle1.BuildSlots - buildSlots < 0)
                    errors.Add("Build slots cannot be negative.");
            }

            return errors;
        }

        public List<string> ValidateTeam(Entities.Team team, int userId)
        {
            List<string> errors = new List<string>();
            //Have builds individually run ValidateBuild() and then highlight if any issues exist
            foreach(Entities.TeamBuild _build in team.TeamBuilds)
            {
                var errorsToAdd = ValidateBuild(_build, userId);
                foreach(string error in errorsToAdd)
                {
                    errors.Add(_build.Name + ": " + error);
                }
            }
            //Sponsor should be something besides "No Sponsor" or "Unmatched"
            if(team.Sponsor == "No Sponsor" || team.Sponsor == "Unmatched")
            {
                errors.Add("Team: Your team does not have a unified sponsor");
            }
            //Rutherford needs to check that Tank/Heli is only one-of

            return errors;
        }

        public List<Entities.SpecialRule> GetSpecialRules(Entities.Build build)
        {
            var rules = new List<Entities.SpecialRule>();

            if (build.Weapons != null)
            {
                var weaponRuleNames = build.Weapons.SelectMany(w => string.IsNullOrEmpty(w.SpecialRules) ? new string[] { "" } : w.SpecialRules.Split(','));
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
