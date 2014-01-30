using System;
using System.Collections.Generic;

namespace AutoFixture.AutoEF
{
    public class CachedEntityTypesProvider : IEntityTypesProvider
    {
        private readonly Lazy<HashSet<Type>> _types;

        public CachedEntityTypesProvider(IEntityTypesProvider decorated)
        {
            _types = new Lazy<HashSet<Type>>(() => new HashSet<Type>(decorated.GetTypes()));
        }

        public IEnumerable<Type> GetTypes()
        {
            return _types.Value;
        }
    }
}
