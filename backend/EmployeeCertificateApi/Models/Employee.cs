using System.ComponentModel.DataAnnotations;

namespace EmployeeCertificateApi.Models;

public class Employee
{
    public int Id { get; set; }

    [MaxLength(120)]
    public string EmployeeName { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime DateOfBirth { get; set; }

    [EmailAddress, MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    public decimal? Salary { get; set; }

    public UserRole Role { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
