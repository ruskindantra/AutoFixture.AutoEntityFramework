# This project has been superseded by [LazyEntityGraph](https://github.com/alexfoxgill/LazyEntityGraph), a general-purpose library for proxying object networks and lazy-loading navigation properties. See the [getting started](https://github.com/alexfoxgill/LazyEntityGraph/wiki) page for more information.

***

AutoFixture.AutoEntityFramework
===============================

An [AutoFixture](https://github.com/AutoFixture/AutoFixture) customization that lazy-loads navigation properties on Entity Framework objects.

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

### How does it work?

The `EntitySpecimenBuilder` filters AutoFixture requests for types that match those provided by the `IEntityTypesProvider`. It then uses [DynamicProxy](http://www.castleproject.org/projects/dynamicproxy/) to generate a proxy class of the requested type, with an interceptor. The proxy class then has its properties populated by AutoFixture, omitting navigation properties (identified as all virtual properties with a recognised entity type).

The interceptor does the other half of the work, by intercepting `get` methods on an entity's navigation properties. If the property has not previously been set explicitly, the interceptor will delegate the creation of the object back to AutoFixture. This new object is then persisted as the value of that property.

### Conventions

When the interceptor creates a new navigation object, it will check for a matching `int ____Id` property. If present, it will set the `Id` property of the new object so that `foo.BarId == foo.Bar.Id`. This also applies when the name of the table is included in the `Id` property, so `foo.BarId == foo.Bar.BarId`.

It will also check for any properties on the new object that have the same type as the parent object. If there is exactly one match, it will set that property to the parent object, along with the accompanying `____Id` property, so that `foo.Bar.Foo == foo` and `foo.Bar.FooId == foo.Id`

**Note**: In future versions of Entity Framework it will be possible to analyse the DbContext metadata in order to determine the actual relationships between objects, so the customization will be able to construct 'correct' object graphs on the fly.
