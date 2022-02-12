using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Auth
{
    public class UserUnauthorizedResult : JsonResult
    {
        public UserUnauthorizedResult(string message)
            : base(new CustomError(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}