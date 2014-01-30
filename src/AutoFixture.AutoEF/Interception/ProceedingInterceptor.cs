using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class ProceedingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            invocation.Proceed();
        }
    }
}