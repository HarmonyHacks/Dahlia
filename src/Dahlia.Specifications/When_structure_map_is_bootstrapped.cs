using Dahlia.Configuration;
using log4net;
using Machine.Specifications;
using StructureMap;

namespace Dahlia.Specifications
{
    [Subject("IoC Container")]
    public class When_structure_map_is_bootstrapped
    {
        Because of = () =>
            AppStart_StructureMap.Start();

        It should_create_a_valid_container_configuration = () =>
            ObjectFactory.AssertConfigurationIsValid();

        It should_create_a_valid_logger_instance = () =>
            ObjectFactory.GetInstance<ILog>().ShouldNotBeNull();
    }

    [Subject("IoC Container")]
    public class When_resolving_a_class_with_a_logger_property
    {
        Because of = () =>
            AppStart_StructureMap.Start();

        It should_populate_the_logger = () =>
                ObjectFactory.GetInstance<SomeClass>().Logger.ShouldNotBeNull();
    }

    internal class SomeClass
    {
        public ILog Logger { get; set; }
    }
}