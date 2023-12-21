using EmployeeTracker.Server.Models;
using EmployeeTracker.Server.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTracker.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
	private readonly EmployeeTrackerService _service;

	public EmployeesController(EmployeeTrackerService service)
	{
		_service = service;
	}

	[HttpPost()]
	public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee newEmployee)
	{
		var employee = await _service.CreateEmployee(newEmployee);
		return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
	}

	[HttpGet("{id}")]
	public async Task<ActionResult<Employee?>> GetEmployeeById(int id)
	{
		var employee = await _service.GetEmployeeById(id);

		if (employee == null)
			return NotFound();

		return Ok(employee);
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
	{
		return await _service.GetEmployees();
	}

	protected async Task<ActionResult> CreateEvent(int id, string eventKind)
	{
		var employee = await _service.GetEmployeeById(id);

		if (employee == null)
			return BadRequest(new { error = $"Employee Id:{id} not found" });

		if (!TimeZoneInfo.TryFindSystemTimeZoneById(employee.TimeZoneId, out var tzInfo))
			return NotFound(new { error = $"Employee TimZoneInfo:{employee.TimeZoneId} not found" });

		var dt = DateTime.Now;
		var dto = new DateTimeOffset(dt, tzInfo.GetUtcOffset(dt));

		var newEvent = new Event
		{
			EmployeeId = employee!.Id,
			EventTime = dto,
			IsBillable = eventKind.EndsWith("Day"),
			EventKind = eventKind,
		};

		await _service.CreateEvent(newEvent);

		return NoContent();
	}

	[HttpPut("{id}/startday")]
	public async Task<ActionResult> StartDay(int id)
	{
		return await CreateEvent(id, "StartDay");
	}

	[HttpPut("{id}/startlunch")]
	public async Task<ActionResult> StartLunch(int id)
	{
		return await CreateEvent(id, "StartLunch");
	}

	[HttpPut("{id}/endlunch")]
	public async Task<ActionResult> EndLunch(int id)
	{
		return await CreateEvent(id, "EndLunch");
	}

	[HttpPut("{id}/endday")]
	public async Task<ActionResult> EndDay(int id)
	{
		return await CreateEvent(id, "EndDay");
	}

	//   // GET: EmployeesController
	//   public ActionResult Index()
	//{
	//	return View();
	//}

	//// GET: EmployeesController/Details/5
	//public ActionResult Details(int id)
	//{
	//	return View();
	//}

	//// GET: EmployeesController/Create
	//public ActionResult Create()
	//{
	//	return View();
	//}

	//// POST: EmployeesController/Create
	//[HttpPost]
	//[ValidateAntiForgeryToken]
	//public ActionResult Create(IFormCollection collection)
	//{
	//	try
	//	{
	//		return RedirectToAction(nameof(Index));
	//	}
	//	catch
	//	{
	//		return View();
	//	}
	//}

	//// GET: EmployeesController/Edit/5
	//public ActionResult Edit(int id)
	//{
	//	return View();
	//}

	//// POST: EmployeesController/Edit/5
	//[HttpPost]
	//[ValidateAntiForgeryToken]
	//public ActionResult Edit(int id, IFormCollection collection)
	//{
	//	try
	//	{
	//		return RedirectToAction(nameof(Index));
	//	}
	//	catch
	//	{
	//		return View();
	//	}
	//}

	//// GET: EmployeesController/Delete/5
	//public ActionResult Delete(int id)
	//{
	//	return View();
	//}

	//// POST: EmployeesController/Delete/5
	//[HttpPost]
	//[ValidateAntiForgeryToken]
	//public ActionResult Delete(int id, IFormCollection collection)
	//{
	//	try
	//	{
	//		return RedirectToAction(nameof(Index));
	//	}
	//	catch
	//	{
	//		return View();
	//	}
	//}
}
