using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Database;

namespace TimeTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TimeTrackerContext _context;

        public TasksController(TimeTrackerContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeTrackerTask>>> GetTasks() => await _context.Tasks.Include(t => t.Project).ToListAsync();

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TimeTrackerTask task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
        }
    }
}
