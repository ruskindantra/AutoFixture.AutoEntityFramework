using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = ReturnValueOverrideInterceptor;

    public class ReturnValueOverrideInterceptorTest
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

        [Fact]
        public void ThrowsOnNullObjectFactory()
        {
            Action act = () => new SUT(null);

            act.ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void InvocationReturnValueIsSetToFactoryFunctionResult(IInvocation invocation, object obj)
        {
            var sut = new SUT(_ => obj);

            sut.Intercept(invocation);

            invocation.ReturnValue.Should().Be(obj);
        }

        [Theory, AutoNSub]
        public void InvocationIsPassedToObjectFactory(IInvocation expected, object obj)
        {
            IInvocation passedInvocation = null;

            Func<IInvocation, object> func = i =>
            {
                passedInvocation = i;
                return obj;
            };

            var sut = new SUT(func);

            sut.Intercept(expected);

            passedInvocation.Should().Be(expected);
        }
    }
}
