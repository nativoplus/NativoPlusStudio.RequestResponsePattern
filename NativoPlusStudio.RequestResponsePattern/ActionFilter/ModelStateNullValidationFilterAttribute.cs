using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace NativoPlusStudio.RequestResponsePattern.ActionFilter
{
    public class ModelStateNullValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly Func<Dictionary<string, object>, bool> _validate = args => args.ContainsValue(null);

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var methodType = context?.HttpContext?.Request?.Method ?? "Unknown";
            var actionArgumentsDictionary = new Dictionary<string, object>(context.ActionArguments);

            if (!context.ModelState.IsValid && !_validate(actionArgumentsDictionary) && (methodType == "POST" || methodType == "PUT"))
            {

                var controllerName = context?.ActionDescriptor?.DisplayName ?? "UnknownController";
                var errors = (from modelState in context.ModelState.Values
                              from error in modelState.Errors
                              select new Error
                              {
                                  Code = controllerName,
                                  Message = error?.ErrorMessage ?? "We could not find a reason for the error. Very strange indeed"
                              }).ToList();
                var mresponse = (new HttpStandardResponse<object>
                {
                    Response = null,
                    Error = errors,
                    Status = false,
                    TransactionId = Guid.NewGuid().ToString()
                });
                context.Result = new BadRequestObjectResult(mresponse);
            }

            base.OnActionExecuting(context);
        }

    }
}
