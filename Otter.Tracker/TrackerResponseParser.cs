using System.Net;
using Otter.Bencode;

namespace Otter.Tracker;

public static class TrackerResponseParser
{
    public static TrackerResponse Parse(byte[] bytes)
    {
        var decoder = new BencodeDecoder(bytes);
        var root = decoder.Decode();

        var dictionary = (BencodeDictionary)root.Value;

        var complete =
            (int)((BencodeInteger)dictionary.Values["complete"].Value).Value;

        var incomplete =
            (int)((BencodeInteger)dictionary.Values["incomplete"].Value).Value;

        var interval =
            (int)((BencodeInteger)dictionary.Values["interval"].Value).Value;

        var peerBytes =
            ((BencodeString)dictionary.Values["peers"].Value).Value;

        var peers = ParsePeers(peerBytes);

        return new TrackerResponse(
            complete,
            incomplete,
            interval,
            peers);
    }


    private static List<Peer> ParsePeers(byte[] bytes)
    {
        var peers = new List<Peer>();

        for (int i = 0; i < bytes.Length; i += 6)
        {
            var ip = new IPAddress(new byte[]
{
    bytes[i],
    bytes[i + 1],
    bytes[i + 2],
    bytes[i + 3]
});

            var port =
                (bytes[i + 4] << 8) |
                bytes[i + 5];

            peers.Add(
                new Peer(
                    ip.ToString(),
                    port));
        }

        return peers;
    }
}
