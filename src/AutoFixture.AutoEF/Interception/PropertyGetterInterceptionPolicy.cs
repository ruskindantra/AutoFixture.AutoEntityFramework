using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class PropertyGetterInterceptionPolicy : IInterceptionPolicy
    {
        public bool ShouldIntercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            return invocation.Method.IsSpecialName
                && invocation.Method.ReturnType != typeof (void);
        }
    }
}