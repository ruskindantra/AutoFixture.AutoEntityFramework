using Ploeh.AutoFixture.Kernel;
using System.Collections.Generic;
using System.Linq;
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

            if (!_entityTypesProvider.GetTypes().Contains(pi.DeclaringType.BaseType))
                return false;

            if (_entityTypesProvider.GetTypes().Contains(pi.PropertyType))
                return true;

            var t = pi.PropertyType;
            return t.IsGenericType
                && t.GetGenericTypeDefinition() == typeof (ICollection<>)
                && _entityTypesProvider.GetTypes().Contains(t.GenericTypeArguments[0]);
        }
    }
}
