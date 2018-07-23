using GaslandsTeamBuilderDataRepo;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GaslandsTeamBuilderCore
{
    public class Converter
    {
        public Converter() { }

        public Entities.Build ConvertToCoreBuild(Build build)
        {
            return new Entities.Build()
            {
                Key = build.Key,
                Name = build.Name,
                Driver = build.Driver,
                Notes = build.Notes,
                Sponsor = build.Sponsor1 != null ? new Entities.Sponsor() { Key = build.Sponsor1.Key, Name = build.Sponsor1.Name } : null,
                Vehicle = build.Vehicle1 != null ? ConvertToCoreVehicle(build.Vehicle1) : null,
                Perks = build.Perks != null ? ConvertToCorePerks(build.Perks) : null,
                Upgrades = build.BuildUpgrades != null ? ConvertToCoreBuildUpgrades(build.BuildUpgrades) : null,
                Weapons = build.BuildWeapons != null ? ConvertToCoreBuildWeapons(build.BuildWeapons) : null
            };
        }

        public Entities.Team ConvertToCoreTeam(Team team)
        {
            return new Entities.Team()
            {
                Key = team.Key,
                Name = team.Name,
                Notes = team.Notes,
                TeamBuilds = team.TeamBuilds != null ? team.TeamBuilds.Select(tb => ConvertToCoreTeamBuild(tb)).ToList() : null
            };
        }

        public List<Entities.Perk> ConvertToCorePerks(ICollection<Perk> perks)
        {
            return perks
                .Select(p => new Entities.Perk()
                {
                    Key = p.Key,
                    CanCost = p.CanCost.Value,
                    Description = HttpUtility.HtmlDecode(p.Description),
                    Name = p.Name
                })
                .OrderBy(p => p.CanCost)
                .ToList();
        }

        public List<Entities.Upgrade> ConvertToCoreUpgrades(ICollection<Upgrade> upgrades)
        {
            return upgrades
                .Select(u => new Entities.Upgrade()
                {
                    UpgradeKey = u.Key,
                    Name = u.Name,
                    CanCost = u.CanCost.Value,
                    CrewEffect = u.CrewEffect.Value,
                    BuildSlotCost = u.BuildSlotCost.Value,
                    HandlingEffect = u.HandlingEffect.Value,
                    HullEffect = u.HullEffect.Value,
                    MaxGearEffect = u.MaxGearEffect.Value,
                    SpecialText = u.SpecialText
                })
                .ToList();
        }

        public List<Entities.BuildUpgrade> ConvertToCoreBuildUpgrades(ICollection<BuildUpgrade> buildUpgrades)
        {
            return buildUpgrades
                .Select(u => new Entities.BuildUpgrade()
                {
                    BuildUpgradeKey = u.Key,
                    UpgradeKey = u.UpgradeKey,
                    Name = u.Upgrade.Name,
                    CanCost = u.Upgrade.CanCost.Value,
                    CrewEffect = u.Upgrade.CrewEffect.Value,
                    BuildSlotCost = u.Upgrade.BuildSlotCost.Value,
                    HandlingEffect = u.Upgrade.HandlingEffect.Value,
                    HullEffect = u.Upgrade.HullEffect.Value,
                    MaxGearEffect = u.Upgrade.MaxGearEffect.Value,
                    SpecialText = u.Upgrade.SpecialText
                })
                .ToList();
        }

        public List<Entities.Weapon> ConvertToCoreWeapons(ICollection<Weapon> weapons)
        {
            return weapons
                .Select(w => new Entities.Weapon()
                {
                    WeaponKey = w.Key,
                    Ammo = w.Ammo,
                    BuildSlotCost = w.BuildSlotCost.Value,
                    CanCost = w.CanCost.Value,
                    Name = w.Name,
                    Range = w.Range,
                    SpecialRules = w.SpecialRules,
                    Attack = w.Attack,
                    FacingAllowed = w.FacingAllowed.Split(',').ToList()
                })
                .ToList();
        }

        public List<Entities.BuildWeapon> ConvertToCoreBuildWeapons(ICollection<BuildWeapon> weapons)
        {
            return weapons
                .Select(w => new Entities.BuildWeapon()
                {
                    BuildWeaponKey = w.Key,
                    WeaponKey = w.WeaponKey,
                    Facing = w.Facing,
                    Ammo = w.Weapon.Ammo,
                    BuildSlotCost = w.Weapon.BuildSlotCost.Value,
                    CanCost = w.Weapon.CanCost.Value,
                    Name = w.Weapon.Name,
                    Range = w.Weapon.Range,
                    SpecialRules = w.Weapon.SpecialRules,
                    Attack = w.Weapon.Attack
                })
                .ToList();
        }

        public List<Entities.Vehicle> ConvertToCoreVehicles(List<Vehicle> vehicles)
        {
            return vehicles
                .Select(v => new Entities.Vehicle()
                {
                    Key = v.Key,
                    Name = v.Name,
                    BuildSlots = v.BuildSlots.Value,
                    CanCost = v.CanCost.Value,
                    Crew = v.Crew.Value,
                    Handling = v.Handling.Value,
                    Hull = v.Hull.Value,
                    MaxGear = v.MaxGear.Value,
                    SpecialRules = v.SpecialRules,
                    WeightClass = v.WeightClass
                })
                .ToList();
        }

        public Entities.Vehicle ConvertToCoreVehicle(Vehicle vehicle)
        {
            return new Entities.Vehicle()
            {
                Key = vehicle.Key,
                Name = vehicle.Name,
                BuildSlots = vehicle.BuildSlots.Value,
                CanCost = vehicle.CanCost.Value,
                Crew = vehicle.Crew.Value,
                Handling = vehicle.Handling.Value,
                Hull = vehicle.Hull.Value,
                MaxGear = vehicle.MaxGear.Value,
                SpecialRules = vehicle.SpecialRules,
                WeightClass = vehicle.WeightClass
            };
        }

        public List<Entities.Sponsor> ConvertToCoreSponsors(List<Sponsor> sponsor)
        {
            return sponsor
                .Select(s => new Entities.Sponsor()
            {
                Key = s.Key,
                Name = s.Name
            })
            .ToList();
        }

        public Build ConvertToDBBuild(Entities.Build build)
        {
            return new Build()
            {
                Key = build.Key,
                Name = build.Name,
                Driver = build.Driver,
                Notes = build.Notes,
                Sponsor = build.Sponsor != null ? build.Sponsor.Key : (int?)null,
                Vehicle = build.Vehicle.Key,
                Perks = ConvertToDBPerks(build.Perks),
                BuildUpgrades = ConvertToDBBuildUpgrades(build.Upgrades, build.Key),
                BuildWeapons = ConvertToDBBuildWeapons(build.Weapons, build.Key)
            };
        }

        public Team ConvertToDBTeam(Entities.Team team)
        {
            return new Team()
            {
                Key = team.Key,
                Name = team.Name,
                Notes = team.Notes,
                TeamBuilds = team.TeamBuilds.Select(tb => ConvertToDBTeamBuild(tb, team.Key)).ToList()
            };
        }

        public List<Perk> ConvertToDBPerks(List<Entities.Perk> perks)
        {
            return perks
                .Select(p => new Perk()
                {
                    Key = p.Key,
                    CanCost = p.CanCost,
                    Description = p.Description,
                    Name = p.Name
                })
                .ToList();
        }

        public List<BuildUpgrade> ConvertToDBBuildUpgrades(List<Entities.BuildUpgrade> upgrades, int buildKey)
        {
            return upgrades
                .Select(u => new BuildUpgrade()
                {
                    Key = u.BuildUpgradeKey,
                    UpgradeKey = u.UpgradeKey,
                    BuildKey = buildKey
                })
                .ToList();
        }

        public List<BuildWeapon> ConvertToDBBuildWeapons(List<Entities.BuildWeapon> weapons, int buildKey)
        {
            return weapons
                .Select(w => new BuildWeapon()
                {
                    Key = w.BuildWeaponKey,
                    WeaponKey = w.WeaponKey,
                    Facing = w.Facing,
                    BuildKey = buildKey
                })
                .ToList();
        }

        public TeamBuild ConvertToDBTeamBuild(Entities.TeamBuild build, int teamKey)
        {
            return new TeamBuild()
            {
                Key = build.TeamBuildKey,
                BuildKey = build.Key,
                TeamKey = teamKey
            };
        }
        public Entities.TeamBuild ConvertToCoreTeamBuild(TeamBuild teamBuild)
        {
            return new Entities.TeamBuild()
            {
                TeamBuildKey = teamBuild.Key,
                Key = teamBuild.BuildKey,
                Name = teamBuild.Build.Name,
                Driver = teamBuild.Build.Driver,
                Notes = teamBuild.Build.Notes,
                Sponsor = teamBuild.Build.Sponsor1 != null ? new Entities.Sponsor() { Key = teamBuild.Build.Sponsor1.Key, Name = teamBuild.Build.Sponsor1.Name } : null,
                Vehicle = teamBuild.Build.Vehicle1 != null ? ConvertToCoreVehicle(teamBuild.Build.Vehicle1) : null,
                Perks = teamBuild.Build.Perks != null ? ConvertToCorePerks(teamBuild.Build.Perks) : null,
                Upgrades = teamBuild.Build.BuildUpgrades != null ? ConvertToCoreBuildUpgrades(teamBuild.Build.BuildUpgrades) : null,
                Weapons = teamBuild.Build.BuildWeapons != null ? ConvertToCoreBuildWeapons(teamBuild.Build.BuildWeapons) : null
            };
        }
    }
}
