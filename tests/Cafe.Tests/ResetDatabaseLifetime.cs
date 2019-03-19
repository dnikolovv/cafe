using Respawn;
using System.Threading.Tasks;
using Xunit;

namespace Cafe.Tests
{
    public class ResetDatabaseLifetime : IAsyncLifetime
    {
        private readonly Checkpoint _checkpoint;

        public ResetDatabaseLifetime()
        {
            _checkpoint = new Checkpoint();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        public async Task InitializeAsync()
        {
            await Reset(_checkpoint, SliceFixture.DbConnectionString);
        }

        private static async Task Reset(Checkpoint checkpoint, string connectionString)
        {
            await checkpoint.Reset(connectionString);
        }
    }
}
