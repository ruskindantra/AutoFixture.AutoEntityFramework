using AutoFixture.AutoEF.Interception;
using Castle.DynamicProxy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = EmptyCollectionReturnValueInterceptionPolicy;

    public class EmptyCollectionReturnValueInterceptionPolicyTest
    {
        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.ShouldIntercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SutIsInterceptionPolicy(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptionPolicy>();
        }

        [Theory]
        [InlineAutoNSub(null)]
        [InlineAutoNSub(1)]
        [InlineAutoNSub(typeof(object))]
        [InlineAutoNSub("string")]
        public void ReturnValueReturnsFalse(object returnValue, SUT sut, IInvocation invocation)
        {
            invocation.ReturnValue = null;

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeFalse();
        }

        [Theory, AutoNSub]
        public void NonEmptyCollectionReturnsFalse(SUT sut, IInvocation invocation, ICollection<object> collection)
        {
            collection.Should().NotBeEmpty();

            invocation.ReturnValue = collection;

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeFalse();
        }

        [Theory, AutoNSub]
        public void EmptyCollectionReturnsTrue(SUT sut, IInvocation invocation, ICollection<object> collection)
        {
            collection.Clear();

            invocation.ReturnValue = collection;

            var result = sut.ShouldIntercept(invocation);

            result.Should().BeTrue();
        }
    }
}
