using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    public class SetPropertyReturnValueInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            var propertyName = invocation.Method.Name.Substring(4);
            var target = invocation.InvocationTarget;

            target.GetType().GetProperty(propertyName)
                .SetValue(target, invocation.ReturnValue);
        }
    }
}
