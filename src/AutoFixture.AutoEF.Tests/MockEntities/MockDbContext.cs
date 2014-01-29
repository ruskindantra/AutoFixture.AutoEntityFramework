using System.Data.Entity;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    class MockDbContext : DbContext
    {
        public DbSet<Foo> Foos { get; set; }
        public DbSet<Bar> Bars { get; set; }
        public DbSet<Qux> Quxes { get; set; }
    }
}
