#region Assembly
using FluentValidation.Results;
#endregion
namespace NativoPlusStudio.RequestResponsePattern
{
    public partial class Error 
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public  IList<Error> FormatErrors (ValidationResult validation)
        {
            //Attempting to guard againts null exception and return value right away. 
            if (validation == null)
            {
                return new List<Error>();
            }
            else if (!validation.IsValid)
            {

                return validation?.Errors
                                  .Select(x => new Error { Code = x.ErrorCode, Message = x.ErrorMessage })
                                  .ToList();
            }
            return new List<Error>();
        }
    }

}
