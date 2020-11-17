using System.Collections.Generic;

namespace NativoPlusStudio.RequestResponsePattern
{
    public partial class HttpStandardListResponse<T>
    {
        public bool Status { get; set; }
        public string TransactionId { get; set; }
        public T Response { get; set; }
        public int TotalCount { get; set; }
        public IList<Error> Error { get; set; }
    }
}
