using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture;
using System;
using Xunit;

namespace AutoFixture.AutoEF.Tests
{
    public class EntityCustomizationTest
    {
        [Fact]
        public void InitializeWithNullTargetTypeThrowsArgumentNullException()
        {
            // Fixture setup
            Action act = () => new EntityCustomization(null);
            // Exercise system and verify outcome
            act.ShouldThrow<ArgumentNullException>();
            // Teardown
        }

        [Fact]
        public void SutIsCustomization()
        {
            // Fixture setup
            var typesProvider = Substitute.For<IEntityTypesProvider>();
            // Exercise system
            var sut = new EntityCustomization(typesProvider);
            // Verify outcome
            sut.Should().BeAssignableTo<ICustomization>();
            // Teardown
        }
    }
}
