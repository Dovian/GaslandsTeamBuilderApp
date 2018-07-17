using System;
using System.Collections.Generic;
using System.Linq;

namespace GaslandsTeamBuilderDataRepo
{
    public class DBReader : IDBReader
    {
        GaslandsAppEntities _db = new GaslandsAppEntities();

        public int Login(string username, string password)
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == username);

            if (user != null)
            {
                var login = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (login)
                {
                    return user.Key;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }

        public Build GetBuild(int key, int userId)
        {
            return _db.Builds.Single(b => b.Key == key && b.User == userId);
        }

        public Team GetTeam(int key, int userId)
        {
            return _db.Teams.Single(t => t.Key == key && t.User == userId);
        }

        public List<Build> GetAllBuilds(int userId)
        {
            return _db.Builds.Where(b => b.User == userId).ToList();
        }

        public List<Team> GetAllTeams(int userId)
        {
            return _db.Teams.Where(t => t.User == userId).ToList();
        }

        public List<Perk> GetPerks(string sponsor)
        {
            var perks = new List<Perk>();
            var perkClasses = _db.Sponsors.Single(s => s.Name == sponsor).PerkClasses.Select(pc => pc.Name).ToList();

            foreach (string perkClass in perkClasses)
            {
                var dbPerks = _db.Perks.Where(p => p.PerkClass.Name.Contains(perkClass)).ToList();
                perks.AddRange(dbPerks);
            }

            return perks;
        }

        public Tuple<string, string> GetSpecialRule(string name)
        {
            var dbRule = _db.SpecialRules.SingleOrDefault(sr => sr.Name == name);
            var coreRule = dbRule != null ? new Tuple<string, string>(dbRule.Name, dbRule.Description) : new Tuple<string, string>("", "");
            return coreRule;
        }

        public List<Sponsor> GetSponsors()
        {
            return _db.Sponsors.ToList();
        }

        public List<Upgrade> GetUpgrades()
        {
            return _db.Upgrades.ToList();
        }

        public List<Vehicle> GetVehicles()
        {
            return _db.Vehicles.ToList();
        }

        public List<Weapon> GetWeapons()
        {
            return _db.Weapons.ToList();
        }

        public Weapon GetWeapon(int key)
        {
            return _db.Weapons.Single(w => w.Key == key);
        }

        public Perk GetPerk(int key)
        {
            return _db.Perks.Single(p => p.Key == key);
        }

        public Upgrade GetUpgrade(int key)
        {
            return _db.Upgrades.Single(u => u.Key == key);
        }
    }
}
