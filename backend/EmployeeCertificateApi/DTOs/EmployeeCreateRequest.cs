using EmployeeCertificateApi.Models;

namespace EmployeeCertificateApi.DTOs;

public class EmployeeCreateRequest
{
    public string EmployeeName { get; set; } = string.Empty;
    public int Age { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public decimal? Salary { get; set; }
    public UserRole Role { get; set; }
}
