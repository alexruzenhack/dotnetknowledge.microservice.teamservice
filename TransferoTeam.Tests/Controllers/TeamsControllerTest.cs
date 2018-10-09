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
        private TeamsController _controller;

        public TeamsControllerTest()
        {
            ITeamRepository memoryTeamRepository = new MemoryTeamRepository();
            memoryTeamRepository.AddTeam(new Team("one"));
            memoryTeamRepository.AddTeam(new Team("two"));

            _controller = new TeamsController(memoryTeamRepository);
        }

        [Fact]
        public async void GetTeamRetrievesTeam()
        {
            string sampleName = "getTeams";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            await _controller.CreateTeamAsync(sampleTeam);

            Team retrievedTeam = (Team)(await _controller.GetTeamAsync(id) as ObjectResult).Value;
            Assert.Equal(sampleName, retrievedTeam.Name);
            Assert.Equal(id, retrievedTeam.ID);
        }

        [Fact]
        public async void GetNonExistentTeamReturnsNotFound()
        {
            Guid id = Guid.NewGuid();
            var result = await _controller.GetTeamAsync(id);
            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async void CreateTeamAddsTeamToList()
        {
            var teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid id = Guid.NewGuid();
            Team t = new Team("createTeam", id);
            var result = await _controller.CreateTeamAsync(t);
            Assert.True(result is CreatedAtRouteResult);
            // TODO: also assert that the destination URL of the new team reflects the team's GUID

            var newTeamsRaw = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            Assert.Equal(original.Count + 1, newTeams.Count);

            var sampleTeam = newTeams.FirstOrDefault(target => target.Name == "createTeam");
            Assert.NotNull(sampleTeam);
        }

        [Fact]
        public async void UpdateTeamModifiesTeamToList()
        {
            var teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid id = Guid.NewGuid();
            Team t = new Team("sample", id);
            var result = await _controller.CreateTeamAsync(t);

            //System.Console.WriteLine("Please enter a numeric argument.");
            Team newTeam = new Team("sample2", id);
            await _controller.UpdateTeamAsync(newTeam, id);

            var newTeamsRaw = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> newTeams = new List<Team>(newTeamsRaw);
            var sampleTeam = newTeams.FirstOrDefault(target => target.Name == "sample");
            Assert.Null(sampleTeam);

            Team retrievedTeam = (Team)(await _controller.GetTeamAsync(id) as ObjectResult).Value;
            Assert.Equal("sample2", retrievedTeam.Name);
        }

        [Fact]
        public async void UpdateNonExistentTeamReturnsNotFound()
        {
            var teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            List<Team> original = new List<Team>(teams);

            Guid newTeamId = Guid.NewGuid();
            Team newTeam = new Team("New Team", newTeamId);
            var result = await _controller.UpdateTeamAsync(newTeam, newTeamId);

            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async void DeleteTeamRemovesFromList()
        {
            var teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            int ct = teams.Count();

            string sampleName = "deleteTeam";
            Guid id = Guid.NewGuid();
            Team sampleTeam = new Team(sampleName, id);
            await _controller.CreateTeamAsync(sampleTeam);

            teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.NotNull(sampleName);

            await _controller.DeleteTeamAsync(sampleTeam.ID);

            teams = (IEnumerable<Team>)(await _controller.GetAllTeamsAsync() as ObjectResult).Value;
            sampleTeam = teams.FirstOrDefault(target => target.Name == sampleName);
            Assert.Null(sampleTeam);
        }

        [Fact]
        public async void DeleteNonExistentTeamReturnsNotFound()
        {
            Guid id = Guid.NewGuid();

            var result = await _controller.DeleteTeamAsync(id);
            Assert.True(result is NotFoundResult);
        }
    }
}
