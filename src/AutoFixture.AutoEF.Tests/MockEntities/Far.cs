namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Far
    {
        public int FarId { get; set; }

        public int BooId { get; set; }
        public virtual Boo Boo { get; set; }
    }
}
