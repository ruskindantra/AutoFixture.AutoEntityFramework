using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public interface IInterceptionPolicy
    {
        bool ShouldIntercept(IInvocation invocation);
    }
}