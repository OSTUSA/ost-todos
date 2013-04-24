using Core.Application;

namespace Infrastructure.Migrations
{
    public class MigrationPersistenceSetup : IPersistenceSetup
    {
        private readonly Runner.Runner _runner;
        private readonly string _profile;

        public MigrationPersistenceSetup(string connString, string profile = "")
        {
            _runner = new Runner.Runner(connString);
            _profile = profile;
        }

        public void Setup()
        {
            _runner.MigrateUp(Runner.Runner.VersionLatest, _profile);
        }
    }
}
