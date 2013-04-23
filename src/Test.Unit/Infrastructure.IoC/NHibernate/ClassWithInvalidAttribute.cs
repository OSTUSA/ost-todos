using Infrastructure.NHibernate;

namespace Test.Unit.Infrastructure.IoC.NHibernate
{
    [SessionFactory("IDontHaveASessionFactoryAtAll")]
    public class ClassWithInvalidAttribute
    {
    }
}
