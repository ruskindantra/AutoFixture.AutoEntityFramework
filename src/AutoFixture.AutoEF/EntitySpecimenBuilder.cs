using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    public class EntitySpecimenBuilder : ISpecimenBuilder
    {
        private readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public object Create(object request, ISpecimenContext context)
        {
            var callLog = new HashSet<PropertyInfo>();
            var intercept = Do(
                ProceedWithCall,
                LogPropertySetters(callLog),
                If(IsPropertyGetter.And(PropertyNotSet(callLog), ReturnsNull.Or(ReturnsEmptyCollection)),
                   Do(OverrideReturnValue(i => context.Resolve(i.Method.ReturnType)),
                      SetIdOfNewObject,
                      SetInverseNavigationProperty,
                      PersistGeneratedValue)));

            var instance = _proxyGenerator.CreateClassProxy((Type)request, intercept);
            // ignore calls in constructor
            callLog.Clear();
            return instance;
        }

        private static IInterceptionPolicy PropertyNotSet(ISet<PropertyInfo> callLog)
        {
            return new InverseInterceptionPolicy(new PropertyLoggedInterceptionPolicy(callLog));
        }

        private static IInterceptionPolicy ReturnsNull { get { return new NullReturnValueInterceptionPolicy(); } }
        private static IInterceptionPolicy ReturnsEmptyCollection { get { return new EmptyCollectionReturnValueInterceptionPolicy(); } }
        private static IInterceptionPolicy IsPropertyGetter { get { return new PropertyGetterInterceptionPolicy(); } }
        private static IInterceptor ProceedWithCall { get { return new ProceedingInterceptor(); } }
        private static IInterceptor PersistGeneratedValue { get { return new SetPropertyReturnValueInterceptor(); } }
        private static IInterceptor SetInverseNavigationProperty { get { return new ParentPropertySetterInterceptor(); } }
        private static IInterceptor SetIdOfNewObject { get { return new IdPropertySetterInterceptor(); } }

        private static IInterceptor If(IInterceptionPolicy policy, IInterceptor interceptor)
        {
            return new FilteringInterceptor(policy, interceptor);
        }

        private static IInterceptor Do(params IInterceptor[] interceptors)
        {
            return new CompositeInterceptor(interceptors);
        }

        private static IInterceptor LogPropertySetters(ISet<PropertyInfo> callLog)
        {
            return If(
                new PropertySetterInterceptionPolicy(),
                new PropertyLoggingInterceptor(callLog));
        }

        private static IInterceptor OverrideReturnValue(Func<IInvocation, object> objectFactory)
        {
            return new ReturnValueOverrideInterceptor(objectFactory);
        }
    }
}