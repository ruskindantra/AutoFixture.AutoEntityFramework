using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class OrInterceptionPolicyTest
    {
        [Fact]
        public void ThrowsOnNullPolicies()
        {
            Action act = () => new OrInterceptionPolicy(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SutIsInterceptionPolicy(OrInterceptionPolicy sut)
        {
            sut.Should().BeAssignableTo<IInterceptionPolicy>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(OrInterceptionPolicy sut)
        {
            sut.Invoking(s => s.ShouldIntercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ReturnsFalseWhenNoPolicies(IInvocation invocation)
        {
            var sut = new OrInterceptionPolicy();

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineAutoNSub(true)]
        [InlineAutoNSub(false)]
        public void ReturnsValueOfSinglePolicy(bool expected, IInterceptionPolicy policy, IInvocation invocation)
        {
            policy.ShouldIntercept(invocation).Returns(expected);

            var sut = new OrInterceptionPolicy(policy);

            var result = sut.ShouldIntercept(invocation);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineAutoNSub(false, false, false)]
        [InlineAutoNSub(false, true, true)]
        [InlineAutoNSub(true, false, true)]
        [InlineAutoNSub(true, true, true)]
        public void ReturnsAllOfMultiplePolicies(bool a, bool b, bool expected,
            IInterceptionPolicy policyA,
            IInterceptionPolicy policyB,
            IInvocation invocation)
        {
            policyA.ShouldIntercept(invocation).Returns(a);
            policyB.ShouldIntercept(invocation).Returns(b);

            var sut = new OrInterceptionPolicy(policyA, policyB);

            var result = sut.ShouldIntercept(invocation);

            result.Should().Be(expected);
        }
    }
}
