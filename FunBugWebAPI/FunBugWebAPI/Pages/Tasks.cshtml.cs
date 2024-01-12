using FunBugWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SqlDataLib;

namespace FunBugWebAPI.Pages
{
    public class TasksModel : PageModel
    {
        public List<UserTask> UserTasks { get; set; } = new List<UserTask>();
        private readonly ApplicationDbContext dbcontext;

        public TasksModel(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public void OnGet()
        {
            dbcontext.Database.Migrate();
            UserTasks = dbcontext.UserTasks.ToList();
        }
    }
}
