using System;
using System.Collections.Generic;

namespace AutoFixture.AutoEF
{
    public interface IEntityTypesProvider
    {
        IEnumerable<Type> GetTypes();
    }
}