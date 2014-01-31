using Castle.DynamicProxy;
using FluentAssertions;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit;
using System;
using Xunit.Extensions;

namespace AutoFixture.AutoEF.Tests
{
    using SUT = InterceptorsFieldRequestSpecification;

    public class InterceptorsFieldRequestSpecificationTest
    {
        [Theory, AutoData]
        public void SutIsRequestSpecification(SUT sut)
        {
            sut.Should().BeAssignableTo<IRequestSpecification>();
        }

        [Theory, AutoData]
        public void ShouldThrowOnNullRequest(SUT sut)
        {
            sut.Invoking(s => s.IsSatisfiedBy(null))
                .ShouldThrow<ArgumentNullException>();
        }

        [Theory, AutoData]
        public void ReturnsFalseWhenRequestIsNotFieldInfo(SUT sut, object request)
        {
            var result = sut.IsSatisfiedBy(request);

            result.Should().BeFalse();
        }

        class ObjectWithField
        {
            public int NotInterceptors;
            public IInterceptor[] Interceptors;
        }

        [Theory, AutoData]
        public void ReturnsFalseWhenRequestIsNotInterceptorArray(SUT sut)
        {
            var field = typeof (ObjectWithField).GetField("NotInterceptors");

            var result = sut.IsSatisfiedBy(field);

            result.Should().BeFalse();
        }

        [Theory, AutoData]
        public void ReturnsTrueWhenRequestIsInterceptorArray(SUT sut)
        {
            var field = typeof(ObjectWithField).GetField("Interceptors");

            var result = sut.IsSatisfiedBy(field);

            result.Should().BeTrue();
        }
    }
}
