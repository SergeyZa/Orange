using EmployeeTracker.Server.Data;
using EmployeeTracker.Server.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTracker.Server.Services;

public class EmployeeTrackerService
{
	private readonly EmployeeTrackerContext _context;

	public EmployeeTrackerService(EmployeeTrackerContext context)
	{
		_context = context;
	}

	public async Task<Employee> CreateEmployee(Employee employee)
	{
		_context.Employees.Add(employee);
		await _context.SaveChangesAsync();
		return employee;
	}

	public async Task<Employee?> GetEmployeeById(int id)
	{
		return await _context.Employees
			.AsNoTracking()
			.SingleOrDefaultAsync(e => e.Id == id);
	}

	public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
	{
		return await _context.Employees
			.AsNoTracking()
			.ToArrayAsync();
	}

	internal async Task CreateEvent(Event newEvent)
	{
		var eventKind = newEvent.EventKind switch
		{
			"StartDay" => "EndDay",
			"EndDay" => "EndLunch",
			"StartLunch" => "StartDay",
			"EndLunch" => "StartLunch",
			_ => string.Empty,
		};

		var prevEvent = await _context.Events
			.AsNoTracking()
			.Where(e => e.EmployeeId == newEvent.EmployeeId)
			.OrderByDescending(e => e.TimeTrack)
			.FirstOrDefaultAsync();

		if (prevEvent != null && prevEvent.EventKind != eventKind)
			throw new Exception("Invalid event");

		await _context.Events.AddAsync(newEvent);

		if (newEvent.EventKind.StartsWith("End"))
		{
			var lunchDuration = TimeSpan.FromMinutes(0);

			if (newEvent.EventKind.EndsWith("Day"))
			{
				lunchDuration = TimeSpan.FromMinutes(
					(await _context.WorkItems
						.AsNoTracking()
						.Where(w => w.EmployeeId == newEvent.EmployeeId)
						.OrderByDescending(w => w.TimeTrack)
						.FirstOrDefaultAsync())
						?.Duration
						?? 0);
				prevEvent = await _context.Events
					.AsNoTracking()
					.Where(e => e.EmployeeId == newEvent.EmployeeId && e.EventKind == "StartDay")
					.OrderByDescending(e => e.TimeTrack)
					.FirstOrDefaultAsync();
			}

			await _context.WorkItems.AddAsync(
				new WorkItem
				{
					EmployeeId = newEvent.EmployeeId,
					IsBillable = newEvent.IsBillable,
					WorkDate = newEvent.EventTime.Date,
					Duration = (int)Math.Round((newEvent.EventTime - prevEvent!.EventTime - lunchDuration).TotalMinutes, MidpointRounding.AwayFromZero)
				});
		}

		await _context.SaveChangesAsync();
	}

	internal async Task CreateWorkItem(WorkItem workItem)
	{
		await _context.WorkItems.AddAsync(workItem);
		await _context.SaveChangesAsync();
	}
}
