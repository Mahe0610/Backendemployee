using EmployeeCertificateApi.DTOs;
using EmployeeCertificateApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCertificateApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var validAdmin = request is { Username: "admin", Password: "admin@123" };
        var validUser = request is { Username: "user", Password: "user@123" };

        if (!validAdmin && !validUser)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var role = validAdmin ? UserRole.Admin : UserRole.User;
        return Ok(new { username = request.Username, role });
    }
}
