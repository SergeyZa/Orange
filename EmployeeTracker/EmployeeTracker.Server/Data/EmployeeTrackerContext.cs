
using EmployeeTracker.Server.Models;

using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Server.Data;

public class EmployeeTrackerContext : DbContext
{
    public EmployeeTrackerContext(DbContextOptions<EmployeeTrackerContext> options) : base(options)
    {
        
    }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();
}
