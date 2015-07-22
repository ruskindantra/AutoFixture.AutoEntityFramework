using AutoFixture.AutoEF.Tests.MockEntities;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using System;
using System.Collections.Generic;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests
{
    public class CachedEntityTypesProviderTest
    {
        [Theory, AutoNSub]
        public void SutIsEntityTypesProvider(CachedEntityTypesProvider sut)
        {
            sut.Should().BeAssignableTo<IEntityTypesProvider>();
        }

        [Theory, AutoNSub]
        public void SutIsDecorator([Frozen] IEntityTypesProvider decorated, CachedEntityTypesProvider sut)
        {
            var types = new[] { typeof (Foo), typeof (Bar), typeof (Qux) };
            decorated.GetTypes().Returns(types);

            var result = sut.GetTypes();

            result.Should().BeEquivalentTo(types);
        }

        [Theory, AutoNSub]
        public void TypesEnumeratedOnlyOnce([Frozen] IEntityTypesProvider decorated, CachedEntityTypesProvider sut)
        {
            IEnumerable<Type> typesFirst = new[] { typeof(Foo), typeof(Bar) };
            IEnumerable<Type> typesSecond = new[] { typeof(Qux), typeof(object) };

            decorated.GetTypes().Returns(typesFirst, typesSecond);

            var resultFirst = sut.GetTypes();
            var resultSecond = sut.GetTypes();

            resultSecond.Should().BeEquivalentTo(typesFirst)
                .And.BeEquivalentTo(resultFirst);
        }

    }
}
