using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoEF.Interception
{
    public class PropertyLoggingInterceptor : IInterceptor
    {
        private readonly ISet<PropertyInfo> _callLog;

        public PropertyLoggingInterceptor(ISet<PropertyInfo> callLog)
        {
            _callLog = callLog;
        }

        public void Intercept(IInvocation invocation)
        {
            var prop = invocation.InvocationTarget.GetType()
                .GetProperty(invocation.Method.Name.Substring(4), BindingFlags.Public | BindingFlags.Instance);

            _callLog.Add(prop);
        }
    }
}
