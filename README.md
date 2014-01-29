AutoFixture.AutoEntityFramework
===============================

An AutoFixture customization that lazy-loads navigation properties on Entity Framework objects.

### Initializing the customization

The `EntityCustomization` constructor requires an instance of an `IEntityTypesProvider`. This tells the customization which types are to be treated as entities. If you are using a `DbContext` to access your entities through `DbSet<>` properties, you can pass the type to a `DbContextEntityTypesProvider` which will enumerate the relevant properties. If you are not, then it is trivial to implement the `IEntityTypesProvider` interface and explicitly enumerate your entity types.

```C#
fixture.Customize(
    new EntityCustomization(
        new DbContextEntityTypesProvider(typeof(MyDbContext)))
```

### As an XUnit Attribute

```C#
public class AutoEFAttribute : AutoDataAttribute
{
    public AutoEFAttribute()
        : base(new Fixture()
            .Customize(new EntityCustomization(new DbContextEntityTypesProvider(typeof(MyDbContext)))))
    { }
}
```
