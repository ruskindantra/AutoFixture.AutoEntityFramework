using AutoFixture.AutoEF.Interception;
using AutoFixture.AutoEF.Tests.MockEntities;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = IdPropertySetterInterceptor;

    public class IdPropertySetterInterceptorTest
    {
        [Theory, AutoNSub]
        public void SutIsInterceptor(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.Intercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SetsIdOnMatchingProperty(SUT sut, IInvocation invocation, int barId)
        {
            var foo = new Foo { BarId = barId };
            var bar = new Bar();

            invocation.Method.Returns(foo.GetType().GetProperty("Bar").GetGetMethod());
            invocation.InvocationTarget.Returns(foo);
            invocation.ReturnValue = bar;

            sut.Intercept(invocation);

            bar.Id.Should().Be(barId);
        }
    }
}
