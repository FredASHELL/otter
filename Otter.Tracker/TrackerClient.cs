using System.Text;

namespace Otter.Tracker;

public class TrackerClient
{
    private readonly HttpClient _http = new();

    public async Task<byte[]> Announce(
        string trackerUrl,
        byte[] infoHash,
        long left)
    {
        var peerId = Encoding.ASCII.GetBytes(
            "-OT0001-" +
            Guid.NewGuid()
                .ToString("N")
                .Substring(0, 12));

        var url =
            $"{trackerUrl}" +
            $"?info_hash={UrlEncoding.EncodeBytes(infoHash)}" +
            $"&peer_id={UrlEncoding.EncodeBytes(peerId)}" +
            $"&port=6881" +
            $"&uploaded=0" +
            $"&downloaded=0" +
            $"&left={left}" +
            $"&compact=1";

        return await _http.GetByteArrayAsync(url);
    }
}
