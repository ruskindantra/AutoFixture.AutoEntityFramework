using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoFixture.AutoEF.Tests.ContextTests;
using Xunit;

namespace AutoFixture.AutoEF.Tests
{
    public class DbModelExploration
    {
        private DbContext Get<TContext>() where TContext : DbContext, new()
        {
            Database.SetInitializer<TContext>(null);
            return new TContext();
        }

        [Fact]
        public void TestOptionalToRequired()
        {
            using (var context = Get<OptionalToRequiredContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestConfigureOptionalToOptional()
        {
            using (var context = Get<OptionalToOptionalContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestNoKey()
        {
            using (var context = Get<NoKeyContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestUnidirectional()
        {
            using (var context = Get<UnidirectionalContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestUnconventionalKey()
        {
            using (var context = Get<UnconventionalKeyContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestOneToMany()
        {
            using (var context = Get<OneToManyContext>())
                GetAllForeignKeys(context);
        }

        [Fact]
        public void TestManyToMany()
        {
            using (var context = Get<ManyToManyContext>())
                GetAllForeignKeys(context);
        }
    }

    public class DbContextGraphFactory
    {
        public IEnumerable<IRelationship> GetRelationships(DbContext context)
        {
            var metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

            var oCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

            var entities = metadata.GetItems<EntityType>(DataSpace.CSpace).ToList();

            var clrTypes = entities.ToDictionary(
                e => e,
                e => oCollection.GetClrType(metadata.GetItem<EntityType>(e.FullName, DataSpace.OSpace)));

            var props =
                from origin in entities
                from p in origin.NavigationProperties
                let dest = p.ToEndMember.GetEntityType()
                let originMultiplicity = p.RelationshipType.RelationshipEndMembers.First(x => x.GetEntityType() == origin).RelationshipMultiplicity
                let destMultiplicity = p.RelationshipType.RelationshipEndMembers.First(x => x.GetEntityType() == dest).RelationshipMultiplicity
                select new NavProperty
                {
                    From = clrTypes[origin],
                    To = clrTypes[dest],
                    ForeignKeyName = p.GetDependentProperties().Select(dp => dp.Name).FirstOrDefault(),
                    NavPropertyName = p.Name,
                    Relationship = p.RelationshipType,
                };

            return props
                .GroupBy(p => p.Relationship)
                .Select(GetRelationship);
        }

        private IRelationship GetRelationship(IEnumerable<NavProperty> properties)
        {
            var list = properties.ToList();
            if (list.Count == 0)
                throw new ArgumentException("properties is empty");
            if (list.Select(np => np.Relationship).Distinct().Count() > 1)
                throw new ArgumentException("properties should share relationship");

            if (list.Count == 1)
                return new Single(list[0]);
            if (list.Count == 2)
                return new Pair(list[0], list[1]);

            throw new InvalidOperationException("more than two properties share a relationship");
        }
    }

    public class Relationship
    {
        public class Unidirectional : Relationship
        {
            public NavProperty X { get; set; }

            public Unidirectional(NavProperty x)
            {
                X = x;
            }
        }
    }

    public interface IRelationship
    {
    }

    public class Single : IRelationship
    {
        public NavProperty X { get; private set; }

        public Single(NavProperty x)
        {
            X = x;
        }
    }

    public class Pair : IRelationship
    {
        public NavProperty A { get; private set; }
        public NavProperty B { get; private set; }

        public Pair(NavProperty a, NavProperty b)
        {
            A = a;
            B = b;
        }
    }

    public class NavProperty
    {
        public Type From { get; set; }
        public Type To { get; set; }
        public string NavPropertyName { get; set; }
        public string ForeignKeyName { get; set; }
        public RelationshipType Relationship { get; set;}
    }
}
