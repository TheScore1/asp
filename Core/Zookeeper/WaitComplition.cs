using org.apache.zookeeper;

namespace Core.ZooKeeper;

public class WaitCompletionSource : Watcher
{
	private readonly TaskCompletionSource<bool> _waitCompletionSource;

	public WaitCompletionSource(TaskCompletionSource<bool> waitCompletionSource)
	{
		_waitCompletionSource = waitCompletionSource;
	}

	public override Task process(WatchedEvent @event)
	{
		if (@event.getState() == Event.KeeperState.SyncConnected)
		{
			_waitCompletionSource.TrySetResult(true);
		}

		return Task.CompletedTask;
	}
}