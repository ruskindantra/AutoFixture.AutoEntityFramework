
using System.Collections.Generic;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Boo
    {
        public Boo()
        {
            this.Qixes = new HashSet<Qix>();
        }

        public int BooId { get; set; }

        public int FarId { get; set; }
        public virtual Far Far { get; set; }

        public virtual ICollection<Qix> Qixes { get; set; }
    }
}
