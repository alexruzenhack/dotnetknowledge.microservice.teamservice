using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferoTeam.Models;

namespace TransferoTeam.Persistence
{
    public class MemoryTeamRepository : ITeamRepository
    {
        protected static ICollection<Team> teams;

        public MemoryTeamRepository()
        {
            if(teams == null)
            {
                teams = new List<Team>();
            }
        }

        public IEnumerable<Team> GetTeams()
        {
            return teams;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return teams;
        }

        public void AddTeam(Team team)
        {
            teams.Add(team);
        }

        public async Task<Team> AddTeamAsync(Team team)
        {
            teams.Add(team);
            return team;
        }

        public async Task<Team> GetTeamAsync(Guid id)
        {
            return teams.FirstOrDefault(t => t.ID == id);
        }

        public async Task<Team> UpdateTeamAsync(Team newTeam)
        {
            var repoTeam = teams.FirstOrDefault(t => t.ID == newTeam.ID);
            teams.Remove(repoTeam);
            teams.Add(newTeam);
            return newTeam;
        }

        public async Task DeleteTeamAsync(Guid id)
        {
            var repoTeamToDelete = teams.FirstOrDefault(t => t.ID == id);
            teams.Remove(repoTeamToDelete);
        }
    }
}
