using System;

namespace Tamturk.AspNetCore.RequestServiceExtender {
    public class DisposableProxyServiceProvider<T> : ProxyServiceProvider<T>, IDisposable {
        public DisposableProxyServiceProvider(IServiceProvider provider, T data) : base(provider, data) {
        }

        public DisposableProxyServiceProvider(IServiceProvider provider, Func<T> fnc) : base(provider, fnc) {
        }

        public void Dispose() {
            (provider as IDisposable)?.Dispose();
        }
    }
}
