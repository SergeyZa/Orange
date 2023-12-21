using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Server.Models;

public class WorkItem
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public DateTimeOffset WorkDate { get; set; }
    public int Duration { get; set; }
    public bool IsBillable { get; set; } = true;
    [Timestamp]
    public byte[] TimeTrack { get; internal set; } = Array.Empty<byte>();
}
