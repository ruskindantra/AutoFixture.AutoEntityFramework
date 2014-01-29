
using System.Collections.Generic;

namespace AutoFixture.AutoEF.Tests.MockEntities
{
    public class Bar
    {
        public Bar()
        {
            this.Quxes = new HashSet<Qux>();
        }

        public int Id { get; set; }

        public int FooId { get; set; }
        public virtual Foo Foo { get; set; }

        public virtual ICollection<Qux> Quxes { get; set; }
    }
}
