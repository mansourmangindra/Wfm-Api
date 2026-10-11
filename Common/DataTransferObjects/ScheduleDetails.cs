using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataTransferObjects
{
    public class ScheduleDetails
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public string Date { get; set; }     
        public string StartTime { get; set; } 
        public string EndTime { get; set; }   
        public string Status { get; set; }
        public virtual Users? UserDetail { get; set; }

    }
}
