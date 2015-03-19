using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class InverseInterceptionPolicy : IInterceptionPolicy
    {
        private readonly IInterceptionPolicy _wrapped;

        public InverseInterceptionPolicy(IInterceptionPolicy wrapped)
        {
            _wrapped = wrapped;
        }

        public bool ShouldIntercept(IInvocation invocation)
        {
            return !_wrapped.ShouldIntercept(invocation);
        }
    }
}
