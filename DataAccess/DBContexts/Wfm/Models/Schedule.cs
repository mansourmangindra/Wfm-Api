using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.Wfm.Models;

[Table("schedules")]
public partial class Schedule
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("agent_id")]
    public int AgentId { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("start_time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [ForeignKey("AgentId")]
    [InverseProperty("Schedules")]
    public virtual User Agent { get; set; } = null!;
}
