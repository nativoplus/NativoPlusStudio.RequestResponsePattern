using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NativoPlusStudio.RequestResponsePattern
{
    public interface IHttpRequest : IRequest<IActionResult>
    {
    }
}
