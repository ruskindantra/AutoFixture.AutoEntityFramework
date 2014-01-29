using AutoFixture.AutoEF.Tests.MockEntities;
using FluentAssertions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests
{
    public class AutoEFDataAttribute : AutoDataAttribute
    {
        public AutoEFDataAttribute(Type dbContextType)
            : base(new Fixture()
                .Customize(new EntityCustomization(new DbContextEntityTypesProvider(dbContextType))))
        { }
    }

    public class IntegrationTests
    {
        [Theory, AutoEFData(typeof(MockDbContext))]
        public void FixtureCanCreateEFEntity(Foo foo)
        {
            foo.Should().NotBeNull();
        }

        [Theory, AutoEFData(typeof (MockDbContext))]
        public void GeneratedPropertyIsNotNull(Foo foo)
        {
            foo.Bar.Should().NotBeNull();
        }
        
        [Theory, AutoEFData(typeof (MockDbContext))]
        public void GeneratedCollectionIsNotEmpty(Bar bar)
        {
            bar.Quxes.Should().NotBeEmpty();
        }

        [Theory, AutoEFData(typeof (MockDbContext))]
        public void IdShouldMatchNavigationPropertyId(Foo foo)
        {
            foo.BarId.Should().Be(foo.Bar.Id);
        }

        [Theory, AutoEFData(typeof(MockDbContext))]
        public void RepeatedAccessYieldsSameObject(Foo foo)
        {
            var bar1 = foo.Bar;
            var bar2 = foo.Bar;

            bar1.Should().BeSameAs(bar2);
        }

        [Theory, AutoEFData(typeof (MockDbContext))]
        public void UnfrozenTypeYieldsDifferentObject(Bar bar, Foo foo)
        {
            foo.Bar.Should().NotBeSameAs(bar);
        }

        [Theory, AutoEFData(typeof(MockDbContext))]
        public void FrozenTypeYieldsSameObject([Frozen] Bar bar, Foo foo)
        {
            foo.Bar.Should().BeSameAs(bar);
        }
    }
}
