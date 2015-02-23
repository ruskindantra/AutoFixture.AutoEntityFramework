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

        public void GetAllForeignKeys(DbContext context)
        {
            var metadata = ((IObjectContextAdapter) context).ObjectContext.MetadataWorkspace;

            var oCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

            var entities = metadata.GetItems<EntityType>(DataSpace.CSpace).ToList();

            var clrTypes = entities.ToDictionary(
                e => e,
                e => oCollection.GetClrType(metadata.GetItem<EntityType>(e.FullName, DataSpace.OSpace)));

            var props = 
                from e in entities
                from p in e.NavigationProperties
                let toType = p.ToEndMember.GetEntityType()
                select new
                {
                    FromType = clrTypes[e],
                    ToType = clrTypes[toType],
                    ForeignKey = p.GetDependentProperties().Select(dp => dp.Name).FirstOrDefault(),
                    NavPropName = p.Name,
                    p.RelationshipType,
                };

            // NavigationProperty.RelationshipType is what joins two NavigationProperties
            var res = props.ToList();
        }
    }
}
