using Castle.DynamicProxy;
using System;
using System.Reflection;

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
            
            var bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            var idProp = target.GetType().GetProperty(propertyName + "Id", bindingFlags);
            var proxyIdProp = invocation.ReturnValue.GetType().GetProperty("Id", bindingFlags);

            if (proxyIdProp == null)
            {
                proxyIdProp = invocation.ReturnValue.GetType().GetProperty(propertyName + "Id", bindingFlags);
            }

            if (idProp != null && proxyIdProp != null)
            {
                proxyIdProp.SetValue(invocation.ReturnValue, idProp.GetValue(target));
            }
        }
    }
}
