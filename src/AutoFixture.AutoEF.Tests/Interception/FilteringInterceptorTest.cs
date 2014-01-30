using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class FilteringInterceptorTest
    {
        [Theory, AutoNSub]
        public void ThrowsOnNullPolicy(IInterceptor interceptor, IInterceptor elseInterceptor)
        {
            Action act = () => new FilteringInterceptor(null, interceptor, elseInterceptor);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInterceptor(IInterceptionPolicy policy, IInterceptor elseInterceptor)
        {
            Action act = () => new FilteringInterceptor(policy, null, elseInterceptor);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullElseInterceptor(IInterceptionPolicy policy, IInterceptor interceptor)
        {
            Action act = () => new FilteringInterceptor(policy, interceptor, null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SutIsInterceptor(FilteringInterceptor sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoNSub]
        public void TruePolicyDecisionCallsInterceptor(IInterceptionPolicy policy,
            IInterceptor interceptor,
            IInterceptor elseInterceptor,
            IInvocation invocation)
        {
            var sut = new FilteringInterceptor(policy, interceptor, elseInterceptor);
            policy.ShouldIntercept(invocation).Returns(true);

            sut.Intercept(invocation);

            interceptor.Received().Intercept(invocation);
        }

        [Theory, AutoNSub]
        public void FalsePolicyDecisionCallsElseInterceptor(IInterceptionPolicy policy,
            IInterceptor interceptor,
            IInterceptor elseInterceptor,
            IInvocation invocation)
        {
            var sut = new FilteringInterceptor(policy, interceptor, elseInterceptor);
            policy.ShouldIntercept(invocation).Returns(false);

            sut.Intercept(invocation);

            elseInterceptor.Received().Intercept(invocation);
        }

        [Theory, AutoNSub]
        public void FalsePolicyDecisionDoesntCallInterceptor(IInterceptionPolicy policy,
            IInterceptor interceptor,
            IInterceptor elseInterceptor,
            IInvocation invocation)
        {
            var sut = new FilteringInterceptor(policy, interceptor, elseInterceptor);
            policy.ShouldIntercept(invocation).Returns(true);

            sut.Intercept(invocation);

            elseInterceptor.DidNotReceive().Intercept(invocation);
        }

        [Theory, AutoNSub]
        public void TruePolicyDecisionDoesntCallElseInterceptor(IInterceptionPolicy policy,
            IInterceptor interceptor,
            IInterceptor elseInterceptor,
            IInvocation invocation)
        {
            var sut = new FilteringInterceptor(policy, interceptor, elseInterceptor);
            policy.ShouldIntercept(invocation).Returns(false);

            sut.Intercept(invocation);

            interceptor.DidNotReceive().Intercept(invocation);
        }
    }
}
