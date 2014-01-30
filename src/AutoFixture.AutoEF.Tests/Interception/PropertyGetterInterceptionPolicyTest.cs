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
    using SUT = PropertyGetterInterceptionPolicy;

    public class PropertyGetterInterceptionPolicyTest
    {
        [Theory, AutoData]
        public void SutIsInterceptionPolicy(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptionPolicy>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.ShouldIntercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void ReturnsTrueOnGetter(SUT sut, IInvocation invocation)
        {
            invocation.Method.Returns(typeof (Foo).GetProperty("Bar").GetGetMethod());

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeTrue();
        }

        [Theory, AutoNSub]
        public void ReturnsFalseOnSetter(SUT sut, IInvocation invocation)
        {
            invocation.Method.Returns(typeof(Foo).GetProperty("Bar").GetSetMethod());

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeFalse();
        }
    }
}
