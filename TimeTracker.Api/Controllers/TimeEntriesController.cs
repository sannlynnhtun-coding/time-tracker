using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Database;

namespace TimeTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeEntriesController : ControllerBase
    {
        private readonly TimeTrackerContext _context;

        public TimeEntriesController(TimeTrackerContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeEntry>>> GetTimeEntries()
        {
            return await _context.TimeEntries
                .Include(te => te.User)
                .Include(te => te.Task)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> LogTimeEntry([FromBody] TimeEntry timeEntry)
        {
            var user = await _context.Users.FindAsync(timeEntry.UserId);
            var task = await _context.Tasks.FindAsync(timeEntry.TaskId);

            if (user == null || task == null) return BadRequest("Invalid user or task ID");

            timeEntry.User = user;
            timeEntry.Task = task;

            _context.TimeEntries.Add(timeEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTimeEntries), new { id = timeEntry.Id }, timeEntry);
        }
    }
}
