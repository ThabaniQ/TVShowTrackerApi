using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVShowTracker.Contracts.Request;
using TVShowTracker.Extentions;
using TVShowTracker.Model;
using TVShowTracker.Repository.Interface;
using TVShowTracker.Middleware;

namespace TVShowTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShowsController : ControllerBase
    {
        private readonly IWrapperRepository _repositoryContext;
        private readonly ILogger<ShowsController> _logger;

        public ShowsController(IWrapperRepository repositoryContext, ILogger<ShowsController> logger)
        {
            _repositoryContext = repositoryContext;
            _logger = logger;
        }

        [HttpGet("GetAllShows")]
        public async Task<ActionResult<IEnumerable<Show>>> GetShows()
        {
            try
            {
                var userId = HttpContext.GetUserId();

                if (_repositoryContext.Show == null)
                {
                    _logger.LogError("The Show repository is null.");
                    return NotFound();
                }

                var shows = await _repositoryContext.Show.GetAllShowsAsync(Guid.Parse(userId));
                return Ok(shows);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving shows.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("GetShow/{id}")]
        public async Task<ActionResult<Show>> GetShow(Guid id)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                if (_repositoryContext.Show == null)
                {
                    _logger.LogError("The Show repository is null.");
                    return NotFound();
                }

                var show = await _repositoryContext.Show.GetShowAsync(id, Guid.Parse(userId));

                if (show == null)
                {
                    _logger.LogError("Show not found with ID: {Id}", id);
                    return NotFound();
                }

                return show;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving a show.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("AddShows")]
        public async Task<ActionResult<Show>> PostShow([FromBody] ShowDTO show)
        {
            try
            {
                var newShow = new Show
                {
                    Id = Guid.NewGuid(),
                    Title = show.Title,
                    Description = show.Description,
                    UserId = HttpContext.GetUserId()
                };

                _repositoryContext.Show.AddShow(newShow);
                await _repositoryContext.SaveAsync();

                return CreatedAtAction("GetShow", new { id = newShow.Id }, newShow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new show.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("DeleteShow/{id}")]
        public async Task<IActionResult> DeleteShow([FromRoute] Guid id)
        {
            try
            {
                var userId = HttpContext.GetUserId();

                if (_repositoryContext.Show == null)
                {
                    _logger.LogError("The Show repository is null.");
                    return NotFound();
                }

                var show = await _repositoryContext.Show.GetShowAsync(id, Guid.Parse(userId));

                if (show == null)
                {
                    _logger.LogError("Show not found with ID: {Id}", id);
                    return NotFound();
                }

                _repositoryContext.Show.DeleteShow(show);
                await _repositoryContext.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting a show.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
