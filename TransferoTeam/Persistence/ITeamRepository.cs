using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferoTeam.Models;

namespace TransferoTeam.Persistence
{
    public interface ITeamRepository
    {
        IEnumerable<Team> GetTeams();
        Task<IEnumerable<Team>> GetTeamsAsync();
        void AddTeam(Team team);
        Task<Team> AddTeamAsync(Team team);
        Task<Team> GetTeamAsync(Guid id);
        Task<Team> UpdateTeamAsync(Team newTeam);
        Task DeleteTeamAsync(Guid id);
    }
}
