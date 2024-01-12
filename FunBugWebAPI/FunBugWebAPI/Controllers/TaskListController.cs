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
        #region Get
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
        #endregion Get
        #region Post
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
        [HttpPost("Create")]
        public async Task<IActionResult> CreateTask([FromQuery] ulong discordUserId, [FromQuery] string taskDescription, [FromQuery] bool isCompleted)
        {
            if (string.IsNullOrWhiteSpace(taskDescription))
            {
                return BadRequest("Task description cannot be empty.");
            }

            var newTask = new UserTask
            {
                DiscordUserId = discordUserId,
                TaskDescription = taskDescription,
                IsCompleted = isCompleted
            };

            dbcontext.UserTasks.Add(newTask);
            await dbcontext.SaveChangesAsync();

            return Ok(newTask);
        }
        [HttpPost("ToggleTaskCompletion/{taskId}")]
        public async Task<IActionResult> ToggleTaskCompletion(int taskId)
        {
            var task = await dbcontext.UserTasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound($"Task with ID {taskId} not found.");
            }

            // Toggle the completion status
            task.IsCompleted = !task.IsCompleted;

            dbcontext.UserTasks.Update(task);
            await dbcontext.SaveChangesAsync();

            return Ok(task);
        }

        #endregion Post
        #region Delete
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
        #endregion Delete
    }
}
