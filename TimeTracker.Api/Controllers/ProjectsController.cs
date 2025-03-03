using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Database;

namespace TimeTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TimeTrackerContext _context;

        public ProjectsController(TimeTrackerContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects() => await _context.Projects.Include(p => p.Client).ToListAsync();

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProjects), new { id = project.Id }, project);
        }
    }
}
