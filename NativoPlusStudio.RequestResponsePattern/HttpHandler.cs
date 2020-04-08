#region
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using FluentValidation.Results;
using System.Collections.Generic;
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
            _logger.Information(nameof(Handle));
            _logger.Information("#Handle {@Request}", request);
            if (request == null)
            {
                return new NotFoundResult();
            }
            // https://medium.com/bynder-tech/c-why-you-should-use-configureawait-false-in-your-library-code-d7837dce3d7f
            var model = await HandleAsync(request, cancellationToken).ConfigureAwait(false);
            _logger.Information("#Handle {@Response}", model?.Response ?? new object());
            return new OkObjectResult(model?.Response ?? new HttpResponse() { HttpStatusCode = HttpStatusCode.BadRequest, Response = null });
        }

        protected abstract Task<HttpResponse> HandleAsync(TRequest input, CancellationToken cancellationToken = default);

        public HttpResponse Ok<TResponse>(TResponse response, string transactionId = "") where TResponse : class, new()
        {
            var mresponse = (new HttpStandardResponse<TResponse>
            {
                Response = response,
                Error = null,
                Status = true,
                TransactionId = string.IsNullOrWhiteSpace(transactionId) ? Guid.NewGuid().ToString() : transactionId
            });
            _logger.Information("#HandleAsync {@Response}", response ?? new TResponse());
            return new HttpResponse
            {
                Response = mresponse,
                HttpStatusCode = HttpStatusCode.OK,
            };
        }
        public HttpResponse NullBadRequest<TResponse>(string transactionId, string code =null, string errorMessage = null) where TResponse : class, new()
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                transactionId = Guid.NewGuid().ToString();
            }
            var response = (new HttpStandardResponse<TResponse>
            {
                Response = null,
                Error = new List<Error>() {
                     new Error{ 
                      Code =code ?? "NullRequest",
                      Message = errorMessage ?? "The request was null"
                     }
                 },
                Status = false,
                TransactionId = string.IsNullOrWhiteSpace(transactionId) ? Guid.NewGuid().ToString() : transactionId
            });
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
        public HttpResponse BadRequest<TResponse>(TResponse response, string transactionId = "") where TResponse : class, new()
        {
            if (response == null)
            {
                response = new TResponse();
                _logger.Error("#BadRequest the response was null when sent to the middleware pipeline");
            }
            // At least we get to record what the response was.
            _logger.Error("#BadRequest {@Response}", response ?? new TResponse());

            var mresponse = (new HttpStandardResponse<TResponse>
            {
                Response = response,
                Error = null,
                Status = false,
                TransactionId = string.IsNullOrWhiteSpace(transactionId) ? Guid.NewGuid().ToString() : transactionId
            });

            return new HttpResponse
            {
                Response = mresponse,
                HttpStatusCode = HttpStatusCode.BadRequest,

            };
        }
          
        public HttpResponse BadRequest<TResponse>(ValidationResult validation, string transactionId = "") where TResponse : class, new()
        {

            if (validation == null)
            {
                validation = new ValidationResult();
            }
            var response = (new HttpStandardResponse<TResponse>
            {
                Response = null,
                Error = new Error().FormatErrors(validation),
                Status = false,
                TransactionId = string.IsNullOrWhiteSpace(transactionId) ? Guid.NewGuid().ToString() : transactionId
            });
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.BadRequest,
            };
        }
        public HttpResponse InternalServerError<TResponse>(TResponse response) where TResponse : class, new()
        {
            _logger.Error("#InternalServerError {@Response}", response ?? new TResponse());
            return new HttpResponse
            {
                Response = response,
                HttpStatusCode = HttpStatusCode.InternalServerError,
            };
        }
    }
}