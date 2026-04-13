using EmployeeCertificateApi.Data;
using EmployeeCertificateApi.DTOs;
using EmployeeCertificateApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCertificateApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> List([FromQuery] string? search = null)
    {
        var query = dbContext.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(e =>
                e.EmployeeName.Contains(search) ||
                e.Email.Contains(search));
        }

        var items = await query.OrderByDescending(x => x.Id).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeCreateRequest request)
    {
        if (request.Role == UserRole.User)
        {
            request.Salary = null;
        }

        var employee = new Employee
        {
            EmployeeName = request.EmployeeName,
            Age = request.Age,
            DateOfBirth = request.DateOfBirth,
            Email = request.Email,
            Salary = request.Salary,
            Role = request.Role
        };

        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await dbContext.Employees.FindAsync(id);
        return employee is null ? NotFound() : Ok(employee);
    }
}
