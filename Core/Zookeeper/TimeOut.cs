namespace Core.ZooKeeper;
public readonly struct TimeOutValue : IEquatable<TimeOutValue>, IComparable<TimeOutValue>
{
    public TimeOutValue(TimeSpan? timeout, string paramName = "timeout")
    {
        if (timeout is { } timeoutValue)
        {
            var totalMilliseconds = (long)timeoutValue.TotalMilliseconds;
            if (totalMilliseconds < -1 || totalMilliseconds > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: paramName,
                    actualValue: timeoutValue,
                    message: $"Must be {nameof(Timeout)}.{nameof(Timeout.InfiniteTimeSpan)} ({Timeout.InfiniteTimeSpan}) or a non-negative value <= {TimeSpan.FromMilliseconds(int.MaxValue)})"
                );
            }

            this.InMilliseconds = (int)totalMilliseconds;
        }
        else
        {
            this.InMilliseconds = Timeout.Infinite;
        }
    }

    public int InMilliseconds { get; }
    public int InSeconds => this.IsInfinite ? throw new InvalidOperationException("infinite timeout cannot be converted to seconds") : this.InMilliseconds / 1000;
    public bool IsInfinite => this.InMilliseconds == Timeout.Infinite;
    public bool IsZero => this.InMilliseconds == 0;
    public TimeSpan TimeSpan => TimeSpan.FromMilliseconds(this.InMilliseconds);

    public bool Equals(TimeOutValue that) => this.InMilliseconds == that.InMilliseconds;
    public override bool Equals(object? obj) => obj is TimeOutValue that && this.Equals(that);
    public override int GetHashCode() => this.InMilliseconds;

    public int CompareTo(TimeOutValue that) =>
        this.IsInfinite ? (that.IsInfinite ? 0 : 1)
            : that.IsInfinite ? -1
            : this.InMilliseconds.CompareTo(that.InMilliseconds);

    public static bool operator ==(TimeOutValue a, TimeOutValue b) => a.Equals(b);
    public static bool operator !=(TimeOutValue a, TimeOutValue b) => !(a == b);

    public static implicit operator TimeOutValue(TimeSpan? timeout) => new(timeout);

    public override string ToString() =>
        this.IsInfinite ? "∞"
            : this.IsZero ? "0"
            : this.TimeSpan.ToString();
}