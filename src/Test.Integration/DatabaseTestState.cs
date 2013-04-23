using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Infrastructure.Migrations.Runner;
using Infrastructure.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace Test.Integration
{
    public class DatabaseTestState
    {
        protected string ConnectionString;
        protected string MigrationProfile;
        protected readonly SessionFactoryBuilder Builder = new SessionFactoryBuilder();

        public DatabaseTestState(string connString)
        {
            ConnectionString = connString;
        }

        public ISessionFactory Configure<TMapping>(string profile = "")
        {
            MigrationProfile = profile;
            var factory = Builder.GetFactory(ConnectionString, GetSessionFactory<TMapping>);
            CreateSchema();
            return factory;
        }

        protected ISessionFactory GetSessionFactory<TMapping>()
        {
            return Fluently.Configure()
                    .Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey(ConnectionString)))
                    .Mappings(cfg => cfg.FluentMappings.AddFromAssemblyOf<TMapping>())
                    .BuildConfiguration()
                    .CurrentSessionContext<ThreadStaticSessionContext>().BuildSessionFactory();
        }

        protected void CreateSchema()
        {
            var connString = ConfigurationManager.ConnectionStrings[ConnectionString].ToString();
            var runner = new Runner(connString);
            runner.RollbackToVersion(0);
            runner.MigrateUp(Runner.VersionLatest, MigrationProfile);
        }
    }
}
