AutoFixture.AutoEntityFramework
===============================

An AutoFixture customization that lazy-loads navigation properties on Entity Framework objects.

### Customizing the fixture

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
        : base(new Fixture().Customize(new EntityCustomization(new DbContextEntityTypesProvider(typeof(MyDbContext)))))
    { }
}
```
