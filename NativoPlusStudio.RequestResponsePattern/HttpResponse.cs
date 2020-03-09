#region Assemblies
using System.Net;
#endregion
namespace NativoPlusStudio.RequestResponsePattern
{
    public partial class HttpResponse
    {
        public virtual object Response { get; set; }
        public virtual HttpStatusCode HttpStatusCode { get; set; }
        public virtual bool Succeeded => HttpStatusCode.OK == HttpStatusCode || HttpStatusCode.Created == HttpStatusCode || HttpStatusCode.Accepted == HttpStatusCode;
    }
}

