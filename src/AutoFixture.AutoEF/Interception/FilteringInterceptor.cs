using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class FilteringInterceptor : IInterceptor
    {
        private readonly IInterceptor _interceptor;
        private readonly IInterceptionPolicy _policy;
        private readonly IInterceptor _elseInterceptor;

        public FilteringInterceptor(IInterceptionPolicy policy, IInterceptor interceptor, IInterceptor elseInterceptor)
        {
            if (policy == null)
                throw new ArgumentNullException("policy");
            if (interceptor == null)
                throw new ArgumentNullException("interceptor");
            if (elseInterceptor == null)
                throw new ArgumentNullException("elseInterceptor");

            _interceptor = interceptor;
            _policy = policy;
            _elseInterceptor = elseInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_policy.ShouldIntercept(invocation))
                _interceptor.Intercept(invocation);
            else
                _elseInterceptor.Intercept(invocation);
        }
    }
}