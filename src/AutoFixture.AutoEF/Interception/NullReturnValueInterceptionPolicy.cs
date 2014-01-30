using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class NullReturnValueInterceptionPolicy : IInterceptionPolicy
    {
        public bool ShouldIntercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            return invocation.ReturnValue == null;
        }
    }
}