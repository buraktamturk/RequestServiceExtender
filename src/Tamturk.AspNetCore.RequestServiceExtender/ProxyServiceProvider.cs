using System;

namespace Tamturk.AspNetCore.RequestServiceExtender {
    public class ProxyServiceProvider<T> : IServiceProvider {
        protected IServiceProvider provider;
        Func<T> fnc;

        public ProxyServiceProvider(IServiceProvider provider, T data) {
            this.provider = provider;
            this.fnc = () => data;
        }

        public ProxyServiceProvider(IServiceProvider provider, Func<T> fnc) {
            this.provider = provider;
            this.fnc = fnc;
        }

        public object GetService(Type serviceType) {
            return serviceType == typeof(T) ? fnc() : provider.GetService(serviceType);
        }
    }
}
