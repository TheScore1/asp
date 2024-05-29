using Microsoft.Extensions.ObjectPool;

namespace Core.RabbitRpc.Helpers;

public class PoolObject<T> : IDisposable where T : class
{
    private readonly DefaultObjectPool<T> _pool;
    public T Item { get; set; }

    public PoolObject(DefaultObjectPool<T> pool)
    {
        this._pool = pool;
        this.Item = this._pool.Get();
    }

    public void Dispose()
    {
        this._pool.Return(this.Item);
    }
}