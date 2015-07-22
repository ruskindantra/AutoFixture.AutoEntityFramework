using AutoFixture.AutoEF.Interception;
using AutoFixture.AutoEF.Tests.MockEntities;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests.Interception
{
    using SUT = ParentPropertySetterInterceptor;

    public class ParentPropertySetterInterceptorTest
    {
        [Theory, AutoNSub]
        public void SutIsInterceptor(SUT sut)
        {
            sut.Should().BeAssignableTo<IInterceptor>();
        }

        [Theory, AutoNSub]
        public void ThrowsOnNullInvocation(SUT sut)
        {
            sut.Invoking(s => s.Intercept(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoNSub]
        public void SetsParentWhenPropertyExists(SUT sut, IInvocation invocation, int fooId)
        {
            var foo = new Foo() { Id = fooId };
            var bar = new Bar();

            invocation.InvocationTarget.Returns(foo);
            invocation.ReturnValue = bar;

            sut.Intercept(invocation);

            bar.Foo.Should().BeSameAs(foo);
        }

        [Theory, AutoNSub]
        public void SetsParentIdWhenPropertyExists(SUT sut, IInvocation invocation, int fooId)
        {
            var foo = new Foo() { Id = fooId };
            var bar = new Bar();

            invocation.InvocationTarget.Returns(foo);
            invocation.ReturnValue = bar;

            sut.Intercept(invocation);

            bar.FooId.Should().Be(foo.Id);
        }

        [Theory, AutoNSub]
        public void SetsParentIdWhenPropertyExistsWhenTableNameId(SUT sut, IInvocation invocation, int farId)
        {
            var far = new Far() { FarId = farId };
            var boo = new Boo();

            invocation.InvocationTarget.Returns(far);
            invocation.ReturnValue = boo;

            sut.Intercept(invocation);

            boo.FarId.Should().Be(far.FarId);
        }
    }
}
