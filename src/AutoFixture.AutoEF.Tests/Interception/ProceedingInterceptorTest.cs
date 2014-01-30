using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

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
            sut.Invoking(s => s.Intercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void CallsProceedOnInvocation(ProceedingInterceptor sut, IInvocation invocation)
        {
            sut.Intercept(invocation);

            invocation.Received().Proceed();
        }
    }
}
