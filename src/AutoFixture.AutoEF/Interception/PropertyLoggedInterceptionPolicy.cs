using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoEF.Interception
{
    public class PropertyLoggedInterceptionPolicy : IInterceptionPolicy
    {
        private readonly ISet<PropertyInfo> _callLog;

        public PropertyLoggedInterceptionPolicy(ISet<PropertyInfo> callLog)
        {
            _callLog = callLog;
        }

        public bool ShouldIntercept(IInvocation invocation)
        {
            var prop = invocation.InvocationTarget.GetType()
                .GetProperty(invocation.Method.Name.Substring(4), BindingFlags.Public | BindingFlags.Instance);

            return _callLog.Contains(prop);
        }
    }
}
