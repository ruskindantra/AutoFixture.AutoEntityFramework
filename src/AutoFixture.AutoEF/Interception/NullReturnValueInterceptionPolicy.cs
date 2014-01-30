using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class NullReturnValueInterceptionPolicy : IInterceptionPolicy
    {
        public bool ShouldIntercept(IInvocation invocation)
        {
            return invocation.ReturnValue == null;
        }
    }
}