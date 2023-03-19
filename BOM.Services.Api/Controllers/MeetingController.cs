using BOM.Services.Api.Attributes;
using BOM.Services.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BOM.Services.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet("{sessionId}")]
        [CustomAuthorizeAttribute]
        public async Task<IActionResult> Connect([FromRoute, Required] string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                return BadRequest();

            var userId = Request.HttpContext.Items["userId"];
            var _id = Request.HttpContext.Items["_id"];
            var token = await _meetingService.ConnetToMeeting(ObjectId.Parse(_id.ToString()), int.Parse(userId.ToString()), sessionId);
            return Ok(new { token = token });
        }
    }
}
