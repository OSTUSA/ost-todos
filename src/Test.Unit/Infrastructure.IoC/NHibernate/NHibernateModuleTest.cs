using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Infrastructure.IoC.NHibernate;
using Infrastructure.NHibernate;
using Moq;
using NHibernate;
using NUnit.Framework;
using Ninject;
using System.Linq;
using Ninject.Activation;
using Ninject.Parameters;
using Ninject.Planning.Bindings;
using Ninject.Planning.Targets;

namespace Test.Unit.Infrastructure.IoC.NHibernate
{
    [TestFixture]
    public class NHibernateModuleTest
    {
        protected NHibernateModule Module;

        [SetUp]
        public void Init()
        {
            NHibernateModule.DefaultFactory = () => new Mock<ISessionFactory>().Object;
            Module = new NHibernateModule();
        }

        [Test]
        public void Constructor_has_default_empty_dictionary_of_ISessionFactory()
        {
            Assert.IsInstanceOf<Dictionary<string, ISessionFactory>>(Module.Factories);
        }

        [Test]
        public void Add_does_thread_safe_add_to_dictionary()
        {
            Module.Add("OTHER", () => new Mock<ISessionFactory>().Object);
            Assert.True(Module.Factories.ContainsKey("OTHER"));
        }

        [Test]
        public void OnLoad_should_perform_binding_to_ISession()
        {
            var std = new StandardKernel();
            Module.OnLoad(std);
            var bound = Module.Bindings.SingleOrDefault(b => b.Service == typeof (ISession));
            Assert.IsNotNull(bound);
        }

        [Test]
        public void Constructor_should_set_default_factory()
        {
            Assert.IsInstanceOf<ISessionFactory>(Module.Factories["Default"]);
        }

        [Test]
        public void Overriding_default_factory_function_should_return_provided_factory()
        {
            ClearDefaultFactory();

            var mocked = new Mock<ISessionFactory>().Object;
            NHibernateModule.DefaultFactory = () => mocked;
            var module = new NHibernateModule();
            Assert.AreSame(mocked, module.Factories["Default"]);
        }

        [Test]
        public void Test_GetSession_returns_factory_for_attribute()
        {
            ClearDefaultFactory();
            var context = GetMockContext<ClassWithAttribute>();
            var module = new NHibernateModule();
            var result = InvokeGetSession(module, "Default");
            Assert.IsNotNull(result);
        }

        [Test]
        public void Test_Kernel_can_resolve_factory_for_class_with_attribute()
        {
            var module = GetLoadedModule();
            var ctx = GetMockContext<ClassWithAttribute>();
            var req = new Request(ctx.Object, typeof(ISession), ctx.Object.Request.Target, null);
            Assert.True(module.Kernel.CanResolve(req));
        }

        [Test]
        public void Test_Kernel_can_resolve_factory_for_class_with_no_attribute()
        {
            var module = GetLoadedModule();
            var ctx = GetMockContext<ClassWithNoAttribute>();
            var req = new Request(ctx.Object, typeof(ISession), ctx.Object.Request.Target, null);
            Assert.True(module.Kernel.CanResolve(req));
        }

        [Test]
        public void Test_Kernel_cant_resolve_for_invalid_factory()
        {
            var module = GetLoadedModule();
            var ctx = GetMockContext<ClassWithInvalidAttribute>();
            var req = new Request(ctx.Object, typeof(ISession), ctx.Object.Request.Target, null);
            Assert.False(module.Kernel.CanResolve(req));
        }

        [Test]
        [ExpectedException(typeof(InvalidFactoryException))]
        public void Test_Invalid_Factory_name_in_attribute_throws_exception()
        {
            var context = GetMockContext<ClassWithInvalidAttribute>();
            var module = new NHibernateModule();
            //test the inner exception, not the one from reflection
            try
            {
                InvokeGetSession(module, "IDontHaveASessionFactoryAtAll");
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        protected Mock<IContext> GetMockContext<T>()
        {
            var factory = new Mock<ISessionFactory>();
            factory.Setup(f => f.OpenSession()).Returns(new Mock<ISession>().Object);
            NHibernateModule.DefaultFactory = () => factory.Object;
            var context = new Mock<IContext>();
            var request = new Mock<IRequest>();
            var target = new Mock<ITarget>();
            var info = new Mock<MemberInfo>();
            info.SetupGet(i => i.ReflectedType).Returns(typeof(T));
            target.SetupGet(t => t.Member).Returns(info.Object);
            request.SetupGet(r => r.Target).Returns(target.Object);
            request.SetupGet(r => r.ActiveBindings).Returns(new Stack<IBinding>());
            request.SetupGet(r => r.Service).Returns(typeof (ISession));
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Parameters).Returns(new List<IParameter>());
            return context;
        }

        protected static ISession InvokeGetSession(NHibernateModule module, string factoryName)
        {
            MethodInfo method = typeof(NHibernateModule).GetMethod("GetSession", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = method.Invoke(module, new object[] { factoryName }) as ISession;
            return result;
        }

        protected void ClearDefaultFactory()
        {
            var builder =
                typeof(NHibernateModule).GetField("Builder", BindingFlags.NonPublic | BindingFlags.Static).GetValue(Module) as
                SessionFactoryBuilder;
            var scoper =
                typeof(SessionFactoryBuilder).GetField("_factorySingleton", BindingFlags.NonPublic | BindingFlags.Instance)
                                              .GetValue(builder) as SingletonInstanceScoper<ISessionFactory>;
            scoper.ClearInstance("Default");
        }

        protected NHibernateModule GetLoadedModule()
        {
            ClearDefaultFactory();
            var module = new NHibernateModule();
            var kernel = new StandardKernel();
            module.OnLoad(kernel);
            return module;
        }
    }
}
