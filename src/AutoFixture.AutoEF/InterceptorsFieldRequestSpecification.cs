using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    public class InterceptorsFieldRequestSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var fi = request as FieldInfo;
            return fi != null && fi.FieldType == typeof(IInterceptor[]);
        }
    }
}
