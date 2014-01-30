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
            return _proxyGenerator.CreateClassProxy((Type)request, new EntityInterceptor(context));
        }
    }
}