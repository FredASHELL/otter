namespace Otter.Tracker;

public class Peer
{
    public string Ip { get; }
    public int Port { get; }

    public Peer(string ip, int port)
    {
        Ip = ip;
        Port = port;
    }

    public override string ToString()
    {
        return $"{Ip}:{Port}";
    }
}
