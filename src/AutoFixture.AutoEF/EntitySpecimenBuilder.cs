using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    public class EntitySpecimenBuilder : ISpecimenBuilder
    {
        private readonly IEntityTypesProvider _entityTypesProvider;
        private ISet<Type> _entityTypes;
        private readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        public EntitySpecimenBuilder(IEntityTypesProvider entityTypesProvider)
        {
            _entityTypesProvider = entityTypesProvider;
        }

        private ISet<Type> EntityTypes
        {
            get { return _entityTypes ?? (_entityTypes = new HashSet<Type>(_entityTypesProvider.GetTypes())); }
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;
            if (EntityTypes.Contains(t))
                return _proxyGenerator.CreateClassProxy(t, new EntityInterceptor(context));

            // Ignore navigation properties on EF objects (these are handled by the DynamicProxy)
            if (IsNavigationProperty(request as PropertyInfo))
                return new OmitSpecimen();

            // Ignore DynamicProxy interceptor field
            if (IsDynamicProxyField(request as FieldInfo))
                return new OmitSpecimen();

            return new NoSpecimen(request);
        }

        private bool IsNavigationProperty(PropertyInfo pi)
        {
            return pi != null
                && EntityTypes.Contains(pi.DeclaringType.BaseType)
                && EntityTypes.Contains(pi.PropertyType);
        }

        private static bool IsDynamicProxyField(FieldInfo fi)
        {
            return fi != null
                && fi.FieldType == typeof (IInterceptor[]);
        }
    }
}