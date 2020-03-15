using FluentValidation.Results;
using System;

namespace NativoPlusStudio.RequestResponsePattern
{
    public static class HttpResponseHelpers
    {
        public static HttpStandardResponse<object> BadRequestHelper(string transactionId, ValidationResult validation)
        {
            if (validation == null)
            {
                validation = new ValidationResult();
            }
            return (new HttpStandardResponse<object>
            {
                Response = null,
                Error = new Error().FormatErrors(validation),
                Status = false,
                TransactionId = transactionId ?? Guid.NewGuid().ToString()
            });
        }
    }
}
