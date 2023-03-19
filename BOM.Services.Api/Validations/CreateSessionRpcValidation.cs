using BOM.Services.Api.Proto;
using FluentValidation;

namespace BOM.Services.Api.Validations
{
    public class CreateSessionRpcValidation : AbstractValidator<RpcCreateSessionRequest>
    {
        public CreateSessionRpcValidation()
        {
            RuleFor(request => request.UserId).NotEmpty();
        }
    }
}
