using AutoFixture.AutoEF.Tests.MockEntities;
using Castle.DynamicProxy;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Kernel;
using System;
using Xunit;

namespace AutoFixture.AutoEF.Tests
{
    public class EntityInterceptorTest
    {
        private IInvocation GetBarPropertyInvocation(Foo foo)
        {
            var invocation = Substitute.For<IInvocation>();
            invocation.Method.Returns(foo.GetType().GetProperty("Bar").GetGetMethod());
            invocation.InvocationTarget.Returns(foo);
            invocation.ReturnValue = foo.Bar;
            return invocation;
        }

        [Fact]
        public void InitializeWithNullContextThrowsNullArgumentException()
        {
            // Fixture setup
            Action act = () => new EntityInterceptor(null);
            // Exercise system
            // Verify outcome
            act.ShouldThrow<ArgumentNullException>();
            // Teardown
        }

        [Fact]
        public void EmptyPropertyGetterIsIntercepted()
        {
            // Fixture setup
            var foo = new Foo();
            var bar = new Bar();

            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(typeof (Bar)).Returns(bar);

            var sut = new EntityInterceptor(context);

            var invocation = GetBarPropertyInvocation(foo);

            // Exercise system
            sut.Intercept(invocation);
            // Verify outcome
            invocation.ReturnValue.Should().Be(bar);
            // Teardown
        }

        [Fact]
        public void GeneratedPropertyIsSetOnTarget()
        {
            // Fixture setup
            var foo = new Foo();
            var bar = new Bar();

            var context = Substitute.For<ISpecimenContext>();
            context.Resolve(typeof(Bar)).Returns(bar);

            var sut = new EntityInterceptor(context);

            var invocation = GetBarPropertyInvocation(foo);

            // Exercise system
            sut.Intercept(invocation);
            // Verify outcome
            foo.Bar.Should().Be(bar);
            // Teardown
        }

        [Fact]
        public void ExistingPropertyIsUsedIfSet()
        {
            // Fixture setup
            var foo = new Foo();
            var bar = new Bar();
            foo.Bar = bar;

            var sut = new EntityInterceptor(Substitute.For<ISpecimenContext>());

            var invocation = GetBarPropertyInvocation(foo);

            // Exercise system
            sut.Intercept(invocation);
            // Verify outcome
            invocation.ReturnValue.Should().Be(bar);
            // Teardown
        }

        [Fact]
        public void NoEntityGeneratedIfPropertySet()
        {
            // Fixture setup
            var foo = new Foo();
            var bar = new Bar();
            foo.Bar = bar;

            var context = Substitute.For<ISpecimenContext>();
            var sut = new EntityInterceptor(context);

            var invocation = GetBarPropertyInvocation(foo);

            // Exercise system
            sut.Intercept(invocation);
            // Verify outcome
            context.DidNotReceiveWithAnyArgs().Resolve(null);
            // Teardown
        }
    }
}
