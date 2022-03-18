using BurglarAlarm.Domain.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.IO;
using System.Net;
using System.Text;
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
            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                await _next.Invoke(context);

                buffer.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(buffer);

                using (var bufferReader = new StreamReader(buffer))
                {
                    string body = await bufferReader.ReadToEndAsync();
  
                    var response = new Response()
                    {
                        IsSuccess = true,
                        Data = body
                    };

                    var jsonString = JsonSerializer.Serialize(response);

                    context.Response.Body = new MemoryStream();

                    // Commented below lines.
                    // byte[] bytess = Encoding.ASCII.GetBytes(jsonString);
                    // var data = new MemoryStream(bytess);
                    // context.Response.Body = data;

                    // Added new code
                    await context.Response.WriteAsync(jsonString);
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    // below code is not working with .Net 6 and it requires CopyToAsync.
                    //context.Response.Body.CopyTo(stream);
                    await context.Response.Body.CopyToAsync(stream); //it prevents it must be async, if it isn't there is an exception in .Net 6.
                    context.Response.Body = stream;
                }
            }


            //var originalBody = context.Response.Body;
            //using var newBody = new MemoryStream();
            //context.Response.Body = newBody;

            //try
            //{
            //    await _next(context);

            //    newBody.Seek(0, SeekOrigin.Begin);
            //    var bodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();

            //    newBody.Seek(0, SeekOrigin.Begin);
            //    await newBody.CopyToAsync(originalBody);

            //    await context.Response.WriteAsync("bye");
            //    context.Response.Body.Seek(0, SeekOrigin.Begin);

            //    var stream = new MemoryStream();
            //    var bb = Encoding.UTF8.GetBytes("bye");
            //    stream.Write(bb, 0, bb.Length);

            //    await context.Response.Body.CopyToAsync(stream);
            //    context.Response.Body = stream;

            //    await context.Response.WriteAsync("bye", Encoding.UTF8);
            //}
            //catch (Exception ex)
            //{


            //}

            //try
            //{
            //    context.Request.EnableBuffering();
            //    context.Request.EnableBuffering();
            //    context.Response.StatusCode = (int)HttpStatusCode.OK;

            //    var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            //    var attribute = endpoint?.Metadata.GetMetadata<IgnoreMiddlewareAttribute>();
            //    context.Response.ContentType = "application/json";
            //    var originBody = context.Response.Body;

            //    await _next(context);

            //    if (attribute?.Event == IgnoreMiddleware.Never)
            //    {
            //        var responseBody = new StreamReader(originBody).ReadToEnd();
            //        context.Response.Body = new MemoryStream();

            //        var response = new Response()
            //        {
            //            IsSuccess = true,
            //            Data = JsonSerializer.Deserialize<object>(responseBody)
            //        }.ToString();

            //        await context.Response.WriteAsync(response ?? string.Empty, CancellationToken.None);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    context.Response.Body = new MemoryStream();

            //    var response = new Response()
            //    {
            //        IsSuccess = false,
            //        Error = ex.Message
            //    };

            //    string result = string.Empty;

            //    if(response is not null)
            //    {
            //        result = JsonSerializer.Serialize(response);
            //    }

            //    await context.Response.WriteAsJsonAsync(result);
            //}
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
