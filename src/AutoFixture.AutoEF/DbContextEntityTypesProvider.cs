using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    /// <summary>
    /// Builds list of EF types by enumerating through DbSet properties on a DbContext subclass
    /// </summary>
    public class DbContextEntityTypesProvider : IEntityTypesProvider
    {
        private readonly Type _dbContext;

        public DbContextEntityTypesProvider(Type dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            _dbContext = dbContext;
        }

        public IEnumerable<Type> GetTypes()
        {
            return from prop in _dbContext.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                   let t = prop.PropertyType 
                   where t.IsGenericType
                       && t.Name.Remove(t.Name.IndexOf('`')) == "DbSet"
                   select t.GenericTypeArguments[0];
        }
    }
}