using System;
using System.Collections.Generic;
using TransferoTeam.Controllers;
using TransferoTeam.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TransferoTeam.Persistence;

namespace TransferoTeam.Tests
{
    public class TeamsControllerTest
    {
        [Fact]
        public async void QueryTeamListReturnsCorrectTeams()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            var rawTeams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> teams = new List<Team>(rawTeams);
            Assert.Equal(2, teams.Count);
            Assert.Equal("one", teams[0].Name);
            Assert.Equal("two", teams[1].Name);
        }

        [Fact]
        public async void GetTeamRetrievesTeam()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            string sampleName = "getTeams";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            await controller.CreateTeamAsync(sampleTeam);

            Team retrievedTeam = (Team)(await controller.GetTeamAsync(id) as ObjectResult).Value;
            Assert.Equal(sampleName, retrievedTeam.Name);
            Assert.Equal(id, retrievedTeam.ID);
        }

        [Fact]
        public async void GetNonExistentTeamReturnsNotFound()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            Guid id = Guid.NewGuid();
            var result = await controller.GetTeamAsync(id);
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async void CreateTeamAddsTeamToList()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            var teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid id = Guid.NewGuid();
            Team t = new Team("createTeam", id);
            var result = await controller.CreateTeamAsync(t);
            Assert.True(result is CreatedAtRouteResult);
            // TODO: also assert that the destination URL of the new team reflects the team's GUID

            var newTeamsRaw = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(original.Count + 1, newTeams.Count);

            var sampleTeam = newTeams.FirstOrDefault(target => target.Name == "createTeam");
            Assert.NotNull(sampleTeam);
        }

        [Fact]
        public async void UpdateTeamModifiesTeamToList()
        {
            ITeamRepository memory = new MemoryTeamRepository();
            TeamsController controller = new TeamsController(memory);
            var teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid id = Guid.NewGuid();
            Team t = new Team("sample", id);
            var result = await controller.CreateTeamAsync(t);

            //System.Console.WriteLine("Please enter a numeric argument.");
            Team newTeam = new Team("sample2", id);
            await controller.UpdateTeamAsync(newTeam, id);

            var newTeamsRaw = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            var sampleTeam = newTeams.FirstOrDefault(target => target.Name == "sample");
            Assert.Null(sampleTeam);

            Team retrievedTeam = (Team)(await controller.GetTeamAsync(id) as ObjectResult).Value;
            Assert.Equal("sample2", retrievedTeam.Name);
        }

        [Fact]
        public async void UpdateNonExistentTeamReturnsNotFound()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            var teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid newTeamId = Guid.NewGuid();
            Team newTeam = new Team("New Team", newTeamId);
            var result = await controller.UpdateTeamAsync(newTeam, newTeamId);

            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async void DeleteTeamRemovesFromList()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            var teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            int ct = teams.Count();

            string sampleName = "deleteTeam";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            await controller.CreateTeamAsync(sampleTeam);

            teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.NotNull(sampleName);

            await controller.DeleteTeamAsync(sampleTeam.ID);

            teams = (IEnumerable<Team>)(await controller.GetAllTeamsAsync() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.Null(sampleTeam);
        }

        [Fact]
        public async void DeleteNonExistentTeamReturnsNotFound()
        {
            TeamsController controller = new TeamsController(new MemoryTeamRepository());
            Guid id = Guid.NewGuid();

            var result = await controller.DeleteTeamAsync(id);
            Assert.True(result is NotFoundResult);
        }
    }
}
