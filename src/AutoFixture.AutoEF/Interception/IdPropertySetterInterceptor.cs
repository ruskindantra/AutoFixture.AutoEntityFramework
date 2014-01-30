using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class IdPropertySetterInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var propertyName = invocation.Method.Name.Substring(4);
            var target = invocation.InvocationTarget;

            // If the target object has a matching xxxxId property, set it for consistency
            var idProp = target.GetType().GetProperty(propertyName + "Id");
            var proxyIdProp = invocation.ReturnValue.GetType().GetProperty("Id");

            if (idProp != null && proxyIdProp != null)
            {
                proxyIdProp.SetValue(invocation.ReturnValue, idProp.GetValue(target));
            }
        }
    }
}
