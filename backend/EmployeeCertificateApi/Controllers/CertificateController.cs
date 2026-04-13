using EmployeeCertificateApi.Data;
using EmployeeCertificateApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCertificateApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificateController(AppDbContext dbContext, ICertificateExportService certificateExportService, IWebHostEnvironment env) : ControllerBase
{
    [HttpGet("{employeeId:int}/excel")]
    public async Task<IActionResult> DownloadExcel(int employeeId)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
        if (employee is null)
        {
            return NotFound();
        }

        var bytes = certificateExportService.ExportToExcel(employee);
        var fileName = $"certificate-{employee.Id}.xlsx";
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
    }

    [HttpGet("{employeeId:int}/pdf")]
    public async Task<IActionResult> DownloadPdf(int employeeId)
    {
        var employee = await dbContext.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
        if (employee is null)
        {
            return NotFound();
        }

        var logoPath = Path.Combine(env.ContentRootPath, "Assets", "logo.png");
        var bytes = certificateExportService.ExportToPdf(employee, logoPath);
        var fileName = $"certificate-{employee.Id}.pdf";
        return File(bytes, "application/pdf", fileName);
    }
}
