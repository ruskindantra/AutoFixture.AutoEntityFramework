using System.Collections.Generic;
using System.Data.Entity;

namespace AutoFixture.AutoEF.Tests.ContextTests
{
    class Foo
    {
        public int FooId { get; set; }

        public int? BarId { get; set; }
        public int UnconventionalBarId { get; set; }
        public virtual Bar Bar { get; set; }

        public virtual ICollection<Bar> Bars { get; set; }
    }

    class Bar
    {
        public int BarId { get; set; }

        public int? FooId { get; set; }
        public virtual Foo Foo { get; set; }

        public virtual ICollection<Foo> Foos { get; set; }
    }

    class OptionalToRequiredContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.UnconventionalBarId)
                .Ignore(f => f.Bars);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasOptional(f => f.Bar)
                .WithRequired(b => b.Foo);
        }
    }

    class OptionalToOptionalContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.UnconventionalBarId)
                .Ignore(f => f.Bars);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasOptional(f => f.Bar)
                .WithOptionalPrincipal(b => b.Foo);
        }
    }

    class NoKeyContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.BarId)
                .Ignore(f => f.UnconventionalBarId)
                .Ignore(f => f.Bars);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasOptional(f => f.Bar)
                .WithOptionalPrincipal(b => b.Foo);
        }
    }

    class UnidirectionalContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.BarId)
                .Ignore(f => f.UnconventionalBarId)
                .Ignore(f => f.Bars);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foo)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasOptional(f => f.Bar);
        }
    }

    class UnconventionalKeyContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.BarId)
                .Ignore(f => f.Bars);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasOptional(f => f.Bar)
                .WithOptionalPrincipal(b => b.Foo)
                .Map(f => f.MapKey("UnconventionalBarId"));
        }
    }

    class OneToManyContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.BarId)
                .Ignore(f => f.Bar)
                .Ignore(f => f.UnconventionalBarId);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.Foos);

            builder.Entity<Foo>()
                .HasMany(f => f.Bars)
                .WithRequired(b => b.Foo)
                .HasForeignKey(b => b.FooId);
        }
    }

    class ManyToManyContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Entity<Foo>()
                .HasKey(f => f.FooId)
                .Ignore(f => f.BarId)
                .Ignore(f => f.Bar)
                .Ignore(f => f.UnconventionalBarId);

            builder.Entity<Bar>()
                .HasKey(b => b.BarId)
                .Ignore(b => b.FooId)
                .Ignore(b => b.Foo);

            builder.Entity<Foo>()
                .HasMany(f => f.Bars)
                .WithMany(b => b.Foos);
        }
    }
}
