namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Foo
    {
        public int Id { get; set; }

        public int BarId { get; set; }
        public virtual Bar Bar { get; set; }
    }
}
