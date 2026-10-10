using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataTransferObjects
{
    public class ScheduleDetails
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Status { get; set; }
        public Users UserDetail { get; set; }

    }
}
