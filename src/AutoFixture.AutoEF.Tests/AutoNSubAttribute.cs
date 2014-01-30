using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Ploeh.AutoFixture.Xunit;

namespace AutoFixture.AutoEF.Tests
{
    public class AutoNSubAttribute : AutoDataAttribute
    {
        public AutoNSubAttribute()
            : base(new Fixture().Customize(new AutoNSubstituteCustomization()))
        { }
    }

    public class InlineAutoNSubAttribute : InlineAutoDataAttribute
    {
        public InlineAutoNSubAttribute(params object[] values)
            : base(new AutoNSubAttribute(), values)
        { }
    }
}
