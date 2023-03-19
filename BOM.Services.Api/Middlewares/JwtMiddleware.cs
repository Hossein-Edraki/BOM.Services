using BOM.Services.Api.Interfaces;
using BOM.Services.Api.Services;

namespace BOM.Services.Api.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtService _jwtService;
        private readonly UserService _userService;

        public JwtMiddleware(RequestDelegate next
            , IJwtService jwtService
            , UserService userService)
        {
            _next = next;
            _jwtService = jwtService;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = _jwtService.ValidateToken(token);
            if (userId != null)
            {
                var user = await _userService.GetLastSessionAsync(userId.Value, context.Request.RouteValues?.FirstOrDefault(r => r.Key == "sessionId").Value.ToString());
                if (user != null)
                {
                    context.Items.Add("userId", user.UserId);
                    context.Items.Add("_id", user._id);
                }
            }

            await _next(context);
        }
    }
}
