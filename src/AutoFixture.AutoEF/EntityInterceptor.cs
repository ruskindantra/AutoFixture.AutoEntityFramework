using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;

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

            if (!IsEmptyPropertyGetter(invocation))
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

        private static bool IsEmptyPropertyGetter(IInvocation invocation)
        {
            return invocation.Method.ReturnType != typeof (void)
                && invocation.Method.IsSpecialName
                && invocation.ReturnValue == null;
        }
    }
}