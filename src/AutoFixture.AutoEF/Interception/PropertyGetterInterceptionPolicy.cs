using Castle.DynamicProxy;

namespace AutoFixture.AutoEF.Interception
{
    public class PropertyGetterInterceptionPolicy : IInterceptionPolicy
    {
        public bool ShouldIntercept(IInvocation invocation)
        {
            return invocation.Method.IsSpecialName
                && invocation.Method.ReturnType != typeof (void);
        }
    }
}