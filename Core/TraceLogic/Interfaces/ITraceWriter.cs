namespace Core.TraceLogic.Interfaces;

public interface ITraceWriter
{
    string Name { get; }

    string GetValue();
}