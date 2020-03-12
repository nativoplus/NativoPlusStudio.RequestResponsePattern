using System;
using System.Collections.Generic;
using System.Text;

namespace NativoPlusStudio.RequestResponsePattern
{
    public partial  class HttpStandardResponse<T>
    {
        public bool Status { get; set; }
        public string TransactionId { get; set; }
        public T Response { get; set; }
        public IList<Error> Error { get; set; }
    }
}
