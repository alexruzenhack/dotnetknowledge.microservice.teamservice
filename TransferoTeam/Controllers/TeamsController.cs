using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransferoTeam.Models;
using TransferoTeam.Persistence;

namespace TransferoTeam.Controllers
{
    [Route("api/teams")]
    public class TeamsController : Controller
    {
        private ITeamRepository _teamRepository;

        public TeamsController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        public async virtual Task<IActionResult> GetAllTeamsAsync()
        {
            return this.Ok(await _teamRepository.GetTeamsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeamAsync([FromBody] Team t)
        {
            if (t == null)
            {
                return BadRequest();
            }

            var result = await _teamRepository.AddTeamAsync(t);
            if (result == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return CreatedAtRoute("GetTeam", new { id = result.ID }, result);
        }

        [HttpGet("{id}", Name = "GetTeam")]
        public async Task<IActionResult> GetTeamAsync(Guid id)
        {
            var teamFromRepo = await _teamRepository.GetTeamAsync(id);
            if (teamFromRepo == null)
            {
                return NotFound();
            }
            return Ok(teamFromRepo);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateTeamAsync(Team newTeam, Guid id)
        {
            var oldTeamFromRepo = await _teamRepository.GetTeamAsync(id);
            if (oldTeamFromRepo == null)
            {
                return NotFound();
            }

            var teamFromRepo = await _teamRepository.UpdateTeamAsync(newTeam);
            if (teamFromRepo == null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok(teamFromRepo);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteTeamAsync(Guid id)
        {
            var teamFromRepo = await _teamRepository.GetTeamAsync(id);
            if (teamFromRepo == null)
            {
                return NotFound();
            }
            await _teamRepository.DeleteTeamAsync(id);
            return Ok();
        }
    }
}
