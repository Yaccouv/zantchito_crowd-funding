using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Staff.DTOs;
using Unit_Trust_Backend.Staff.Interfaces;

namespace Unit_Trust_Backend.Staff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;
        public StaffController(IStaffService staffService) 
        { 
            _staffService = staffService;
        }

        [HttpPost("staffSignUp")]
        public async Task<IActionResult> Signup(StaffSignUpDTO staffSignUpDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _staffService.StaffSignUpAsync(staffSignUpDTO);
            if (!result)
                return Conflict(new { message = "Email already registered" });

            return Ok(new { message = "Staff registered successfully." });
        }


        [HttpPost("staffSignIn")]
        public async Task<IActionResult> Signin(StaffSignInDTO staffSignInDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _staffService.StaffSignInAsync(staffSignInDTO);
            if (!result)
                return Unauthorized(new { message = "Invalid Staff username or password." });

            return Ok(new { message = "Staff Login successful" });
        }

    }
}
