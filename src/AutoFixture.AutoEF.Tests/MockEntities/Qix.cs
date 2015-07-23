using System.Collections.Generic;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Qix
    {
        public Qix()
        {
            this.Boos = new HashSet<Boo>();
        }

        public int QixId { get; set; }

        public int BooId { get; set; }
        public virtual Boo Boo { get; set; }

        public virtual ICollection<Boo> Boos { get; set; }
    }
}
