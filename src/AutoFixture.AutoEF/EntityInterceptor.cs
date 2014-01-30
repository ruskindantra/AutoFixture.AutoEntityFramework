using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections;
using System.Linq;

namespace AutoFixture.AutoEF
{
    public class EntityInterceptor : IInterceptor
    {
        private readonly ISpecimenContext _context;

        public EntityInterceptor(ISpecimenContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            if (!IsEmptyNavigationPropertyGetter(invocation))
                return;

            var propertyName = invocation.Method.Name.Substring(4);
            var target = invocation.InvocationTarget;

            // Generate populated proxy class through AutoFixture
            var generatedProxy = _context.Resolve(invocation.Method.ReturnType);

            // If the target object has a matching xxxxId property, set it for consistency
            var idProp = target.GetType().GetProperty(propertyName + "Id");
            var proxyIdProp = generatedProxy.GetType().GetProperty("Id");

            if (idProp != null && proxyIdProp != null)
            {
                proxyIdProp.SetValue(generatedProxy, idProp.GetValue(target));
            }

            invocation.ReturnValue = generatedProxy;
            
            // Set property on target so that repeated queries return the same object
            target.GetType().GetProperty(propertyName).SetValue(target, generatedProxy);
        }

        /// <summary>
        /// Determines whether an invocation is the getter for
        /// either a null navigation property or an empty collection
        /// </summary>
        private static bool IsEmptyNavigationPropertyGetter(IInvocation invocation)
        {
            if (invocation.Method.ReturnType == typeof (void)
                || !invocation.Method.IsSpecialName)
                return false;

            if (invocation.ReturnValue == null)
                return true;

            var collection = invocation.ReturnValue as IEnumerable;
            return collection != null && !collection.Cast<object>().Any();
        }
    }
}