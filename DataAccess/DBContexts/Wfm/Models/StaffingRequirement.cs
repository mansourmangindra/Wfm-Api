using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.Wfm.Models;

[Table("staffing_requirements")]
public partial class StaffingRequirement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("time_period")]
    [StringLength(20)]
    public string TimePeriod { get; set; } = null!;

    [Column("required_agents")]
    public int RequiredAgents { get; set; }

    [Column("scheduled_agents")]
    public int ScheduledAgents { get; set; }
}
