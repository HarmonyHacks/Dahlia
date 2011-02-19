using Dahlia.Configuration;
using Machine.Specifications;
using StructureMap;

namespace Dahlia.Specifications
{
    public class When_structure_map_is_bootstrapped
    {
        Because of = () =>
            AppStart_Structuremap.Start();

        It should_create_a_valid_container_configuration = () =>
            ObjectFactory.AssertConfigurationIsValid();
    }
}