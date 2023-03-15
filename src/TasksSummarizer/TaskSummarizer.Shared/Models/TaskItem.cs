using System;
using System.Collections.Generic;
using System.Text;

namespace TaskSummarizer.Shared.Models
{
    public class TaskItem
    {
        public string? Description { get; set; }
        public string? Importance { get; set; }
        public string? Status { get; set; }
        public List<TaskItemSubTask>? SubTasks { get; set; } 
        public string? TaskTitle { get; set; }
        public string? OtherPeopleHelping {get; set;}
    }

    public class TaskItemSubTask
    {
        public string? DisplayName { get; set; }
        public string? SubTaskStatus { get; set; }
    }
}
