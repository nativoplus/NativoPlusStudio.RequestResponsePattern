namespace NativoPlusStudio.RequestResponsePattern
{
    public abstract class HttpRequest : IHttpRequest
    {
        public string TransactionId { get; set; }
    }
}
