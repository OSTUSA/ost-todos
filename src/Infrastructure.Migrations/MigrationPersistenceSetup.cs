using Core.Application;

namespace Infrastructure.Migrations
{
    public class MigrationPersistenceSetup : IPersistenceSetup
    {
        private readonly Runner.Runner _runner;

        public MigrationPersistenceSetup(string connString)
        {
            _runner = new Runner.Runner(connString);
        }

        public void Setup()
        {
            _runner.MigrateUp(Runner.Runner.VersionLatest);
        }
    }
}
