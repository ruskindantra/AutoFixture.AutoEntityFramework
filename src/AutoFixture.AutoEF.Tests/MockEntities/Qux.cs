namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Qux
    {
        public int Id { get; set; }

        public int BarId { get; set; }
        public virtual Bar Bar { get; set; }
    }
}
