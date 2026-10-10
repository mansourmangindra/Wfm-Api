using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DataTransferObjects
{
    public class StaffingRequirementsDetails
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string TimePeriod { get; set; }
        public int RequiredAgents { get; set; }
        public int ScheduledAgents { get; set; }
    }
}
