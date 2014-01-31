using AutoFixture.AutoEF.Tests.MockEntities;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
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
    }
}
