namespace Core.RabbitRpc.Helpers;

public class RabbitConnectionOptions
{
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string HostName { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 5672;
    public string VHost { get; set; } = "/";
    public int MaxChannelCount { get; set; } = Environment.ProcessorCount * 2;
}