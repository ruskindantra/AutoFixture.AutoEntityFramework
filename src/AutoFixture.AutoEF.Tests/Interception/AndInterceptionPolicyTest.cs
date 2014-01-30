using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class AndInterceptionPolicyTest
    {
        [Fact]
        public void ThrowsOnNullPolicies()
        {
            Action act = () => new AndInterceptionPolicy(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SutIsInterceptionPolicy(AndInterceptionPolicy sut)
        {
            sut.Should().BeAssignableTo<IInterceptionPolicy>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(AndInterceptionPolicy sut)
        {
            sut.Invoking(s => s.ShouldIntercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ReturnsTrueWhenNoPolicies(IInvocation invocation)
        {
            var sut = new AndInterceptionPolicy();

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeTrue();
        }

        [Theory]
        [InlineAutoNSub(true)]
        [InlineAutoNSub(false)]
        public void ReturnsValueOfSinglePolicy(bool expected, IInterceptionPolicy policy, IInvocation invocation)
        {
            policy.ShouldIntercept(invocation).Returns(expected);

            var sut = new AndInterceptionPolicy(policy);

            var result = sut.ShouldIntercept(invocation);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineAutoNSub(false, false, false)]
        [InlineAutoNSub(false, true, false)]
        [InlineAutoNSub(true, false, false)]
        [InlineAutoNSub(true, true, true)]
        public void ReturnsAllOfMultiplePolicies(bool a, bool b, bool expected,
            IInterceptionPolicy policyA,
            IInterceptionPolicy policyB,
            IInvocation invocation)
        {
            policyA.ShouldIntercept(invocation).Returns(a);
            policyB.ShouldIntercept(invocation).Returns(b);

            var sut = new AndInterceptionPolicy(policyA, policyB);

            var result = sut.ShouldIntercept(invocation);

            result.Should().Be(expected);
        }
    }
}
