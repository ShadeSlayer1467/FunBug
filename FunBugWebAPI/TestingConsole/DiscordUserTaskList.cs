using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataLib
{
    public class DiscordUserTaskList
    {
        [Key]
        public int UserId { get; set; }
        public ulong DiscordUserId { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }

    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        [ForeignKey("DiscordUserTaskList")]
        public int UserID { get; set; }
    }

}
