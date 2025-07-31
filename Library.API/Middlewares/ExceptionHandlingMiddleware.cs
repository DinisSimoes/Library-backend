using Library.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace Library.API.Middlewares
{
    /// <summary>
    /// Middleware para capturar e tratar exceções não manipuladas globalmente.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Inicializa uma nova instância do <see cref="ExceptionHandlingMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegate para o próximo middleware no pipeline.</param>
        /// <param name="logger">Logger para registrar exceções.</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoca o middleware para capturar exceções durante o processamento da requisição.
        /// </summary>
        /// <param name="context">O contexto HTTP atual.</param>
        /// <returns>Uma <see cref="Task"/> representando a operação assíncrona.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Manipula exceções e escreve uma resposta JSON padronizada.
        /// </summary>
        /// <param name="context">O contexto HTTP.</param>
        /// <param name="exception">A exceção capturada.</param>
        /// <returns>Uma <see cref="Task"/> representando a operação assíncrona de escrita da resposta.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "An unexpected error occurred.";

            switch (exception)
            {
                case NotFoundException notFoundEx:
                    code = HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    break;
                case ArgumentException argEx:
                    code = HttpStatusCode.BadRequest;
                    message = argEx.Message;
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                success = false,
                error = message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
