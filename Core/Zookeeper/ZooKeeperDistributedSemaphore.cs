using System.Text;
using Core.ZooKeeper.Interfaces;
using org.apache.zookeeper;

namespace Core.ZooKeeper;

public class ZooKeeperDistributedSemaphore : IDistributedSemaphore
{
    private readonly string _semaphorePath;
    private readonly org.apache.zookeeper.ZooKeeper _zooKeeper;
    private readonly int _maxCount;
    private static readonly IReadOnlyList<byte> AcquiredMarker = Encoding.UTF8.GetBytes("ACQUIRED");

    public ZooKeeperDistributedSemaphore(org.apache.zookeeper.ZooKeeper zooKeeper, string semaphorePath, int maxCount)
    {
        _semaphorePath = semaphorePath;
        _zooKeeper = zooKeeper;
        _maxCount = maxCount;
    }

    public async Task<LockHandler> AcquireAsync(TimeOutValue timeout, CancellationToken cancellationToken = default)
    {
        string nodePath = null;
        var timeoutSource = new CancellationTokenSource(timeout.TimeSpan);

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                nodePath = await _zooKeeper.createAsync(_semaphorePath + "/lock-", null, ZooDefs.Ids.OPEN_ACL_UNSAFE,
                        CreateMode.EPHEMERAL_SEQUENTIAL)
                    .ConfigureAwait(false);

                var children = await _zooKeeper.getChildrenAsync(_semaphorePath, false)
                    .ConfigureAwait(false);

                var sortedChildren = await ZooKeeperSequentialPathHelper.FilterAndSortAsync(
                    parentNode: _semaphorePath,
                    childrenNames: children.Children,
                    prefix: "semaphore-",
                    _zooKeeper,
                    alternatePrefix: null
                ).ConfigureAwait(false);



                var state = new State(nodePath, sortedChildren);

                var currentNodeIndex = Array.FindIndex(state.SortedChildren, t => t.Path == state.EphemeralNodePath);

                if (currentNodeIndex < _maxCount)
                {

                    await _zooKeeper.setDataAsync(nodePath, AcquiredMarker.ToArray()).ConfigureAwait(false);

                    return new LockHandler(this, nodePath);
                }



                var waitCompletionSource = new TaskCompletionSource<bool>();
                using var timeoutRegistration =
                    timeoutSource.Token.Register(state => ((TaskCompletionSource<bool>)state).TrySetResult(false),
                        waitCompletionSource);
                using var cancellationRegistration =
                    cancellationToken.Register(state => ((TaskCompletionSource<bool>)state).TrySetCanceled(),
                        waitCompletionSource);

                var watcher = new WaitCompletionSource(waitCompletionSource);


                if (!waitCompletionSource.Task.IsCompleted
                    && await WaitAsync(_zooKeeper, watcher, state))
                {
                    waitCompletionSource.TrySetResult(true);
                }

                if (!await waitCompletionSource.Task.ConfigureAwait(false))
                {
                    return null;
                }
            }
            finally
            {
                timeoutSource.Dispose();
            }
        }
    }


    private async Task<bool> WaitAsync(org.apache.zookeeper.ZooKeeper zooKeeper, Watcher watcher, State state)
    {
        var ephemeralNodeIndex =
            Array.FindIndex(state.SortedChildren, t => t.Path == state.EphemeralNodePath);

        if (ephemeralNodeIndex == _maxCount)
        {
            var childNames = new HashSet<string>((await zooKeeper.getChildrenAsync(_semaphorePath, watcher).ConfigureAwait(false)).Children);
            return state.SortedChildren.Take(ephemeralNodeIndex)
                .Any(t => !childNames.Contains(t.Path.Substring(t.Path.LastIndexOf('/') + 1)));
        }

        var nextLowestChildData =
            await zooKeeper.getDataAsync(state.SortedChildren[ephemeralNodeIndex - 1].Path, watcher).ConfigureAwait(false);

        return nextLowestChildData.Data.SequenceEqual(AcquiredMarker);
    }

    public async Task ReleaseAsync(string nodePath)
    {
        await _zooKeeper.deleteAsync(nodePath).ConfigureAwait(false);
    }

    public record State(string EphemeralNodePath, (string Path, int SequenceNumber, string Prefix)[] SortedChildren);
}