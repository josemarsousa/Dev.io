using DevIO.API.Controllers;
using DevIO.Business.Intefaces;
using Elmah.Io.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DevIO.API.V2.Controllers
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/teste")]
    public class TesteController : MainController
    {
        private readonly ILogger _logger;

        public TesteController(INotificador notificador,
                               IUser appUser,
                               ILogger<TesteController> logger) : base(notificador, appUser)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Valor()
        {
            //teste para elmah.io usando o middleware - ExceptionMiddleware.cs
            //throw new Exception("Error");

            //teste para elmah.io
            //try
            //{
            //    var i = 0;
            //    var result = 42 / i;
            //}
            //catch (DivideByZeroException e)
            //{
            //    e.Ship(HttpContext); //Ship() enviar log pro elmah.io
            //}

            //log do asp.net - config para se usar no elmah.io comentada em LoggerConfig.cs
            _logger.LogTrace("Log de Trace"); //uso em ambiente dev - desabilitada como default 
            _logger.LogDebug("Log de Debug"); //uso em ambiente dev - desabilitada como default 
            _logger.LogInformation("Log de Informação");
            _logger.LogWarning("Log de Aviso");
            _logger.LogError("Log de Erro");
            _logger.LogCritical("Log de Problema Critico");
            return "Sou a V2";
        }
    }
}