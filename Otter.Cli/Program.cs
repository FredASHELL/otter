using Otter.Torrent;
using Otter.Tracker;
using Otter.Peer;
using System.Text;

if (args.Length != 1)
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  otter <torrent-file>");
    return;
}

try
{
    var torrent = TorrentFile.Load(args[0]);

    Console.WriteLine("🦦 Otter");
    Console.WriteLine();

    Console.WriteLine($"Name         : {torrent.Name}");
    Console.WriteLine($"Tracker      : {torrent.Announce}");
    Console.WriteLine($"Size         : {torrent.Length} bytes");
    Console.WriteLine($"Piece length : {torrent.PieceLength} bytes");
    Console.WriteLine($"Pieces       : {torrent.Pieces.Length / 20}");
    Console.WriteLine($"Info hash    : {torrent.InfoHash}");
    Console.WriteLine();

    Console.WriteLine("Contacting tracker...");

    var tracker = new TrackerClient();

    var response = await tracker.Announce(
        torrent.Announce,
        torrent.InfoHashBytes,
        torrent.Length);

    Console.WriteLine($"Tracker replied with {response.Length} bytes.");

var trackerResponse =
    TrackerResponseParser.Parse(response);

Console.WriteLine();

Console.WriteLine("Tracker response");
Console.WriteLine();

if (trackerResponse.Peers.Count == 0)
{
    Console.WriteLine("Tracker returned no peers.");
    return;
}

var firstPeer = trackerResponse.Peers.First();

var connection = new PeerConnection(
    firstPeer.Ip,
    firstPeer.Port);

var peerId = Encoding.ASCII.GetBytes(
    "-OT0001-" +
    Guid.NewGuid()
        .ToString("N")
        .Substring(0, 12));

await connection.Connect(
    torrent.InfoHashBytes,
    peerId);

Console.WriteLine($"Complete   : {trackerResponse.Complete}");
Console.WriteLine($"Incomplete : {trackerResponse.Incomplete}");
Console.WriteLine($"Interval   : {trackerResponse.Interval}");
Console.WriteLine();

Console.WriteLine("Peers:");

foreach (var peer in trackerResponse.Peers)
{
    Console.WriteLine($"  {peer}");
}

}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("Error:");
    Console.WriteLine(ex.Message);
}
