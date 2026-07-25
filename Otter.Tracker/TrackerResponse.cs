namespace Otter.Tracker;

public class TrackerResponse
{
    public int Complete { get; }
    public int Incomplete { get; }
    public int Interval { get; }
    public List<Peer> Peers { get; }

    public TrackerResponse(
        int complete,
        int incomplete,
        int interval,
        List<Peer> peers)
    {
        Complete = complete;
        Incomplete = incomplete;
        Interval = interval;
        Peers = peers;
    }
}
