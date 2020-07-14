using NativoPlusStudio.RequestResponsePattern;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NativoPlusStudio.RequestResponsePatternTest
{
    public class MyRequest :IHttpRequest {
        public int Test { get; set; }
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
    }
    public class MyResponse {
        public int MyResultTest { get; set; }
    }
    public class Handler : HttpHandler<MyRequest>
    {
        private readonly new ILogger _logger;

        public Handler(ILogger logger) : base(logger) => _logger = logger 
            ?? throw new ArgumentNullException(nameof(logger));

        protected  async override  Task<HttpResponse> HandleAsync(MyRequest input, CancellationToken cancellationToken)
        {
            _logger.Information(nameof(HandleAsync));
            return await Task.FromResult(Ok(new MyResponse 
            {
                MyResultTest = 1
            }));
        }
    }
}
