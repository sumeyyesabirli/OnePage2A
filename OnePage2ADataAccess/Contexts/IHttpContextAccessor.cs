using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace OnePage2ADataAccess.Contexts
{
    public interface IHttpContextAccessor
    {
        HttpContext HttpContext { get; set; }
    }

    public class HttpContextAccessor : IHttpContextAccessor
    {
        private static AsyncLocal<HttpContextHolder> _httpContextCurrent = new AsyncLocal<HttpContextHolder>();

        public HttpContext HttpContext
        {
            get => _httpContextCurrent.Value?.Context;
            set
            {
                var holder = _httpContextCurrent.Value;
                if (holder != null)
                {
                    holder.Context = null;
                }

                if (value != null)
                {
                    _httpContextCurrent.Value = new HttpContextHolder { Context = value };
                }
            }
        }

        private class HttpContextHolder
        {
            public HttpContext Context;
        }
    }
}
