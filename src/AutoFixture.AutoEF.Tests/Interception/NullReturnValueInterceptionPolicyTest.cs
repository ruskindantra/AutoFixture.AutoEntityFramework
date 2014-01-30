using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = NullReturnValueInterceptionPolicy;

    public class NullReturnValueInterceptionPolicyTest
    {
        [Theory, AutoData]
        public void SutIsInterceptionPolicy(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptionPolicy>();
        }

        [Theory, AutoData]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.ShouldIntercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void NullReturnValueReturnsTrue(SUT sut, IInvocation invocation)
        {
            invocation.ReturnValue = null;

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeTrue();
        }

        [Theory, AutoNSub]
        public void NonNullReturnValueReturnsFalse(SUT sut, IInvocation invocation, object value)
        {
            invocation.ReturnValue = value;

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeFalse();
        }
    }
}
