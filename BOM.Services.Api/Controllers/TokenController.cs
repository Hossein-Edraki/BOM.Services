using BOM.Services.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BOM.Services.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public TokenController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromRoute, Required] string userId)
        {
            if(string.IsNullOrWhiteSpace(userId))
                return BadRequest(); 

            var token = _jwtService.GenerateToken(userId);
            return Ok(token); 
        }
    }
}
