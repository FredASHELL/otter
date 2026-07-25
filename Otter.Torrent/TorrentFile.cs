using Otter.Bencode;

namespace Otter.Torrent;

public class TorrentFile
{
    public string Announce { get; }
    public string Name { get; }
    public long Length { get; }
    public long PieceLength { get; }
    public byte[] Pieces { get; }

    // Raw 20-byte SHA-1 info hash
    public byte[] InfoHashBytes { get; }

    // Hex string for display
    public string InfoHash =>
        Convert.ToHexString(InfoHashBytes).ToLowerInvariant();

    public byte[] OriginalBytes { get; }

    public TorrentFile(
        string announce,
        string name,
        long length,
        long pieceLength,
        byte[] pieces,
        byte[] infoHashBytes,
        byte[] originalBytes)
    {
        Announce = announce;
        Name = name;
        Length = length;
        PieceLength = pieceLength;
        Pieces = pieces;
        InfoHashBytes = infoHashBytes;
        OriginalBytes = originalBytes;
    }

    public static TorrentFile Load(string path)
    {
        var bytes = File.ReadAllBytes(path);

        var decoder = new BencodeDecoder(bytes);

        var node = decoder.Decode();

        var dictionary = (BencodeDictionary)node.Value;

        return TorrentParser.Parse(dictionary, bytes);
    }
}
