using System.ComponentModel.DataAnnotations;

namespace EmployeeTracker.Server.Models;

public class Event
{
	public int Id { get; set; }
	public int EmployeeId { get; set; }
    [Timestamp]
    public byte[] TimeTrack { get; set; } = [];
    public DateTimeOffset EventTime { get; set; }
    public string EventKind { get; set; } = default!;
    public bool IsBillable { get; set; } = true;
}
