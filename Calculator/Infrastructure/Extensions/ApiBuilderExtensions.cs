using Microsoft.AspNetCore.Builder;
using Infrastructure.Middleware;

namespace Infrastructure.Extensions
{
    public static class ApiBuilderExtensions
    {
        public static IApplicationBuilder UseApiApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiLoggingMiddleware>();
        }

        public static IApplicationBuilder UseApiAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiAuthenticationMiddleware>();
        }
        public static IApplicationBuilder UseApiContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiContextMiddleware>();
        }

        public static IApplicationBuilder UseApiErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiErrorHandlingMiddleware>();
        }
    }
}
