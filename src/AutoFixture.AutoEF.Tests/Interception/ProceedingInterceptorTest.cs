using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using AutoFixture.Xunit2;
using System;
using Xunit.Extensions;
using Theory = Xunit.TheoryAttribute;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class ProceedingInterceptorTest
    {
        [Theory, AutoData]
        public void SutIsInterceptor(ProceedingInterceptor sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoData]
        public void ThrowsOnNullInvocation(ProceedingInterceptor sut)
        {
            sut.Invoking(s => s.Intercept(null)).
                Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void CallsProceedOnInvocation(ProceedingInterceptor sut, IInvocation invocation)
        {
            sut.Intercept(invocation);

            invocation.Received().Proceed();
        }
    }
}
