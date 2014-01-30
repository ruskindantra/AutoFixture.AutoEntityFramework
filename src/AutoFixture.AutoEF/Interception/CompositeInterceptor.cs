using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

namespace AutoFixture.AutoEF.Interception
{
    public class CompositeInterceptor : IInterceptor
    {
        private readonly IEnumerable<IInterceptor> _interceptors;

        public CompositeInterceptor(params IInterceptor[] interceptors)
            : this((IEnumerable<IInterceptor>)interceptors)
        { }

        public CompositeInterceptor(IEnumerable<IInterceptor> interceptors)
        {
            if (interceptors == null)
                throw new ArgumentNullException("interceptors");

            _interceptors = interceptors;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            foreach (var interceptor in _interceptors)
                interceptor.Intercept(invocation);
        }
    }
}