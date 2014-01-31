using AutoFixture.AutoEF.Tests.MockEntities;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests
{
    using SUT = NavigationPropertyRequestSpecification;

    public class NavigationPropertyRequestSpecificationTest
    {
        [Theory, AutoNSub]
        public void SutIsRequestSpecification(SUT sut)
        {
            sut.Should().BeAssignableTo<IRequestSpecification>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullRequest(SUT sut)
        {
            sut.Invoking(s => s.IsSatisfiedBy(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ReturnsFalseWhenDeclaringTypeNotRegistered([Frozen] IEntityTypesProvider typesProvider, SUT sut)
        {
            typesProvider.GetTypes().Returns(new[] { typeof(Bar) });
            var property = typeof (Foo).GetProperty("Bar");

            var result = sut.IsSatisfiedBy(property);

            result.Should().BeFalse();
        }

        [Theory, AutoNSub]
        public void ReturnsFalseWhenPropertyTypeNotRegistered([Frozen] IEntityTypesProvider typesProvider, SUT sut)
        {
            typesProvider.GetTypes().Returns(new[] { typeof(Foo) });
            var property = typeof(Foo).GetProperty("Bar");

            var result = sut.IsSatisfiedBy(property);

            result.Should().BeFalse();
        }

        [Theory, AutoNSub]
        public void ReturnsTrueWhenBothTypesRegistered([Frozen] IEntityTypesProvider typesProvider, SUT sut)
        {
            typesProvider.GetTypes().Returns(new[] { typeof(Foo), typeof(Bar) });
            var property = typeof(Foo).GetProperty("Bar");

            var result = sut.IsSatisfiedBy(property);

            result.Should().BeTrue();
        }

        [Theory, AutoNSub]
        public void ReturnsTrueWhenPropertyIsCollection([Frozen] IEntityTypesProvider typesProvider, SUT sut)
        {
            typesProvider.GetTypes().Returns(new[] { typeof(Qux), typeof(Bar) });
            var property = typeof(Bar).GetProperty("Quxes");

            var result = sut.IsSatisfiedBy(property);

            result.Should().BeTrue();
        }
    }
}
