namespace Core.TraceLogic.Interfaces;

public interface ITraceReader
{
	string Name { get; }

	void WriteValue(string value);
}