using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Elmah.Io.AspNetCore;

namespace DevIO.API.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        //async await funciona até a versão elmah 3.6.63
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            await exception.ShipAsync(context); // enviando para o elmah.io
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
