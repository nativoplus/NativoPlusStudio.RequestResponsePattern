#region
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
#endregion
namespace NativoPlusStudio.RequestResponsePattern
{
    public abstract class HttpHandler<TRequest> : IRequestHandler<TRequest, IActionResult>
       where TRequest : class, IHttpRequest, new()
    {
        public readonly ILogger _logger;
        protected HttpHandler(ILogger logger = null)
        {
            if (logger == null)
            {
                _logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .CreateLogger();
            }
            else
            {
                _logger = logger;
            }
        }

        public async Task<IActionResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            _logger.Information("#Handle {@Request}", request);
            _logger.Information(nameof(Handle));
            if (request == null)
            {
                return new NotFoundResult();
            }
            _logger.Debug("Beginning request: {@request}", request);
            // https://medium.com/bynder-tech/c-why-you-should-use-configureawait-false-in-your-library-code-d7837dce3d7f
            var model = await HandleAsync(request, cancellationToken).ConfigureAwait(false);

            if (model.Response != null)
            {
                _logger.Information("#Handle {@Response}", model.Response);
                return new OkObjectResult(model.Response);
            }
            return new StatusCodeResult((int)model.HttpStatusCode);
        }

        protected abstract Task<HttpResponse> HandleAsync(TRequest input, CancellationToken cancellationToken = default);

        protected HttpResponse Ok<TResponse>(TResponse response) where TResponse : class
        {

            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            //TODO validation based on the object here. 
            _logger.Information("#HandleAsync {@Response}", response);
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.OK,

            };
        }
        protected HttpResponse BadRequest<TResponse>(TResponse response) where TResponse : class
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            _logger.Error("#BadRequest {@Response}", response);
            //TODO validation based on the object here. 
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.BadRequest,

            };
        }
        protected HttpResponse InternalServerError<TResponse>(TResponse response) where TResponse : class
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            _logger.Error("#InternalServerError {@Response}", response);
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.InternalServerError,

            };
        }
    }
}



