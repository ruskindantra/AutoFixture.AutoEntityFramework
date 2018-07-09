using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using Xunit;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class CompositeInterceptorTest
    {
        [Fact]
        public void ThrowsOnNullInterceptors()
        {
            Action act = () => new CompositeInterceptor(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SutIsInterceptor(CompositeInterceptor sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(CompositeInterceptor sut)
        {
            sut.Invoking(s => s.Intercept(null))
                .Should().Throw<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SingleInterceptorCalled(IInterceptor interceptor, IInvocation invocation)
        {
            var sut = new CompositeInterceptor(interceptor);

            sut.Intercept(invocation);

            interceptor.Received().Intercept(invocation);
        }

        [Theory, AutoNSub]
        public void InterceptorsCalledInOrder([Frozen] IInterceptor[] interceptors,
            CompositeInterceptor sut,
            IInvocation invocation)
        {
            var results = new List<IInterceptor>();

            foreach (var i in interceptors)
                i.When(x => x.Intercept(invocation)).Do(_ => results.Add(i));

            sut.Intercept(invocation);

            results.Should().Equal(interceptors);
        }
    }
}
