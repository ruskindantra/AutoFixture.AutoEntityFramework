using System.Collections.Generic;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Qux
    {
        public Qux()
        {
            this.Bars = new HashSet<Bar>();
        }

        public int Id { get; set; }

        public int BarId { get; set; }
        public virtual Bar Bar { get; set; }

        public virtual ICollection<Bar> Bars { get; set; }
    }
}
