using System.Configuration;
using Core.Application;
using Infrastructure.Migrations;
using Ninject.Modules;

namespace Infrastructure.IoC.Migrations
{
    public class MigrationsModule : NinjectModule
    {
        public override void Load()
        {
            var str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            Bind<IPersistenceSetup>().ToMethod(ctx => new MigrationPersistenceSetup(str));
        }
    }
}
