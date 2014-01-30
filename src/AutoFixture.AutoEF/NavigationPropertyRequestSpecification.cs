﻿using Ploeh.AutoFixture.Kernel;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    public class NavigationPropertyRequestSpecification : IRequestSpecification
    {
        private readonly IEntityTypesProvider _entityTypesProvider;

        public NavigationPropertyRequestSpecification(IEntityTypesProvider entityTypesProvider)
        {
            _entityTypesProvider = entityTypesProvider;
        }

        public bool IsSatisfiedBy(object request)
        {
            var pi = request as PropertyInfo;
            if (pi == null)
                return false;

            if (!pi.GetGetMethod().IsVirtual)
                return false;

            var entityTypes = new HashSet<Type>(_entityTypesProvider.GetTypes());

            if (!entityTypes.Contains(pi.DeclaringType.BaseType))
                return false;

            if (entityTypes.Contains(pi.PropertyType))
                return true;

            var t = pi.PropertyType;
            return t.IsGenericType
                && t.GetGenericTypeDefinition() == typeof (ICollection<>)
                && entityTypes.Contains(t.GenericTypeArguments[0]);
        }
    }
}
