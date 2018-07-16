using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaslandsTeamBuilderCore
{
    public class AppUserState
    {
        public string Username { get; set; }
        public int UserId { get; set; }

        public override string ToString()
        {
            return string.Join("|", new string[] { Username, UserId.ToString() });
        }

        public AppUserState FromString(string aus)
        {
            var strings = aus.Split('|').ToList();

            if(strings.Count != 2)
            {
                return null;
            }

            return new AppUserState()
            {
                Username = strings[0],
                UserId = int.Parse(strings[1])
            };
        }
    }
}
