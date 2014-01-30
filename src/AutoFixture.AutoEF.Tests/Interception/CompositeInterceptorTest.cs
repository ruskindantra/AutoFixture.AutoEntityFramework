using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    public class CompositeInterceptorTest
    {
        [Fact]
        public void ThrowsOnNullInterceptors()
        {
            Action act = () => new CompositeInterceptor(null);

            act.ShouldThrow<ArgumentNullException>();
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
                .ShouldThrow<ArgumentNullException>();
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
