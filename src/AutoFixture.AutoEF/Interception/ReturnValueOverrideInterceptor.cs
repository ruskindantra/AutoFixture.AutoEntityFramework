using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class ReturnValueOverrideInterceptor : IInterceptor
    {
        private readonly Func<IInvocation, object> _objectFactory;

        public ReturnValueOverrideInterceptor(Func<IInvocation, object> objectFactory)
        {
            if (objectFactory == null)
                throw new ArgumentNullException("objectFactory");

            _objectFactory = objectFactory;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            invocation.ReturnValue = _objectFactory(invocation);
        }
    }
}
