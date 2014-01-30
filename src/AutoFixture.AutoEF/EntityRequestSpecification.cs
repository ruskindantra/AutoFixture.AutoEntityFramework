using Ploeh.AutoFixture.Kernel;
using System;
using System.Linq;

namespace AutoFixture.AutoEF
{
    public class EntityRequestSpecification : IRequestSpecification
    {
        private readonly IEntityTypesProvider _entityTypesProvider;

        public EntityRequestSpecification(IEntityTypesProvider entityTypesProvider)
        {
            _entityTypesProvider = entityTypesProvider;
        }

        public bool IsSatisfiedBy(object request)
        {
            var type = request as Type;
            return type != null && _entityTypesProvider.GetTypes().Contains(type);
        }
    }
}
