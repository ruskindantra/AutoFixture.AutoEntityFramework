using Castle.DynamicProxy;
using System;
using System.Collections;
using System.Linq;

namespace AutoFixture.AutoEF.Interception
{
    /// <summary>
    /// Sets the parent property so that foo.Bar.Foo == foo
    /// </summary>
    public class ParentPropertySetterInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException("invocation");

            var parent = invocation.InvocationTarget;

            var collection = invocation.ReturnValue as ICollection;
            if (collection != null)
            {
                foreach (var child in collection)
                    SetProperties(parent, child);
            }
            else
            {
                SetProperties(parent, invocation.ReturnValue);
            }
        }

        private void SetProperties(object parent, object child)
        {
            var matching = from prop in child.GetType().GetProperties()
                           where prop.PropertyType.IsInstanceOfType(parent)
                           select prop;

            if (matching.Count() != 1)
                return;

            var parentProperty = matching.Single();

            parentProperty.SetValue(child, parent);

            var childIdProperty = child.GetType().GetProperty(parentProperty.Name + "Id");
            var parentIdProperty = parent.GetType().GetProperty("Id");

            if (childIdProperty != null && parentIdProperty != null)
            {
                childIdProperty.SetValue(child, parentIdProperty.GetValue(parent));
            }
        }
    }
}
