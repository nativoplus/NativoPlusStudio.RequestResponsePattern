#region Assembly
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
#endregion
namespace NativoPlusStudio.RequestResponsePattern
{
    public partial class Error 
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public  IList<Error> FormatErrors (ValidationResult validation)
        {
            if (!validation.IsValid)
                return validation?.Errors
                                  .Select(x => new Error { Code = x.ErrorCode, Message = x.ErrorMessage })
                                  .ToList();
            else return new List<Error>();
        
        }
    }

}
