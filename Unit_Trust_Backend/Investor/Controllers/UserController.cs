using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unit_Trust_Backend.DTOs;
using Unit_Trust_Backend.Investor.Interfaces;

namespace Unit_Trust_Backend.Investor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupDTO signupDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(signupDTO);
            if (!result)
                return Conflict(new { message = "Email already registered" });

            return Ok(new { message = "User registered successfully." });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Signin(UserSignInDTO signInDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (IsSuccessfull, UserType, Token) = await _userService.LoginUserAsync(signInDTO);

            if (!IsSuccessfull)
                return Unauthorized(new { message = "Invalid username or password." });

            // Success - return the token along with the user type
            return Ok(new
            {
                message = "Login successful!",
                UserType,
                Token // Return the JWT token in the response
            });
        }

    }
}
