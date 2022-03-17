using BurglarAlarm.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BurglarAlarm.Web.Server.Component
{
    public class AOPMiddleware
    {
        private readonly RequestDelegate _next;

        public AOPMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();
                context.Request.EnableBuffering();
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var attribute = endpoint?.Metadata.GetMetadata<IgnoreMiddlewareAttribute>();
                context.Response.ContentType = "application/json";

                await _next(context);

                if (attribute?.Event == IgnoreMiddleware.Never)
                {
                    var originBody = context.Response.Body;
                    var responseBody = new StreamReader(originBody).ReadToEnd();
                    context.Response.Body = new MemoryStream();

                    var response = new Response()
                    {
                        IsSuccess = true,
                        Data = JsonSerializer.Deserialize<object>(responseBody)
                    }.ToString();

                    await context.Response.WriteAsync(response ?? string.Empty, CancellationToken.None);
                }
            }
            catch (Exception ex)
            {
                context.Response.Body = new MemoryStream();

                var response = new Response()
                {
                    IsSuccess = false,
                    Error = ex.Message
                };

                string result = string.Empty;

                if(response is not null)
                {
                    result = JsonSerializer.Serialize(response);
                }

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }

    public static class AOPMiddlewareExtensions
    {
        public static IApplicationBuilder UseAOPMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AOPMiddleware>();
        }
    }
}
