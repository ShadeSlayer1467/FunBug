using SqlDataLib;

namespace TestingConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.AddTasksToList();
        }

        private void AddTasksToList()
        {
            // mock list data for testing DiscordUserTaskList

            var taskList = new DiscordUserTaskList
            {
                DiscordUserId = 123456789,
                Tasks = new List<TaskItem>
                {
                    new TaskItem
                    {
                        Description = "Task 1",
                        IsCompleted = false
                    },
                    new TaskItem
                    {
                        Description = "Task 2",
                        IsCompleted = false
                    },
                    new TaskItem
                    {
                        Description = "Task 3",
                        IsCompleted = false
                    }
                }
            };
            var taskList2 = new DiscordUserTaskList
            {
                DiscordUserId = 987654321,
                Tasks = new List<TaskItem>
                {
                    new TaskItem
                    {
                        Description = "Task 1",
                        IsCompleted = false
                    },
                    new TaskItem
                    {
                        Description = "Task 2",
                        IsCompleted = false
                    },
                    new TaskItem
                    {
                        Description = "Task 3",
                        IsCompleted = false
                    }
                }
            };

            // add mock data to database

        }
    }
}