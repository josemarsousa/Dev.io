using Elmah.Io.AspNetCore;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.API.Configuration
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services)
        {
            //adicionando o provedor elmah.io
            services.AddElmahIo(o =>
            {
                o.ApiKey = "4a3acfdcc6374f60a6047e1b5490222c";
                o.LogId = new Guid("30976159-3665-41b4-bd5b-e92bc7f8f06e");
            });

            //config para enviar o logger do asp.net para elmah.io - exemplo na V2/TesteController.cs
            //services.AddLogging(builder => 
            //{
            //    builder.AddElmahIo(o =>
            //    {
            //        o.ApiKey = "4a3acfdcc6374f60a6047e1b5490222c";
            //        o.LogId = new Guid("30976159-3665-41b4-bd5b-e92bc7f8f06e");
            //    });
            //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning); //enviando somente os logs a partir de warning (LogWarning,LogError,LogCritical)"
            //});

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
