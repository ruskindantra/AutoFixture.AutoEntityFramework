using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Linq;

namespace AutoFixture.AutoEF.Interception
{
    public class EmptyCollectionReturnValueInterceptionPolicy : IInterceptionPolicy
    {
        public bool ShouldIntercept(IInvocation invocation)
        {
            var t = invocation.ReturnValue.GetType();
            if (!t.IsGenericType)
                return false;

            // Get the implemented ICollection<T> interface type
            var iCollection = t
                .GetInterfaces()
                .SingleOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof (ICollection<>));

            if (iCollection == null)
                return false;

            var count = iCollection.GetProperty("Count", typeof (int));
            return (int)count.GetValue(invocation.ReturnValue) == 0;
        }
    }
}