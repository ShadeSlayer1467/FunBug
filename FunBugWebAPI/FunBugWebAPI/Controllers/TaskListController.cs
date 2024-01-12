using FunBugWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SqlDataLib;

namespace FunBugWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ToDoListController : ControllerBase
    {
        private readonly ApplicationDbContext dbcontext;

        public ToDoListController(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserTask>>> GetAll()
        {
            var tasks = await dbcontext.UserTasks.ToListAsync();

            if (tasks == null || !tasks.Any())
            {
                return NotFound("No tasks found.");
            }

            return Ok(tasks);
        }
        [HttpGet("{discordUserId}")]
        public async Task<ActionResult<IEnumerable<UserTask>>> Get(ulong discordUserId)
        {
            var tasks = await dbcontext.UserTasks
                                      .Where(t => t.DiscordUserId == discordUserId)
                                      .ToListAsync();

            if (tasks == null || !tasks.Any())
            {
                return NotFound($"No tasks found for Discord user ID {discordUserId}.");
            }

            return Ok(tasks);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserTask userTask)
        {
            if (userTask == null)
            {
                return BadRequest("Invalid task data.");
            }

            dbcontext.UserTasks.Add(userTask);
            await dbcontext.SaveChangesAsync();

            return Ok(userTask);
        }
        [HttpDelete("Task/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var task = await dbcontext.UserTasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found.");
            }

            dbcontext.UserTasks.Remove(task);
            await dbcontext.SaveChangesAsync();

            return Ok($"Task with ID {taskId} deleted.");
        }
        [HttpDelete("User/{discordUserId}")]
        public async Task<IActionResult> DeleteUserTasks(ulong discordUserId)
        {
            var tasks = await dbcontext.UserTasks
                                      .Where(t => t.DiscordUserId == discordUserId)
                                      .ToListAsync();

            if (!tasks.Any())
            {
                return NotFound($"No tasks found for Discord user ID {discordUserId}.");
            }

            dbcontext.UserTasks.RemoveRange(tasks);
            await dbcontext.SaveChangesAsync();
                
            return Ok($"All tasks for Discord user ID {discordUserId} deleted.");
        }
    }
}
