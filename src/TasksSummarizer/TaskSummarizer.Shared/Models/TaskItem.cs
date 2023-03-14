using System;
using System.Collections.Generic;
using System.Text;

namespace TaskSummarizer.Shared.Models
{
    public class TaskItem
    {
        public class TaskItems
        {
            public string? Description { get; set; }
            public string? Importance { get; set; }
            public string? Status { get; set; }
            public List<SubTask>? SubTasks { get; set; }
            public string? TaskTitle { get; set; }
        }

        public class SubTask
        {
            public string? DisplayName { get; set; }
            public string? SubTaskStatus { get; set; }
        }
    }
}
