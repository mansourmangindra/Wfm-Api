using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.Wfm.Models;

[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [Column("role")]
    [StringLength(50)]
    public string Role { get; set; } = null!;

    [InverseProperty("Agent")]
    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
