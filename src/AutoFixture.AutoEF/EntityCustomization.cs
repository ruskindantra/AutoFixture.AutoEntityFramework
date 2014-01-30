using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using System;

namespace AutoFixture.AutoEF
{
    public class EntityCustomization : ICustomization
    {
        private readonly IEntityTypesProvider _entityTypesProvider;

        public EntityCustomization(IEntityTypesProvider entityTypesProvider)
        {
            if (entityTypesProvider == null)
                throw new ArgumentNullException("entityTypesProvider");

            _entityTypesProvider = entityTypesProvider;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Insert(0, 
                new Postprocessor(
                    new FilteringSpecimenBuilder(
                        new EntitySpecimenBuilder(),
                        new EntityRequestSpecification(_entityTypesProvider)),
                    new AutoPropertiesCommand(
                        new InverseRequestSpecification(
                            new OrRequestSpecification(
                                new NavigationPropertyRequestSpecification(_entityTypesProvider),
                                new DynamicProxyFieldRequestSpecification())
                            ))));
        }
    }
}