using AutoFixture.AutoEF.Interception;
using AutoFixture.AutoEF.Tests.MockEntities;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = SetPropertyReturnValueInterceptor;

    public class SetPropertyReturnValueInterceptorTest
    {
        [Theory, AutoData]
        public void SutIsInterceptor(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoData]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.Intercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SetsPropertyOnValueReturn(SUT sut, IInvocation invocation)
        {
            // Setup fixture
            var foo = new Foo();
            var bar = new Bar();
            invocation.Method.Returns(foo.GetType().GetProperty("Bar").GetGetMethod());

            invocation.InvocationTarget.Returns(foo);
            invocation.ReturnValue = bar;

            // Exercise system
            sut.Intercept(invocation);

            // Verify outcome
            foo.Bar.Should().Be(bar);
        }
    }
}
