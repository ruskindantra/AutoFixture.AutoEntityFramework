using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;

namespace AutoFixture.AutoEF
{
    public class EntitySpecimenBuilder : ISpecimenBuilder
    {
        private readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public object Create(object request, ISpecimenContext context)
        {
            return _proxyGenerator.CreateClassProxy((Type) request, 
                    new CompositeInterceptor(
                        // first allow the invocation to proceed
                        new ProceedingInterceptor(),
                        // then check -
                        new FilteringInterceptor(    
                            new AndInterceptionPolicy(
                                // the invocation was a property getter
                                new PropertyGetterInterceptionPolicy(), 
                                // and the return value was null or empty collection
                                new OrInterceptionPolicy(               
                                    new NullReturnValueInterceptionPolicy(),
                                    new EmptyCollectionReturnValueInterceptionPolicy())),
                            // if the above conditions pass
                            new CompositeInterceptor(
                                // override the return value with an AutoFixture resolved object
                                new ReturnValueOverrideInterceptor(i => context.Resolve(i.Method.ReturnType)),
                                // set the Id property of the new object
                                new IdPropertySetterInterceptor(),
                                // then persist the property value
                                new SetPropertyReturnValueInterceptor()))));
        }
    }
}