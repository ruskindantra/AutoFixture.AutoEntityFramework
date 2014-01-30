using Castle.DynamicProxy;
using System;

namespace AutoFixture.AutoEF.Interception
{
    /// <summary>
    /// Sets the "Id" property of the invocation.ReturnValue to
    /// the value of a matching target.___Id property if it exists
    /// </summary>
    public class IdPropertySetterInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            var propertyName = invocation.Method.Name.Substring(4);
            var target = invocation.InvocationTarget;

            var idProp = target.GetType().GetProperty(propertyName + "Id");
            var proxyIdProp = invocation.ReturnValue.GetType().GetProperty("Id");

            if (idProp != null && proxyIdProp != null)
            {
                proxyIdProp.SetValue(invocation.ReturnValue, idProp.GetValue(target));
            }
        }
    }
}
