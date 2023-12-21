using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace EmployeeTracker.Server.Models;

public class Employee
{
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string TimeZoneId { get; set; } = default!;
}
