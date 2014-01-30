using Castle.DynamicProxy;
using Ploeh.AutoFixture.Kernel;
using System.Reflection;

namespace AutoFixture.AutoEF
{
    public class DynamicProxyFieldRequestSpecification : IRequestSpecification
    {
        public bool IsSatisfiedBy(object request)
        {
            var fi = request as FieldInfo;
            return fi != null && fi.FieldType == typeof(IInterceptor[]);
        }
    }
}
