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
                        new ProceedingInterceptor(),
                        new FilteringInterceptor(
                            new AndInterceptionPolicy(
                                new PropertyGetterInterceptionPolicy(),
                                new OrInterceptionPolicy(
                                    new NullReturnValueInterceptionPolicy(),
                                    new EmptyCollectionReturnValueInterceptionPolicy())),
                            new CompositeInterceptor(
                                new ReturnValueOverrideInterceptor(i => context.Resolve(i.Method.ReturnType)),
                                new IdPropertySetterInterceptor(),
                                new SetPropertyReturnValueInterceptor()), 
                            new NullInterceptor())));
        }
    }
}