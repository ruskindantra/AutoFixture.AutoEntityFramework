using AutoFixture.AutoEF.Tests.MockEntities;
using FluentAssertions;
using System;
using Xunit;

namespace AutoFixture.AutoEF.Tests
{
    public class DbContextEntityTypesProviderTest
    {
        [Fact]
        public void NullTypeArgumentThrowsException()
        {
            // Fixture setup
            Action act = () => new DbContextEntityTypesProvider(null);
            // Exercise system and verify outcome
            act.ShouldThrow<ArgumentNullException>();
            // Teardown
        }

        [Fact]
        public void SutIsEntityTypesProvider()
        {
            // Fixture setup
            // Exercise system
            var sut = new DbContextEntityTypesProvider(typeof(MockDbContext));
            // Verify outcome
            sut.Should().BeAssignableTo<IEntityTypesProvider>();
            // Teardown
        }

        [Fact]
        public void AllEntityTypesReturned()
        {
            // Fixture setup
            var sut = new DbContextEntityTypesProvider(typeof (MockDbContext));
            // Exercise system
            var types = sut.GetTypes();
            // Verify outcome
            types.Should().BeEquivalentTo(typeof (Foo), typeof (Bar), typeof(Qux));
            // Teardown
        }
    }
}
