using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class NullInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation) { }
    }
}
