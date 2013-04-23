using System.Reflection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace Infrastructure.Migrations.Runner
{
    public class Runner
    {
        public IAnnouncer Announcer { get; set; }
        public string ConnectionString { get; set; }
        public const long VersionLatest = -1;

        public Runner(string connString, IAnnouncer announcer = null)
        {
            ConnectionString = connString;
            Announcer = announcer ?? new NullAnnouncer();
        }

        public void MigrateUp(long version = -1, string profile = "")
        {
            GetMigrationRunner(version, profile).MigrateUp(true);
        }

        public void MigrateDown(long version)
        {
            GetMigrationRunner(version).MigrateDown(version);
        }

        public void RollbackToVersion(long version)
        {
            GetMigrationRunner(-1).RollbackToVersion(version);
        }

        private MigrationRunner GetMigrationRunner(long version, string profile = "")
        {
            if(string.IsNullOrEmpty(ConnectionString))
                throw new EmptyConnectionStringException("ConnectionString property not initialized");
            var options = new MigrationOptions() {PreviewOnly = false, Timeout = 60};
            var factory = new FluentMigrator.Runner.Processors.SqlServer.SqlServer2008ProcessorFactory();
            var processor = factory.Create(ConnectionString, Announcer, options);
            var assembly = Assembly.GetExecutingAssembly();
            var runner = new MigrationRunner(assembly, GetMigrationContext(version, profile), processor);
            return runner;
        }

        private RunnerContext GetMigrationContext(long version, string profile)
        {
            var context = new RunnerContext(Announcer);
            if (version > -1)
                context.Version = version;
            if (!string.IsNullOrEmpty(profile))
                context.Profile = profile;
            return context;
        }
    }
}
