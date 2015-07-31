using System.Data.Entity;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    class MockDbContextIDbSet : DbContext
    {
        public IDbSet<Foo> Foos { get; set; }
        public IDbSet<Bar> Bars { get; set; }
        public IDbSet<Qux> Quxes { get; set; }
        public IDbSet<Far> Fars { get; set; }
        public IDbSet<Boo> Boos { get; set; }
        public IDbSet<Qix> Qixes { get; set; }
    }
}
