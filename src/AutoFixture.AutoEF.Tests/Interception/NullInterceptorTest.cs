using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class NullInterceptorTest
    {
        [Theory, AutoData]
        public void SutIsInterceptor(NullInterceptor sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }
    }
}
