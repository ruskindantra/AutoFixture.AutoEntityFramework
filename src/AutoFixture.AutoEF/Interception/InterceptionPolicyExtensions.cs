using System.Linq;

namespace AutoFixture.AutoEF.Interception
{
    static class InterceptionPolicyExtensions
    {
        public static IInterceptionPolicy And(this IInterceptionPolicy a, params IInterceptionPolicy[] others)
        {
            return new AndInterceptionPolicy(new []{a}.Concat(others));
        }

        public static IInterceptionPolicy Or(this IInterceptionPolicy a, params IInterceptionPolicy[] others)
        {
            return new OrInterceptionPolicy(new[] { a }.Concat(others));
        }
    }
}
