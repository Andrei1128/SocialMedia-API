using FirstProject_API.Models;
using FirstProject_API.Models.DTOs;
using FirstProject_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FirstProject_API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;
        protected APIResponse _response;
        public AuthController(IUserRepository userRepository)
        {
            this._response = new APIResponse();
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepository.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Username or password is incorrect!");
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            bool isEmailUnique = _userRepository.IsEmailUnique(model.Email);
            Console.WriteLine(isEmailUnique);
            if (isEmailUnique)
            {
                var registerResponse = await _userRepository.Register(model);
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = registerResponse;
                return Ok(_response);
            }
            else
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("Email already used!");
                return BadRequest(_response);
            }
        }
    }
}
