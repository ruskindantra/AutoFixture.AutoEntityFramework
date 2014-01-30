using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class ProceedingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}