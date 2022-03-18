using BurglarAlarm.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Text.Json;
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
            var buffer = new MemoryStream();
            var reader = new StreamReader(buffer);
            using var stream = context.Response.Body;

            try
            {
                context.Response.Body = buffer;

                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                var attribute = endpoint?.Metadata.GetMetadata<IgnoreMiddlewareAttribute>();

                if (attribute?.Event == IgnoreMiddleware.Never)
                {
                    context.Response.ContentType = "application/json";
                }

                await _next.Invoke(context);

                if (attribute?.Event == IgnoreMiddleware.Never)
                {
                    buffer.Seek(0, SeekOrigin.Begin);

                    using var bufferReader = new StreamReader(buffer);

                    string body = await bufferReader.ReadToEndAsync();

                    var response = new Response()
                    {
                        IsSuccess = true,
                        Data = JsonSerializer.Deserialize<object>(body)
                    };

                    var jsonString = JsonSerializer.Serialize(response);

                    context.Response.Body = new MemoryStream();

                    await context.Response.WriteAsync(jsonString);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await context.Response.Body.CopyToAsync(stream);
                    context.Response.Body = stream;
                }
            }
            catch (Exception ex)
            {
                var response = new Response()
                {
                    IsSuccess = false,
                    Error = ex.Message
                };

                buffer.Seek(0, SeekOrigin.Begin);

                using var bufferReader = new StreamReader(buffer);

                var jsonString = JsonSerializer.Serialize(response);

                context.Response.Body = new MemoryStream();

                await context.Response.WriteAsync(jsonString);
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await context.Response.Body.CopyToAsync(stream);
                context.Response.Body = stream;
            }
            finally
            {
                buffer.Close();
                buffer.Dispose();
                reader.Close();
                reader.Dispose();
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
