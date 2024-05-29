namespace Core.ZooKeeper;


public class LockHandler(ZooKeeperDistributedSemaphore semaphore, string nodePath) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await semaphore.ReleaseAsync(nodePath)
            .ConfigureAwait(false);
    }
}