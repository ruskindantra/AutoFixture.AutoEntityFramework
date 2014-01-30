using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class ReturnValueOverrideInterceptor : IInterceptor
    {
        private readonly Func<IInvocation, object> _objectFactory;

        public ReturnValueOverrideInterceptor(Func<IInvocation, object> objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.ReturnValue = _objectFactory(invocation);
        }
    }
}
