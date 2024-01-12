using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataLib
{
    public class UserTask
    {
        [Key]
        public int Id { get; set; }
        public ulong DiscordUserId { get; set; }
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }

}
