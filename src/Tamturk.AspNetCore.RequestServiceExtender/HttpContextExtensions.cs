using Microsoft.AspNetCore.Http;
using System;

namespace Tamturk.AspNetCore.RequestServiceExtender {
    public static class HttpContextExtensions {
        public static HttpContext AddScoped<T>(this HttpContext context, T data) {
            return context.AddScoped(() => data);
        }

        public static HttpContext AddScoped<T>(this HttpContext context, Func<T> data) {
            if(context.RequestServices as IDisposable != null) {
                context.RequestServices = new DisposableProxyServiceProvider<T>(context.RequestServices, data);
            } else {
                context.RequestServices = new ProxyServiceProvider<T>(context.RequestServices, data);
            }

            return context;
        }
    }
}
