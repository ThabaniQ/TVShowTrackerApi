using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TVShowTracker.Contracts.Request;
using TVShowTracker.Extentions;
using TVShowTracker.Model;
using TVShowTracker.Repository.Interface;

namespace TVShowTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EpisodesController : ControllerBase
    {
        private readonly IWrapperRepository _repositoryContext;
        private readonly ILogger<EpisodesController> _logger;

        public EpisodesController(IWrapperRepository context, ILogger<EpisodesController> logger)
        {
            _repositoryContext = context;
            _logger = logger;
        }

        [HttpGet("GetAllEpisodes/{showId}")]
        public async Task<ActionResult<IEnumerable<Episode>>>
            GetAllEpisodes([FromRoute] Guid showId)
        {
            try
            {
                var episodes = await _repositoryContext.Episode.GetAllEpisodesAsync(showId);

                if (episodes == null)
                {
                    _logger.LogError("Episodes not found for Show ID: {ShowId}", showId);
                    return NotFound("Episodes not found");
                }

                return Ok(episodes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetEpisode/{id}/{showId}")]
        public async Task<ActionResult<Episode>> GetEpisode(
            Guid id,
            [FromRoute] Guid showId)
        {
            try
            {
                var episode = await _repositoryContext.Episode.GetEpisodeAsync(id, showId);

                if (episode == null)
                {
                    _logger.LogError("Episode not found with ID: {Id}", id);
                    return NotFound("Episode not found");
                }

                return Ok(episode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("UpdateEpisode/{id}/{showId}")]
        public async Task<IActionResult> PutEpisode(
            Guid id,
            [FromRoute] Guid showId,
            UpdateEpisodeDTO episode)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var existingShow = await _repositoryContext.Show.GetShowAsync(showId, Guid.Parse(userId));
                var existingEpisode = await _repositoryContext.Episode.GetEpisodeAsync(id, showId);

                if (existingShow == null)
                {
                    return NotFound("Show not found");
                }

                if (existingEpisode == null)
                {
                    return NotFound("Episode not found");
                }

                existingEpisode.Watched = episode.Watched;

                _repositoryContext.Episode.UpdateEpisode(existingEpisode);
                await _repositoryContext.SaveAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("AddEpisode/{showId}")]
        public async Task<ActionResult<Episode>> PostEpisode([FromRoute] Guid showId,[FromBody] EpisodeDTO episode)
        {
            try
            {
                var userId = HttpContext.GetUserId();
                var existingShow = await _repositoryContext.Show.GetShowAsync(showId, Guid.Parse(userId));

                if (existingShow == null)
                {
                    return NotFound("Show not found");
                }

                var newEpisode = new Episode
                {
                    Id = Guid.NewGuid(),
                    Title = existingShow.Title,
                    SeasonNumber = episode.SeasonNumber,
                    EpisodeNumber = episode.EpisodeNumber,
                    ShowId = showId,
                    Watched = episode.Watched
                };

                _repositoryContext.Episode.AddEpisode(newEpisode);
                await _repositoryContext.SaveAsync();

                return CreatedAtAction("GetEpisode", new { id = newEpisode.Id, showId = newEpisode.ShowId }, newEpisode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEpisode/{id}/{showId}")]
        public async Task<IActionResult> DeleteEpisode(
            Guid id,
            [FromRoute] Guid showId)
        {
            try
            {
                var episode = await _repositoryContext.Episode.GetEpisodeAsync(id, showId);

                if (episode == null)
                {
                    _logger.LogError("Episode not found with ID: {Id}", id);
                    return NotFound("Episode not found");
                }

                _repositoryContext.Episode.DeleteEpisode(episode);
                await _repositoryContext.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred: {ErrorMessage}", ex.Message);
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
