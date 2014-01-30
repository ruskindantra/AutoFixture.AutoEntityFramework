using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class SetPropertyReturnValueInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4);
            var target = invocation.InvocationTarget;

            target.GetType().GetProperty(propertyName)
                .SetValue(target, invocation.ReturnValue);
        }
    }
}
